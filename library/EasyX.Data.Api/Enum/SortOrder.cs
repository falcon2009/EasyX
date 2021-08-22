using System.Text.Json.Serialization;

namespace EasyX.Data.Api.Enum
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SortOrder
    {
        Unspecified = -1,
        Ascending = 0,
        Descending = 1
    }
}
