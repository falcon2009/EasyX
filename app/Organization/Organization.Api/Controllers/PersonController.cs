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
    public class EmployeeController : CrudControllerBase<EmployeeModel, EmployeeKey>
    {
        public EmployeeController(IModelService<EmployeeKey> service) : base(service)
        { }

        [HttpGet("{organizationId}/{personId}/{modelName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetModelAsync(Guid organizationId, Guid personId, string modelName)
        {
            EmployeeKey key = new() { OrganizationId = organizationId, PersonId = personId };
            KeyProvider<EmployeeKey> keyProvider = new()
            {
                Key = key
            };
            dynamic result = await service.GetModelAsync(modelName, keyProvider);

            return Ok(result);
        }
    }
}
