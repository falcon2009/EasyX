using EasyX.Crud.Api.Model;
using EasyX.Data.Core.Entity;
using EasyX.WebApi;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Organization.Share.Key;
using Organization.Share.Model;
using System;
using System.Threading.Tasks;

namespace Organization.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrganizationController : CrudControllerBase<OrganizationModel, OrganizationKey>
    {
        public OrganizationController(IModelService<OrganizationKey> service) : base(service)
        { }

        [HttpGet("{modelName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetModelAsync([FromQuery] Guid id, [FromRoute] string modelName)
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
