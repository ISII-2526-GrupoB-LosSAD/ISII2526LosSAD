namespace AppForSEII2526.API.DTOS.DevicesDTO
{
    public class RepairParaRepararDTO
    {
        public RepairParaRepararDTO(int id, string name, string scale, string description, float cost)
        {
            this.id = id;
            this.name = name;
            this.scale = scale;
            this.description = description;
            this.cost = cost;
        }
        public int id { get; set; }
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.", MinimumLength = 4)]
        public string name { get; set; }
        public string scale { get; set; }
        [StringLength(200, ErrorMessage = "Description cannot be longer than 200 characters.", MinimumLength = 4)]
        public string description { get; set; }
        [Range(0.0, 1000.0, ErrorMessage = "El valor debe estar entre 0 y 1000.")]
        [Display(Name = "Float Value")]
        public float cost { get; set; }
    }
}
