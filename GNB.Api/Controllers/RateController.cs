using System.Linq;
using Application.Definition;
using Core.DataTransferObject;
using Crosscutting.DependencyInjectionFactory;
using Microsoft.AspNetCore.Mvc;

namespace GNB.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RateController : ControllerBase
    {
        private readonly IRateAppService _rateAppService;
        public RateController()
        {
            _rateAppService = Factory.Resolve<IRateAppService>();
        }

        [HttpGet]
        public IActionResult Get()
        {
            var r = new BaseApiResult();
            if (!ModelState.IsValid)
            {
                var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                //StatusCode(HttpStatusCode.BadRequest);
                r.Message = string.Join(" ,", allErrors.Select(s => s.ErrorMessage).ToArray());
            }
            else
            {

                r = _rateAppService.GetAll();
            }

            return Ok(r);
        }
    }
}
