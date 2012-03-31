#region � Copyright Web Applications (UK) Ltd, 2011.  All rights reserved.
// Solution: WebApplications.Utilities 
// Project: WebApplications.Utilities
// File: Reflection.cs
// 
// This software, its object code and source code and all modifications made to
// the same (the �Software�) are, and shall at all times remain, the proprietary
// information and intellectual property rights of Web Applications (UK) Limited. 
// You are only entitled to use the Software as expressly permitted by Web
// Applications (UK) Limited within the Software Customisation and
// Licence Agreement (the �Agreement�).  Any copying, modification, decompiling,
// distribution, licensing, sale, transfer or other use of the Software other than
// as expressly permitted in the Agreement is expressly forbidden.  Web
// Applications (UK) Limited reserves its rights to take action against you and
// your employer in accordance with its contractual and common law rights
// (including injunctive relief) should you breach the terms of the Agreement or
// otherwise infringe its copyright or other intellectual property rights in the
// Software.
// 
// � Copyright Web Applications (UK) Ltd, 2011.  All rights reserved.
#endregion

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using WebApplications.Utilities.Relection;

namespace WebApplications.Utilities
{
    /// <summary>
    ///   Extensions to the <see cref="System.Reflection">reflection namespace</see>.
    /// </summary>
    public static partial class Reflection
    {
        /// <summary>
        ///   Binding flags for returning all fields/properties from a type.
        /// </summary>
        [UsedImplicitly]
        public const BindingFlags AccessorBindingFlags =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static |
            BindingFlags.FlattenHierarchy;

        /// <summary>
        ///   A cache for the getter methods, so that when requested they can be retrieved rather than recomputed.
        /// </summary>
        [NotNull]
        private static readonly ConcurrentDictionary<string, object> _getterCache =
            new ConcurrentDictionary<string, object>();

        /// <summary>
        ///   A cache for the setter methods, so that when requested they can be retrieved rather than recomputed.
        /// </summary>
        [NotNull]
        private static readonly ConcurrentDictionary<string, object> _setterCache =
            new ConcurrentDictionary<string, object>();

        /// <summary>
        ///   A cache for all methods so that when requested they can be retrieved rather than recomputed.
        /// </summary>
        [NotNull]
        private static readonly ConcurrentDictionary<string, object> _funcCache =
            new ConcurrentDictionary<string, object>();

        /// <summary>
        ///   A cache for all actions so that when requested they can be retrieved rather than recomputed.
        /// </summary>
        [NotNull]
        private static readonly ConcurrentDictionary<string, object> _actionCache =
            new ConcurrentDictionary<string, object>();

        /// <summary>
        ///   A cache for the constructors, so that when requested they can be retrieved rather than recomputed.
        /// </summary>
        [NotNull]
        private static readonly ConcurrentDictionary<string, object> _constructorCache =
            new ConcurrentDictionary<string, object>();

        /// <summary>
        ///   Retrieves the lambda function equivalent of the specified getter method.
        /// </summary>
        /// <typeparam name="T">The type of the parameter the function encapsulates.</typeparam>
        /// <typeparam name="TValue">The type of the value returned.</typeparam>
        /// <param name="name">The name of the field or property whose getter we want to retrieve.</param>
        /// <param name="checkAssignability">
        ///   If set to <see langword="true"/> performs assignability checks.
        /// </param>
        /// <returns>
        ///   A function that takes an object of the type T and returns the value of the property or field.
        /// </returns>
        [UsedImplicitly]
        [NotNull]
        public static Func<T, TValue> GetGetter<T, TValue>([NotNull]string name, bool checkAssignability = false)
        {
            return (Func<T, TValue>)GetGetter(typeof(T), name, null, typeof(TValue), checkAssignability);
        }

        /// <summary>
        ///   Retrieves the lambda function equivalent of the specified getter method.
        /// </summary>
        /// <param name="declaringType">The declaring class' type.</param>
        /// <param name="name">The name of the field or property whose getter we want.</param>
        /// <param name="parameterType">
        ///   <para>The type of the parameter.</para>
        ///   <para>By default this is the <paramref name="declaringType"/>.</para>
        /// </param>
        /// <param name="returnType">
        ///   <para>The return value's type.</para>
        ///   <para>By default this is the member (the field or property) type.</para>
        /// </param>
        /// <param name="checkAssignability">
        ///   If <see langword="true"/> this checks that the member type is assignable to the <paramref name="returnType"/>
        ///   and that the parameter type is assignable from the <paramref name="declaringType"/>.
        /// </param>
        /// <returns>
        ///   A function that takes an object of the <paramref name="declaringType"/> and returns the value of the property or field.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <para>There is no getter for the field or property specified.</para>
        ///   <para>-or-</para>
        ///   <para><paramref name="parameterType"/> is not assignable from <paramref name="declaringType"/>.</para>
        ///   <para>-or-</para>
        ///   <para>The <paramref name="returnType"/> is not assignable from the member type.</para>
        /// </exception>
        /// <seealso cref="System.Type.IsAssignableFrom"/>
        [UsedImplicitly]
        [NotNull]
        public static object GetGetter(
            [NotNull] this Type declaringType,
            [NotNull] string name,
            [CanBeNull] Type parameterType = null,
            [CanBeNull] Type returnType = null,
            bool checkAssignability = false)
        {
            return
                _getterCache.GetOrAdd(
                    String.Format("{0}:{1}|{2}|{3}", declaringType, name, parameterType, returnType),
                    k =>
                    {
                        // Find the field or property
                        bool isField;
                        bool isStatic;
                        Type memberType;
                        FieldInfo fieldInfo = declaringType.GetField(name, AccessorBindingFlags);
                        MethodInfo propertyAccesssor;
                        if (fieldInfo != null)
                        {
                            memberType = fieldInfo.FieldType;
                            propertyAccesssor = null;
                            isStatic = fieldInfo.IsStatic;
                            isField = true;
                        }
                        else
                        {
                            PropertyInfo propertyInfo = declaringType.GetProperty(
                                name, AccessorBindingFlags);
                            if ((propertyInfo == null) ||
                                ((propertyAccesssor = propertyInfo.GetGetMethod(true)) == null))
                            {
                                throw new ArgumentOutOfRangeException(
                                    "name",
                                    String.Format(
                                        Resources.Reflection_GetGetter_NoGetterForFieldOrProperty,
                                        name,
                                        declaringType));
                            }
                            memberType = propertyInfo.PropertyType;
                            isStatic = propertyAccesssor.IsStatic;
                            isField = false;
                        }

                        //  Check the parameter type can be assigned from the declaring type.
                        if (parameterType != null)
                        {
                            if ((checkAssignability) && (parameterType != declaringType) &&
                                (!parameterType.IsAssignableFrom(declaringType)))
                            {
                                throw new ArgumentOutOfRangeException(
                                    "parameterType",
                                    String.Format(
                                        Resources.Reflection_GetGetter_ParameterTypeNotAssignable,
                                        name,
                                        isField ? "field" : "property",
                                        declaringType,
                                        parameterType));
                            }
                        }
                        else
                            parameterType = declaringType;

                        // Check the return type can be assigned from the member type
                        if (returnType != null)
                        {
                            if ((checkAssignability) && (returnType != memberType) &&
                                (!returnType.IsAssignableFrom(memberType)))
                            {
                                throw new ArgumentOutOfRangeException(
                                    "returnType",
                                    String.Format(
                                        Resources.Reflection_GetGetter_ReturnTypeNotAssignable,
                                        name,
                                        isField ? "field" : "property",
                                        declaringType,
                                        memberType,
                                        returnType));
                            }
                        }
                        else
                            returnType = memberType;

                        Expression expression = null;
                        // Create input parameter expression
                        ParameterExpression parameterExpression = Expression.Parameter(
                            parameterType, "target");

                        // If we're not static we need a parameter expression
                        if (!isStatic)
                        {
                            expression = parameterExpression;

                            // Cast parameter if necessary
                            if (parameterType != declaringType)
                                expression = Expression.Convert(expression, declaringType);
                        }

                        // Get a member access expression
                        expression = isField
                                         ? Expression.Field(expression, fieldInfo)
                                         : Expression.Property(expression, propertyAccesssor);

                        // Cast return value if necessary
                        if (returnType != memberType)
                            expression = Expression.Convert(expression, returnType);

                        // Create lambda and compile
                        return Expression.Lambda(expression, parameterExpression).Compile();
                    });
        }

