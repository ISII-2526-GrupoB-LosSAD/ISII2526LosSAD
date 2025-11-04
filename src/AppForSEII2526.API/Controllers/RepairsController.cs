using AppForSEII2526.API.DTOS.DevicesDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepairsController : ControllerBase
    {
        //used to enable your controller to access to the database
        private readonly ApplicationDbContext _context;

        //used to log any information when your system is running
        private readonly ILogger<RepairsController> _logger;

        public RepairsController(ApplicationDbContext context, ILogger<RepairsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(decimal), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> ComputeDivision(decimal op1, decimal op2)
        {
            if (op2 == 0)
            {
                _logger.LogError($"{DateTime.Now} Exception: op2=0, division by 0");
                return BadRequest("op2 must be different from 0");
            }
            decimal result = decimal.Round(op1 / op2, 2);
            return Ok(result);
        }
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<RepairParaRepararDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetDevicesParaRepararDTO(string? name, string? scale)
        {
            var device = await _context.Repairs
                .Include(d=>d.Scale)
                .Where(d => (name == null || d.Name.Contains(name)  ) && (scale == null|| d.Scale.Name.Contains(scale)))
                .Select(d => new RepairParaRepararDTO(d.Id, d.Name, d.Scale.Name, d.Description, d.Cost
                ))
                .ToListAsync();
            return Ok(device);
        }
    }
}
