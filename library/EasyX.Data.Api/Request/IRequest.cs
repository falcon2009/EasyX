using EasyX.Data.Api.Enum;
using System.Collections.Generic;

namespace EasyX.Data.Api.Request
{
    public interface IRequest
    {
        int? Skip { get; }
        int? Take { get; }
        IList<string> FilterFieldList { get; }
        IList<string> FilterValueList { get; }
        IList<FilterType> FilterTypeList { get; }
        ISortItem SortItem { get; }
    }
}
