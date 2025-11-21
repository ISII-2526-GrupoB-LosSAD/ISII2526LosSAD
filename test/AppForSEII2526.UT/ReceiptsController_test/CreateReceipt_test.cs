using AppForSEII2526.API.DTOs.ReceiptDTOs;
using AppForSEII2526.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.API.Controllers;

namespace AppForSEII2526.UT.ReceiptsController_test
{
    // Clase de pruebas unitarias para el controlador ReceiptsController
    public class CreateReceipt_test : AppForSEII25264SqliteUT
    {
        //  Variables constantes usadas en los tests
        private const string _CustomerUserName1 = "Elena";
        private const string _CustomerUserSurname1 = "Navarro Martinez";
        private const string _DeliveryAddress1 = "Calle Gran Vía 14, Madrid";
        private const string _DeliveryAddress2 = "123 Main St";
        private const string _CustomerUserName2 = "Sandra";
        private const string _CustomerUserSurname2 = "Garcia Alcolea";
        private const string _Country = "España";

        // Guardamos la fecha actual para los tests
        public DateTime today = DateTime.Today;


        // Constructor: inicializa los datos base de prueba
        public CreateReceipt_test()
        {
            // Se crean diferentes tipos de dispositivos (escalas)
            var scale = new List<Scale>() {
                new Scale(){Name="Phone"},
                new Scale(){Name="Tablet"},
                new Scale(){Name="Laptop"}
            };

            // Se crean tipos de reparaciones asociadas a esas escalas
            var repair = new List<Repair>() {
                new Repair(){Name="Screen replacement", Description="Replace broken screen", Cost=150, Scale=scale[0]},
                new Repair(){Name="Battery replacement", Description="Replace old battery", Cost=80, Scale=scale[1]},
                new Repair(){Name="Keyboard repair", Description="Fix keyboard issues", Cost=120, Scale=scale[2]}
            };

            // Añadimos las escalas y reparaciones al contexto (base de datos en memoria)
            _context.AddRange(scale);
            _context.AddRange(repair);

            // Creamos un usuario de ejemplo registrado
            ApplicationUser user = new ApplicationUser("1", _CustomerUserName1, _CustomerUserSurname1, _Country);
            _context.Add(user);

            // Creamos un recibo de ejemplo vinculado a ese usuario
            var receipt = new Receipt(
                _DeliveryAddress1,            
                1,                            
                ReceiptPaymentMethodTypes.CreditCard,  
                today,                        
                150,                          
                new List<Receiptitem>(),      
                user                          
            );

            // Añadimos un ítem al recibo (por ejemplo, reparación de pantalla de iPhone X)
            receipt.Receiptitems.Add(
                new Receiptitem("iPhone X", receipt.Id, receipt, repair[0].Id, repair[0])
            );

            // Guardamos los cambios en la base de datos en memoria
            _context.Add(receipt);
            _context.SaveChanges();
        }

        //Casos de prueba con errores esperados

        public static IEnumerable<object[]> TestCasesFor_CreateReceipt()
        {
            // Lista de ítems de ejemplo que se incluyen en los recibos
            IList<ReceiptitemDTO> Receiptitems = new List<ReceiptitemDTO>() {
                new ReceiptitemDTO("Screen replacement", "Phone", 150, "iPhone X")
            };

            // Casos de prueba: recibos con errores (nombre, apellido o dirección faltantes)
            ReceiptForCreateDTO receiptNoName = new ReceiptForCreateDTO(
                _CustomerUserName2, _CustomerUserSurname1, _DeliveryAddress1,
                ReceiptPaymentMethodTypes.CreditCard, Receiptitems
            );

            ReceiptForCreateDTO receiptNoSurname = new ReceiptForCreateDTO(
                _CustomerUserName1, _CustomerUserSurname2, _DeliveryAddress1,
                ReceiptPaymentMethodTypes.CreditCard, Receiptitems
            );

            ReceiptForCreateDTO receiptNoAddress = new ReceiptForCreateDTO(
                _CustomerUserName1, _CustomerUserSurname1, null,
                ReceiptPaymentMethodTypes.CreditCard, Receiptitems
            );
            ReceiptForCreateDTO receiptIncorrectAddress = new ReceiptForCreateDTO(
                _CustomerUserName1, _CustomerUserSurname1, _DeliveryAddress2,
                ReceiptPaymentMethodTypes.CreditCard, Receiptitems
            );

            // Se definen los errores esperados para cada caso
            var allTests = new List<object[]>
            {
                new object[] { receiptNoName, "Error! UserName is not registered" },
                new object[] { receiptNoSurname, "Error! UserSurname is not registered" },
                new object[] { receiptNoAddress, "Error! Delivery address is required" },
                new object[] { receiptIncorrectAddress, "Error en la dirección de envío. Por favor, introduce una dirección válida incluyendo las palabras Calle o Avenida" },
            };

            return allTests;
        }

        //Test: casos con errores (validaciones fallidas)
        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_CreateReceipt))]
        public async Task CreateReceipt_Error_test(ReceiptForCreateDTO receiptForCreateDTO, string errorExpected)
        {
            // Arrange: se crea un mock del logger y el controlador
            var mock = new Mock<ILogger<ReceiptsController>>();
            ILogger<ReceiptsController> logger = mock.Object;
            var controller = new ReceiptsController(_context, logger);

            // Act: se ejecuta el método CreateRepair con los datos del test
            var result = await controller.CreateRepair(receiptForCreateDTO);

            // Assert:
            // Se verifica que la respuesta sea un BadRequest
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            // Se obtiene el detalle del problema (mensaje de validación)
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);
            var errorActual = problemDetails.Errors.First().Value[0];

            // Se comprueba que el mensaje devuelto comienza con el esperado
            Assert.StartsWith(errorExpected, errorActual);
        }

        //Test: caso exitoso (recibo creado correctamente)

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreateRental_Success_test()
        {
            // Arrange: se prepara el controlador con mock de logger
            var mock = new Mock<ILogger<ReceiptsController>>();
            ILogger<ReceiptsController> logger = mock.Object;
            var controller = new ReceiptsController(_context, logger);

            // Se crea un DTO de recibo válido
            ReceiptForCreateDTO receiptDTO = new ReceiptForCreateDTO(
                _CustomerUserName1,
                _CustomerUserSurname1,
                _DeliveryAddress1,
                ReceiptPaymentMethodTypes.CreditCard,
                new List<ReceiptitemDTO>()
            );

            // Se añade un ítem al recibo (reparación de pantalla)
            receiptDTO.receiptItems.Add(
                new ReceiptitemDTO("Screen replacement", "Phone", 150, "iPhone X")
            );

            // Se define el resultado esperado del recibo creado
            // (ID 2 porque ya existe uno en la base de datos)
            ReceiptDetailDTO expectedrentalDetailDTO = new ReceiptDetailDTO(
                _CustomerUserName1,
                _CustomerUserSurname1,
                _DeliveryAddress1,
                150,
                today,
                new List<ReceiptitemDTO>()
                {
                    new ReceiptitemDTO("Screen replacement", "Phone", 150, "iPhone X")
                }
            );

            // Act: se llama al método CreateRepair
            var result = await controller.CreateRepair(receiptDTO);

            // Assert:
            //Se verifica que la respuesta sea "CreatedAtActionResult"
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);

            //Se obtiene el objeto del cuerpo de la respuesta
            var actualRentalDetailDTO = Assert.IsType<ReceiptDetailDTO>(createdResult.Value);

            //Se compara el DTO esperado con el obtenido
            Assert.Equal(expectedrentalDetailDTO, actualRentalDetailDTO);
        }
    }
}

