namespace AppForSEII2526.API.Models
{
    public class Review
    {
        public int CustomerCount { get; set; }
        public int CustomerId { get; set; }
        public DateTime DateOfReview { get; set; }
        public int OverallRating { get; set; }
        public int ReviewId { get; set; }
        public string ReviewTitle { get; set; }
        public List<ReviewItem> ReviewItems { get; set; }

    }
}
