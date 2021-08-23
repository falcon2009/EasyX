using EasyX.Http;
using EasyX.Infra.Api;
using Person.Share.Key;
using ServiceCore.Services;
using System.Net.Http;

namespace Person.Proxy
{
    public class PersonServiceProxy : ModelServiceHttpGeneric<PersonKey>
    {
        public PersonServiceProxy(IHttpService httpService, HttpClient httpClient, ITypeResolver typeResolver, string controller) : base(httpService, httpClient, typeResolver, controller)
        { }
    }
}