        /// <summary>
        ///   Retrieves the lambda function equivalent of the specified setter method.
        /// </summary>
        /// <typeparam name="T">The declaring class' type.</typeparam>
        /// <typeparam name="TValue">The type of the value returned.</typeparam>
        /// <param name="name">The name of the field or property whose setter we want to retrieve.</param>
        /// <param name="checkAssignability">If set to <see langword="true"/> performs assignability checks.</param>
        /// <returns>
        ///   A function that takes an object of the declaring type as well as a value and sets the field or property to that value
        ///   (and returns the new value).
        /// </returns>
        [UsedImplicitly]
        [NotNull]
        public static Func<T, TValue, TValue> GetSetter<T, TValue>([NotNull] string name, bool checkAssignability = false)
        {
            return
                (Func<T, TValue, TValue>)GetSetter(typeof(T), name, null, null, typeof(TValue), checkAssignability);
        }

        /// <summary>
        ///   Retrieves the lambda function equivalent of the specified setter method.
        /// </summary>
        /// <param name="declaringType">The declaring class' type.</param>
        /// <param name="name">The name of the field or property.</param>
        /// <param name="parameterType">
        ///   <para>The type of the parameter.</para>
        ///   <para>By default this is the <paramref name="declaringType"/>.</para>
        /// </param>
        /// <param name="valueType">
        ///   <para>The value's type.</para>
        ///   <para>By default this is the member (the field or property) type.</para>
        /// </param>
        /// <param name="returnType">
        ///   <para>The return value's type.</para>
        ///   <para>By default this is the member (the field or property) type.</para>
        /// </param>
        /// <param name="checkAssignability">
        ///   If set to <see langword="true"/> checks if the member type is assignable from the <paramref name="valueType"/>,
        ///   the <paramref name="returnType"/> is assignable from the member type and that the <paramref name="parameterType"/>
        ///   is assignable from the <paramref name="declaringType"/>.
        /// </param>
        /// <returns>
        ///   A function that takes an object of the <paramref name="declaringType"/> as well as a value and sets the field
        ///   or property to that value (and returns the new value).
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <para>There is no setter for the field or property specified.</para>
        ///   <para>-or-</para>
        ///   <para><paramref name="parameterType"/> is not assignable from <paramref name="declaringType"/>.</para>
        ///   <para>-or-</para>
        ///   <para>The member type is not assignable from <paramref name="valueType"/>.</para>
        ///   <para>-or-</para>
        ///   <para><paramref name="returnType"/> is not assignable from the member type.</para>
        /// </exception>
        /// <seealso cref="System.Type.IsAssignableFrom"/>
        [UsedImplicitly]
        [NotNull]
        public static object GetSetter(
            [NotNull] this Type declaringType,
            [NotNull] string name,
            [CanBeNull] Type parameterType = null,
            [CanBeNull] Type valueType = null,
            [CanBeNull] Type returnType = null,
            bool checkAssignability = false)
        {
            return
                _setterCache.GetOrAdd(
                    String.Format(
                        "{0}:{1}|{2}|{3}|{4}", declaringType, name, parameterType, valueType, returnType),
                    k =>
                    {
                        // Find the field or property
                        bool isField;
                        bool isStatic;
                        Type memberType;
                        FieldInfo fieldInfo = declaringType.GetField(name, AccessorBindingFlags);
                        MethodInfo propertyAccesssor;
                        if (fieldInfo != null)
                        {
                            memberType = fieldInfo.FieldType;
                            propertyAccesssor = null;
                            isStatic = fieldInfo.IsStatic;
                            isField = true;
                        }
                        else
                        {
                            PropertyInfo propertyInfo = declaringType.GetProperty(
                                name, AccessorBindingFlags);
                            if ((propertyInfo == null) ||
                                ((propertyAccesssor = propertyInfo.GetSetMethod(true)) == null))
                            {
                                throw new ArgumentOutOfRangeException(
                                    "name",
                                    String.Format(
                                        Resources.Reflection_GetSetter_NoSetterForFieldOrProperty,
                                        name,
                                        declaringType));
                            }
                            memberType = propertyInfo.PropertyType;
                            isStatic = propertyAccesssor.IsStatic;
                            isField = false;
                        }

                        //  Check the parameter type can be assigned from the declaring type.
                        if (parameterType != null)
                        {
                            if ((checkAssignability) && (parameterType != declaringType) &&
                                (!parameterType.IsAssignableFrom(declaringType)))
                            {
                                throw new ArgumentOutOfRangeException(
                                    "parameterType",
                                    String.Format(
                                        Resources.Reflection_GetSetter_ParameterTypeNotAssignable,
                                        name,
                                        isField ? "field" : "property",
                                        declaringType,
                                        parameterType));
                            }
                        }
                        else
                            parameterType = declaringType;

                        // Check the member type can be assigned from the value type
                        if (valueType != null)
                        {
                            if ((checkAssignability) && (valueType != memberType) &&
                                (!memberType.IsAssignableFrom(valueType)))
                            {
                                throw new ArgumentOutOfRangeException(
                                    "valueType",
                                    String.Format(
                                        Resources.Reflection_GetSetter_MemberTypeNotAssignable,
                                        name,
                                        isField ? "field" : "property",
                                        declaringType,
                                        memberType,
                                        valueType));
                            }
                        }
                        else
                            valueType = memberType;

                        // Check the return type can be assigned from the member type
                        if (returnType != null)
                        {
                            if ((checkAssignability) && (returnType != memberType) &&
                                (!returnType.IsAssignableFrom(memberType)))
                            {
                                throw new ArgumentOutOfRangeException(
                                    "returnType",
                                    String.Format(
                                        Resources.Reflection_GetSetter_ReturnTypeNotAssignable,
                                        name,
                                        isField ? "field" : "property",
                                        declaringType,
                                        memberType,
                                        returnType));
                            }
                        }
                        else
                            returnType = memberType;

                        Expression expression = null;
                        // Create input parameter expression
                        ParameterExpression parameterExpression = Expression.Parameter(
                            parameterType, "target");
                        ParameterExpression valueParameterExpression = Expression.Parameter(
                            valueType, "value");

                        // If we're not static we need a parameter expression
                        if (!isStatic)
                        {
                            expression = parameterExpression;

                            // Cast parameter if necessary
                            if (parameterType != declaringType)
                                expression = Expression.Convert(expression, declaringType);
                        }

                        // Cast the value parameter if necessary
                        Expression valueExpression = valueParameterExpression;
                        if (valueType != memberType)
                            valueExpression = Expression.Convert(valueExpression, memberType);

                        // Get a member access expression
                        expression = isField
                                         ? Expression.Field(expression, fieldInfo)
                                         : Expression.Property(expression, propertyAccesssor);

                        // Perform assignment
                        expression = Expression.Assign(expression, valueExpression);

                        // Cast return value if necessary
                        if (returnType != memberType)
                            expression = Expression.Convert(expression, returnType);

                        // Create lambda and compile
                        return
                            Expression.Lambda(expression, parameterExpression, valueParameterExpression)
                                .Compile();
                    });
        }

