using EasyX.Crud.Api.Model;
using EasyX.WebApi;
using Microsoft.AspNetCore.Mvc;
using Organization.Share.Key;

namespace OrganizatonOrc.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrganizationController : OrcControllerBase<OrganizationKey>
    {
        public OrganizationController(IOrchestratorService<OrganizationKey> service) : base(service)
        { }
    }
}
