using EasyX.Data.Api.Enum;
using EasyX.Data.Api.Filter;
using EasyX.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace EasyX.Data.Core.Internal
{
    public static class PredicateProvider
    {
        private static readonly string entityLambdaParameterName = "entity";

        public static Expression<Func<TEntity, object>> GetPredicateForSorting<TEntity>(string field) where TEntity : class
        {
            ParameterExpression lambdaParam = Expression.Parameter(typeof(TEntity), entityLambdaParameterName);
            Expression sortParam = GetPropertyExpression<TEntity>(lambdaParam, field);
            Expression<Func<TEntity, object>> predicate = Expression.Lambda<Func<TEntity, object>>(sortParam, new[] { lambdaParam });

            return predicate;   
        }
        public static Expression<Func<TEntity, bool>> GetPredicateForEntity<TEntity, TKey>(TKey key) where TEntity : class where TKey : class
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key), Constant.Errors.KeyNull);
            }

            ParameterExpression lambdaParam = Expression.Parameter(typeof(TEntity), entityLambdaParameterName);
            Expression expression = null;
            foreach (PropertyInfo propertyInfo in typeof(TKey).GetProperties())
            {
                expression = (expression == null) ? Expression.Equal(GetPropertyExpression<TEntity>(lambdaParam, propertyInfo.Name, false), Expression.Constant(propertyInfo.GetValue(key)))
                                                  : Expression.And(expression, Expression.Equal(GetPropertyExpression<TEntity>(lambdaParam, propertyInfo.Name, false), Expression.Constant(propertyInfo.GetValue(key))));
            }

            return Expression.Lambda<Func<TEntity, bool>>(expression, lambdaParam);
        }
        public static Expression<Func<TEntity, bool>> GetPredicateForFilter<TEntity>(IFilterItem filterItem) where TEntity : class
        {
            try
            {
                ParameterExpression entityLambdaParam = Expression.Parameter(typeof(TEntity), entityLambdaParameterName);
                List<FilterItemInfo> filterItemInfoList = FilterItemInfo.GetFilterItemInfoList<TEntity>(filterItem.FilterField);
                LambdaExpression result = filterItemInfoList.First()
                                                            .GetLambdaExpression(new(filterItemInfoList.Last().PropertyInfo, filterItem.FilterValue), filterItem.FilterType, entityLambdaParam) as LambdaExpression;

                return result as Expression<Func<TEntity, bool>>;
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                throw;
            }
        }

        #region private
        private static Expression GetPropertyExpression<TEntity>(ParameterExpression lambdaParam, string field, bool shouldConvert = true) where TEntity : class
        {
            return GetPropertyExpression(lambdaParam, typeof(TEntity), field.Split(","), shouldConvert);
        }
        private static Expression GetPropertyExpression(ParameterExpression lambdaParam, Type currentType, string[] fieldNameArray, bool shouldConvert = true)
        {
            PropertyInfo propertyInfo = null;
            Expression parameter = lambdaParam;

            for (int i = 0; i < fieldNameArray.Length; i++)
            {
                propertyInfo = currentType.GetProperties()
                                          .FirstOrDefault(prop => string.Compare(prop.Name, fieldNameArray[i], StringComparison.InvariantCultureIgnoreCase) == 0);
                if (propertyInfo == null)
                {
                    throw new ArgumentException($"{fieldNameArray[i]} is not valid", fieldNameArray[i]);
                }
                parameter = Expression.Property(parameter, propertyInfo);
                if (parameter.Type.IsValueType && shouldConvert)
                {
                    parameter = Expression.Convert(parameter, typeof(object));
                }
                currentType = propertyInfo.PropertyType;
            }

            return parameter;
        }
        #endregion
    }
}
