using EasyX.Http;
using EasyX.Infra.Api;
using ServiceCore.Services;
using System.Net.Http;
using UAC.Share.Key;

namespace UAC.Proxy
{
    public class RoleServiceProxy : ModelServiceHttpGeneric<RoleKey>
    {
        public RoleServiceProxy(IHttpService httpService, HttpClient httpClient, ITypeResolver typeResolver) : base(httpService, httpClient, typeResolver)
        {
        }

        protected override string Controller => "Role";
    }
}