        /// <summary>
        ///   Gets the lambda function equivalent of a method, for much better runtime performance than an invocation.
        /// </summary>
        /// <param name="methodBase">The information about the method.</param>
        /// <param name="checkParameterAssignability">
        ///   If set to <see langword="true"/> checks that the function parameters can be assigned to the method safely.
        /// </param>
        /// <param name="funcTypes">The parameter types, followed by the return type.</param>
        /// <returns>
        ///   A functional equivalent of the specified method info.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="methodBase"/> is a <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <para>The method specified is a static constructor.</para>
        ///   <para>-or-</para>
        ///   <para>No parameter/return types specified.</para>
        ///   <para>-or-</para>
        ///   <para>The method specified doesn't return a value. (An Action should be used instead.)</para>
        ///   <para>-or-</para>
        ///   <para>The number of parameters specified in <paramref name="funcTypes"/> is incorrect.</para>
        ///   <para>-or-</para>
        ///   <para>The parameter type is not assignable to the type specified.</para>
        ///   <para>-or-</para>
        ///   <para>The return type is not assignable to the type specified.</para>
        /// </exception>
        /// <exception cref="InvalidOperationException">No parameter/return types specified.</exception>
        [NotNull]
        [UsedImplicitly]
        public static object GetFunc(
            [NotNull] this MethodBase methodBase,
            bool checkParameterAssignability,
            [NotNull] params Type[] funcTypes)
        {
            if (methodBase == null)
                throw new ArgumentNullException("methodBase");

            return
                _funcCache.GetOrAdd(
                    String.Format(
                        "{0}:{1}|{2}",
                        methodBase.DeclaringType,
                        methodBase,
                        String.Join("|", (IEnumerable<Type>)funcTypes)),
                    mb =>
                    {
                        bool isConstructor = methodBase.IsConstructor;
                        bool isStatic = methodBase.IsStatic;
                        MethodInfo methodInfo;
                        ConstructorInfo constructorInfo;
                        if (isConstructor)
                        {
                            // Cannot support static constructors (cannot be called directly!)
                            if (isStatic)
                            {
                                throw new ArgumentOutOfRangeException(
                                    "methodBase",
                                    String.Format(
                                        Resources.Reflection_GetFunc_MethodIsStaticConstructor,
                                        methodBase));
                            }

                            constructorInfo = methodBase as ConstructorInfo;
                            methodInfo = null;
                        }
                        else
                        {
                            methodInfo = methodBase as MethodInfo;
                            constructorInfo = null;
                            if ((methodInfo == null) ||
                                (methodInfo.ReturnType == typeof(void)))
                            {
                                throw new ArgumentOutOfRangeException(
                                    "methodBase",
                                    String.Format(
                                        Resources.Reflection_GetFunc_MethodHasNoReturnType,
                                        methodBase));
                            }
                        }

                        int count = funcTypes.Count();
                        if (count < 1)
                        {
                            throw new ArgumentOutOfRangeException(
                                "funcTypes",
                                String.Format(
                                    Resources.Reflection_GetFunc_NoFuncTypesSpecified,
                                    methodBase));
                        }

                        // Validate method info
                        ParameterInfo[] parameterInfos = methodBase.GetParameters();
                        List<Type> methodTypes = new List<Type>(count);

                        // If we're not static and not a constructor, the first parameter is implicitly the declaring type of the method.
                        if (!isStatic &&
                            !isConstructor)
                            methodTypes.Add(methodBase.DeclaringType);
                        // ReSharper disable PossibleNullReferenceException
                        methodTypes.AddRange(parameterInfos.Select(pi => pi.ParameterType));
                        // ReSharper restore PossibleNullReferenceException

                        // Finally add return type - in the case of a constructor, this is the object type.
                        methodTypes.Add(isConstructor ? methodBase.ReflectedType : methodInfo.ReturnType);

                        if (methodTypes.Count() != count)
                        {
                            throw new ArgumentOutOfRangeException(
                                "methodBase",
                                String.Format(
                                    Resources.Reflection_GetFunc_IncorrectParameterCount,
                                    methodBase));
                        }

                        // Create expressions
                        ParameterExpression[] parameterExpressions = new ParameterExpression[count - 1];
                        List<Expression> pExpressions = new List<Expression>(count - 1);
                        for (int i = 0; i < count; i++)
                        {
                            Type funcType = funcTypes[i];
                            Type methodType = methodTypes[i];

                            Expression expression;
                            // Create parameter expressions for all
                            if (i < count - 1)
                            {
                                // Check assignability
                                if (checkParameterAssignability && !methodType.IsAssignableFrom(funcType))
                                {
                                    throw new ArgumentOutOfRangeException(
                                        "methodBase",
                                        String.Format(
                                            Resources.Reflection_GetFunc_ParameterNotAssignable,
                                            methodBase,
                                            funcType,
                                            methodType));
                                }

                                // Create parameter expression
                                expression = parameterExpressions[i] = Expression.Parameter(funcType);

                                // Check if we need to do a cast to the method type
                                if (funcType != methodType)
                                    expression = Expression.Convert(expression, methodType);

                                pExpressions.Add(expression);
                                continue;
                            }

                            // Check assignability
                            if (checkParameterAssignability && !funcType.IsAssignableFrom(methodType))
                            {
                                throw new ArgumentOutOfRangeException(
                                    "methodBase",
                                    String.Format(
                                        Resources.Reflection_GetFunc_ReturnTypeNotAssignable,
                                        methodBase,
                                        methodType,
                                        funcType));
                            }

                            if (isConstructor)
                            {
                                // We are a constructor so use the New expression.
                                expression = Expression.New(constructorInfo, pExpressions);
                            }
                            else
                            {
                                // Create call expression, instance methods use the first parameter of the Func<> as the instance, static
                                // methods do not supply an instance.
                                expression = isStatic
                                                 ? Expression.Call(methodInfo, pExpressions)
                                                 : Expression.Call(
                                                     pExpressions[0],
                                                     methodInfo,
                                                     pExpressions.Skip(1));
                            }

                            // Check if we need to do a cast to the func result type
                            if (funcType != methodType)
                                expression = Expression.Convert(expression, funcType);

                            return Expression.Lambda(expression, parameterExpressions).Compile();
                        }
                        // Sanity check, shouldn't be able to get here anyway.
                        throw new InvalidOperationException(
                            String.Format(
                                Resources.Reflection_GetFunc_NoFuncTypesSpecified,
                                methodBase));
                    });
        }

