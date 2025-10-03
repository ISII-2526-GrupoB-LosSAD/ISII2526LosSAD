namespace AppForSEII2526.API.Models
{
    public class Device
    {
        public string Brand { get; set; }
        public string Color { get; set; }
        public string Description { get; set; }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public double priceForPurchase { get; set; }
        public double priceForRent { get; set; }

        public string Quality { get; set; }

        public int quauntityForPurchase { get; set; }

        public int quauntityForRent { get; set; }

        public int Year { get; set; }
        public List<PurchaseItem> PurchaseItems { get; set; }
        public Model Model { get; set; }

    }
}
