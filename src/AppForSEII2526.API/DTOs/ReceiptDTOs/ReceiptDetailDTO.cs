
namespace AppForSEII2526.API.DTOs.ReceiptDTOs
{
    public class ReceiptDetailDTO
    {
        public int ReceiptId { get; set; }
        public string CustomerUserName { get; set; }
        public string CustomerUserSurname { get; set; }
        public string DeliveryAddress { get; set; }
        public double TotalPrice { get; set; }
        public DateTime ReceiptDate { get; set; }
        public IList<ReceiptitemDTO> Receiptitems { get; set; }

        public ReceiptDetailDTO()
        {
        }

        public ReceiptDetailDTO(string CustomerUserName, string CustomerUserSurname, string DeliveryAddress, double TotalPrice, DateTime ReceiptDate, IList<ReceiptitemDTO> Receiptitems)
        {
            this.CustomerUserName = CustomerUserName;
            this.CustomerUserSurname = CustomerUserSurname;
            this.DeliveryAddress = DeliveryAddress;
            this.TotalPrice = TotalPrice;
            this.ReceiptDate = ReceiptDate;
            this.Receiptitems = Receiptitems;
        }

        public ReceiptDetailDTO(int receiptId, string customerUserName, string customerUserSurname, string deliveryAddress, double totalPrice, DateTime receiptDate, IList<ReceiptitemDTO> receiptitems)
        {
            ReceiptId = receiptId;
            CustomerUserName = customerUserName;
            CustomerUserSurname = customerUserSurname;
            DeliveryAddress = deliveryAddress;
            TotalPrice = totalPrice;
            ReceiptDate = receiptDate;
            Receiptitems = receiptitems;
        }

        public override bool Equals(object? obj)
        {
            return obj is ReceiptDetailDTO dTO &&
                   CustomerUserName == dTO.CustomerUserName &&
                   CustomerUserSurname == dTO.CustomerUserSurname &&
                   DeliveryAddress == dTO.DeliveryAddress &&
                   TotalPrice == dTO.TotalPrice &&
                   ReceiptDate == dTO.ReceiptDate &&
                   Receiptitems.SequenceEqual(dTO.Receiptitems);
        }
    }
}
