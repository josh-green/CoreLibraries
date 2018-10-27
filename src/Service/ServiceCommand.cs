#region � Copyright Web Applications (UK) Ltd, 2015.  All rights reserved.
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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using WebApplications.Utilities.Annotations;
using WebApplications.Utilities.Formatting;
using WebApplications.Utilities.Logging;
using WebApplications.Utilities.Reflect;
using WebApplications.Utilities.Service.Common;

namespace WebApplications.Utilities.Service
{
    /// <summary>
    /// Implements a command that can be executed by a <see cref="BaseService">service</see>.
    /// </summary>
    public class ServiceCommand : Resolvable
    {
#if DEBUG
        /// <summary>
        /// The debug view
        /// </summary>
        [PublicAPI]
        public readonly string DebugView;
#endif

        /// <summary>
        /// The <see cref="MethodInfo"/> for <see cref="string.Split(char[])"/>.
        /// </summary>
        [NotNull]
        private static readonly MethodInfo _stringSplit =
            InfoHelper.GetMethodInfo<string>(s => s.Split(default(char[]), default(StringSplitOptions)), true);

        /// <summary>
        /// The <see cref="MethodInfo"/> for <see cref="string.IsNullOrEmpty(string)"/>.
        /// </summary>
        [NotNull]
        private static readonly MethodInfo _stringIsNullOrEmpty =
            InfoHelper.GetMethodInfo(() => string.IsNullOrEmpty(default(string)), true);

        /// <summary>
        /// The <see cref="MethodInfo"/> for <see cref="Enum.Parse(Type, string, bool)"/>.
        /// </summary>
        [NotNull]
        private static readonly MethodInfo _enumParse =
            InfoHelper.GetMethodInfo(() => Enum.Parse(default(Type), default(string), default(bool)), true);

        /// <summary>
        /// The <see cref="MethodInfo"/> for <see cref="Task.ContinueWith{T}(Func{Task,T}, CancellationToken, TaskContinuationOptions, TaskScheduler)"/>
        /// </summary>
        [NotNull]
        private static readonly MethodInfo _continueWith =
            InfoHelper.GetMethodInfo<Task>(
                t => t.ContinueWith(
                    default(Func<Task, bool>),
                    default(CancellationToken),
                    default(TaskContinuationOptions),
                    default(TaskScheduler)),
                true);

        /// <summary>
        /// Parameter types that are reserved for special parameters
        /// </summary>
        [NotNull]
        private static readonly HashSet<Type> _reservedParameterTypes = new HashSet<Type>
        {
            typeof(TextWriter),
            typeof(string[]),
            typeof(CancellationToken)
        };

        [NotNull]
        private readonly string[] _names;

        [NotNull]
        private readonly Func<object, TextWriter, Guid, string, CancellationToken, Task<bool>> _execute;

        /// <summary>
        /// Gets the primary name.
        /// </summary>
        /// <value>The name.</value>
        [NotNull]
        [PublicAPI]
        public string Name
        {
            get
            {
                string name = _names[0];
                Debug.Assert(name != null);
                return name;
            }
        }

        /// <summary>
        /// The instance type.
        /// </summary>
        [NotNull]
        [PublicAPI]
        public readonly Type InstanceType;

        /// <summary>
        /// Gets all names that can be used to run the command.
        /// </summary>
        /// <value>The aliases.</value>
        [NotNull]
        public IEnumerable<string> AllNames
        {
            get { return _names; }
        }

        /// <summary>
        /// The description.
        /// </summary>
        [NotNull]
        [PublicAPI]
        public string Description
        {
            get { return _description; }
        }

        /// <summary>
        /// Gets or sets the parameters to the command, including the writer and ID parameters, if any.
        /// </summary>
        /// <value>The parameters.</value>
        [NotNull]
        [PublicAPI]
        public IEnumerable<ParameterInfo> Parameters
        {
            get { return _parameters.Keys; }
        }

