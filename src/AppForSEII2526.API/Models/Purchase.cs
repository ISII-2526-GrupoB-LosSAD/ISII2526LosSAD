namespace AppForSEII2526.API.Models
{
    public enum PurchasePaymentMethodTypes
    {
        Paypal,
        CreditCard
    }
    public class Purchase
    {

        public Purchase() { }

        public Purchase(string deliveryAddress, PurchasePaymentMethodTypes paymentMethod, ApplicationUser applicationUser)
        {
            PurchaseItems = new List<PurchaseItem>();
            DeliveryAddress = deliveryAddress;
            PaymentMethod = paymentMethod;
            ApplicationUser = applicationUser;
        }

        public Purchase(string deliveryAddress, int id, double totalPrice, int totalQuantity, PurchasePaymentMethodTypes paymentMethod, DateTime receiptDate, IList<PurchaseItem> purchaseItems, ApplicationUser applicationUser)
        {
            DeliveryAddress = deliveryAddress;
            Id = id;
            TotalPrice = totalPrice;
            TotalQuantity = totalQuantity;
            PaymentMethod = paymentMethod;
            ReceiptDate = receiptDate;
            PurchaseItems = purchaseItems;
            ApplicationUser = applicationUser;
        }

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

       

        [Display(Name = "Payment Method")]
        public PurchasePaymentMethodTypes PaymentMethod { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ReceiptDate { get; set; }

        public IList<PurchaseItem> PurchaseItems { get; set; }

        public ApplicationUser ApplicationUser { get; set; }


    }
}
