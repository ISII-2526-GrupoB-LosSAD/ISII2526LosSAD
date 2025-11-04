using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ReceiptDTOs;
using AppForSEII2526.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.ReceiptsController_test
{
    public class GetReceipt_test : AppForSEII25264SqliteUT
    {
        public GetReceipt_test()
        {
            // Inicialización de datos de prueba específicos para los tests de ReceiptsController
            var scale = new List<Scale>() {
                new Scale(){Name="Phone"},
                new Scale(){Name="Tablet"},
                new Scale(){Name="Laptop"}
            };
            var repair = new List<Repair>() {
            new Repair(){Name="Screen replacement",Description="Replace broken screen",Cost=150,Scale=scale[0]},
            new Repair(){Name="Battery replacement",Description="Replace old battery",Cost=80,Scale=scale[1]},
            new Repair(){Name="Keyboard repair",Description="Fix keyboard issues",Cost=120,Scale=scale[2]}
            };

            // Añadimos los datos al contexto (la base de datos en memoria que usamos para testear)
            _context.AddRange(scale);
            _context.AddRange(repair);

            ApplicationUser user = new ApplicationUser("Elena", "Navarro Martinez", "elena@uclm.es");

            // Creamos un recibo de ejemplo con una fecha, dirección, método de pago, precio y el usuario anterior
            var receipt = new Receipt("123 Main St", 1, ReceiptPaymentMethodTypes.CreditCard, new DateTime(2011, 10, 20), 150, new List<Receiptitem>(),user);
            // Añadimos un ítem dentro del recibo (por ejemplo, una reparación de pantalla para un iPhone X)
            receipt.Receiptitems.Add(new Receiptitem("iPhone X", receipt.Id, receipt, repair[0].Id, repair[0]));


            // Guardamos el usuario y el recibo en la base de datos
            _context.Users.Add(user);
            _context.Add(receipt);
            _context.SaveChanges();
        }

        // Primer test: se prueba que si se busca un recibo con un id que no existe, se devuelve NotFound
        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetReceipt_ById_OK()
        {
            // Arrange: se crea un mock del logger para pasárselo al controlador
            var mock = new Mock<ILogger<ReceiptsController>>();
            ILogger<ReceiptsController> logger = mock.Object;

            // Se crea el controlador con el contexto de prueba
            var controller = new ReceiptsController(_context, logger);

            // Act: se intenta obtener un recibo con id 0 (que no existe)
            var result = await controller.GetReceipt(0);
            // Assert: comprobamos que el resultado sea un NotFoundResult
            Assert.IsType<NotFoundResult>(result);
        }

        // Segundo test: se comprueba que el controlador devuelva correctamente un recibo existente
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetReceipt_Found_test()
        {
            // Arrange: se prepara el logger (mock) y el controlador
            var mock = new Mock<ILogger<ReceiptsController>>();
            ILogger<ReceiptsController> logger = mock.Object;
            var controller = new ReceiptsController(_context, logger);

            // Se crea el DTO esperado (lo que debería devolver el método del controlador)
            var expectedReceipt = new ReceiptDetailDTO("Elena", "Navarro Martinez", "123 Main St", 150, new DateTime(2011, 10, 20), new List<ReceiptitemDTO>());

            // Se añade al DTO el ítem que esperamos que tenga el recibo
            expectedReceipt.Receiptitems.Add(new ReceiptitemDTO("Screen replacement", "Phone",150, "iPhone X"));

            // Act: se llama al método del controlador con el id del recibo que sí existe (1)
            var result = await controller.GetReceipt(1);

            //Assert: comprobamos que la respuesta sea OK y que los datos sean los esperados
            var okResult = Assert.IsType<OkObjectResult>(result);
            var receiptDTOActual = Assert.IsType<ReceiptDetailDTO>(okResult.Value);

            // Se compara el DTO esperado con el recibido
            Assert.Equal(expectedReceipt, receiptDTOActual);
        }
    }
}
