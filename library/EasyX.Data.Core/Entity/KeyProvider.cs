using EasyX.Data.Api.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyX.Data.Core.Entity
{
    public class KeyProvider<TKey> : IKey<TKey> where TKey : class
    {
        public TKey Key { get; set; }
    }
}
