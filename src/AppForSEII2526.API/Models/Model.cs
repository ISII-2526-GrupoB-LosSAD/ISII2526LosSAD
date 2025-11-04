namespace AppForSEII2526.API.Models
{
    public class Model
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "Name Model cannot be longer than 50 characters.", MinimumLength = 4)]
        public string NameModel { get; set; }

        public IList<Device> Devices { get; set; }
    }
}
