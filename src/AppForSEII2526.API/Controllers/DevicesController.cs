using AppForSEII2526.API.DTOs.DevicesDTO;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly ILogger<DevicesController> _logger;
        private readonly ApplicationDbContext _context;

        public DevicesController(ILogger<DevicesController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
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
        [ProducesResponseType(typeof(IList<DevicesParaComprarDTO>), (int)HttpStatusCode.OK)]

        public async Task<ActionResult> GetDevicesParaComprarDTO(string? name, string? color)
        {
            var devices = await _context.Devices
                .Where(d => (name == null || d.Name.Contains(name)) && (color == null || d.Color.Contains(color)))
                .Select(d => new DevicesParaComprarDTO( d.Id, d.Name, d.priceForPurchase, d.Brand, d.Model.NameModel, d.Color ))
                .ToListAsync();
            return Ok(devices);

        }


    }
}
