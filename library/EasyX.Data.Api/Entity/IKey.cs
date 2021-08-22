using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyX.Data.Api.Entity
{
    public interface IKey<out TKey> where TKey: class
    {
        public TKey Key { get; }
    }
}
