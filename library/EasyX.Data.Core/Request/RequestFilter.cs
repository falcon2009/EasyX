using EasyX.Data.Api.Enum;
using EasyX.Data.Api.Request;
using System.Collections.Generic;

namespace EasyX.Data.Core.Request
{
    public class Request : IRequest
    {
        public int? Skip { get; set; }
        public int? Take { get; set; }
        public List<string> FilterField { get; set; } = new List<string>();
        public List<string> FilterValue { get; set; } = new List<string>();
        public List<FilterType> FilterType { get; set; } = new List<FilterType>();
        public SortItem Sort { get; set; }

        IList<string> IRequest.FilterFieldList => FilterField;

        IList<string> IRequest.FilterValueList => FilterValue;

        IList<FilterType> IRequest.FilterTypeList => FilterType;

        ISortItem IRequest.SortItem => Sort;
    }
}
