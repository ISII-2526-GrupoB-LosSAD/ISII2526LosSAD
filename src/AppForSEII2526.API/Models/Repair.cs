namespace AppForSEII2526.API.Models
{
    public class Repair
    {
        public float Cost { get; set; }
        public string Description { get; set; }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Scaledid { get; set; }
        public Scale Scale { get; set; }
        public IList<Receiptitem> Receiptitems { get; set; }
    }
}
