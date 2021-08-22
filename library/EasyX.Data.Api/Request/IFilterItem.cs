using EasyX.Data.Api.Enum;

namespace EasyX.Data.Api.Filter
{
    public interface IFilterItem
    {
        string FilterField { get; }
        string FilterValue { get; }
        FilterType FilterType { get; }
    }
}
