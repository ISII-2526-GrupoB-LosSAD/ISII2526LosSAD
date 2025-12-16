
using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.DTOs.PurchaseDTO
{
    public class PurchaseDetailDTO
    {
        public int Id { get; set; }
        public string CostumerUserName { get; set; }
        public string CustomerUserSurname { get; set; }
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

        public PurchaseDetailDTO(string costumerUserName, string customerUserSurname, string deliveryAddress, DateTime purchaseDate, double totalPrice, int totalQuantity, IList<PurchaseItemDTO> purchaseItems)
        {
            CostumerUserName = costumerUserName;
            CustomerUserSurname = customerUserSurname;
            DeliveryAddress = deliveryAddress;
            PurchaseDate = purchaseDate;
            TotalPrice = totalPrice;
            TotalQuantity = totalQuantity;
            PurchaseItems = purchaseItems;
        }

        public PurchaseDetailDTO(int id, string costumerUserName, string customerUserSurname, string deliveryAddress, DateTime purchaseDate, double totalPrice, int totalQuantity, IList<PurchaseItemDTO> purchaseItems)
        {
            Id = id;
            CostumerUserName = costumerUserName;
            CustomerUserSurname = customerUserSurname;
            DeliveryAddress = deliveryAddress;
            PurchaseDate = purchaseDate;
            TotalPrice = totalPrice;
            TotalQuantity = totalQuantity;
            PurchaseItems = purchaseItems;
        }

        public override bool Equals(object? obj)
        {
            return obj is PurchaseDetailDTO dTO &&
                   CostumerUserName == dTO.CostumerUserName &&
                   DeliveryAddress == dTO.DeliveryAddress &&
                   PurchaseDate == dTO.PurchaseDate &&
                   TotalPrice == dTO.TotalPrice &&
                   TotalQuantity == dTO.TotalQuantity &&
                   PurchaseItems.SequenceEqual(dTO.PurchaseItems);
        }
    }
}
