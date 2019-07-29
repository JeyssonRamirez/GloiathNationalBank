using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ModelBinding.Binders;

namespace Presentation.Api.GNB.Models
{

    public class TransactionBySkuAndCurrency
    {
        [Required]
        [FromQuery]
        public string CurrencyCode { get; set; }
        [Required]
        [FromQuery]
        public string SkuCode { get; set; }
    }
}
