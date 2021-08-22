using EasyX.Data.Api.Enum;
using EasyX.Data.Core.Extention;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace EasyX.Data.Core.Internal
{
    public class ValueProvider
    {
        private static readonly string startsWithMethodName = "StartsWith";
        private static readonly string containsMethodName = "Contains";
        private static readonly string toStringMethodName = "ToString";
        private static readonly string parseMethodName = "Parse";
        private static readonly string addMethodName = "Add";
        private static readonly string nullValue = "NULL";
        public static MethodInfo StringStartsWithMethod
        {
            get => typeof(string).GetMethod(startsWithMethodName, new[] { typeof(string) });
        }
        public static MethodInfo StringContainsMethod
        {
            get =>  typeof(string).GetMethod(containsMethodName, new[] { typeof(string)});
        }
        private string value { get; set; }

        public ValueProvider(PropertyInfo propertyInfo, string value)
        {
            this.value = value;
            PropertyName = propertyInfo.Name;
            ValueType = propertyInfo.PropertyType;
            IsArray = value.StartsWith("array", StringComparison.InvariantCultureIgnoreCase);
            Type underlyingType = Nullable.GetUnderlyingType(ValueType);
            IsNullable = underlyingType != null;
            TypeForParceMethod = IsNullable ? underlyingType : ValueType;
            IsEnum = TypeForParceMethod.BaseType.Name == "Enum";
            IsStringValue = TypeForParceMethod == typeof(string);
            IsDateTimeValue = TypeForParceMethod == typeof(DateTime) || TypeForParceMethod == typeof(DateTimeOffset);
            IsNullValue = string.Compare(value, nullValue, StringComparison.InvariantCultureIgnoreCase) == 0;
        }
        public Type ValueType { get; private set; }
        public bool IsNullable { get; private set; }
        public bool IsEnum { get; private set; }
        public bool IsStringValue { get; private set; }
        public bool IsDateTimeValue { get; private set; }
        public bool IsNullValue { get; private set; }
        public object OriginalValue { get => value; }
        public bool IsArray { get; private set; }
        public object ParcedValue
        {
            get
            {
                if (IsArray)
                {
                    return null;
                }

                return GetParcedValue(value);
            }
        }
        public object ParcedValueList
        {
            get
            {
                if (!IsArray)
                {
                    return null;
                }

                Type genericListType = typeof(List<>).MakeGenericType(ValueType);
                object listInstance = Activator.CreateInstance(genericListType);
                string[] valueArray = value.ToValueArray();
                for (int i = 0; i <= valueArray.Length - 1; i++)
                {
                    object item = GetParcedValue(valueArray[i]);
                    genericListType.GetMethod(addMethodName).Invoke(listInstance, new[] { item });
                }

                return listInstance;
            }
        }
        public string PropertyName { get; private set; }
        public Type TypeForParceMethod { get; private set; }
        public MethodInfo ValueTypeToStringMethod
        {
            get => ValueType.GetMethod(toStringMethodName);
        }
        public Expression GetBodyExpression(Expression expression, FilterType filterType)
        {
            if (IsNullValue)
            {
                return GetBodyExpresionForNull(expression, filterType);
            }

            if (IsArray)
            {
                return GetBodyExpresionForArray(expression);
            }

            if (IsDateTimeValue)
            {
                return GetBodyExpressionForDateTime(expression, filterType);
            }

            return GetValueBodyExpression(expression, filterType);
        }


        #region private
        private static Expression GetBodyExpresionForNull(Expression expression, FilterType filterType)
        {
            if (!(filterType == FilterType.Equal || filterType == FilterType.NotEqual))
            {
                throw new ArgumentException($"Invalid filter type: { filterType } for NULL", nameof(filterType));
            }

            return (filterType == FilterType.Equal) ? Expression.Equal(expression, Expression.Constant(null))
                                                    : Expression.NotEqual(expression, Expression.Constant(null));
        }
        private Expression GetBodyExpresionForArray(Expression expression)
        {
            //if (expression.Type.IsValueType && (!(IsEnum || TypeForParceMethod == typeof(Guid))))
            //{
            //    expression = Expression.Convert(expression, typeof(object));
            //}

            Type genericListType = typeof(List<>).MakeGenericType(ValueType);
            MethodCallExpression containMethodEpression = Expression.Call(Expression.Constant(ParcedValueList),
                                                                          genericListType.GetMethod(containsMethodName),
                                                                          expression);
            return containMethodEpression;
        }
        private Expression GetValueBodyExpression(Expression expression, FilterType filterType)
        {
            Expression body = IsDateTimeValue ? GetBodyExpressionForDateTime(expression, filterType)
                                              : GetBody(expression, filterType);

            return body;
        }
        private Expression GetBodyExpressionForDateTime(Expression expression, FilterType filterType)
        {
            DateTimeFormatInfo invariantFormat = DateTimeFormatInfo.InvariantInfo;
            char offsetMarker = value.FirstOrDefault(letter => letter.Equals('+') || letter.Equals('-'));
            bool hasOffset = offsetMarker != default(char);
            Expression body = null;
            if (filterType == FilterType.Contains)
            {
                DateTime selectedDateTime = hasOffset ? DateTimeOffset.Parse(value, invariantFormat).DateTime : DateTime.Parse(value, invariantFormat);
                DateTime minDateTime = new (selectedDateTime.Year, selectedDateTime.Month, selectedDateTime.Day, 0, 0, 0);
                DateTime maxDateTime = new (selectedDateTime.Year, selectedDateTime.Month, selectedDateTime.Day, 23, 59, 59);
                Expression left = Expression.GreaterThan(expression, Expression.Constant(minDateTime));// GetBody(expression, PropertyName, typeof(DateTimeOffset), minDateTime, null, FilterType.GreaterThanOrEqual);
                Expression right = Expression.LessThanOrEqual(expression, Expression.Constant(maxDateTime)); //GetBody(expression, PropertyName, typeof(DateTimeOffset), maxDateTime, null, FilterType.LessThanOrEqual);
                body = Expression.And(left, right);
            }
            else
            {
                object parcedValue = hasOffset ? DateTimeOffset.Parse(value, invariantFormat) : DateTime.Parse(value, invariantFormat);
                body = GetBody(expression, filterType);
            }
            return body;
        }

        private Expression GetBody(Expression expression, FilterType filterType)
        {
            if (IsEnum && (!(filterType == FilterType.Equal || filterType == FilterType.NotEqual)))
            {
                throw new ArgumentException($"Invalid filter type: { filterType } for Enum", nameof(filterType));
            }
            switch (filterType)
            {
                case FilterType.Equal:
                    return Expression.Equal(expression, Expression.Constant(ParcedValue));
                case FilterType.LessThan:
                    return Expression.LessThan(expression, Expression.Constant(ParcedValue));
                case FilterType.LessThanOrEqual:
                    return Expression.LessThanOrEqual(expression, Expression.Constant(ParcedValue));
                case FilterType.GreaterThan:
                    return Expression.GreaterThan(expression, Expression.Constant(ParcedValue));
                case FilterType.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(expression, Expression.Constant(ParcedValue));
                case FilterType.NotEqual:
                    return Expression.NotEqual(expression, Expression.Constant(ParcedValue));
                case FilterType.StartsWith:
                    if (ValueType == typeof(string))
                    {
                        return Expression.Call(expression, typeof(string).GetMethod(startsWithMethodName, new[] { typeof(string) }), Expression.Constant(ParcedValue));
                    }
                    return Expression.Call(Expression.Call(expression, ValueType.GetMethod(toStringMethodName)), typeof(string).GetMethod(containsMethodName, new[] { typeof(string) }), Expression.Constant(value));
                case FilterType.Contains:
                    if (ValueType == typeof(string))
                    {
                        return Expression.Call(expression, typeof(string).GetMethod(containsMethodName, new[] { typeof(string) }), Expression.Constant(ParcedValue));
                    }
                    return Expression.Call(Expression.Call(expression, ValueType.GetMethod(toStringMethodName)), typeof(string).GetMethod(containsMethodName, new[] { typeof(string) }), Expression.Constant(value));

                default:
                    throw new ArgumentException("This filter type is not supported", nameof(ValueType));
            }
        }

        private object GetParcedValue(string anyValue)
        {
            if (IsEnum)
            {
                return Enum.Parse(TypeForParceMethod, anyValue);
            }

            MethodInfo methodInfo = TypeForParceMethod.GetMethod(parseMethodName, new[] { typeof(string) });
            return methodInfo == null ? anyValue : methodInfo.Invoke(null, new object[] { anyValue });
        }
        #endregion
    }
}
