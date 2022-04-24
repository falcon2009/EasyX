using EasyX.Crud.Api.Model;
using EasyX.Data.Core.Entity;
using EasyX.WebApi;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Organization.Share.Key;
using System;
using System.Threading.Tasks;

namespace OrganizatonOrc.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrganizationController : OrcControllerBase<OrganizationKey>
    {
        public OrganizationController(IOrchestratorService<OrganizationKey> service) : base(service)
        { }
        [HttpGet("{id}/{modelName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetModelAsync(Guid id, string modelName)
        {
            OrganizationKey key = new() { Id = id };
            KeyProvider<OrganizationKey> keyProvider = new()
            {
                Key = key
            };
            dynamic result = await service.GetModelAsync(modelName, keyProvider);

            return Ok(result);
        }
    }
}
