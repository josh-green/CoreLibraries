using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;

namespace WebApplications.Utilities.Reflect
{
    /// <summary>
    /// Wrap <see cref="FieldInfo"/> with additional information.
    /// </summary>
    /// <remarks></remarks>
    [DebuggerDisplay("{Info} [Extended]")]
    public class Field
    {
        /// <summary>
        /// The extended type.
        /// </summary>
        [NotNull]
        public readonly ExtendedType ExtendedType;

        /// <summary>
        /// The underlying <see cref="FieldInfo"/>.
        /// </summary>
        [NotNull]
        public readonly FieldInfo Info;

        /// <summary>
        /// Initializes a new instance of the <see cref="Field"/> class.
        /// </summary>
        /// <param name="extendedType">Type of the extended.</param>
        /// <param name="info">The info.</param>
        /// <remarks></remarks>
        public Field([NotNull]ExtendedType extendedType, [NotNull]FieldInfo info)
        {
            ExtendedType = extendedType;
            Info = info;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Field"/> to <see cref="System.Reflection.FieldInfo"/>.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns>The result of the conversion.</returns>
        /// <remarks></remarks>
        public static implicit operator FieldInfo(Field field)
        {
            return field == null ? null : field.Info;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Reflection.FieldInfo"/> to <see cref="Field"/>.
        /// </summary>
        /// <param name="fieldInfo">The field info.</param>
        /// <returns>The result of the conversion.</returns>
        /// <remarks></remarks>
        public static implicit operator Field(FieldInfo fieldInfo)
        {
            return fieldInfo == null
                       ? null
                       : ((ExtendedType)fieldInfo.DeclaringType).GetField(fieldInfo);
        }

        /// <summary>
        /// Gets the field type.
        /// </summary>
        /// <value>The field type.</value>
        /// <remarks></remarks>
        public Type ReturnType
        {
            get { return Info.FieldType; }
        }

        /// <summary>
        /// Retrieves the lambda function equivalent of the specified static getter method.
        /// </summary>
        /// <typeparam name="TValue">The type of the value returned.</typeparam>	
        /// <param name="checkAssignability">If set to <see langword="true" /> performs assignability checks.</param>
        /// <returns>A function that takes an object of the type T and returns the value of the field.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        /// <remarks></remarks>
        [UsedImplicitly]
        [CanBeNull]
        public Func<TValue> Getter<TValue>(
#if DEBUG
 bool checkAssignability = true
#else
            bool checkAssignability = false
#endif
)
        {
            // Only valid for static fields.
            if (!Info.IsStatic)
                return null;

            Type fieldType = Info.FieldType;
            Type returnType = typeof(TValue);
            Type declaringType = ExtendedType.Type;

            // Check the return type can be assigned from the member type
            if ((checkAssignability) && (returnType != fieldType) &&
                (!returnType.IsAssignableFrom(fieldType)))
            {
                throw new ArgumentOutOfRangeException(
                    String.Format(
                        Resources.Reflection_GetGetter_ReturnTypeNotAssignable,
                        Info.Name,
                        "field",
                        declaringType,
                        fieldType,
                        returnType));
            }

            // Get a member access expression
            Expression expression = Expression.Field(null, Info);

            Contract.Assert(expression != null);

            // Cast return value if necessary
            if (returnType != fieldType)
                expression = Expression.Convert(expression, returnType);

            Contract.Assert(expression != null);

            // Create lambda and compile
            return (Func<TValue>)Expression.Lambda(expression).Compile();
        }

        /// <summary>
        /// Retrieves the lambda function equivalent of the specified instance getter method.
        /// </summary>
        /// <typeparam name="T">The type of the parameter the function encapsulates.</typeparam>	
        /// <typeparam name="TValue">The type of the value returned.</typeparam>	
        /// <param name="checkAssignability">If set to <see langword="true" /> performs assignability checks.</param>
        /// <returns>A function that takes an object of the type T and returns the value of the field.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        /// <remarks></remarks>
        [UsedImplicitly]
        [CanBeNull]
        public Func<T, TValue> Getter<T, TValue>(
#if DEBUG
 bool checkAssignability = true
#else
            bool checkAssignability = false
#endif
)
        {
            // Only valid for instance fields.
            if (Info.IsStatic)
                return null;

            Type fieldType = Info.FieldType;
            Type declaringType = ExtendedType.Type;
            Type parameterType = typeof(T);

            //  Check the parameter type can be assigned from the declaring type.
            if ((checkAssignability) && (parameterType != declaringType) &&
                (!parameterType.IsAssignableFrom(declaringType)))
            {
                throw new ArgumentOutOfRangeException(
                    String.Format(
                        Resources.Reflection_GetGetter_ParameterTypeNotAssignable,
                        Info.Name,
                        "field",
                        declaringType,
                        parameterType));
            }

            Type returnType = typeof(TValue);

            // Check the return type can be assigned from the member type
            if ((checkAssignability) && (returnType != fieldType) &&
                (!returnType.IsAssignableFrom(fieldType)))
            {
                throw new ArgumentOutOfRangeException(
                    String.Format(
                        Resources.Reflection_GetGetter_ReturnTypeNotAssignable,
                        Info.Name,
                        "field",
                        declaringType,
                        fieldType,
                        returnType));
            }

            // Create input parameter expression
            ParameterExpression parameterExpression = Expression.Parameter(parameterType, "target");
            
            // Cast parameter if necessary
            Expression expression = parameterType != declaringType
                             ? (Expression)Expression.Convert(parameterExpression, declaringType)
                             : parameterExpression;

            // Get a member access expression
            expression = Expression.Field(expression, Info);

            Contract.Assert(expression != null);
            Contract.Assert(returnType != null);

            // Cast return value if necessary
            if (returnType != fieldType)
                expression = Expression.Convert(expression, returnType);

            Contract.Assert(expression != null);
            Contract.Assert(parameterExpression != null);

            // Create lambda and compile
            return (Func<T,TValue>) Expression.Lambda(expression, parameterExpression).Compile();
        }
    }
}