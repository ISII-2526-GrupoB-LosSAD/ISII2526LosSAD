using Microsoft.AspNetCore.Mvc;
namespace AppForSEII2526.API.Controllers
{
    public class ScalesController: Controller
    {
        private readonly ApplicationDbContext _context;
        private ILogger _logger;

        public ScalesController(ApplicationDbContext context, ILogger<RepairsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Movies/GetMoviesForPurchase
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<string>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetScales(string? scaleName)
        {

            IList<string> scale = await _context.Scales
                .Where(scale => (scaleName == null || scale.Name.Contains(scaleName))) // where clause             
                .OrderBy(scale => scale.Name)
                .Select(scale => scale.Name)
                .ToListAsync();

            return Ok(scale);
        }
    }
}
