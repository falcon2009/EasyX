using EasyX.Data.Api.Request;
using System.Collections.Generic;

namespace EasyX.Data.Core.Request
{
    public class RequestResult<TModel> : IRequestResult<TModel> where TModel : class
    {
        public IList<TModel> ModelList { get; set; }

        public int Total { get; set; }
    }
}
