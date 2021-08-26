using EasyX.Http;
using EasyX.Infra.Api;
using Organization.Share.Key;
using ServiceCore.Services;
using System.Net.Http;

namespace Person.Proxy
{
    public class PositionServiceProxy : ModelServiceHttpGeneric<PositionKey>
    {
        public PositionServiceProxy(IHttpService httpService, HttpClient httpClient, ITypeResolver typeResolver) : base(httpService, httpClient, typeResolver)
        { }

        protected override string Controller => "Position";
    }
}
