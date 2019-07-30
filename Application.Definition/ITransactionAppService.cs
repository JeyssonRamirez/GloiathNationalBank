using Core.DataTransferObject;
using Core.Entities;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Application.Definition
{
    public interface ITransactionAppService
    {
        BaseApiResult GetAll();
        BaseApiResult GetTransactionsBySkuInOtherCurrency(string sku,string currency);
    }
}