        /// <summary>
        ///   Gets the lambda action equivalent of a method, for much better runtime performance than an invocation.
        /// </summary>
        /// <param name="methodInfo">The information about the method.</param>
        /// <param name="checkParameterAssignability">
        ///   If set to <see langword="true"/> checks that the function parameters can be assigned to the method safely.
        /// </param>
        /// <param name="paramTypes">The parameter types.</param>
        /// <returns>
        ///   A functional equivalent of the specified method info.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="methodInfo"/> is a <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <para>The number of parameters specified in <paramref name="paramTypes"/> is incorrect.</para>
        ///   <para>-or-</para>
        ///   <para>The parameter type is not assignable to the type specified.</para>
        /// </exception>
        [UsedImplicitly]
        [NotNull]
        public static object GetAction(
            [NotNull]this MethodInfo methodInfo,
            bool checkParameterAssignability,
            [NotNull]params Type[] paramTypes)
        {
            if (methodInfo == null)
                throw new ArgumentNullException("methodInfo");

            return
                _actionCache.GetOrAdd(
                    String.Format(
                        "{0}:{1}|{2}",
                        methodInfo.DeclaringType,
                        methodInfo,
                        String.Join("|", (IEnumerable<Type>)paramTypes)),
                    mi =>
                    {
                        bool isStatic = methodInfo.IsStatic;

                        int count = paramTypes.Count();

                        // Validate method info
                        ParameterInfo[] parameterInfos = methodInfo.GetParameters();
                        List<Type> methodTypes = new List<Type>(count);

                        // If we're not static the first parameter is implicitly the declaring type of the method.
                        if (!isStatic)
                            methodTypes.Add(methodInfo.DeclaringType);
                        methodTypes.AddRange(parameterInfos.Select(pi => pi.ParameterType));

                        if (methodTypes.Count() != count)
                        {
                            throw new ArgumentOutOfRangeException(
                                "methodInfo",
                                String.Format(
                                    Resources.Reflection_GetAction_IncorrectParameterCount,
                                    methodInfo));
                        }

                        // Create expressions
                        ParameterExpression[] parameterExpressions = new ParameterExpression[count];
                        List<Expression> pExpressions = new List<Expression>(count);

                        Expression expression;
                        for (int i = 0; i < count; i++)
                        {
                            Type funcType = paramTypes[i];
                            Type methodType = methodTypes[i];
                            // Check assignability
                            if (checkParameterAssignability && !methodType.IsAssignableFrom(funcType))
                            {
                                throw new ArgumentOutOfRangeException(
                                    "methodInfo",
                                    String.Format(
                                        Resources.Reflection_GetAction_ParameterNotAssignable,
                                        methodInfo,
                                        funcType,
                                        methodType));
                            }

                            // Create parameter expression
                            expression = parameterExpressions[i] = Expression.Parameter(funcType);

                            // Check if we need to do a cast to the method type
                            if (funcType != methodType)
                                expression = Expression.Convert(expression, methodType);

                            pExpressions.Add(expression);
                        }


                        // Create call expression, instance methods use the first parameter of the action as the instance, static
                        // methods do not supply an instance.
                        expression = isStatic
                                         ? Expression.Call(methodInfo, pExpressions)
                                         : Expression.Call(
                                             pExpressions[0], methodInfo, pExpressions.Skip(1));
                        return
                            Expression.Lambda(Expression.Block(expression), parameterExpressions).
                                Compile();
                    });
        }

