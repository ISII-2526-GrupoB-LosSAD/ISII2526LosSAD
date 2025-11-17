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
            _logger.LogInformation("TodoService initialized");
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ReviewDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetReview(int id)
        {
            // Verifica si la tabla de Reviews existe
            if (_context.Reviews == null)
            {
                _logger.LogError("Error: Review table does not exist");
                return NotFound();
            }
            // Busca en la base de datos la reseña con el ID especificado
            var review = await _context.Reviews
                .Where(r => r.ReviewId == id) // Filtra por ID
                    .Include(r => r.ApplicationUser)  // Incluye información del usuario que hizo la reseña
                    .Include(r => r.ReviewItems) // Incluye los ítems de la reseña
                        .ThenInclude(ri => ri.Device) // Incluye el dispositivo reseñado
                            .ThenInclude(rep => rep.Model)// Incluye el modelo del dispositivo
                 // Proyecta los datos a un DTO (objeto simplificado para enviar al cliente)
                .Select(r => new ReviewDetailDTO(r.ApplicationUser.CustomerUserName, r.ApplicationUser.CustomerCountry, r.DateOfReview, r.ReviewTitle,
                    r.ReviewItems.Select(ri => new ReviewItemDTO(
                        ri.Device.Name, ri.Device.Model.NameModel, ri.Device.Year, ri.Rating, ri.Comments))
                    .ToList<ReviewItemDTO>()
                ))
                .FirstOrDefaultAsync();
            // Si no se encuentra la reseña
            if (review == null)
            {
                _logger.LogWarning($"Error: Rental with id {id} does not exist");
                return NotFound();
            }
            return Ok(review);
        }


        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ReviewDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]

        public async Task<ActionResult> CreateReview(ReviewForCreateDTO reviewForCreate)
        {
            // Verifica que al menos haya un item en la reseña
            if (reviewForCreate.ReviewItems.Count == 0)
                ModelState.AddModelError("RentalItems", "Error! You must include at least one movie to be rented");

            if (reviewForCreate.ReviewTitle == null)
                ModelState.AddModelError("Title", "Error! Title is required");
            if (reviewForCreate.Country == null)
                ModelState.AddModelError("Country", "Error! Country is required");
            // if (!_context.ApplicationUsers.Any(au=>au.UserName==rentalForCreate.CustomerUserName))
            // Busca el usuario que hizo la reseña en la base de datos
            var user = _context.Users.FirstOrDefault(au => au.CustomerUserName == reviewForCreate.CustomerUserName);
            // Si hay errores de validación, devuelve 400 con los detalles
            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // Obtiene los nombres de los dispositivos reseñados
            var deviceTitles = reviewForCreate.ReviewItems.Select(ri => ri.Name).ToList();
            // Busca esos dispositivos en la base de datos, incluyendo su modelo
            var devices = _context.Devices.Include(m => m.ReviewItems)
                .Include(ri => ri.Model)
                .Where(m => deviceTitles.Contains(m.Name))
                .ToList();

            // Crea una nueva reseña con la fecha actual, sin ítems aún
            Review review = new Review(reviewForCreate.ReviewTitle,DateTime.Today, new List<ReviewItem>(), user );

            // Recorre los ítems enviados desde el cliente
            foreach (var item in reviewForCreate.ReviewItems)
            {
                // Busca el dispositivo correspondiente
                var reviewItem = devices.FirstOrDefault(m => m.Name == item.Name);
                //we must check that there is enough quantity to be rented in the database
                // Si el dispositivo no existe
                if ((reviewItem == null))
                {
                    ModelState.AddModelError("RentalItems", $"Error! Device titled '{item.Name}' is not available for being review");
                }
                else
                {   // Comprueba que hay comentarios calificación, mirando que sean validos
                    if (string.IsNullOrWhiteSpace(item.Comments) || item.Comments.Length < 4)
                    {
                        ModelState.AddModelError("ReviewItems", $"Error! You must provide a comment (minimum 4 characters) for device '{item.Name}'");
                    }

                    if (item.Rating < 1 || item.Rating > 5)
                    {
                        ModelState.AddModelError("ReviewItems", $"Error! Rating for device '{item.Name}' must be between 1 and 5");
                    }
                    // Si pasa las validaciones, agrega el ReviewItem a la reseña
                    review.ReviewItems.Add(new ReviewItem(item.Comments, item.Rating, review, reviewItem));
                }
            }


            //if there is any problem because of the available quantity of movies or because the movie does not exist
            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }
            // Calcula el promedio de calificaciones si hay ítems
            if (review.ReviewItems.Any())
            {
                review.OverallRating = (int)Math.Round(review.ReviewItems.Average(ri => ri.Rating));

            }
            // Agrega la reseña al contexto de base de datos
            _context.Add(review);

            try
            {
                //we store in the database both rental and its rentalitems
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Si ocurre un error al guardar, lo registra en el log
                _logger.LogError(ex.Message);
                ModelState.AddModelError("Rental", $"Error! There was an error while saving your rental, plese, try again later");
                return Conflict("Error" + ex.Message);

            }

            //it returns reviewDetail
            // Crea un objeto DTO con los detalles de la reseña creada
            var reviewDetail = new ReviewDetailDTO(
                reviewForCreate.CustomerUserName, 
                reviewForCreate.Country,
                review.DateOfReview, 
                review.ReviewTitle, 
                reviewForCreate.ReviewItems );

            return CreatedAtAction("GetReview", new { id = review.ReviewId }, reviewDetail);

        }
    }
}
