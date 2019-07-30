using Core.Entities;
using System.Collections.Generic;

namespace Core.DataTransferObject
{
    public class TotalTransactionsConverted
    {
        public IEnumerable<TransactionConverted> Transactions { get; set; }
        public decimal TotalConverted { get; set; }
        public string CurrencyConverted { get; set; }
    }

}