        /// <summary>
        ///   Gets the lambda function equivalent of a constructor, for much better runtime performance than an invocation.
        /// </summary>
        /// <param name="type">The object type.</param>
        /// <param name="checkParameterAssignability">
        ///   If set to <see langword="true"/> checks that the function parameters can be assigned to the method safely.
        /// </param>
        /// <param name="paramTypes">The parameter types.</param>
        /// <returns>
        ///   A functional equivalent of a constructor.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="type"/> is a <see langword="null"/>.
        /// </exception>
        [NotNull]
        [UsedImplicitly]
        public static object GetConstructorFunc(
            [NotNull]this Type type,
            bool checkParameterAssignability,
            [NotNull]params Type[] paramTypes)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            return
                _constructorCache.GetOrAdd(
                    String.Format("{0}:{1}", type, String.Join("|", (IEnumerable<Type>)paramTypes)),
                    k =>
                    GetFunc(
                        type.GetConstructor(
                            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
                            BindingFlags.CreateInstance,
                            null,
                            paramTypes.Take(paramTypes.Count() - 1).ToArray(),
                            null),
                        checkParameterAssignability,
                        paramTypes));
        }

        /// <summary>
        ///   Determines whether or not the specified <see cref="PropertyInfo">property</see> is automatic.
        /// </summary>
        /// <param name="property">The property we want to look at.</param>
        /// <returns>
        ///   Returns <see langword="true"/> if the specified property is automatic; otherwise <see langword="false"/>.
        /// </returns>
        [UsedImplicitly]
        public static bool IsAutomatic([NotNull]this PropertyInfo property)
        {
            return GetAutomaticFieldInfo(property) != null;
        }

        /// <summary>
        ///   Determines whether or not the specified <see cref="PropertyInfo">property</see> is automatic
        ///   and returns its underlying field.
        /// </summary>
        /// <param name="property">The property we want to look at.</param>
        /// <returns>
        ///   Returns the field info if the property is automatic; otherwise returns null.
        /// </returns>
        [UsedImplicitly]
        [CanBeNull]
        public static FieldInfo GetAutomaticFieldInfo([NotNull]this PropertyInfo property)
        {
            MethodInfo getMethod;
            MethodBody methodBody;

            // If the get/set accessor is missing or we can't retrieve the method body for the get accessor,
            // then we're not an automatic property.
            if (!property.CanRead || !property.CanWrite || ((getMethod = property.GetGetMethod()) == null) ||
                ((methodBody = getMethod.GetMethodBody()) == null))
                return null;

            // Evaluate MSIL to resolve underlying field that is accessed.
            byte[] getter = methodBody.GetILAsByteArray();
            byte ldfld = (byte)(property.GetGetMethod().IsStatic ? OpCodes.Ldsfld : OpCodes.Ldfld).Value;
            byte[] fieldToken = getter.SkipWhile(b => b != ldfld).Skip(1).Take(4).ToArray();
            if (fieldToken.Length != 4)
                return null;

            // Grab the field
            FieldInfo field;
            try
            {
                field = property.DeclaringType.Module.ResolveField(BitConverter.ToInt32(fieldToken, 0));
            }
            catch
            {
                return null;
            }

            // Compilers don't strictly have to add this attribute, so could relax this check, but this ensures
            // that we are indeed looking at an automatic property.
            return field != null && field.IsDefined(typeof(CompilerGeneratedAttribute), false) ? field : null;
        }

        /// <summary>
        ///   Gets the conversion from the input type to the output type as a lambda expression.
        /// </summary>
        /// <typeparam name="TIn">The type to convert from.</typeparam>
        /// <typeparam name="TOut">The type to convert to.</typeparam>
        /// <returns>A lambda expression for converting (if any); otherwise returns <see langword="null"/>.</returns>
        /// <remarks>
        ///   Conversions are cached, which saves a conversion being recomputed if it's requested more than once.
        /// </remarks>
        [UsedImplicitly]
        public static Func<TIn, TOut> GetConversion<TIn, TOut>()
        {
            return GetConversion<TIn, TOut>(typeof(TIn));
        }

        /// <summary>
        /// Gets the conversion from the input type to the output type as a lambda expression.
        /// </summary>
        /// <returns>A lambda expression for converting (if any); otherwise returns <see langword="null"/>.</returns>
        /// <remarks>
        ///   Conversions are cached, which saves a conversion being recomputed if it's requested more than once.
        /// </remarks>
        [UsedImplicitly]
        public static Func<object, object> GetConversion()
        {
            return GetConversion<object, object>(typeof(object));
        }

        /// <summary>
        ///   Gets the conversion from the input type to the output type as a lambda expression.
        /// </summary>
        /// <param name="inputType">The type to convert from.</param>
        /// <param name="outputType">The type to convert to.</param>
        /// <returns>A lambda expression for converting between the two types (if any); otherwise returns <see langword="null"/>.</returns>
        /// <remarks>
        ///   Conversions are cached, which saves a conversion being recomputed if it's requested more than once.
        /// </remarks>
        [UsedImplicitly]
        public static Func<object, object> GetConversion(this Type inputType, Type outputType = null)
        {
            return GetConversion<object, object>(inputType, outputType);
        }

        /// <summary>
        /// Cache the conversions so that when requested they can be retrieved rather than recomputed.
        /// </summary>
        private readonly static ConcurrentDictionary<string, object> _converters = new ConcurrentDictionary<string, object>();

