namespace AppForSEII2526.API.DTOs.PurchaseDTO
{
    public class PurchaseItemDTO
    {

        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string Brand { get; set; }
        public string Color { get; set; }
        public string Model { get; set; }

        public PurchaseItemDTO(string description, double price, int quantity, string brand, string color, string model)
        {
            this.Description = description;
            this.Price = price;
            this.Quantity = quantity;
            this.Brand = brand;
            this.Color = color;
            this.Model = model;
        }
    }
}
