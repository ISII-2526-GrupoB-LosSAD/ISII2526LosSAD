namespace AppForSEII2526.API.Models
{
    public class PurchaseItem
    {
        public string Description { get; set; }
        public int DeviceId { get; set; }

        public double Price { get; set; }
        public int PurchaseId { get; set; }
        public int Quantity { get; set; }


    }
}