        /// <summary>
        ///   Gets the conversion from the input type to the output type as a lambda expression.
        /// </summary>
        /// <typeparam name="TIn">The type of the input for the function.</typeparam>
        /// <typeparam name="TOut">The type of the output for the function.</typeparam>
        /// <param name="inputType">The type to convert from.</param>
        /// <param name="outputType">The type to convert to.</param>
        /// <returns>A lambda expression for converting (if any); otherwise null.</returns>
        /// <remarks>
        ///   Conversions are cached, which saves a conversion being recomputed if it's requested more than once.
        /// </remarks>
        [UsedImplicitly]
        public static Func<TIn, TOut> GetConversion<TIn, TOut>([NotNull]this Type inputType, Type outputType = null)
        {
            if (outputType == null)
                outputType = typeof(TOut);

            return (Func<TIn, TOut>)_converters.GetOrAdd(
                string.Format("{0}|{1}|{2}|{3}",
                              typeof(TIn).FullName,
                              inputType.FullName,
                              outputType.FullName,
                              typeof(TOut).FullName),
                k =>
                {
                    // Build the expression as a series of conversions.
                    ParameterExpression parameterExpression = Expression.Parameter(typeof(TIn), "inputValue");
                    Expression body = parameterExpression;
                    return !body.TryConvert(inputType, out body) ||
                           !body.TryConvert(outputType, out body) ||
                           !body.TryConvert(typeof(TOut), out body)
                               ? (object)null
                               : Expression.Lambda<Func<TIn, TOut>>(Expression.Block(body), parameterExpression)
                                     .Compile();
                });
        }

        /// <summary>
        /// The <see cref="MethodInfo"/> for <see cref="object.ToString()"/>.
        /// </summary>
        [NotNull]
        private static readonly MethodInfo _toStringMethodInfo = typeof(object).GetMethod("ToString",
                                                               BindingFlags.Instance | BindingFlags.Public);

        /// <summary>
        ///   The <see cref="Expression"/> to get the <see cref="CultureInfo.CurrentCulture">current CultureInfo</see>.
        /// </summary>
        [NotNull]
        private static readonly Expression _currentCultureExpression =
            Expression.Call(
            typeof(CultureInfo).GetProperty("CurrentCulture", BindingFlags.Static | BindingFlags.Public).
          GetGetMethod());

        /// <summary>
        ///   The standard conversion methods implemented by <see cref="System.IConvertible"/>.
        /// </summary>
        /// <remarks>
        ///   Does not include:
        ///   <list type="bullet">
        ///     <item><description><see cref="System.IConvertible.ToType">ToType</see> - It isn't specific.</description></item>
        ///     <item><description><see cref="System.IConvertible.GetTypeCode">GetTypeCode</see> - Isn't actually a conversion method.</description></item>
        ///   </list>
        /// </remarks>
        private static readonly Dictionary<Type, string> _iConvertibleMethods =
            new Dictionary<Type, string>
                {
                    {typeof (bool), "ToBoolean"},
                    {typeof (char), "ToChar"},
                    {typeof (sbyte), "ToSByte"},
                    {typeof (byte), "ToByte"},
                    {typeof (short), "ToInt16"},
                    {typeof (ushort), "ToUInt16"},
                    {typeof (int), "ToInt32"},
                    {typeof (uint), "ToUInt32"},
                    {typeof (long), "ToInt64"},
                    {typeof (ulong), "ToUInt64"},
                    {typeof (float), "ToSingle"},
                    {typeof (double), "ToDouble"},
                    {typeof (decimal), "ToDecimal"},
                    {typeof (DateTime), "ToDateTime"},
                    {typeof (string), "ToString"}
                };

        /// <summary>
        ///   Converts the specified expression to the output type.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="outputType">The type of the output.</param>
        /// <returns>An expression where the type has been converted.</returns>
        /// <remarks>
        ///   This version is more powerful than the Expression.Convert CLR method in that it supports
        ///   ToString() conversion, IConvertible and TypeConverters.
        /// </remarks>
        /// <exception cref="InvalidOperationException">The conversion is not supported.</exception>
        [NotNull]
        [UsedImplicitly]
        public static Expression Convert([NotNull]this Expression expression, [NotNull]Type outputType)
        {
            Expression outputExpression;
            if (!TryConvert(expression, outputType, out outputExpression))
                throw new InvalidOperationException(
                    string.Format(Resources.Reflection_Convert_ConversionFailed,
                                  expression.Type,
                                  outputType));
            return outputExpression;
        }

        /// <summary>
        ///   Converts the specified expression to the output type.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="outputType">The type of the output.</param>
        /// <param name="outputExpression">The output expression.</param>
        /// <returns>
        ///   Returns <see langword="true"/> if the conversion succeeded; otherwise returns <see langword="false"/>.
        /// </returns>
        /// <remarks>
        ///   This version is more powerful than the Expression.Convert CLR method in that it supports
        ///   ToString() conversion, IConvertible and TypeConverters. It also prevents exceptions being thrown.
        /// </remarks>
        [UsedImplicitly]
        public static bool TryConvert([NotNull]this Expression expression, [NotNull]Type outputType, [NotNull]out Expression outputExpression)
        {
            // If the types are the same we don't need to convert.
            if (expression.Type == outputType)
            {
                outputExpression = expression;
                return true;
            }

            try
            {
                // Try creating conversion.
                outputExpression = Expression.Convert(expression, outputType);
                return true;
            }
            catch (InvalidOperationException)
            {
                // Ignore failures due to lack of coercion operator.
            }

            // Look for IConvertible method
            string method;
            if ((_iConvertibleMethods.TryGetValue(outputType, out method)) &&
                (expression.Type.GetInterface("IConvertible") != null))
            {
                MethodInfo mi = expression.Type.GetMethod(
                    method,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy,
                    null,
                    new[] {typeof (IFormatProvider)},
                    null);
                if (mi != null)
                {
                    // Call the IConvertible method on the object, passing in CultureInfo.CurrentCulture as the parameter.
                    outputExpression = Expression.Call(expression, mi, _currentCultureExpression);
                    return true;
                }
            }

            /*
             * TypeConverter support
             */

            // Look for TypeConverter on output type.
            bool useTo = false;
            TypeConverterAttribute typeConverterAttribute = outputType
                .GetCustomAttributes(typeof (TypeConverterAttribute), false)
                .OfType<TypeConverterAttribute>()
                .FirstOrDefault();

            if ((typeConverterAttribute == null) ||
                (string.IsNullOrWhiteSpace(typeConverterAttribute.ConverterTypeName)))
            {
                // Look for TypeConverter on expression type.
                useTo = true;
                typeConverterAttribute = expression.Type
                    .GetCustomAttributes(typeof (TypeConverterAttribute), false)
                    .OfType<TypeConverterAttribute>()
                    .FirstOrDefault();
            }

            if ((typeConverterAttribute != null) &&
                (!string.IsNullOrWhiteSpace(typeConverterAttribute.ConverterTypeName)))
            {
                try
                {
                    // Try to get the type for the typeconverter
                    Type typeConverterType = Type.GetType(typeConverterAttribute.ConverterTypeName);

                    if (typeConverterType != null)
                    {
                        // Try to create an instance of the typeconverter without parameters
                        TypeConverter converter = Activator.CreateInstance(typeConverterType) as TypeConverter;
                        if ((converter != null) &&
                            (useTo ? converter.CanConvertTo(outputType) : converter.CanConvertFrom(expression.Type)))
                        {
                            // We have a converter that supports the necessary conversion
                            MethodInfo mi = useTo
                                                ? typeConverterType.GetMethod(
                                                    "ConvertTo",
                                                    BindingFlags.Instance | BindingFlags.Public |
                                                    BindingFlags.FlattenHierarchy,
                                                    null,
                                                    new[] {typeof (object), typeof (Type)},
                                                    null)
                                                : typeConverterType.GetMethod(
                                                    "ConvertFrom",
                                                    BindingFlags.Instance | BindingFlags.Public |
                                                    BindingFlags.FlattenHierarchy,
                                                    null,
                                                    new[] {typeof (object)},
                                                    null);
                            if (mi != null)
                            {
                                // The convert methods accepts the value as an object parameters, so we may need a cast.
                                if (expression.Type != typeof (object))
                                    expression = Expression.Convert(expression, typeof (object));

                                // Create an expression which creates a new instance of the type converter and passes in
                                // the existing expression as the first parameter to ConvertTo or ConvertFrom.
                                outputExpression = useTo
                                                       ? Expression.Call(Expression.New(typeConverterType), mi,
                                                                         expression,
                                                                         Expression.Constant(outputType, typeof (Type)))
                                                       : Expression.Call(Expression.New(typeConverterType), mi,
                                                                         expression);

                                return true;
                            }
                        }
                    }
                }
                catch
                {
                }
            }

            // Finally, if we want to output to string, call ToString() method.
            if (outputType == typeof (string))
            {
                outputExpression = Expression.Call(expression, _toStringMethodInfo);
                return true;
            }

            outputExpression = expression;
            return false;
        }


