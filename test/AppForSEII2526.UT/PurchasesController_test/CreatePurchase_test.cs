using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.PurchaseDTO;
using AppForSEII2526.API.Models; // Necesario para Model, Device, ApplicationUser, PurchasePaymentMethodTypes
using Microsoft.AspNetCore.Mvc; // Para BadRequestObjectResult, CreatedAtActionResult, ValidationProblemDetails
using Microsoft.Extensions.Logging; // Para ILogger
using Moq; // Para Mock<ILogger>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit; // Para [Fact], [Theory], [Trait], Assert


namespace AppForSEII2526.UT.PurchasesController_test
{
    // Clase de pruebas unitarias para el controlador PurchasesController
    public class CreatePurchase_test : AppForSEII25264SqliteUT
    {
        //  Variables constantes usadas en los tests
        private const string _CustomerUserName1 = "Elena";
        private const string _CustomerUserSurname1 = "Navarro Martinez";
        private const string _DeliveryAddress1 = "123 Main St";
        private const string _CustomerUserName2 = "Sandra";
        private const string _CustomerUserSurname2 = "Garcia Alcolea";
        private const string _Country = "España";

        // Guardamos la fecha actual para los tests
        public DateTime today = DateTime.Today;

        //Constructor: inicializa los datos base para los tests
        public CreatePurchase_test()
        {
            // Se crean modelos y dispositivos de ejemplo
            var models = new List<Model>() {
                new Model{ NameModel= "Model A" },
                new Model{ NameModel= "Model B" },
            };

            // Dispositivos asociados a los modelos anteriores
            var devices = new List<Device>(){ // esto crea 3 dispositivos
                new Device{ Name= "Device A1", Quality= "High", Year=2022, Brand="BrandA", Color="Black", Description="DescriptionA", priceForPurchase=500, priceForRent=50, quauntityForPurchase=10, quauntityForRent=5, Model=models[0]}, // ID 1
                new Device{ Name= "Device B1", Quality= "Medium", Year=2021, Brand="BrandB", Color="White", Description="DescriptionB", priceForPurchase=300, priceForRent=30, quauntityForPurchase=8, quauntityForRent=4, Model=models[1]}, // ID 2
                new Device{ Name= "Device A2", Quality= "Low", Year=2020, Brand="BrandA", Color="Blue", Description="DescriptionC", priceForPurchase=200, priceForRent=20, quauntityForPurchase=5, quauntityForRent=2, Model=models[0]},
            };

            // Se añaden los datos al contexto (base de datos en memoria)
            _context.AddRange(models);
            _context.AddRange(devices);

            // Se crea un usuario de ejemplo registrado
            ApplicationUser user = new ApplicationUser(_CustomerUserName1, _CustomerUserSurname1, _Country);

            // Se añade el usuario al contexto
            _context.Add(user);

            // Se crea una compra de ejemplo asociada al usuario
            var purchase = new Purchase(
                _DeliveryAddress1, 1, 500, 2, PurchasePaymentMethodTypes.CreditCard, today, new List<PurchaseItem>(), user
            );

            // Se añade un ítem de compra asociado a la compra y al dispositivo
            purchase.PurchaseItems.Add(
                new PurchaseItem("Device A1", purchase.Id, purchase, devices[0].Id, devices[0], 2, 500)
            );

            // Se guarda la compra en el contexto
            _context.Add(purchase);
            _context.SaveChanges();
        }

        // --- 1. Casos de Prueba con Errores ---
        public static IEnumerable<object[]> TestCasesFor_CreatePurchase() // Datos de prueba para casos con errores
        {
            // Lista de ítems de ejemplo válidos
            IList<PurchaseItemDTO> ValidPurchaseitems1 = new List<PurchaseItemDTO>() {
                new PurchaseItemDTO("DescriptionA" ,500, 2, "BrandA", "Black", "Model A")
            }; 
            IList<PurchaseItemDTO> ValidPurchaseitems2 = new List<PurchaseItemDTO>() {
                new PurchaseItemDTO("DescriptionA" ,500, 2, "Huawei", "Black", "Model B")
            };


            // Casos de prueba:

            // Caso 1: Nombre de usuario no registrado
            PurchaseForCreateDTO purchaseNoName = new PurchaseForCreateDTO(
                _CustomerUserName2, _CustomerUserSurname1, _DeliveryAddress1,
                PurchasePaymentMethodTypes.CreditCard
            )
            { PurchaseItems = ValidPurchaseitems1 }; // <<-- ASIGNACIÓN AÑADIDA

            // Caso 2: Apellido de usuario no registrado
            PurchaseForCreateDTO purchaseNoSurname = new PurchaseForCreateDTO(
                _CustomerUserName1, _CustomerUserSurname2, _DeliveryAddress1,
                PurchasePaymentMethodTypes.CreditCard
            )
            { PurchaseItems = ValidPurchaseitems1 }; // <<-- ASIGNACIÓN AÑADIDA

            // Caso 3: Dirección de entrega nula
            PurchaseForCreateDTO purchaseNoAddress = new PurchaseForCreateDTO(
                _CustomerUserName1, _CustomerUserSurname1, null,
                PurchasePaymentMethodTypes.CreditCard
            )
            { PurchaseItems = ValidPurchaseitems1 }; // <<-- ASIGNACIÓN AÑADIDA


            //modificacion examen
            PurchaseForCreateDTO purchaseMarca = new PurchaseForCreateDTO(
               _CustomerUserName1, _CustomerUserSurname1, _DeliveryAddress1,
                PurchasePaymentMethodTypes.CreditCard
            )
            {PurchaseItems = ValidPurchaseitems2}; // <<- nueva asignacion examen
           

            // Se definen los errores esperados para cada caso
            var allTests = new List<object[]>
            {
                new object[] { purchaseNoName, "Error! UserName is not registered" },
                new object[] { purchaseNoSurname, "Error! UserSurname is not registered" },
                new object[] { purchaseNoAddress, "Error! Delivery address is required" },
                new object[] { purchaseMarca, "Error, marca o modelo contiene Xiaomi o Huawei "}
            };

            return allTests; // Devuelve la lista de casos de prueba
        }