        /// <summary>
        /// Gets or sets the parameters to the command that can be given by the user as arguments.
        /// </summary>
        /// <value>The parameters.</value>
        [NotNull]
        [PublicAPI]
        public IEnumerable<ParameterInfo> ArgumentParameters
        {
            get
            {
                return _parameters.Keys.Where(
                    p => (p != _idParameter) &&
                         // ReSharper disable once PossibleNullReferenceException
                         (p.ParameterType != typeof(TextWriter)) &&
                         (p.ParameterType != typeof(CancellationToken)));
            }
        }

        /// <summary>
        /// The minimum arguments.
        /// </summary>
        [PublicAPI]
        public readonly int MinimumArguments;

        /// <summary>
        /// The maximum arguments.
        /// </summary>
        [PublicAPI]
        public readonly int MaximumArguments;

        [NotNull]
        private readonly string _description;

        [NotNull]
        private readonly Dictionary<ParameterInfo, string> _parameters;

        private readonly ParameterInfo _idParameter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCommand"/> class.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="attribute">The attribute.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        internal ServiceCommand([NotNull] MethodInfo method, [NotNull] ServiceCommandAttribute attribute)
        {
            if (method == null) throw new ArgumentNullException("method");
            if (attribute == null) throw new ArgumentNullException("attribute");
            if (method.DeclaringType == null)
                throw new ArgumentException(ServiceResources.ServiceCommand_NoDeclaringType, "method");
            if (method.IsGenericMethod) throw new ArgumentException(ServiceResources.ServiceCommand_Generic, "method");

            InstanceType = method.DeclaringType;

            bool needIdParameter = attribute.IDParameter != null;

            ParameterExpression instanceParameterExpression = Expression.Parameter(typeof(object), "instance");
            ParameterExpression argsParameterExpression = Expression.Parameter(typeof(string), "arguments");
            ParameterExpression writerParameterExpression = Expression.Parameter(typeof(TextWriter), "writer");
            ParameterExpression connectionParameterExpression = Expression.Parameter(typeof(Guid), "connection");
            ParameterExpression cancellationTokenExpression = Expression.Parameter(typeof(CancellationToken), "token");

            Expression instance = InstanceType == typeof(object)
                ? (Expression)instanceParameterExpression
                : Expression.Convert(instanceParameterExpression, InstanceType);

            ParameterInfo[] parameters = method.GetParameters();
            int minimumArguments;
            int maximumArguments;
            Expression[] inputs = new Expression[parameters.Length];
            List<ParameterExpression> locals = new List<ParameterExpression>();
            List<Expression> body = new List<Expression>();
            ParameterExpression splitArgs = null;
            int extraParams = 0;
            bool hasParams = false;

            // Populate the ID parameter and any parameters that aren't specified by the user
            for (int i = 0; i < inputs.Length; i++)
            {
                ParameterInfo parameter = parameters[i];
                Debug.Assert(parameter != null);
                Type parameterType = parameter.ParameterType;

                if (parameter.Name == attribute.IDParameter)
                {
                    if (parameterType != typeof(Guid))
                        throw new ServiceException(
                            LoggingLevel.Error,
                            () => ServiceResources.Err_ServiceCommand_IDParameterWrongType,
                            parameterType);
                    inputs[i] = connectionParameterExpression;
                    needIdParameter = false;
                    _idParameter = parameter;
                    extraParams++;
                }
                else if (parameterType == typeof(string[]))
                {
                    if (splitArgs == null)
                    {
                        // Split the arguments
                        splitArgs = Expression.Variable(typeof(string[]), "splitArguments");

                        // Assign split arguments to body
                        locals.Add(splitArgs);
                        body.Add(
                            Expression.Assign(
                                splitArgs,
                                Expression.Call(
                                    argsParameterExpression,
                                    _stringSplit,
                                    Expression.Constant(null, typeof(char[])),
                                    Expression.Constant(StringSplitOptions.RemoveEmptyEntries))));
                    }

                    hasParams = true;
                    inputs[i] = splitArgs;
                    extraParams++;
                }
                else if (parameterType == typeof(TextWriter))
                {
                    inputs[i] = writerParameterExpression;
                    extraParams++;
                }
                else if (parameterType == typeof(CancellationToken))
                {
                    inputs[i] = cancellationTokenExpression;
                    extraParams++;
                }
            }

            // If the parameters have not been found
            if (needIdParameter)
                throw new ServiceException(
                    LoggingLevel.Error,
                    () => ServiceResources.Err_ServiceCommand_IDParameterMissing,
                    attribute.IDParameter,
                    method.Name);

            if (attribute.ConsumeLine)
            {
                if (parameters.Length != 1 + extraParams)
                    throw new ServiceException(() => ServiceResources.Err_ServiceCommand_Invalid_Command_Method, method);

                ParameterInfo parameter = null;
                int p = 0;
                for (; p < parameters.Length; p++)
                {
                    Debug.Assert(parameters[p] != null);
                    if (parameters[p].ParameterType == typeof(string))
                    {
                        parameter = parameters[p];
                        break;
                    }
                }

                if (parameter == null)
                    throw new ServiceException(() => ServiceResources.Err_ServiceCommand_Invalid_Command_Method, method);

                minimumArguments = parameter.IsOptional ? 0 : 1;
                maximumArguments = 1;

                // Pass the line straight in.
                inputs[p] = argsParameterExpression;
            }
            else if (parameters.Length < 1)
            {
                minimumArguments = 0;
                maximumArguments = 0;
            }
            else
            {
                minimumArguments =
                    // ReSharper disable once PossibleNullReferenceException
                    parameters.Select((p, i) => p.IsOptional ? i : int.MaxValue)
                        .Union(new[] { parameters.Length })
                        .Min() -
                    parameters.Count(
                        // ReSharper disable PossibleNullReferenceException
                        p => !p.IsOptional &&
                             // ReSharper restore PossibleNullReferenceException
                             (p.Name == attribute.IDParameter ||
                              _reservedParameterTypes.Contains(p.ParameterType)));

                maximumArguments = parameters.Length - extraParams;

                if (maximumArguments > 0)
                    // Map args to inputs
                    for (int p = 0, a = 0; p < parameters.Length; p++)
                    {
                        ParameterInfo parameter = parameters[p];
                        Debug.Assert(parameter != null);

                        // These should already have been set
                        if (parameter.Name == attribute.IDParameter)
                        {
                            Debug.Assert(inputs[p] != null);
                            continue;
                        }
                        if (inputs[p] != null)
                            continue;

                        if (splitArgs == null)
                        {
                            // Split the arguments
                            splitArgs = Expression.Variable(typeof(string[]), "splitArguments");

                            // Assign split arguments to body
                            locals.Add(splitArgs);
                            body.Add(
                                Expression.Assign(
                                    splitArgs,
                                    Expression.Call(
                                        argsParameterExpression,
                                        _stringSplit,
                                        Expression.Constant(null, typeof(char[])),
                                        Expression.Constant(StringSplitOptions.RemoveEmptyEntries))));
                        }

                        Type parameterType = parameter.ParameterType;
                        Expression argExpression = Expression.ArrayIndex(splitArgs, Expression.Constant(a));
                        Expression input;

                        // We can pass strings straight through
                        if (parameterType == typeof(string))
                            input = argExpression;
                        else
                        {
                            // We need to parse the string

                            // Check for nullable
                            bool isNullable = parameterType.IsGenericType &&
                                              parameterType.GetGenericTypeDefinition() == typeof(Nullable<>);

                            if (isNullable)
                                parameterType = parameterType.GetGenericArguments()[0];

                            Debug.Assert(parameterType != null);
                            MethodInfo parseMethod;
                            if (parameterType.IsEnum)
                                input = Expression.Convert(
                                    Expression.Call(
                                        _enumParse,
                                        Expression.Constant(parameterType),
                                        argExpression,
                                        Expression.Constant(true)),
                                    parameterType);
                            else if ((parseMethod = parameterType.GetMethod("Parse", new[] { typeof(string) })) != null)
                                input = Expression.Call(parseMethod, argExpression);
                            else if (!argExpression.TryConvert(parameterType, out input))
                                throw new ServiceException(
                                    () => ServiceResources.Err_ServiceCommand_Parameter_Conversion_Unsupported,
                                    parameterType,
                                    parameter.Name,
                                    method);

                            // Convert parsed output to nullable type
                            if (isNullable)
                                input = Expression.Convert(input, parameter.ParameterType);
                        }

                        if ((a >= minimumArguments) &&
                            parameter.IsOptional)
                            // Argument is optional
                            input = Expression.Condition(
                                Expression.LessThan(
                                    Expression.ArrayLength(splitArgs),
                                    Expression.Constant(a + 1)),
                                Expression.Constant(parameter.RawDefaultValueSafe(), parameter.ParameterType),
                                input);

                        a++;
                        inputs[p] = input;
                    }
            }

            // Call the method passing in the inputs
            Expression methodCall = Expression.Call(instance, method, inputs);

            // If the method doesn't return a bool, return true
            if (method.ReturnType != typeof(Task<bool>))
                if (method.ReturnType == typeof(Task))
                    methodCall = Expression.Call(
                        methodCall,
                        _continueWith,
                        Expression.Constant(new Func<Task, bool>(t => true), typeof(Func<Task, bool>)),
                        cancellationTokenExpression,
                        Expression.Constant(
                            TaskContinuationOptions.OnlyOnRanToCompletion | TaskContinuationOptions.ExecuteSynchronously,
                            typeof(TaskContinuationOptions)),
                        Expression.Constant(TaskScheduler.Default, typeof(TaskScheduler)));
                else if (method.ReturnType == typeof(bool))
                    methodCall = Expression.Condition(
                        methodCall,
                        Expression.Constant(TaskResult.True, typeof(Task<bool>)),
                        Expression.Constant(TaskResult.False, typeof(Task<bool>)));
                else
                    methodCall = Expression.Block(methodCall, Expression.Constant(TaskResult.True, typeof(Task<bool>)));

            // Validate min & max arguments
            if (minimumArguments > 0)
                methodCall = Expression.Condition(
                    splitArgs != null
                        ? (Expression)
                        Expression.LessThan(Expression.ArrayLength(splitArgs), Expression.Constant(minimumArguments))
                        : Expression.Call(_stringIsNullOrEmpty, argsParameterExpression),
                    Expression.Constant(TaskResult.False, typeof(Task<bool>)),
                    methodCall);
            if (!hasParams)
                if (maximumArguments == 0)
                    methodCall = Expression.Condition(
                        Expression.Not(Expression.Call(_stringIsNullOrEmpty, argsParameterExpression)),
                        Expression.Constant(TaskResult.False, typeof(Task<bool>)),
                        methodCall);
                else if (!attribute.ConsumeLine &&
                         (maximumArguments < int.MaxValue))
                {
                    Debug.Assert(splitArgs != null);
                    methodCall = Expression.Condition(
                        Expression.GreaterThan(Expression.ArrayLength(splitArgs), Expression.Constant(maximumArguments)),
                        Expression.Constant(TaskResult.False, typeof(Task<bool>)),
                        methodCall);
                }
            MinimumArguments = minimumArguments;
            MaximumArguments = maximumArguments;

            body.Add(methodCall);

            // Build 
            Expression<Func<object, TextWriter, Guid, string, CancellationToken, Task<bool>>> lambda = Expression
                .Lambda<Func<object, TextWriter, Guid, string, CancellationToken, Task<bool>>>(
                    body.Blockify(locals),
                    instanceParameterExpression,
                    writerParameterExpression,
                    connectionParameterExpression,
                    argsParameterExpression,
                    cancellationTokenExpression);

#if DEBUG
            // Update the DebugView
            DebugView = lambda.GetDebugView();
#endif

            _execute = lambda.Compile();

            // Get the names
            string methodName = method.Name;
            Translation names = Translation.Get(attribute.ResourceType, attribute.NamesProperty);
            if ((names == null) ||
                string.IsNullOrWhiteSpace(names.Message))
            {
                Log.Add(LoggingLevel.Warning, () => ServiceResources.Wrn_No_Names_For_Command, methodName);
                // Use the method name.
                _names = new[] { methodName };
            }
            else
                _names = names.Message
                    .Split(',')
                    // ReSharper disable PossibleNullReferenceException
                    .Select(n => n.Trim())
                    .Where(n => n.Length > 0)
                    // ReSharper restore PossibleNullReferenceException
                    .Distinct(StringComparer.CurrentCultureIgnoreCase)
                    .ToArray();

            Debug.Assert(_names.Length > 0);

            // Get the description.
            Translation description = Translation.Get(attribute.ResourceType, attribute.DescriptionProperty);
            if ((description == null) ||
                string.IsNullOrWhiteSpace(description.Message))
            {
                Log.Add(LoggingLevel.Warning, () => ServiceResources.Wrn_No_Description_For_Command, Name);
                // ReSharper disable once AssignNullToNotNullAttribute
                _description = ServiceResources.Cmd_Description;
            }
            else
                _description = description.Message;

            _parameters = parameters.ToDictionary(
                p => p,
                p =>
                {
                    ServiceCommandParameterAttribute att =
                        // ReSharper disable once AssignNullToNotNullAttribute
                        p.GetCustomAttributes(typeof(ServiceCommandParameterAttribute))
                            .Cast<ServiceCommandParameterAttribute>()
                            .FirstOrDefault();

                    if (att == null)
                        return null;

                    // Get the description.
                    Translation d = Translation.Get(att.ResourceType, att.DescriptionProperty);
                    if ((d == null) ||
                        string.IsNullOrWhiteSpace(d.Message))
                    {
                        Log.Add(
                            LoggingLevel.Warning,
                            () => ServiceResources.Wrn_No_Description_For_Parameter,
                            // ReSharper disable once PossibleNullReferenceException
                            p.Name,
                            Name);
                        return ServiceResources.Cmd_Param_Description;
                    }
                    return d.Message;
                });
        }

