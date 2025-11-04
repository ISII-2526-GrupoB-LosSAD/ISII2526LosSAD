namespace AppForSEII2526.API.Models
{
    public class Repair
    {
        [Range(0.0, 1000.0, ErrorMessage = "El valor debe estar entre 0 y 1000.")]
        [Display(Name = "Float Value")]
        public float Cost { get; set; }

        [StringLength(200, ErrorMessage = "Description cannot be longer than 200 characters.", MinimumLength = 4)]
        public string Description { get; set; }

        [Key]
        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.", MinimumLength = 4)]
        public string Name { get; set; }

        public int ScaleId { get; set; }

        public Scale Scale { get; set; }

        public IList<Receiptitem> Receiptitems { get; set; }

        public Repair()
        {
        }

        public Repair(float cost, string name, Scale scale)
        {
            Cost = cost;
            Name = name;
            Scale = scale;
        }
    }
}
