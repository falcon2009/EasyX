using System;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;

namespace EasyX.Infra.Extension
{
    public static class StringExtension
    {
        public static TResult DeserializeJson<TResult>(this string source, JsonSerializerOptions option) where TResult : class
        {
            if (string.IsNullOrEmpty(source))
            {
                return default;
            }

            return JsonSerializer.Deserialize<TResult>(source, option);
        }

        public static TResult DeserializeXml<TResult>(this string source) where TResult : class
        {
            if (string.IsNullOrEmpty(source))
            {
                return default;
            }

            XmlDocument document = new ();
            document.LoadXml(source);
            using XmlNodeReader nodeReader = new XmlNodeReader(document);
            return new XmlSerializer(typeof(TResult)).Deserialize(nodeReader) as TResult;
        }

        public static string SerializeJson<TResult>(this TResult item, JsonSerializerOptions option) where TResult : class
        {
            if (item == default)
            {
                return string.Empty;
            }

            return JsonSerializer.Serialize(item, option);
        }

        public static Guid? ToGuidOrNull(this string str)
        {
            return Guid.TryParse(str, out var result) ? result : null;
        }

        public static Guid ToGuidOrDefault(this string str)
        {
            return Guid.TryParse(str, out var result) ? result : Guid.Empty;
        }
    }
}
