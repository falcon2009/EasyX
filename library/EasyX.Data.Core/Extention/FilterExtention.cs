using EasyX.Data.Api.Enum;
using EasyX.Data.Api.Filter;
using EasyX.Data.Api.Request;
using EasyX.Data.Core.Request;
using EasyX.Infra;
using System;
using System.Collections.Generic;

namespace EasyX.Data.Core.Extention
{
    public static class FilterExtention
    {
        private static FilterType defaultFilter = FilterType.Equal;
        public static IEnumerable<IFilterItem> GetFilterItemList(this IRequest request)
        {

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), Constant.Errors.RequestFilterNull);
            }

            int fieldCount = request.FilterFieldList?.Count ?? 0;
            int valueCount = request.FilterValueList?.Count ?? 0;
            int typeCount = request.FilterTypeList?.Count ?? 0;

            if (fieldCount != valueCount || fieldCount != typeCount)
            {
                throw new ArgumentException(Constant.Errors.RequestInvalid);
            }

            if (fieldCount < 1)
            {
                return new IFilterItem[] { };
            }

            IFilterItem[] itemArray = new IFilterItem[fieldCount];
            for (int i = 0; i < fieldCount; i++)
            {
                string field = request.FilterFieldList[i];
                string value = request.FilterValueList[i];
                FilterType type = request.FilterTypeList[i];
                bool isInvalidField = string.IsNullOrEmpty(field);
                if (isInvalidField)
                {
                    continue;
                }

                IFilterItem item = new FilterItem(field, value, type);
                itemArray[i] = item;
            }

            return itemArray;
        }
        public static void RemoveFilter(this IRequest param, string filterName)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param), Constant.Errors.RequestFilterNull);
            }

            if (string.IsNullOrEmpty(filterName))
            {
                return;
            }

            int filterIndex = param.FilterFieldList.IndexOf(filterName);
            bool hasFilter = filterIndex >= 0;
            if (!hasFilter)
            {
                return;
            }

            param.FilterFieldList.RemoveAt(filterIndex);
            param.FilterValueList.RemoveAt(filterIndex);
            param.FilterTypeList.RemoveAt(filterIndex);
        }
        public static void AddFilter(this IRequest param, string field, string value)
        {
            AddFilter(param, field, value, defaultFilter);
        }
        public static void AddFilter(this IRequest param, string field, string value, FilterType filterType)
        {
            IFilterItem item = new FilterItem(field, value, filterType);
            AddFilter(param, item);
        }
        public static void AddFilter(this IRequest param, IFilterItem item)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param), Constant.Errors.RequestFilterNull);
            }

            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), Constant.Errors.FilterItemNull);
            }

            bool isValidParam = !string.IsNullOrEmpty(item.FilterField.TrimEnd().TrimStart());
            if (!isValidParam)
            {
                throw new ArgumentException(Constant.Errors.FilterItemInvalid, nameof(item));
            }

            param.FilterFieldList.Add(item.FilterField);
            param.FilterTypeList.Add(item.FilterType);
            param.FilterValueList.Add(item.FilterValue);
        }
        public static string ToQuery(this IRequest param)
        {
            if (param == null)
            {
                return string.Empty;
            }

            List<string> paramList = new List<string>();
            if (param.Skip.HasValue)
            {
                paramList.Add($"skip={param.Skip}");
            }
            if (param.Take.HasValue)
            {
                paramList.Add($"take={param.Take}");
            }
            string sortParam = param.SortItem?.ToQuery() ?? string.Empty;
            if (!string.IsNullOrEmpty(sortParam))
            {
                paramList.Add(sortParam);
            }
            IEnumerable<IFilterItem> filterList = param.GetFilterItemList();
            foreach (IFilterItem item in filterList)
            {
                string filterParam = item.ToQuery();
                if (!string.IsNullOrEmpty(filterParam))
                {
                    paramList.Add(filterParam);
                }
            }

            return string.Join("&", paramList);
        }
        public static string ToQuery(this IFilterItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return $"filterFieldList={item.FilterField}&filterValueList={item.FilterValue}&filterTypeList={item.FilterType}"; ;
        }
        public static string ToQuery(this ISortItem item)
        {
            if (string.IsNullOrEmpty(item?.Field))
            {
                throw new ArgumentNullException(nameof(item));
            }

            return $"sortField={item.Field}&sortOrder={item.Order}";
        }
    }
}
