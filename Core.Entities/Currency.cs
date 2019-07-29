using System;

namespace Core.Entities
{

    public class Transaction :Entity
    {
        public string Sku { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }

    }
    public class Currency
    {
        public string Code { get; set; }
        public string symbol { get; set; }
        public int DigitalCode { get; set; }
        public string Name { get; set; }        

    }
}
