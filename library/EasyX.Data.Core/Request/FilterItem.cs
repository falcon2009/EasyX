using EasyX.Data.Api.Enum;
using EasyX.Data.Api.Filter;

namespace EasyX.Data.Core.Request
{
    public class FilterItem : IFilterItem
    {
        public FilterItem(string field, string value, FilterType type)
        {
            FilterField = field;
            FilterValue = value;
            FilterType = type;
        }
        public string FilterField { get ;}
        public string FilterValue { get ; }
        public FilterType FilterType { get ; }
    }
}
