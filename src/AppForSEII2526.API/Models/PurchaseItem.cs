namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(DeviceId), nameof(PurchaseId))]

    public class PurchaseItem
    {

        public PurchaseItem() { }


        public PurchaseItem( string? description, double price,int quantity, Device device, Purchase purchaseItem)
        {
             Description = description;

            Price = price;

            Quantity = quantity;

            Device = Device;

            Purchase = purchaseItem;
            
        }

        public PurchaseItem(string? description, int purchaseId, Purchase purchase, int deviceId, Device device, int quantity, double price)
        {
            Description = description;
            PurchaseId = purchaseId;
            Purchase = purchase;
            DeviceId = deviceId;
            Device = device;
            Quantity = quantity;
            Price = price;
             
        }

        [StringLength(200, ErrorMessage = "Description cannot be longer than 200 characters.", MinimumLength = 4)]
        public string? Description { get; set; }

        public Device Device { get; set; }

        public int DeviceId { get; set; }

        [Precision(10, 2)]
        public double Price { get; set; }

        public Purchase Purchase { get; set; }

        public int PurchaseId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Minimum 1")]
        public int Quantity { get; set; }

    }
}
