namespace AppForSEII2526.API.DTOs.DevicesDTO
{
    public class DevicesParaComprarDTO
    {
        public DevicesParaComprarDTO(int id, string name, double price, string brand, string model, string color)
        {
            Id = id;
            Name = name;
            Price = price;
            Brand = brand;
            Model = model;
            Color = color;
        }

        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "DeliveryAddress cannot be longer than 50 characters.", MinimumLength = 4)]
        public string Name { get; set; }

        [Range(0.0, 1000.0, ErrorMessage = "El valor debe estar entre 0 y 1000.")]
        [Display(Name = "Double Value")]
        public double Price { get; set; }

        [StringLength(50, ErrorMessage = "DeliveryAddress cannot be longer than 50 characters.", MinimumLength = 4)]
        public string Brand { get; set; }

        [StringLength(50, ErrorMessage = "DeliveryAddress cannot be longer than 50 characters.", MinimumLength = 4)]
        public string Model { get; set; }

        [StringLength(50, ErrorMessage = "DeliveryAddress cannot be longer than 50 characters.", MinimumLength = 4)]
        public string Color { get; set; }


    }
}
