using EasyX.Data.Api.Enum;
using EasyX.Data.Api.Request;

namespace EasyX.Data.Core.Request
{
    public class SortItem : ISortItem
    {
        public SortItem() { }
        public SortItem(string field, SortOrder order)
        {
            Field = field;
            Order = order;
        }

        public string Field { get; set; }
        public SortOrder Order { get; set; }
    }
}
