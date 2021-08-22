using EasyX.Http;
using EasyX.Infra.Api;
using ServiceCore.Services;
using System.Net.Http;
using UAC.Share.Key;

namespace UAC.Proxy
{
    public class UserServiceProxy : ModelServiceHttpGeneric<UserKey>
    {
        public UserServiceProxy(IHttpService httpService, HttpClient httpClient, ITypeResolver typeResolver, string controller) : base(httpService, httpClient, typeResolver, controller)
        { }
    }
}
