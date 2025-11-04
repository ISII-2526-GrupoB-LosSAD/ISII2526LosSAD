using System.Drawing;

namespace AppForSEII2526.API.DTOS.DevicesDTO
{
    public class DevicesparareseniaDTO
    {
        public DevicesparareseniaDTO(int id, string brand, string color, string name, int year, string model)
        {
            Id = id;
            Brand = brand;
            Color = color;
            Name = name;
            Year = year;
            Model = model;
        }

        public int Id { get; set; }

		[StringLength(50, ErrorMessage = "Brand cannot be longer than 50 characters.", MinimumLength = 4)]
		public string Brand { get; set; }

		[StringLength(50, ErrorMessage = "Color cannot be longer than 50 characters.", MinimumLength = 4)]
		public string Color { get; set; }

		[StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.", MinimumLength = 4)]
		public string Name { get; set; }

		[Range(1, int.MaxValue, ErrorMessage = "Minimum 1")]
		public int Year { get; set; }

		[StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.", MinimumLength = 4)]
		public string Model { get; set; }


		
	}
}
