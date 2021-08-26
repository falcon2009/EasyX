using EasyX.Crud.Api.Model;
using EasyX.Data.Api.Request;
using EasyX.Data.Core.Request;
using EasyX.Infra;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EasyX.WebApi
{
    public class OrcControllerBase<TKey> : ControllerBase where TKey : class
    {
        protected readonly IOrchestratorService<TKey> service;
        public OrcControllerBase(IOrchestratorService<TKey> service)
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

        [HttpGet("first/{modelName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<IActionResult> GetFirstFromModelListAsync([FromRoute] string modelName, [FromQuery] Request filter)
        {
            if (filter == null)
            {
                return BadRequest(Constant.Errors.RequestFilterNull);
            }

            dynamic result = await service.GetFirstFromModelListAsync(modelName, filter).ConfigureAwait(false);

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
    }
}