        // Test, casos con errores (validaciones fallidas)
        [Theory] // Indica que es un test parametrizado, se ejecutará varias veces con distintos datos
        [Trait("LevelTesting", "Unit Testing")] // Categoría del test
        [Trait("Database", "WithoutFixture")] 
        [MemberData(nameof(TestCasesFor_CreatePurchase))] // Se obtienen los datos de prueba del método estático
        public async Task CreatePurchase_ErrorCases(PurchaseForCreateDTO purchaseToCreate, string expectedErrorMessage) // Parámetros del test
        {
            // Arrange: se crea un mock del logger y el controlador
            var mock = new Moq.Mock<ILogger<PurchaseController>>();
            ILogger<PurchaseController> logger = mock.Object;
            var controller = new PurchaseController(_context, logger);

            // Act: se intenta crear la compra
            var result = await controller.CreatePurchase(purchaseToCreate);

            // Assert: se verifica que el resultado sea un BadRequest
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            // Se obtiene el detalle del problema (mensaje de validación)
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);
            var errorActual = problemDetails.Errors.First().Value[0];

            // Se comprueba que el mensaje devuelto comienza con el esperado
            Assert.StartsWith(expectedErrorMessage, errorActual);
        }

        // --- 2. Caso de Prueba Exitoso ---
        [Fact] // Indica que es un test simple (no parametrizado), se ejecutará una sola vez
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreatePurchase_SuccessfulCase()
        {
            // Arrange: se crea un mock del logger y el controlador
            var mock = new Mock<ILogger<PurchaseController>>();
            ILogger<PurchaseController> logger = mock.Object;
            var controller = new PurchaseController(_context, logger);

            // DTO de entrada
            PurchaseForCreateDTO purchaseDTO = new PurchaseForCreateDTO(
                _CustomerUserName1,
                _CustomerUserSurname1,
                _DeliveryAddress1,
                PurchasePaymentMethodTypes.CreditCard,
                new List<PurchaseItemDTO>()
            );

            // Ítem de compra (entrada y salida esperada)
            var purchaseItem = new PurchaseItemDTO("DescriptionA", 500, 2, "BrandA", "Black", "Model A");

            // Se añade el ítem al DTO de entrada
            purchaseDTO.PurchaseItems.Add(purchaseItem);

            // Se define el resultado esperado del detalle de compra (PurchaseDetailDTO)
            // Constructor asumido: (UserName, UserSurname, DeliveryAddress, Date, TotalPrice, TotalQuantity, List<Items>)
            PurchaseDetailDTO expectedpurchaseDetailDTO = new PurchaseDetailDTO(
                _CustomerUserName1,
                _DeliveryAddress1,
                today,
                1000, // TotalPrice = 500 * 2
                2,    // TotalQuantity = 2
                new List<PurchaseItemDTO>()

                {
                    // Ítem esperado en la respuesta
                    new PurchaseItemDTO("DescriptionA", 500, 2, "BrandA", "Black", "Model A")

                }
            );

            // Act: se llama al método CreatePurchase
            var result = await controller.CreatePurchase(purchaseDTO);

            // Assert: se verifica que el resultado sea CreatedAtActionResult
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualPurchaseDetailDTO = Assert.IsType<PurchaseDetailDTO>(createdResult.Value);

            // Se compara el DTO esperado con el obtenido
            Assert.Equal(expectedpurchaseDetailDTO, actualPurchaseDetailDTO);
        }
    }
}