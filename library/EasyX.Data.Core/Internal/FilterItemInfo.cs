using EasyX.Data.Api.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace EasyX.Data.Core.Internal
{
    public class FilterItemInfo
    {
        private static readonly string itemLambdaParameterName = "item";
        public static List<FilterItemInfo> GetFilterItemInfoList<TEntity>(string field) where TEntity: class
        {
            List<FilterItemInfo> result = new ();
            Type currentType = typeof(TEntity);
            string[] fieldNameArray = field.Split(new[] { '.' });

            for (int i = 0; i <= fieldNameArray.Length - 1; i++)
            {
                PropertyInfo propertyInfo = currentType.GetProperty(fieldNameArray[i]);
                if (propertyInfo == null)
                {
                    string message = $"{currentType.Name} cannot be filtered by {fieldNameArray[i]}";
                    throw new ArgumentException(message, fieldNameArray[i]);
                }

                result.Add(new FilterItemInfo(fieldNameArray[i], propertyInfo));
                currentType = result[^1].PropertyOrGenericType;
            }
            for (int i = 0; i <= fieldNameArray.Length - 1; i++)
            {
                result[i].Previous = (i == 0) ? null : result[i - 1];
                result[i].Next = (i == (fieldNameArray.Length - 1)) ? null : result[i + 1];
            }

            return result;
        }

        public FilterItemInfo(string name, PropertyInfo propertyInfo):this(name, propertyInfo, null, null)
        {
        }
        public FilterItemInfo(string name, PropertyInfo propertyInfo, FilterItemInfo next, FilterItemInfo previous)
        {
            FilterItemName = name;
            PropertyInfo = propertyInfo;
            IsEnumerable = propertyInfo.Name.Contains("list", StringComparison.InvariantCultureIgnoreCase);
            PropertyOrGenericType = IsEnumerable ? propertyInfo.PropertyType.GenericTypeArguments[0] : propertyInfo.PropertyType;
            Previous = previous;
            Next = next;
        }
        public PropertyInfo PropertyInfo { get; private set; }
        public bool IsEnumerable { get; private set; }
        public Type PropertyOrGenericType { get; private set; }
        public string FilterItemName { get; private set; }
        public FilterItemInfo Previous { get; set; }
        public FilterItemInfo Next { get; set; }

        public Expression GetLambdaExpression(ValueProvider valueProvider, FilterType filterType, Expression lambdaExpression)
        {
            if (Previous == null && Next == null)
            {
                Expression expression = Expression.Property(lambdaExpression, FilterItemName);
                Expression body = valueProvider.GetBodyExpression(expression, filterType);
                LambdaExpression result = Expression.Lambda(body, new[] { lambdaExpression as ParameterExpression });

                return result;
            }

            if (IsEnumerable)
            {
                MemberExpression memberExpression = Expression.Property(lambdaExpression, FilterItemName);
                MethodInfo anyMethodInfo = typeof(Enumerable).GetMethods()
                                                             .FirstOrDefault(item => string.Compare(item.Name, "Any", StringComparison.InvariantCultureIgnoreCase) == 0 && item.GetParameters().Length == 2)
                                                             .MakeGenericMethod(PropertyOrGenericType);
                Expression predicate = Next.GetLambdaExpression(valueProvider, filterType, memberExpression);
                MethodCallExpression body = Expression.Call(anyMethodInfo, memberExpression, predicate);
                LambdaExpression result = Expression.Lambda(body, new[] { lambdaExpression as ParameterExpression });

                return result;
            }

            if (Previous?.IsEnumerable ?? false)
            {
                ParameterExpression childLambdaParam = Expression.Parameter(Previous.PropertyOrGenericType, itemLambdaParameterName);
                Expression childBody = Expression.Property(childLambdaParam, FilterItemName);
                Expression resultLambda = Expression.Equal(childBody, Expression.Constant(1));
                LambdaExpression predicate = Expression.Lambda(resultLambda, new[] { childLambdaParam });

                return predicate;
            }
            else
            {
                Expression childBody = Expression.Property(lambdaExpression, FilterItemName);

                return Next.GetLambdaExpression(valueProvider, filterType, childBody);
            }
        }
    }
}
