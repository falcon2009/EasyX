using System;

namespace EasyX.Data.Core.Extention
{
    public static class StringExtentsion
    {
        public static string[] ToValueArray(this string value)
        {
            return string.IsNullOrEmpty(value) ? Array.Empty<string>()
                                               : value.Replace("array[", string.Empty, StringComparison.InvariantCultureIgnoreCase)
                                                      .Replace("]", string.Empty, StringComparison.InvariantCultureIgnoreCase)
                                                      .Split(new[] { ',' });
        }
    }
}