        /// <summary>
        ///   Returns the method info of an explicit/implicit cast on <paramref name="type"/>
        ///   that casts between <paramref name="type"/> to <paramref name="destinationType"/>.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="destinationType">The destination type.</param>
        /// <param name="forwards">
        ///   If set to <see langword="true"/> returns cast from <paramref name="type"/> to <paramref name="destinationType"/>;
        ///   otherwise returns casts in the reverse direction (from <paramref name="destinationType"/> to <paramref name="type"/>).
        /// </param>
        /// <param name="includeImplicit">If <see langword="true"/> include implicit casts.</param>
        /// <param name="includeExplicit">If <see langword="true"/> include explicit casts.</param>
        /// <returns>
        ///   Returns the <see cref="MethodInfo"/> (if any); otherwise returns a <see langword="null"/>.
        /// </returns>
        [CanBeNull]
        [UsedImplicitly]
        public static MethodInfo GetCastMethod(
            [NotNull] this Type type,
            [NotNull] Type destinationType,
            bool forwards = true,
            bool includeImplicit = true,
            bool includeExplicit = true)
        {
            return type
                .GetMethods(BindingFlags.Static | BindingFlags.Public)
                .FirstOrDefault(
                    m =>
                    {
                        // Check for correct name, and return type
                        if (((!includeImplicit || m.Name != "op_Implicit") &&
                             (!includeExplicit || m.Name != "op_Explicit")) ||
                            (m.ReturnType != (forwards ? destinationType : type)))
                            return false;

                        // Check parameters
                        ParameterInfo[] parameters = m.GetParameters();
                        return (parameters.Length == 1) &&
                               (parameters[0].ParameterType == (forwards ? type : destinationType));
                    });
        }

        /// <summary>
        ///   Determines whether or not a type implicitly casts to the specified destination type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="destinationType">The destination type.</param>
        /// <returns>
        ///   Returns <see langword="true"/> if the types are the same or if <paramref name="type"/> can be implicitly cast
        ///   to <paramref name="destinationType"/>; otherwise returns <see langword="false"/>.
        /// </returns>
        [UsedImplicitly]
        public static bool ImplicitlyCastsTo([NotNull]this Type type, [NotNull]Type destinationType)
        {
            return (type == destinationType) ||
                   (GetCastMethod(type, destinationType, includeExplicit: false) != null) ||
                   (GetCastMethod(destinationType, type, forwards: false, includeExplicit: false) != null);
        }

        /// <summary>
        ///   Determines whether or not a type explicitly casts to the specified destination type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="destinationType">The destination type.</param>
        /// <returns>
        ///   Returns <see langword="true"/> if the types are the same or if <paramref name="type"/> can be explicitly cast
        ///   to <paramref name="destinationType"/>; otherwise returns <see langword="false"/>.
        /// </returns>
        [UsedImplicitly]
        public static bool ExplicitlyCastsTo([NotNull]this Type type, [NotNull]Type destinationType)
        {
            return (type != destinationType) &&
                   ((GetCastMethod(type, destinationType, includeImplicit: false) != null) ||
                    (GetCastMethod(destinationType, type, forwards: false, includeImplicit: false) != null));
        }

        /// <summary>
        ///   Determines whether or not a type casts to the specified destination type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="destinationType">The destination type.</param>
        /// <returns>
        ///   Returns <see langword="true"/> if the types are the same or if <paramref name="type"/> can be cast
        ///   to <paramref name="destinationType"/>; otherwise returns <see langword="false"/>.
        /// </returns>
        [UsedImplicitly]
        public static bool CastsTo([NotNull]this Type type, [NotNull]Type destinationType)
        {
            return (type == destinationType) ||
                   (GetCastMethod(type, destinationType) != null) ||
                   (GetCastMethod(destinationType, type, forwards: false) != null);
        }

