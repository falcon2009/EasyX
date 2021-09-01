using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyX.Infra.Extension
{
    public static class GuidExtension
    {
        public static string ToNSting(this Guid item)
        {
            return item.ToString("N");
        }

        public static string ToDSting(this Guid item)
        {
            return item.ToString("D");
        }

        public static string ToBSting(this Guid item)
        {
            return item.ToString("B");
        }

        public static string ToPSting(this Guid item)
        {
            return item.ToString("P");
        }
    }
}
