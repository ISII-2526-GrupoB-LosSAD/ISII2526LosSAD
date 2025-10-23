using AppForSEII2526.API.DTOs.ReceiptDTOs;
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
    }
}
