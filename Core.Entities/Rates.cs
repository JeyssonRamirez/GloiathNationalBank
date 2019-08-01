namespace Core.Entities
{
    public class Rates : Entity
    {
        public string From { get; set; }
        public string To { get; set; }
        public decimal Rate { get; set; }

    }
}
