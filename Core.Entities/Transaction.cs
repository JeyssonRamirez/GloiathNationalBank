namespace Core.Entities
{
    public class Transaction :Entity
    {
        public string Sku { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }

    }

    public class Rates : Entity
    {
        public string From { get; set; }
        public string To { get; set; }
        public decimal Rate { get; set; }

    }
}
