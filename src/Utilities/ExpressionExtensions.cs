﻿#region © Copyright Web Applications (UK) Ltd, 2015.  All rights reserved.
// Copyright (c) 2015, Web Applications UK Ltd
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//     * Redistributions of source code must retain the above copyright
//       notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in the
//       documentation and/or other materials provided with the distribution.
//     * Neither the name of Web Applications UK Ltd nor the
//       names of its contributors may be used to endorse or promote products
//       derived from this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL WEB APPLICATIONS UK LTD BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using WebApplications.Utilities.Annotations;
using WebApplications.Utilities.Reflect;

namespace WebApplications.Utilities
{
    /// <summary>
    /// <see cref="Expression"/> extension methods.
    /// </summary>
    [PublicAPI]
    public static partial class ExpressionExtensions
    {
        /// <summary>
        /// The <see cref="IEnumerator.MoveNext" /> method.
        /// </summary>
        [NotNull]
        private static readonly MethodInfo _enumeratorMoveNextMethod =
            InfoHelper.GetMethodInfo<IEnumerator>(e => e.MoveNext(), true);

        /// <summary>
        /// The <see cref="IDisposable.Dispose"/> method.
        /// </summary>
        [NotNull]
        private static readonly MethodInfo _disposeMethod =
            InfoHelper.GetMethodInfo<IDisposable>(d => d.Dispose(), true);