        /// <summary>
        /// Runs the command on the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="writer">The writer.</param>
        /// <param name="connectionId">The connection identifier.</param>
        /// <param name="arguments">The arguments.</param>
        /// <param name="token">The token.</param>
        /// <returns>
        ///   <see langword="true" /> if succeeded, <see langword="false" /> otherwise.
        /// </returns>
        [NotNull]
        public Task<bool> RunAsync(
            [NotNull] object instance,
            [NotNull] TextWriter writer,
            Guid connectionId,
            [NotNull] string arguments,
            CancellationToken token = default(CancellationToken))
        {
            if (instance == null) throw new ArgumentNullException("instance");
            if (writer == null) throw new ArgumentNullException("writer");
            if (arguments == null) throw new ArgumentNullException("arguments");
            if (!InstanceType.IsInstanceOfType(instance))
                throw new ArgumentException(CommonResources.Bad_Instance, "instance");

            // ReSharper disable once AssignNullToNotNullAttribute
            return _execute(instance, writer, connectionId, arguments, token);
        }

        /// <summary>
        /// Resolves the parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        [NotNull]
        public object ResolveParameter([NotNull] ParameterInfo parameter)
        {
            Debug.Assert(_parameters.ContainsKey(parameter));
            return new DictionaryResolvable
            {
                { "name", parameter.Name },
                {
                    "defaultvalue",
                    parameter.HasDefaultValue
                        ? parameter.RawDefaultValueSafe() ?? "<null>"
                        : Resolution.Null
                },
                { "description", _parameters[parameter] ?? Resolution.Null },
                {
                    "params",
                    !parameter.IsOptional && (parameter.ParameterType == typeof(string[]))
                        ? Resolution.Empty
                        : Resolution.Null
                }
            };
        }

        /// <summary>
        /// Resolves the specified tag.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="chunk">The chunk.</param>
        /// <returns>
        /// An object that will be cached unless it is a <see cref="T:WebApplications.Utilities.Formatting.Resolution" />.
        /// </returns>
        public override object Resolve(FormatWriteContext context, FormatChunk chunk)
        {
            Debug.Assert(chunk.Tag != null);
            switch (chunk.Tag.ToLowerInvariant())
            {
                case "name":
                    return Name;
                case "altnames":
                    return _names.Length > 1
                        ? _names.Skip(1)
                        : Resolution.Null;
                case "description":
                    return Description;
                case "parameters":
                    object[] parameters = ArgumentParameters.Select(ResolveParameter).ToArray();
                    return parameters.Length > 0 ? parameters : Resolution.Null;
                default:
                    return Resolution.Unknown;
            }
        }
    }
}