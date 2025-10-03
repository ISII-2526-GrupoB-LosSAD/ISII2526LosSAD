namespace AppForSEII2526.API.Models
{
    public class Model
    {
        [Key]
        public int Id { get; set; }
        public string NameModel { get; set; }
        public List<Device> Devices { get; set; }
    }
}
