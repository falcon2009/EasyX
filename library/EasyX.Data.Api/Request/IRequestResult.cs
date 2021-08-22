using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyX.Data.Api.Request
{
    public interface IRequestResult<TModel> where TModel: class
    {
        IList<TModel> ModelList { get; set; }
        int Total { get; set; }
    }
}
