namespace AppForSEII2526.API.Models
{
    public class Review
    {
        public Review()
        {
        }

        public Review( string reviewTitle,DateTime dateTime, IList<ReviewItem> reviewItems, ApplicationUser applicationUser)
        {

            
            ReviewTitle = reviewTitle;
            DateOfReview = dateTime;
            ReviewItems = reviewItems;
            ApplicationUser = applicationUser;
        }

        public Review(int customerId, DateTime dateOfReview, int overallRating, int reviewId, string reviewTitle, IList<ReviewItem> reviewItems, ApplicationUser applicationUser)
        {
            CustomerId = customerId;
            DateOfReview = dateOfReview;
            OverallRating = overallRating;
            ReviewId = reviewId;
            ReviewTitle = reviewTitle;
            ReviewItems = reviewItems;
            ApplicationUser = applicationUser;
        }

        public int CustomerId { get; set; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfReview { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Minimum 1")]
        public int OverallRating { get; set; }
        public int ReviewId { get; set; }
        [StringLength(50, ErrorMessage = "Review Title cannot be longer than 50 characters.", MinimumLength = 4)]
        public string ReviewTitle { get; set; }
        public IList<ReviewItem> ReviewItems { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
