using EasyX.Http;
using EasyX.Infra.Api;
using Organization.Share.Key;
using ServiceCore.Services;
using System.Net.Http;

namespace Organization.Proxy
{
    public class OrganizationServiceProxy : ModelServiceHttpGeneric<OrganizationKey>
    {
        public OrganizationServiceProxy(IHttpService httpService, HttpClient httpClient, ITypeResolver typeResolver) : base(httpService, httpClient, typeResolver)
        { }

        protected override string Controller => "Organization";
    }
}
