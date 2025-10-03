namespace AppForSEII2526.API.Models
{
    public class Receipt
    {
        public int Id { get; set; }
        public enum PaymentMethodTypes
        {
            //Metodos de pago
        }
        public DateTime ReceiptDate { get; set; }
        public double TotalPrice { get; set; }
        public IList<Receiptitem> Receiptitems { get; set; }
    }
}
