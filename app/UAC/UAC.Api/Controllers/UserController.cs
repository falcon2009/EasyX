using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EasyX.WebApi;
using UAC.Share.Model;
using UAC.Share.Key;
using EasyX.Crud.Api.Model;
using EasyX.Data.Core.Entity;

namespace UAC.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : CrudControllerBase<UserModel, UserKey>
    {
        public UserController(IModelService<UserKey> service):base(service)
        {}

        [HttpGet("{id}/{modelName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetModelAsync(Guid id, string modelName)
        {
            UserKey key = new(){ Id = id };
            KeyProvider<UserKey> keyProvider = new ()
            {
                Key = key
            };
            dynamic result = await service.GetModelAsync(modelName, keyProvider);

            return Ok(result);
        }
    }
}
