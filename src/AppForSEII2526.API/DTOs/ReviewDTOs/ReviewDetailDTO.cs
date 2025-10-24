namespace AppForSEII2526.API.DTOs.ReviewDTOs
{
    public class ReviewDetailDTO
    {
        public string CustomerUserName { get; set; }
        public string CustomerCountry { get; set; }

        public DateTime DateOfReview { get; set; }

        public string ReviewTitle { get; set; }
        public IList<ReviewItemDTO> ReviewItems { get; set; }

        public ReviewDetailDTO(string customerUserName, string customerCountry, DateTime dateOfReview, string reviewTitle, IList<ReviewItemDTO> reviewItems)
        {
            CustomerUserName = customerUserName;
            CustomerCountry = customerCountry;
            DateOfReview = dateOfReview;
            ReviewTitle = reviewTitle;
            ReviewItems = reviewItems;
        }
    }
}
