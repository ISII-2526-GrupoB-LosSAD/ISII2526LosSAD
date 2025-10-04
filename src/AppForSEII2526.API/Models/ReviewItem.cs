namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(DeviceId),
nameof(ReviewId))]
    public class ReviewItem
    {
        public string Comments { get; set; }
        public int DeviceId { get; set; }
        [Key]
        public int Id { get; set; }
        public int Rating { get; set; }
        public int ReviewId { get; set; }
    }
}
