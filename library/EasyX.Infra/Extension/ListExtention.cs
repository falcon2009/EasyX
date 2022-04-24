using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyX.Infra.Extension
{
    public static class ListExtention
    {
        public static string ToArrayParam<T>(this List<T> paramList)
        {
            if (paramList?.Any() ?? false)
            {
                string[] stringArray = paramList.Select(item => item.ToString()).ToArray();
                string joinResult = string.Join(",", stringArray);
                string result = $"array[{joinResult}]";

                return result;
            }

            return string.Empty;
        }
    }
}
