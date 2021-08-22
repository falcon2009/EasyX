using EasyX.Crud.Api.Model;
using EasyX.Data.Core.Entity;
using EasyX.WebApi;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UAC.Share.Key;
using UAC.Share.Model;

namespace UAC.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoleController : CrudControllerBase<RoleModel, RoleKey>
    {
        public RoleController(IModelService<RoleKey> service):base(service)
        {}

        [HttpGet("{id}/{modelName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetModelAsync([FromRoute] int id, [FromRoute] string modelName)
        {
            RoleKey key = new(){ Id = id };
            KeyProvider<RoleKey> keyProvider = new ()
            {
                Key = key
            };
            dynamic result = await service.GetModelAsync(modelName, keyProvider);

            return Ok(result);
        }
    }
}
