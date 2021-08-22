
using EasyX.Data.Api.Enum;

namespace EasyX.Data.Api.Request
{
    public interface ISortItem
    {
        string Field { get; set; }
        SortOrder Order { get; set; }
    }
}