        /// <summary>
        /// Gets the debug view of an expression.
        /// </summary>
        [NotNull]
        private static readonly Func<Expression, string> _expressionDebugView =
            // ReSharper disable AssignNullToNotNullAttribute, PossibleNullReferenceException
            typeof(Expression).GetProperty("DebugView", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetGetMethod(true)
                .Func<Expression, string>();

        // ReSharper restore PossibleNullReferenceException, AssignNullToNotNullAttribute

        /// <summary>
        /// Takes an input source enumerable expression (must be of type 
        /// <see cref="IEnumerable{T}" />) and creates a foreach loop,
        /// where the body is generated using the 
        /// <paramref name="getBody"/> function.
        /// </summary>
        /// <param name="sourceEnumerable">The source enumerable.</param>
        /// <param name="getBody">The get body function, where the input parameter is the current item in the loop.</param>
        /// <param name="break">The break target used by the loop body.</param>
        /// <param name="continue">The continue target used by the loop body.</param>
        /// <returns>
        /// BlockExpression.
        /// </returns>
        /// <exception cref="System.ArgumentException">The source enumerable is not of an enumerable type;sourceEnumerable</exception>
        [NotNull]
        public static Expression ForEach(
            [NotNull] this Expression sourceEnumerable,
            [NotNull] [InstantHandle] Func<Expression, Expression> getBody,
            [CanBeNull] LabelTarget @break = null,
            [CanBeNull] LabelTarget @continue = null)
        {
            if (sourceEnumerable == null) throw new ArgumentNullException("sourceEnumerable");
            if (getBody == null) throw new ArgumentNullException("getBody");
            return ForEach(sourceEnumerable, item => new[] { getBody(item) }, @break, @continue);
        }

        /// <summary>
        /// Takes an input source enumerable expression (must be of type 
        /// <see cref="IEnumerable{T}" />) and creates a foreach loop,
        /// where the body is generated using the 
        /// <paramref name="getBody" /> function.
        /// </summary>
        /// <param name="sourceEnumerable">The source enumerable.</param>
        /// <param name="getBody">The get body function, where the input parameter is the current item in the loop.</param>
        /// <param name="break">The break target used by the loop body.</param>
        /// <param name="continue">The continue target used by the loop body.</param>
        /// <returns>
        /// BlockExpression.
        /// </returns>
        /// <exception cref="System.ArgumentException">The source enumerable is not of an enumerable type;sourceEnumerable</exception>
        [NotNull]
        public static Expression ForEach(
            [NotNull] this Expression sourceEnumerable,
            [NotNull] [InstantHandle] Func<Expression, IEnumerable<Expression>> getBody,
            [CanBeNull] LabelTarget @break = null,
            [CanBeNull] LabelTarget @continue = null)
        {
            if (sourceEnumerable == null) throw new ArgumentNullException("sourceEnumerable");
            if (getBody == null) throw new ArgumentNullException("getBody");

            Type enumerableType = sourceEnumerable.Type;
            Type elementType;
            Type enumeratorType;
            bool enumeratorDisposable;
            if (enumerableType == typeof(IEnumerable))
            {
                elementType = typeof(object);
                enumeratorType = typeof(IEnumerator);
                enumeratorDisposable = false;
            }
            else if ((enumerableType.IsGenericType) &&
                     (enumerableType.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
            {
                elementType = enumerableType.GetGenericArguments().Single();
                enumeratorType = typeof(IEnumerator<>).MakeGenericType(elementType);
                enumeratorDisposable = true;
            }
            else
                throw new ArgumentException(
                    Resources.ExpressionExtensions_ForEach_SourceNotEnumerable,
                    "sourceEnumerable");

            Debug.Assert(elementType != null);

            MethodInfo getEnumeratorMethod = enumerableType.GetMethod(
                "GetEnumerator",
                BindingFlags.Public | BindingFlags.Instance);
            Debug.Assert(getEnumeratorMethod != null);

            PropertyInfo currentProperty = enumeratorType.GetProperty("Current", elementType);
            Debug.Assert(currentProperty != null);

            ParameterExpression enumerator = Expression.Variable(enumeratorType, "enumerator");
            if (@break == null)
                @break = Expression.Label();

            // ReSharper disable once AssignNullToNotNullAttribute
            Expression[] expressions = getBody(Expression.Property(enumerator, currentProperty)).ToArray();
            if (expressions.Length < 1) return Expression.Empty();

            Expression loopExpression = Expression.Loop(
                Expression.IfThenElse(
                    Expression.Call(enumerator, _enumeratorMoveNextMethod),
                    // ReSharper disable once AssignNullToNotNullAttribute
                    expressions.Length > 1 ? Expression.Block(expressions) : expressions.First(),
                    Expression.Break(@break)),
                @break,
                @continue);

            if (enumeratorDisposable)
                loopExpression =
                    Expression.TryFinally(
                        loopExpression,
                        Expression.Call(enumerator, _disposeMethod));

            return Expression.Block(
                new[] { enumerator },
                Expression.Assign(enumerator, Expression.Call(sourceEnumerable, getEnumeratorMethod)),
                loopExpression);
        }

        /// <summary>
        /// Takes an enumeration of expressions and locals, and returns the most compact single expression.
        /// </summary>
        /// <param name="expressions">The expressions.</param>
        /// <param name="locals">The locals.</param>
        /// <returns>A single expression</returns>
        [NotNull]
        public static Expression Blockify(
            [CanBeNull] this IEnumerable<Expression> expressions,
            [NotNull] IEnumerable<ParameterExpression> locals)
        {
            if (locals == null) throw new ArgumentNullException("locals");
            return expressions.Blockify(locals.ToArray());
        }

        /// <summary>
        /// Takes an enumeration of expressions (and optional a set of locals), and returns the most compact single expression.
        /// </summary>
        /// <param name="expressions">The expressions.</param>
        /// <param name="locals">The locals.</param>
        /// <returns>A single expression</returns>
        [NotNull]
        public static Expression Blockify(
            [CanBeNull] this IEnumerable<Expression> expressions,
            [CanBeNull] params ParameterExpression[] locals)
        {
            Expression[] e = (expressions ?? Enumerable.Empty<Expression>()).ToArray();
            if ((locals != null) &&
                (locals.Length > 0))
                return e.Length > 0
                    ? (Expression)Expression.Block(locals, e)
                    : Expression.Empty();
            // ReSharper disable once AssignNullToNotNullAttribute
            return e.Length > 1
                ? Expression.Block(e)
                : (e.Length > 0
                    ? e[0]
                    : Expression.Empty());
        }

        /// <summary>
        /// If the expression is a block that has no local variables, then it returns an enumeration of the inner
        /// expressions.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>IEnumerable{Expression}.</returns>
        [NotNull]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Expression> UnBlockify([NotNull] this Expression expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            return UnBlockifyIterator(expression);
        }

        [NotNull]
        private static IEnumerable<Expression> UnBlockifyIterator([NotNull] Expression expression)
        {
            BlockExpression block = expression as BlockExpression;
            // Check we have a block.
            if (block == null)
            {
                yield return expression;
                yield break;
            }

            Debug.Assert(block.Variables != null);
            Debug.Assert(block.Expressions != null);
            if (block.Variables.Count > 0)
            {
                // The block has local variables so we can't un-block it.
                yield return block;
                yield break;
            }
            foreach (Expression e in block.Expressions)
                yield return e;
        }

        /// <summary>
        /// If the expression is a block, then it returns an enumeration of the inner expressions and outputs any
        /// variables.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="variables">The variables.</param>
        /// <returns>
        /// IEnumerable{Expression}.
        /// </returns>
        [NotNull]
        public static IEnumerable<Expression> UnBlockify(
            [NotNull] this Expression expression,
            [NotNull] out IEnumerable<ParameterExpression> variables)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            BlockExpression block = expression as BlockExpression;
            if (block == null)
            {
                // We don't have a block.
                variables = Enumerable.Empty<ParameterExpression>();
                return new[] { expression };
            }
            Debug.Assert(block.Variables != null);
            Debug.Assert(block.Expressions != null);
            variables = block.Variables;
            return block.Expressions;
        }

        /// <summary>
        /// Adds the variables to an existing block.
        /// </summary>
        /// <param name="block">The block.</param>
        /// <param name="variables">The variables.</param>
        /// <returns>BlockExpression.</returns>
        [NotNull]
        public static Expression AddVariables(
            [NotNull] this Expression block,
            [NotNull] IEnumerable<ParameterExpression> variables)
        {
            if (block == null) throw new ArgumentNullException("block");
            if (variables == null) throw new ArgumentNullException("variables");

            return AddVariables(block, variables.ToArray());
        }

        /// <summary>
        /// Adds the variables to an existing block.
        /// </summary>
        /// <param name="block">The block.</param>
        /// <param name="variables">The variables.</param>
        /// <returns>BlockExpression.</returns>
        [NotNull]
        public static Expression AddVariables(
            [NotNull] this Expression block,
            [NotNull] params ParameterExpression[] variables)
        {
            if (block == null) throw new ArgumentNullException("block");
            if (variables == null) throw new ArgumentNullException("variables");

            ParameterExpression[] v = variables.ToArray();
            if (v.Length < 1)
                return block;

            BlockExpression b = block as BlockExpression;
            return b == null
                ? Expression.Block(variables, block)
                // ReSharper disable AssignNullToNotNullAttribute
                : Expression.Block(b.Variables.Concat(v), b.Expressions);
            // ReSharper restore AssignNullToNotNullAttribute
        }

        /// <summary>
        /// Adds the expressions to an existing block.
        /// </summary>
        /// <param name="block">The block.</param>
        /// <param name="expressions">The expressions.</param>
        /// <returns>BlockExpression.</returns>
        [NotNull]
        public static Expression AddExpressions(
            [NotNull] this Expression block,
            [NotNull] IEnumerable<Expression> expressions)
        {
            if (block == null) throw new ArgumentNullException("block");
            if (expressions == null) throw new ArgumentNullException("expressions");

            return block.AddExpressions(expressions.ToArray());
        }

        /// <summary>
        /// Adds the expressions to an existing block.
        /// </summary>
        /// <param name="block">The block.</param>
        /// <param name="expressions">The expressions.</param>
        /// <returns>BlockExpression.</returns>
        [NotNull]
        public static Expression AddExpressions(
            [NotNull] this Expression block,
            [NotNull] params Expression[] expressions)
        {
            if (block == null) throw new ArgumentNullException("block");
            if (expressions == null) throw new ArgumentNullException("expressions");

            if (expressions.Length < 1)
                return block;

            BlockExpression b = block as BlockExpression;
            return b == null
                ? Expression.Block(new[] { block }.Concat(expressions))
                // ReSharper disable once AssignNullToNotNullAttribute
                : Expression.Block(b.Variables, b.Expressions.Concat(expressions));
        }

        /// <summary>
        /// Gets the lambda expression as a strongly typed delegate.
        /// </summary>
        /// <typeparam name="TDelegate">The type of the delegate.</typeparam>
        /// <param name="expression">The lambda expression.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">expression;The expression does not have the same number of parameters as the delegate.</exception>
        [NotNull]
        public static Expression<TDelegate> GetDelegateExpression<TDelegate>([NotNull] this LambdaExpression expression)
        {
            if (!typeof(TDelegate).DescendsFrom(typeof(Delegate)))
                throw new ArgumentException(Resources.ExpressionExtensions_GetDelegateExpression_TypeMustBeDelegate);
            if (expression == null) throw new ArgumentNullException("expression");

            Type delegateType = typeof(TDelegate);
            MethodInfo delegateMethod = delegateType.GetMethod("Invoke");
            Debug.Assert(delegateMethod != null);

            ParameterInfo[] delegateParameters = delegateMethod.GetParameters();

            if (expression.Parameters.Count != delegateParameters.Length)
                throw new ArgumentOutOfRangeException(
                    "expression",
                    Resources.ExpressionExtensions_GetDelegateExpression_ParameterCountMismatch);

            ParameterExpression[] parameters = new ParameterExpression[delegateParameters.Length];

            for (int i = 0; i < delegateParameters.Length; i++)
            {
                ParameterInfo pi = delegateParameters[i];
                Debug.Assert(pi != null);

                parameters[i] = Expression.Parameter(pi.ParameterType, pi.Name);
            }

            // ReSharper disable once CoVariantArrayConversion
            Expression body = expression.Inline(parameters);

            if (delegateMethod.ReturnType != typeof(void) &&
                body.Type != delegateMethod.ReturnType)
                body = body.Convert(delegateMethod.ReturnType);

            return Expression.Lambda<TDelegate>(body, delegateType.Name, parameters);
        }

        /// <summary>
        /// Takes a lambda expression and returns new expression where the parameters have been replaced by the specified
        /// expressions, effectively inlining the expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="parameters">The replacement parameters.</param>
        /// <returns>Expression.</returns>
        [NotNull]
        public static Expression Inline(
            [NotNull] this LambdaExpression expression,
            [NotNull] params Expression[] parameters)
        {
            if (expression == null) throw new ArgumentNullException("expression");
            if (parameters == null) throw new ArgumentNullException("parameters");
            return new ParameterReplacerVisitor(expression, parameters).Visit();
        }

        /// <summary>
        /// Replaces the parameters of a lambda expression.
        /// </summary>
        private class ParameterReplacerVisitor : ExpressionVisitor
        {
            /// <summary>
            /// The lambda
            /// </summary>
            [NotNull]
            private readonly LambdaExpression _lambda;

            /// <summary>
            /// The replacements
            /// </summary>
            [NotNull]
            private readonly IReadOnlyDictionary<ParameterExpression, Expression> _replacements;

            /// <summary>
            /// Initializes a new instance of the <see cref="ParameterReplacerVisitor" /> class.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <param name="parameters">The parameters.</param>
            public ParameterReplacerVisitor(
                [NotNull] LambdaExpression expression,
                [NotNull] params Expression[] parameters)
            {
                if (expression == null) throw new ArgumentNullException("expression");
                if (parameters == null) throw new ArgumentNullException("parameters");

                _lambda = expression;
                int pcount = _lambda.Parameters.Count;

                if (pcount != parameters.Length)
                    throw new ArgumentOutOfRangeException(
                        "parameters",
                        string.Format(
                            // ReSharper disable once AssignNullToNotNullAttribute
                            Resources.ParameterReplacerVisitor_ExpressionCountMismatch,
                            parameters.Length,
                            pcount));

                Dictionary<ParameterExpression, Expression> replacements =
                    new Dictionary<ParameterExpression, Expression>(pcount);
                for (int i = 0; i < pcount; i++)
                {
                    ParameterExpression originalParameter = _lambda.Parameters[i];
                    Expression newParameter = parameters[i];
                    // ReSharper disable PossibleNullReferenceException
                    if (originalParameter.Type != newParameter.Type)
                        // ReSharper restore PossibleNullReferenceException
                        newParameter = newParameter.Convert(originalParameter.Type);

                    replacements.Add(originalParameter, newParameter);
                }
                _replacements = replacements;
            }

            /// <summary>
            /// Visits this instance.
            /// </summary>
            /// <returns>Expression.</returns>
            [NotNull]
            internal Expression Visit()
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                return Visit(_lambda.Body);
            }

            /// <summary>
            /// Visits the <see cref="T:System.Linq.Expressions.ParameterExpression" />.
            /// </summary>
            /// <param name="node">The expression to visit.</param>
            /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
            protected override Expression VisitParameter(ParameterExpression node)
            {
                Expression replacement;
                // ReSharper disable once AssignNullToNotNullAttribute
                return _replacements.TryGetValue(node, out replacement) ? replacement : node;
            }
        }

        /// <summary>
        /// Gets the debug view of the expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>The debug view for the expression.</returns>
        [NotNull]
        public static string GetDebugView([NotNull] this Expression expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            // ReSharper disable once AssignNullToNotNullAttribute
            return _expressionDebugView(expression);
        }

        /// <summary>
        /// Gets a CSharp like view of the given expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        [NotNull]
        public static string GetCSharpView([NotNull] this Expression expression)
        {
            return CSharpExpressionWriter.ToString(expression);
        }

        /// <summary>
        /// Gets the <see cref="ParameterExpression">ParameterExpressions</see> that are used within an expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        [NotNull]
        public static ParameterUsageVisitor GetParameterUsage([NotNull] this Expression expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");
            return ParameterUsageVisitor.Create(expression);
        }

        /// <summary>
        /// Determines whether <paramref name="expression" /> contains <paramref name="subExpression">the sub-expression</paramref>.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="subExpression">The sub expression.</param>
        /// <returns><see langword="true"/> if found; otherwise <see langword="false"/>.</returns>
        public static bool Contains(this Expression expression, Expression subExpression)
        {
            if (ReferenceEquals(expression, subExpression)) return true;
            if (ReferenceEquals(expression, null) ||
                ReferenceEquals(subExpression, null)) return false;
            return FinderExpressionVisitor.Contains(expression, subExpression, false);
        }

        /// <summary>
        /// Determines whether <paramref name="expression" /> uses <paramref name="subExpression">the sub-expression</paramref>.
        /// This will search everywhere except inside the signature of a <see cref="LambdaExpression"/>, which is useful for finding the
        /// usage of a parameter, without including the parameters of signatures.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="subExpression">The sub expression.</param>
        /// <returns><see langword="true"/> if found; otherwise <see langword="false"/>.</returns>
        public static bool Uses(this LambdaExpression expression, Expression subExpression)
        {
            if (ReferenceEquals(expression, subExpression)) return true;
            if (ReferenceEquals(expression, null) ||
                ReferenceEquals(subExpression, null)) return false;
            return FinderExpressionVisitor.Contains(expression, subExpression, true);
        }

        /// <summary>
        /// An <see cref="ExpressionVisitor"/> that looks for an expression in a tree.
        /// </summary>
        private class FinderExpressionVisitor : ExpressionVisitor
        {
            private readonly Expression _targetExpression;
            private readonly bool _skipLambdaSignatures;
            private bool _found;

            /// <summary>
            /// Initializes a new instance of the <see cref="FinderExpressionVisitor" /> class.
            /// </summary>
            /// <param name="targetExpression">The search expression.</param>
            /// <param name="skipLambdaSignatures">if set to <see langword="true" /> skip lambda signatures.</param>
            private FinderExpressionVisitor(Expression targetExpression, bool skipLambdaSignatures)
            {
                _targetExpression = targetExpression;
                _skipLambdaSignatures = skipLambdaSignatures;
            }

            /// <summary>
            /// Dispatches the expression to one of the more specialized visit methods in this class.
            /// </summary>
            /// <param name="node">The expression to visit.</param>
            /// <returns>
            /// The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.
            /// </returns>
            public override Expression Visit(Expression node)
            {
                if (ReferenceEquals(node, _targetExpression))
                    _found = true;

                return base.Visit(node);
            }

            /// <summary>
            /// Visits the lambda.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="node">The node.</param>
            /// <returns></returns>
            protected override Expression VisitLambda<T>(Expression<T> node)
            {
                if (!_skipLambdaSignatures)
                    return base.VisitLambda(node);
                Visit(node.Body);
                return node;
            }

            /// <summary>
            /// Determines whether [contains] [the specified expression].
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <param name="subExpression">The sub expression.</param>
            /// <param name="skipLambdaSignatures">if set to <see langword="true" /> skip lambda signatures.</param>
            /// <returns></returns>
            public static bool Contains(Expression expression, Expression subExpression, bool skipLambdaSignatures)
            {
                if (ReferenceEquals(expression, null) ||
                    ReferenceEquals(subExpression, null)) return false;
                FinderExpressionVisitor fev = new FinderExpressionVisitor(subExpression, skipLambdaSignatures);
                fev.Visit(expression);
                return fev._found;
            }
        }
    }
}