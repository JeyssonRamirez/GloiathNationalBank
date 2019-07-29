using System.Collections.Generic;
using System.Linq;
using Application.Definition;
using Core.DataTransferObject;
using Crosscutting.DependencyInjectionFactory;
using Microsoft.AspNetCore.Mvc;

namespace GNB.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {

        private readonly ITransactionAppService _transactionAppService;
        public TransactionController()
        {
            _transactionAppService = Factory.Resolve<ITransactionAppService>();
        }

        // GET api/Transaction
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

                r = _transactionAppService.GetAll();
            }

            return Ok(r);
        }       
    }
}
