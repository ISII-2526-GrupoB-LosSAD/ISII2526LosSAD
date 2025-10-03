namespace AppForSEII2526.API.Models
{
    public class Purchase
    {
        public string CustomerUserName { get; set; }
        public string CustomerUserSurname { get; set; }
        public string DeliveryAddress { get; set; }
        [Key]
        public int Id { get; set; }
        public double TotalPrice { get; set; }
        public int TotalQuantity { get; set; }
        public enum PaymentMethodTypes
        {
            //Metodos de pago
        }
        public DateTime ReceiptDate { get; set; }

    }
}
