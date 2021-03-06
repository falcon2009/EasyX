using EasyX.Http;
using EasyX.Infra.Api;
using Person.Share.Key;
using ServiceCore.Services;
using System.Net.Http;

namespace Person.Proxy
{
    public class PersonContactServiceProxy : ModelServiceHttpGeneric<PersonContactKey>
    {
        public PersonContactServiceProxy(IHttpService httpService, HttpClient httpClient, ITypeResolver typeResolver) : base(httpService, httpClient, typeResolver)
        { }

        protected override string Controller => "PersonContact";
    }
}
