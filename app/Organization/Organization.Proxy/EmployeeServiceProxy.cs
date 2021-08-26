using EasyX.Http;
using EasyX.Infra.Api;
using Organization.Share.Key;
using ServiceCore.Services;
using System.Net.Http;

namespace Person.Proxy
{
    public class EmployeeServiceProxy : ModelServiceHttpGeneric<EmployeeKey>
    {
        public EmployeeServiceProxy(IHttpService httpService, HttpClient httpClient, ITypeResolver typeResolver) : base(httpService, httpClient, typeResolver)
        { }

        protected override string Controller => "Employee";
    }
}
