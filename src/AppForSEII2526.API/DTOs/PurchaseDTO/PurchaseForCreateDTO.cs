namespace AppForSEII2526.API.DTOs.PurchaseDTO
{
    public class PurchaseForCreateDTO
    {
        public PurchaseForCreateDTO(string customerUserName, string userSurname, string deliveryAddress, PurchasePaymentMethodTypes paymentMethod)
        {
            
            CustomerUserName = customerUserName;
            UserSurname = userSurname;
            DeliveryAddress = deliveryAddress;
            PaymentMethod = paymentMethod;
            
        }

        public PurchaseForCreateDTO()
        {
            PurchaseItems = new List<PurchaseItemDTO>();
        }

        public IList<PurchaseItemDTO> PurchaseItems { get; set; }

        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.", MinimumLength = 4)]
        [Required]
        public string CustomerUserName { get; set; }

        [Required]
        public string UserSurname { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        [Display(Name = "Delivery Address")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your address for delivery")]
        public string DeliveryAddress { get; set; }

        [Display(Name = "Payment Method")]
        [Required]
        public PurchasePaymentMethodTypes PaymentMethod { get; set; }

    }
}
