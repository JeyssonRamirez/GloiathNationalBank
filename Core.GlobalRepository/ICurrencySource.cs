using Core.DataTransferObject;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Core.GlobalRepository
{
    public interface ICurrencySource
    {
        IEnumerable<TransactionResult> GetTransactions();
        IEnumerable<RateResult> GetRates();
    }
}
