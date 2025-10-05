namespace AppForSEII2526.API.Models
{
    public class Receipt
    {
        public int Id { get; set; }

        public enum PaymentMethodTypes
        {
            Paypal, CreditCard, cash
        }
        [Display(Name = "Payment Method")]
        public PaymentMethodTypes PaymentMethod { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ReceiptDate { get; set; }

        [Precision(10, 2)]
        public double TotalPrice { get; set; }

        public IList<Receiptitem> Receiptitems { get; set; }
    }
}
