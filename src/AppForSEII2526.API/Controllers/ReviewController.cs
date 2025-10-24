using AppForSEII2526.API.DTOs.ReviewDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        //used to enable your controller to access to the database 
        private readonly ApplicationDbContext _context;
        //used to log any information when your system is running
        private readonly ILogger<ReviewController> _logger;

        public ReviewController(ApplicationDbContext context, ILogger<ReviewController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ReviewDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetReview(int id)
        {
            if (_context.Reviews == null)
            {
                _logger.LogError("Error: Review table does not exist");
                return NotFound();
            }
            var review = await _context.Reviews
                .Where(r => r.ReviewId == id)
                    .Include(r => r.ApplicationUser)
                    .Include(r => r.ReviewItems)
                        .ThenInclude(ri => ri.Device)
                            .ThenInclude(rep => rep.Model)
                .Select(r => new ReviewDetailDTO( r.ApplicationUser.CustomerUserName, r.ApplicationUser.CustomerCountry, r.DateOfReview, r.ReviewTitle, 
                    r.ReviewItems.Select(ri => new ReviewItemDTO(
                        ri.Device.Name, ri.Device.Model,ri.Device.Year, ri.Rating, ri.Comments ) )
                    .ToList<ReviewItemDTO>()
                ))
                .FirstOrDefaultAsync();
            if (review == null)
            {
                _logger.LogWarning($"Error: Rental with id {id} does not exist");
                return NotFound();
            }
            return Ok(review);
        }
    }
}
