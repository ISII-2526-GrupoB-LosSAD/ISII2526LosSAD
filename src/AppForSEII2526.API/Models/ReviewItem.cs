namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(DeviceId),nameof(ReviewId))]
    public class ReviewItem
    {
        [StringLength(100, ErrorMessage = "Comments cannot be longer than 100 characters.", MinimumLength = 4)]
        public string Comments { get; set; }
        public int DeviceId { get; set; }
        public Device Device { get; set; }  
      
        [Range(1, 5, ErrorMessage = "Minimum 1, Maximun 5")]
        public int Rating { get; set; }
        public int ReviewId { get; set; }
        public Review Review { get; set; }

    }
}
