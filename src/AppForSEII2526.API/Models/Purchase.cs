namespace AppForSEII2526.API.Models
{
    public class Purchase
    {
        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        [Display(Name = "Delivery Address")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your address for delivery")]
        public string DeliveryAddress { get; set; }

        [Key]
        public int Id { get; set; }

        [Precision(10, 2)]
        public double TotalPrice { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Minimum 1")]
        public int TotalQuantity { get; set; }

        public enum PaymentMethodTypes
        {
            Paypal,CreditCard
        }

        [Display(Name = "Payment Method")]
        public PaymentMethodTypes PaymentMethod { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ReceiptDate { get; set; }

        public IList<PurchaseItem> PurchaseItems { get; set; }

    }
}
