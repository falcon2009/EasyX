using System.Text.Json.Serialization;

namespace EasyX.Data.Api.Enum
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FilterType
    {
        Contains = 0,
        Equal = 1,
        NotEqual = 2,
        GreaterThan = 3,
        LessThan = 4,
        GreaterThanOrEqual = 5,
        LessThanOrEqual = 6,
        StartsWith = 7
    }
}
