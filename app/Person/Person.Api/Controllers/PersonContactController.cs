using EasyX.Crud.Api.Model;
using EasyX.Data.Core.Entity;
using EasyX.WebApi;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Person.Share.Key;
using Person.Share.Model.PersonContact;
using System;
using System.Threading.Tasks;

namespace Person.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PersonContactController : CrudControllerBase<PersonContactModel, PersonContactKey>
    {
        public PersonContactController(IModelService<PersonContactKey> service) : base(service)
        { }

        [HttpGet("{id}/{modelName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetModelAsync(Guid id, string modelName)
        {
            PersonContactKey key = new() { Id = id };
            KeyProvider<PersonContactKey> keyProvider = new()
            {
                Key = key
            };
            dynamic result = await service.GetModelAsync(modelName, keyProvider);

            return Ok(result);
        }
    }
}
