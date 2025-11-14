using AppForSEII2526.API.DTOS.DevicesDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{
    // Controlador para operaciones relacionadas con reparaciones
    [Route("api/[controller]")]
    [ApiController]
    public class RepairsController : ControllerBase
    {
        // Contexto de base de datos para acceder a las tablas
        private readonly ApplicationDbContext _context;

        // Logger para registrar información y errores
        private readonly ILogger<RepairsController> _logger;

        // Constructor con inyección de dependencias
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

        // Método GET para obtener dispositivos a reparar con filtros opcionales
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<RepairParaRepararDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetDevicesParaRepararDTO(string? name, string? scale)
        {
            // Consulta a la BD con filtros por nombre y escala
            var device = await _context.Repairs
                .Include(d=>d.Scale)
                .Where(d => (name == null || d.Name.Contains(name)  ) && (scale == null|| d.Scale.Name.Contains(scale)))
                .Select(d => new RepairParaRepararDTO(d.Id, d.Name, d.Scale.Name, d.Description, d.Cost
                ))
                .ToListAsync();
            // Retorna lista de dispositivos encontrados
            return Ok(device);
        }
    }
}
