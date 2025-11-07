namespace AppForSEII2526.API.DTOs.ReceiptDTOs
{
    public class ReceiptForCreateDTO
    {
        public ReceiptForCreateDTO(string customerUserName, string customerUserSurname, string deliveryAddress, ReceiptPaymentMethodTypes receiptPaymentMethodTypes, IList<ReceiptitemDTO> receiptItems)
        {

            CustomerUserName = customerUserName;
            CustomerUserSurname = customerUserSurname;
            DeliveryAddress = deliveryAddress;
            this.receiptPaymentMethodTypes = receiptPaymentMethodTypes;
            this.receiptItems = receiptItems;
        }

        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set a name for user")]
        [StringLength(10, ErrorMessage = "Name cannot be longer than 10 characters.", MinimumLength = 3)]
        public string CustomerUserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set a surname for user")]
        [StringLength(20, ErrorMessage = "Surname cannot be longer than 20 characters.", MinimumLength = 10)]
        public string CustomerUserSurname { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set a direction for DeliveryAddress")]
        [StringLength(50, ErrorMessage = "DeliveryAddress cannot be longer than 50 characters.", MinimumLength = 4)]
        public string DeliveryAddress { get; set; }

        [Required]
        public ReceiptPaymentMethodTypes receiptPaymentMethodTypes { get; set; }

        [Required]
        public IList<ReceiptitemDTO> receiptItems { get; set; }

        //Constructores
        public ReceiptForCreateDTO()
        {
            receiptItems = new List<ReceiptitemDTO>();
        }

    }
}
