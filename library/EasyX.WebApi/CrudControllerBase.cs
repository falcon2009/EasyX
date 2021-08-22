using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using EasyX.Data.Api.Entity;
using EasyX.Crud.Api.Model;
using EasyX.Infra;
using EasyX.Data.Core.Request;
using EasyX.Data.Api.Request;

namespace EasyX.WebApi
{
    public class CrudControllerBase<TModel, TKey> : ControllerBase where TModel : class, IKey<TKey> where TKey : class
    {
        protected readonly IModelService<TKey> service;
        public CrudControllerBase(IModelService<TKey> service)
        {
            this.service = service;
        }


        [HttpGet("list/{modelName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<IActionResult> GetModelListAsync([FromRoute] string modelName, [FromQuery] Request filter)
        {
            if (filter == null)
            {
                return BadRequest(Constant.Errors.RequestFilterNull);
            }

            dynamic result = await service.GetModelListAsync(modelName, filter).ConfigureAwait(false);

            return Ok(result);
        }

        [HttpGet("total/{modelName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult<TotalModel>> GetTotalAsync([FromRoute] string modelName, [FromQuery] Request filter)
        {
            if (filter != null)
            {
                return BadRequest(Constant.Errors.RequestFilterNull);
            }

            ITotalModel result = await service.GetTotalAsync(modelName, filter).ConfigureAwait(false);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public virtual async Task<ActionResult<TModel>> InsertModelAsync([FromBody] TModel model)
        {
            if (model == null)
            {
                return BadRequest(Constant.Errors.ModelNull);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(Constant.Errors.ModelInvalid);
            }

            model = await service.InsertModelAsync(model).ConfigureAwait(false);

            return Ok(model);

        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public virtual async Task<IActionResult> UpdateModelAsync([FromBody] TModel model)
        {
            if (model == null)
            {
                return BadRequest(Constant.Errors.ModelNull);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(Constant.Errors.ModelInvalid);
            }

            model = await service.UpdateModelAsync(model).ConfigureAwait(false);

            return Ok(model);
        }
    }
}
