namespace AppForSEII2526.API.DTOs.ReceiptDTOs
{
    public class ReceiptDetailDTO
    {
        public string CustomerUserName { get; set; }
        public string CustomerUserSurname { get; set; }
        public string DeliveryAddress { get; set; }
        public double TotalPrice { get; set; }
        public DateTime ReceiptDate { get; set; }
        public IList<ReceiptitemDTO> Receiptitems { get; set; }

        public ReceiptDetailDTO(string CustomerUserName, string CustomerUserSurname, string DeliveryAddress, double TotalPrice, DateTime ReceiptDate, IList<ReceiptitemDTO> Receiptitems)
        {
            this.CustomerUserName = CustomerUserName;
            this.CustomerUserSurname = CustomerUserSurname;
            this.DeliveryAddress = DeliveryAddress;
            this.TotalPrice = TotalPrice;
            this.ReceiptDate = ReceiptDate;
            this.Receiptitems = Receiptitems;
        }
    }
}
