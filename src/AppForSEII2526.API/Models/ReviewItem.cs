namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(DeviceId),nameof(ReviewId))]
    public class ReviewItem
    {
        [StringLength(100, ErrorMessage = "Comments cannot be longer than 100 characters.", MinimumLength = 4)]
        public string Comments { get; set; }
        public int DeviceId { get; set; }
        public Device Device { get; set; }  
        [Key]
        public int Id { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Minimum 1")]
        public int Rating { get; set; }
        public int ReviewId { get; set; }
        public Review Review { get; set; }

    }
}
