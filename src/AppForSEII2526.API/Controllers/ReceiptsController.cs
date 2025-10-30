using AppForSEII2526.API.DTOs.ReceiptDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptsController : ControllerBase
    {
        //used to enable your controller to access to the database 
        private readonly ApplicationDbContext _context;
        //used to log any information when your system is running
        private readonly ILogger<ReceiptsController> _logger;

        public ReceiptsController(ApplicationDbContext context, ILogger<ReceiptsController> logger)
        {
            _context = context;
            _logger = logger;
        }

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
            var receipt = await _context.Receipts
                .Where(r => r.Id == id)
                    .Include(r => r.ApplicationUser)
                    .Include(r => r.Receiptitems)
                        .ThenInclude(ri => ri.Repair)
                            .ThenInclude(rep => rep.Scale)
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
            if (receipt == null)
            {
                _logger.LogWarning($"Error: Rental with id {id} does not exist");
                return NotFound();
            }
            return Ok(receipt);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ReceiptDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult>CreateRepair(ReceiptForCreateDTO receiptForCreate)
        {
            if (receiptForCreate.receiptItems.Count == 0)
                ModelState.AddModelError("RentalItems", "Error! You must include at least one movie to be rented");

            var user = _context.Users.FirstOrDefault(au => au.CustomerUserName == receiptForCreate.CustomerUserName);
            if (user == null)
                ModelState.AddModelError("RentalApplicationUser", "Error! UserName is not registered");

            // Si hay errores
            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            var repairNames = receiptForCreate.receiptItems.Select(ri => ri.Name).ToList();
            var repair= _context.Repairs.Include(r=>r.Receiptitems)
                .Include(r=>r.Scale)
                .Where(r => repairNames.Contains(r.Name))
            //creamos un tipo anónimo para traer solo los datos necesarios
                .ToList();
            Receipt receipt = new Receipt(receiptForCreate.DeliveryAddress,DateTime.Now,receiptForCreate.receiptPaymentMethodTypes, new List<Receiptitem>(), user);

            // Crear los receipt items y añadirlos al receipt
            foreach (var item in receiptForCreate.receiptItems)
            {
                var repairItem = repair.FirstOrDefault(r => r.Name == item.Name);
                if (repairItem == null)
                {
                    ModelState.AddModelError("ReceiptItems", $"The repair '{item.Name}' does not exist.");
                }
                else
                {
                    receipt.Receiptitems.Add(new Receiptitem(item.Model,receipt,repairItem));

                    receipt.TotalPrice = receipt.TotalPrice + item.Cost;
                }
            }
            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }
            
            _context.Add(receipt);
            
            try
            {
                
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ModelState.AddModelError("ReceiptCreation", "There was an error while saving your rental, plese, try again later");
                return Conflict("Error" + ex.Message);
            }
            // Devolver el receipt creado
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

            return CreatedAtAction("GetReceipt", new { id = receipt.Id }, receiptDetail);


        }
    }
}
