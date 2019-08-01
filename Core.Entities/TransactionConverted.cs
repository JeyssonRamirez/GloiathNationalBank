using System.Collections.Generic;

namespace Core.Entities
{
    public class TransactionConverted : Entity
    {
        public string Sku { get; set; }
        public decimal OriginalAmount { get; set; }
        public string OriginalCurrency { get; set; }
        public decimal ConvertedAmount { get; set; }
        public string ConvertedCurrency { get; set; }

        public List<string> ConvertionRoute { get; set; }

    }
}
