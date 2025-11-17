using AppForSEII2526.API.Models;
using System.Drawing;

namespace AppForSEII2526.API.DTOs.ReviewDTOs
{
    public class ReviewForCreateDTO //Related to post Method
    {
        public ReviewForCreateDTO(string reviewTitle, string country, string? customerUserName, IList<ReviewItemDTO> reviewItems)
        {
            ReviewTitle = reviewTitle;
            Country = country;
            CustomerUserName = customerUserName;
            this.ReviewItems = reviewItems;
        }

        public ReviewForCreateDTO()
        {
           ReviewItems = new List<ReviewItemDTO>();
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your Review Title")]
        [StringLength(50, ErrorMessage = "Review Title cannot be longer than 50 characters.", MinimumLength = 4)]
        public string ReviewTitle { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your Country")]
        [StringLength(50, ErrorMessage = "Country Title cannot be longer than 50 characters.", MinimumLength = 4)]
        public string Country { get; set;}

       
        [StringLength(50, ErrorMessage = "CustomerUserName cannot be longer than 50 characters.", MinimumLength = 4)]
        public string? CustomerUserName { get; set; }

        public IList<ReviewItemDTO> ReviewItems { get; set; }

        

        

 
       
        

    }

}