        /// <summary>
        ///   Gets the equivalent method on the new type specified.
        /// </summary>
        /// <param name="methodInfo">The method info.</param>
        /// <param name="declaringType">The declaring type.</param>
        /// <returns>
        ///   Returns the equivalent method. If the <paramref name="declaringType"/> is equal to the
        ///   <paramref name="methodInfo"/> declaring type then <paramref name="methodInfo"/> is returned. 
        /// </returns>
        [UsedImplicitly]
        [NotNull]
        public static MethodInfo GetEquivalent([NotNull] this MethodInfo methodInfo, [NotNull] Type declaringType)
        {
            return methodInfo.DeclaringType == declaringType
                       ? methodInfo
                       : declaringType.GetMethod(methodInfo.Name,
                                                 methodInfo.GetParameters().Select(p => p.ParameterType).ToArray());
        }

        /// <summary>
        ///   Implements a safe version of <see cref="ParameterInfo.RawDefaultValue"/> that always returns a default value.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>The default value.</returns>
        /// <remarks>
        ///   If the CLR fails to grab a raw default value then this will return the default value for the type instead.
        /// </remarks>
        /// <seealso cref="System.Reflection.ParameterInfo.RawDefaultValue"/>
        [UsedImplicitly]
        [CanBeNull]
        public static object RawDefaultValueSafe([NotNull] this ParameterInfo parameter)
        {
            object value;
            try
            {
                value = parameter.RawDefaultValue;
            }
            catch (FormatException)
            {
                // Bug in RawDefaultValue that doesn't cope with default(struct) being the default value.
                // just specify the default of the type.
                value = null;
            }
            return (value == null) && (parameter.ParameterType.IsValueType)
                       ? Activator.CreateInstance(parameter.ParameterType, true)
                       : value;
        }

        /// <summary>
        ///   Gets the default instance for the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The default instance for the specified type.</returns>
        [UsedImplicitly]
        [CanBeNull]
        public static object Default([NotNull] this Type type)
        {
            return !type.IsValueType ? null : Activator.CreateInstance(type, true);
        }

        /// <summary>
        ///   <see cref="Regex"/> for matching generic types.
        /// </summary>
        private static readonly Regex _genericRegex =
            new Regex(@"(?<FullName>\[(?<Name>\w[.+'\w]*?),\s*(?<Assembly>\w[.+\w]*?),.*?\])", RegexOptions.Compiled);

        /// <summary>
        ///   Cache the simplified type names, so that when requested they can be retrieved rather than recomputed.
        /// </summary>
        private static readonly ConcurrentDictionary<string, string> _simplifications =
            new ConcurrentDictionary<string, string>();

        /// <summary>
        ///   Simplifies the full assembly qualified name for a type, excluding version and key information.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>A <see cref="string"/> containing the simplified full name.</returns>
        [NotNull]
        [UsedImplicitly]
        public static string SimpleAssemblyQualifiedName([NotNull] this Type type)
        {
            return SimpleAssemblyQualifiedName(type.Assembly.FullName, type.FullName);
        }

        /// <summary>
        ///   Simplifies the type name, excluding the version and key information (works with generics).
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>A <see cref="string"/> containing the simplified type name.</returns>
        [NotNull]
        [UsedImplicitly]
        public static string SimpleFullName([NotNull] this Type type)
        {
            return SimpleTypeFullName(type.FullName);
        }

        /// <summary>
        ///   Simplifies the full name of an assembly, excluding the version, culture and key information.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>A <see cref="string"/> containing only the name component from the assembly's full name.</returns>
        /// <seealso cref="System.Reflection.Assembly"/>
        /// <seealso cref="System.Reflection.AssemblyName"/>
        [NotNull]
        [UsedImplicitly]
        public static string SimpleFullName([NotNull] this Assembly assembly)
        {
            return SimpleAssemblyFullName(assembly.FullName);
        }

        /// <summary>
        ///   Simplifies the full assembly qualified name of the type.
        /// </summary>
        /// <param name="assemblyName">The name of the assembly.</param>
        /// <param name="typeName">The name of the type.</param>
        /// <returns>
        ///   <para>A <see cref="string"/> containing the simplified version of the type's full, assembly qualified name.</para>
        ///   <para><b>Format:</b> "simpleTypeName, simpleAssemblyName"</para>
        /// </returns>
        [NotNull]
        [UsedImplicitly]
        public static string SimpleAssemblyQualifiedName([NotNull] string assemblyName, [NotNull] string typeName)
        {
            // ReSharper disable AssignNullToNotNullAttribute
            return _simplifications.GetOrAdd(String.Format("{0}, {1}", assemblyName, typeName),
                                             _ =>
                                             String.Format("{0}, {1}", SimpleTypeFullName(typeName),
                                                           SimpleAssemblyFullName(assemblyName)));
            // ReSharper restore AssignNullToNotNullAttribute
        }

        /// <summary>
        ///   Simplifies the name of an assembly excluding the version, culture and key information.
        /// </summary>
        /// <param name="assemblyName">The full name of the assembly.</param>
        /// <returns>A <see cref="string"/> containing only the name component from the assembly's full name.</returns>
        /// <seealso cref="System.Reflection.Assembly"/>
        /// <seealso cref="System.Reflection.AssemblyName"/>
        [NotNull]
        [UsedImplicitly]
        public static string SimpleAssemblyFullName([NotNull] string assemblyName)
        {
            return assemblyName.Split(',').First();
        }

        /// <summary>
        ///   Simplifies a type's full name to exclude version, culture and key information by simplifying generic type parameters.
        /// </summary>
        /// <param name="typeName">The name of the type.</param>
        /// <returns>A <see cref="string"/> containing the simplified type name.</returns>
        [NotNull]
        [UsedImplicitly]
        public static string SimpleTypeFullName([NotNull] string typeName)
        {
            Match matches = _genericRegex.Match(typeName);
            if (!matches.Success)
                return typeName;

            StringBuilder newType = new StringBuilder();
            int p = 0;
            while (matches.Success)
            {
                // Append string prior to match
                Group fullNameGroup = matches.Groups["FullName"];
                newType.Append(typeName.Substring(p, fullNameGroup.Index - p));
                p = fullNameGroup.Index + fullNameGroup.Length;

                // Get type name
                Group nameGroup = matches.Groups["Name"];
                string n = nameGroup.Value;

                // Get assembly short name
                Group assGroup = matches.Groups["Assembly"];
                string a = assGroup.Value;

                newType.AppendFormat("[{0}, {1}]", n, a);
                matches = matches.NextMatch();
            }

            // Append remaining string
            newType.Append(typeName.Substring(p));
            return newType.ToString();
        }
    }
}