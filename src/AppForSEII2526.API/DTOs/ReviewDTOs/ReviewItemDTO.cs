using System.Drawing;

namespace AppForSEII2526.API.DTOs.ReviewDTOs
{
    public class ReviewItemDTO
    {
        public string Name { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int Rating { get; set; }
        public string Comments { get; set; }

        public ReviewItemDTO(string name, string model, int year, int rating, string comments)
        {
            Name = name;
            Model = model;
            Year = year;
            Rating = rating;
            Comments = comments;
        }
    }
}
