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
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : CrudControllerBase<OrganizationModel, OrganizationKey>
    {
        public OrganizationController(IModelService<OrganizationKey> service) : base(service)
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
