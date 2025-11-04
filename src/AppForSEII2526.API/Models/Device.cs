namespace AppForSEII2526.API.Models
{


    public class Device
    {
        [StringLength(50, ErrorMessage = "Brand cannot be longer than 50 characters.", MinimumLength = 4)]
        public string Brand { get; set; }

        [StringLength(50, ErrorMessage = "Color cannot be longer than 50 characters.", MinimumLength = 4)]
        public string Color { get; set; }

        [StringLength(50, ErrorMessage = "Description cannot be longer than 50 characters.", MinimumLength = 4)]
        public string Description { get; set; }

        [Key]
        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.", MinimumLength = 4)]
        public string Name { get; set; }

        public double priceForPurchase { get; set; }

        public double priceForRent { get; set; }

        [StringLength(50, ErrorMessage = "Quality cannot be longer than 50 characters.", MinimumLength = 4)]
        public string Quality { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Minimum 1")]
        public int quauntityForPurchase { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Minimum 1")]
        public int quauntityForRent { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Minimum 1")]
        public int Year { get; set; }


        public IList<PurchaseItem> PurchaseItems { get; set; }

        public Model Model { get; set; }

        public IList<ReviewItem> ReviewItems { get; set; }

    }
}
