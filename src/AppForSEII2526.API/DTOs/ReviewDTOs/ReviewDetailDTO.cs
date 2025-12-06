
using Humanizer;

namespace AppForSEII2526.API.DTOs.ReviewDTOs
{
    public class ReviewDetailDTO
    {
        public int ReviewId { get; set; }
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

        public override bool Equals(object? obj)
        {
            return obj is ReviewDetailDTO dTO &&
                   CustomerUserName == dTO.CustomerUserName &&
                   CustomerCountry == dTO.CustomerCountry &&
                   DateOfReview == dTO.DateOfReview &&
                   ReviewTitle == dTO.ReviewTitle &&
                   ReviewItems.SequenceEqual(dTO.ReviewItems);
        }
    }
}
