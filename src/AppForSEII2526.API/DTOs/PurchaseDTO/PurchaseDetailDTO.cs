namespace AppForSEII2526.API.DTOs.PurchaseDTO
{
    public class PurchaseDetailDTO
    {

        public string CostumerUserName { get; set; }
        public string DeliveryAddress { get; set; }
        public DateTime PurchaseDate { get; set; }
        public double TotalPrice { get; set; }
        public int TotalQuantity { get; set; }  

        public IList<PurchaseItemDTO> PurchaseItems { get; set; }

        public PurchaseDetailDTO(string costumerUserName, string deliveryAddress, DateTime purchaseDate, double totalPrice, int totalQuantity, IList<PurchaseItemDTO> PurchaseItems)
        {
            this.CostumerUserName = costumerUserName;
            this.DeliveryAddress = deliveryAddress;
            this.PurchaseDate = purchaseDate;
            this.TotalPrice = totalPrice;
            this.TotalQuantity = totalQuantity;
            this.PurchaseItems = PurchaseItems;
        }
    }
}
