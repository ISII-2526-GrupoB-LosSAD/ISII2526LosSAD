using AppForSEII2526.API.DTOs.ReceiptDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace AppForSEII2526.API.Controllers
{
    // Controlador para manejar operaciones relacionadas con recibos
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptsController : ControllerBase
    {
        // Contexto de la base de datos para acceder a las tablas
        private readonly ApplicationDbContext _context;
        // Logger para registrar errores e información útil
        private readonly ILogger<ReceiptsController> _logger;

        // Constructor con inyección de dependencias
        public ReceiptsController(ApplicationDbContext context, ILogger<ReceiptsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Método GET para obtener un recibo por ID
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ReceiptDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetReceipt(int id)
        {
            if (_context.Receipts == null)
            {
                _logger.LogError("Error: Receipts table does not exist");
                return NotFound();
            }

            // Consulta el recibo y carga sus relaciones
            var receipt = await _context.Receipts
                .Where(r => r.Id == id)
                    .Include(r => r.ApplicationUser)
                    .Include(r => r.Receiptitems)
                        .ThenInclude(ri => ri.Repair)
                            .ThenInclude(rep => rep.Scale)

                // Proyección a DTO para enviar solo la información necesaria
                .Select(r => new ReceiptDetailDTO(
                    r.ApplicationUser.CustomerUserName, r.ApplicationUser.CustomerUserSurname, r.DeliveryAddress, r.TotalPrice, r.ReceiptDate
                    , r.Receiptitems.Select(ri => new ReceiptitemDTO(
                        ri.Repair.Name,
                        ri.Repair.Scale.Name,
                        ri.Repair.Cost,
                        ri.Model
                    )).ToList<ReceiptitemDTO>()
                ))
                .FirstOrDefaultAsync();

            // Si no existe, devolver NotFound
            if (receipt == null)
            {
                _logger.LogWarning($"Error: Rental with id {id} does not exist");
                return NotFound();
            }
            // Retornar el recibo encontrado
            return Ok(receipt);
        }

        // Método POST para crear un nuevo recibo
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ReceiptDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateRepair(ReceiptForCreateDTO receiptForCreate)
        {
            // Validaciones iniciales de modelo
            if (receiptForCreate.receiptItems.Count == 0)
                ModelState.AddModelError("ReceiptItems", "Error! You must include at least one receipt to be repeared");

            if (receiptForCreate.DeliveryAddress == null) { 
                ModelState.AddModelError("DeliveryAddress", "Error! Delivery address is required"); }
            else if (!(receiptForCreate.DeliveryAddress.Contains("Calle") || receiptForCreate.DeliveryAddress.Contains("Avenida")))
            {
                ModelState.AddModelError("DeliveryAddress", "Error en la dirección de envío. Por favor, introduce una dirección válida incluyendo las palabras Calle o Avenida");
            }
            // Buscar usuario por nombre y apellido
            var user = _context.Users.FirstOrDefault(au => au.CustomerUserName == receiptForCreate.CustomerUserName);
            var surnameUser = _context.Users.FirstOrDefault(au => au.CustomerUserSurname == receiptForCreate.CustomerUserSurname);
            if (user == null)
                ModelState.AddModelError("ReceiptApplicationUser", "Error! UserName is not registered");
            if (surnameUser == null)
                ModelState.AddModelError("ReceiptApplicationUser", "Error! UserSurname is not registered");

            // Si hay errores, devolver BadRequest
            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            // Obtener los nombres de las reparaciones enviadas
            var repairNames = receiptForCreate.receiptItems.Select(ri => ri.Name).ToList();
            // Consulta para obtener las reparaciones existentes
            var repair = _context.Repairs.Include(r=>r.Receiptitems)
                .Include(r=>r.Scale)
                .Where(r => repairNames.Contains(r.Name))
            //creamos un tipo anónimo para traer solo los datos necesarios
                .ToList();

            // Crear el objeto Receipt
            Receipt receipt = new Receipt(receiptForCreate.DeliveryAddress,DateTime.Today,receiptForCreate.receiptPaymentMethodTypes, new List<Receiptitem>(), user);


            // Crear los receipt items y añadirlos al receipt
            foreach (var item in receiptForCreate.receiptItems)
            {
                var repairItem = repair.FirstOrDefault(r => r.Name == item.Name);

                // Validar existencia de la reparación
                if (repairItem == null)
                {
                    ModelState.AddModelError("ReceiptItems", $"The repair '{item.Name}' does not exist.");
                }
                else
                {
                    // Agregar el recibo-item a la lista
                    receipt.Receiptitems.Add(new Receiptitem(item.Model,receipt,repairItem));

                    // Sumar al precio total
                    receipt.TotalPrice = receipt.TotalPrice + item.Cost;
                }
            }

            // Validar nuevamente por si se agregaron errores
            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // Agregar recibo al contexto
            _context.Add(receipt);
            
            try
            {
                // Guardar cambios
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Registrar el error y devolver conflicto
                _logger.LogError(ex.Message);
                ModelState.AddModelError("ReceiptCreation", "There was an error while saving your rental, plese, try again later");
                return Conflict("Error" + ex.Message);
            }
            // Crear DTO de respuesta
            var receiptDetail = new ReceiptDetailDTO(
                user.CustomerUserName,
                user.CustomerUserSurname,
                receipt.DeliveryAddress,
                receipt.TotalPrice,
                receipt.ReceiptDate,
                receipt.Receiptitems.Select(ri => new ReceiptitemDTO(
                    ri.Repair.Name,
                    ri.Repair.Scale.Name,
                    ri.Repair.Cost,
                    ri.Model
                )).ToList()
            );
            //Devolver el recibo creado
            return CreatedAtAction("GetReceipt", new { id = receipt.Id }, receiptDetail);


        }
    }
}
