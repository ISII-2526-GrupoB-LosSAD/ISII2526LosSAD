using AppForSEII2526.API.DTOs.PurchaseDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        //used to enable your controller to access to the database 
        private readonly ApplicationDbContext _context;
        //used to log any information when your system is running
        private readonly ILogger<PurchaseController> _logger;

        public PurchaseController(ApplicationDbContext context, ILogger<PurchaseController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PurchaseDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetPurchase(int id)
        {
            if (_context.Purchase == null)
            {
                _logger.LogError("Error: Purchases table does not exist");
                return NotFound();
            }
            var purchase = await _context.Purchase
                .Where(r => r.Id == id)
                    .Include(r => r.ApplicationUser)
                    .Include(r => r.PurchaseItems) 
                        .ThenInclude(ri => ri.Device) 
                            .ThenInclude(dev => dev.Model)
                .Select(r => new PurchaseDetailDTO( 
                    r.ApplicationUser.CustomerUserName, r.DeliveryAddress, r.ReceiptDate, r.TotalPrice, r.TotalQuantity, 
                    r.PurchaseItems.Select(ri => new PurchaseItemDTO( 
                        ri.Device.Description,
                        ri.Device.priceForPurchase,
                        ri.Device.quauntityForPurchase,
                        ri.Device.Brand,
                        ri.Device.Color,
                        ri.Device.Model 
                    )).ToList<PurchaseItemDTO>() 
                ))
                .FirstOrDefaultAsync();
            if (purchase == null)
            {
                _logger.LogWarning($"Error: Rental with id {id} does not exist");
                return NotFound();
            }
            return Ok(purchase);
        }

    }   
    
}
