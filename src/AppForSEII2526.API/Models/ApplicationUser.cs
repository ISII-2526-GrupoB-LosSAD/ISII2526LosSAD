using Microsoft.AspNetCore.Identity;

namespace AppForSEII2526.API.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser {
    [StringLength(10, ErrorMessage = "Name cannot be longer than 10 characters.", MinimumLength = 3)]
    public string CustomerUserName { get; set; }

    [StringLength(20, ErrorMessage = "Surname cannot be longer than 20 characters.", MinimumLength = 10)]
    public string CustomerUserSurname { get; set; }

    [StringLength(10, ErrorMessage = "Name country cannot be longer than 50 characters.", MinimumLength = 4)]
    public string? CustomerCountry { get; set; }
    public IList<Purchase> Purchase { get; set; }
    public IList<Review> Review { get; set; }
    public IList<Receipt> Receipt { get; set; }
}