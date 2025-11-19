using AppForSEII2526.API.DTOs.PurchaseDTO;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")] // la url base
    [ApiController] //indica que este controlador gestiona peticiones HTTP y valida automáticamente el modelo
    public class PurchaseController : ControllerBase // hereda de controllerbase, que proporciona funcionalidades básicas para manejar peticiones HTTP
    {
        // acceso al contexto de base de datos (Entity Framework Core).
        private readonly ApplicationDbContext _context;
        // registro de logs para seguimiento y depuración.
        private readonly ILogger<PurchaseController> _logger;

        public PurchaseController(ApplicationDbContext context, ILogger<PurchaseController> logger) // constructor del controlador
        {
            _context = context; // inyección de dependencia del contexto de base de datos
            _logger = logger; // inyección de dependencia del logger
        }

        [HttpGet] // acción HTTP GET, que se utiliza para obtener datos del servidor
        [Route("[action]")] // la ruta de la acción será api/Purchase/GetPurchase
        [ProducesResponseType(typeof(PurchaseDetailDTO), (int)HttpStatusCode.OK)] // indica que la respuesta exitosa devolverá un DTO de detalles de compra
        [ProducesResponseType((int)HttpStatusCode.NotFound)] // indica que la respuesta puede devolver un estado 404 Not Found
        public async Task<ActionResult> GetPurchase(int id) // acción para obtener los detalles de una compra por su ID
        {
            if (_context.Purchases == null) // verifica si la tabla de compras existe
            {
                _logger.LogError("Error: Purchases table does not exist"); // registra un error si no existe
                return NotFound(); // devuelve un estado 404 Not Found
            }
            var purchase = await _context.Purchases // consulta la tabla de compras
                .Where(r => r.Id == id) // filtra por el ID proporcionado
                    .Include(r => r.ApplicationUser) // incluye los datos del usuario asociado
                    .Include(r => r.PurchaseItems) // incluye los elementos de la compra
                        .ThenInclude(ri => ri.Device) //    incluye los dispositivos asociados a cada elemento de la compra
                            .ThenInclude(dev => dev.Model) // incluye el modelo de cada dispositivo
                .Select(r => new PurchaseDetailDTO(  //mapea los datos a un DTO para la respuesta
                    r.ApplicationUser.CustomerUserName, r.DeliveryAddress, r.ReceiptDate, r.TotalPrice, r.TotalQuantity, 
                    r.PurchaseItems.Select(ri => new PurchaseItemDTO( // mapea cada elemento de la compra a un DTO
                        ri.Device.Description,
                        ri.Device.priceForPurchase,
                        ri.Device.quauntityForPurchase,
                        ri.Device.Brand,
                        ri.Device.Color,
                        ri.Device.Model.NameModel 
                    )).ToList<PurchaseItemDTO>() // convierte la colección de elementos de la compra en una lista de DTOs
                ))
                .FirstOrDefaultAsync(); // obtiene el primer resultado o null si no existe
            if (purchase == null) // verifica si se encontró la compra
            {
                _logger.LogWarning($"Error: Rental with id {id} does not exist"); // registra una advertencia si no se encontró
                return NotFound(); // devuelve un estado 404 Not Found
            }
            return Ok(purchase); // devuelve un estado 200 OK con los detalles de la compra
        }

        [HttpPost] // acción HTTP POST, que se utiliza para enviar datos al servidor
        [Route("[action]")] // la ruta de la acción será api/Purchase/CreatePurchase
        [ProducesResponseType((int)HttpStatusCode.Created)] // indica que la respuesta exitosa devolverá un estado 201 Created
        [ProducesResponseType((int)HttpStatusCode.BadRequest)] // indica que la respuesta puede devolver un estado 400 Bad Request
        [ProducesResponseType((int)HttpStatusCode.Conflict)] // indica que la respuesta puede devolver un estado 409 Conflict

        public async Task<ActionResult> CreatePurchase(PurchaseForCreateDTO purchaseForCreate) // acción para crear una nueva compra
        {
            // validaciones iniciales, asegurándose de que los datos proporcionados sean correctos

            if (purchaseForCreate.PurchaseItems.Count == 0) // verifica si se proporcionaron elementos de compra
                ModelState.AddModelError("PurchaseItems", "Error! You must include at least one movie to be rented"); // agrega un error al estado del modelo si no hay elementos

            if (purchaseForCreate.DeliveryAddress == null) // verifica si se proporcionó una dirección de entrega
                ModelState.AddModelError("Delivery", "Error! Delivery address is required");

            // Buscar usuario por nombre y apellido
            var user = _context.Users.FirstOrDefault(au => au.CustomerUserName == purchaseForCreate.CustomerUserName); // busca el usuario por nombre

            var surnameUser = _context.Users.FirstOrDefault(au => au.CustomerUserSurname == purchaseForCreate.UserSurname); // busca el usuario por apellido

            if (user == null) // verifica si el usuario existe
                ModelState.AddModelError("UserName", "Error! UserName is not registered");
            if (surnameUser == null) // verifica si el usuario existe
                ModelState.AddModelError("UserSurname", "Error! UserSurname is not registered");

            if (ModelState.ErrorCount > 0) // verifica si hay errores en el estado del modelo
                return BadRequest(new ValidationProblemDetails(ModelState)); // devuelve un estado 400 Bad Request con los detalles de validación

            var devicesTitles = purchaseForCreate.PurchaseItems.Select(ri => ri.Model).ToList<string>(); // obtiene los títulos de los dispositivos a comprar

            var devices = _context.Devices.Include(m => m.PurchaseItems) // consulta la tabla de dispositivos
               
               .Include(m => m.Model) // incluye el modelo de cada dispositivo
                .Where(m => devicesTitles.Contains(m.Model.NameModel)) // filtra los dispositivos que coinciden con los títulos proporcionados
                .ToList(); // convierte la consulta en una lista

            Purchase purchase = new Purchase(purchaseForCreate.DeliveryAddress, purchaseForCreate.PaymentMethod, user); // crea una nueva instancia de compra


            foreach (var item in purchaseForCreate.PurchaseItems) // itera sobre cada elemento de compra proporcionado
            {
                var purchaseItem = devices.FirstOrDefault(m => m.Model.NameModel == item.Model); // busca el dispositivo correspondiente en la lista obtenida de la base de datos

                //we must check that there is enough quantity to be rented in the database
                if ((purchaseItem == null)) // verifica si el dispositivo existe
                {
                    ModelState.AddModelError("RentalItems", $"Error! Movie titled '{item.Model}' is not available for being rented from the database"); // agrega un error si el dispositivo no existe
                }
                else
                {
                    var newItem = new PurchaseItem(item.Description, item.Price, item.Quantity, purchaseItem, purchase); // crea un nuevo elemento de compra
                    newItem.DeviceId = purchaseItem.Id; // asigna el ID del dispositivo al elemento de compra
                    purchase.PurchaseItems.Add(newItem); // agrega el elemento de compra a la compra

                }
            }

            purchase.ReceiptDate = DateTime.Today; //  ESTABLECE LA FECHA ACTUAL
            purchase.TotalQuantity = purchase.PurchaseItems.Sum(item => item.Quantity); // calcula la cantidad total de elementos en la compra
            purchase.TotalPrice = purchase.PurchaseItems.Sum(item => item.Quantity * item.Price); // calcula el precio total de la compra

            // esto es para validar si se han agregado errores en el proceso anterior
            if (ModelState.ErrorCount > 0) // verifica si hay errores en el estado del modelo
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            _context.Add(purchase); // agrega la compra al contexto de la base de datos

            try
            {
                //we store in the database both rental and its rentalitems
                await _context.SaveChangesAsync(); // guarda los cambios en la base de datos de forma asíncrona
            }
            catch (Exception ex) // captura cualquier excepción que ocurra durante el guardado
            {
                _logger.LogError(ex.Message); // registra el mensaje de error
                ModelState.AddModelError("Rental", $"Error! There was an error while saving your rental, plese, try again later");
                return Conflict("Error" + ex.Message); // devuelve un estado 409 Conflict con el mensaje de error

            }

            //it returns rentalDetail
            var purchaseDetail = new PurchaseDetailDTO( // crea un DTO de detalles de compra para la respuesta
                purchaseForCreate.CustomerUserName, 
                purchaseForCreate.DeliveryAddress, 
                purchase.ReceiptDate,
                purchase.TotalPrice,
                purchase.TotalQuantity,
                purchaseForCreate.PurchaseItems);

            return CreatedAtAction("GetPurchase", new { id = purchase.Id }, purchaseDetail); // devuelve un estado 201 Created con los detalles de la compra creada

        }
    }

}



