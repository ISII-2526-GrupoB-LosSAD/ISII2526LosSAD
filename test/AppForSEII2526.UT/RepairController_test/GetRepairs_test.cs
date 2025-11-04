using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOS.DevicesDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.RepairController_test
{
    public class GetRepairs_test : AppForSEII25264SqliteUT
    {
        public GetRepairs_test()
        {
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
            //ApplicationUser user = new ApplicationUser("Elena", "Navarro Martinez", "elena@uclm.es");

            //var receipt = new Receipt("123 Main St", 1, ReceiptPaymentMethodTypes.CreditCard, DateTime.Now, 350, new List<Receiptitem>());
            //var receiptitem = new Receiptitem("iPhone X", receipt.Id, receipt, repair[0].Id, repair[0]);

            // Se añaden los datos al contexto (base de datos en memoria)
            _context.AddRange(scale);
            _context.AddRange(repair);
            _context.SaveChanges();
        }

        // Este método devuelve varios casos de prueba que se usarán en el test con [MemberData]
        public static IEnumerable<object[]> TestCaseFor_GetRepairParaReparar_OK()
        {
            // Lista de reparaciones esperadas (lo que debería devolver el controlador)
            var repairDTOs = new List<RepairParaRepararDTO>() {
                new RepairParaRepararDTO(1,"Screen replacement","Phone","Replace broken screen",150),
                new RepairParaRepararDTO(2,"Battery replacement","Tablet","Replace old battery",80),
                new RepairParaRepararDTO(3,"Keyboard repair","Laptop","Fix keyboard issues",120)
            };
            // Creamos varios conjuntos esperados según los filtros que se probarán
            var repairDTOsTC1 = new List<RepairParaRepararDTO>() { repairDTOs[0], repairDTOs[1], repairDTOs[2] };
            var repairDTOsTC2 = new List<RepairParaRepararDTO>() { repairDTOs[0]};
            var repairDTOsTC3 = new List<RepairParaRepararDTO>() { repairDTOs[1]};

            // Aquí se definen los parámetros con los que se va a llamar al método y el resultado esperado
            var allTests =new List<object[]>
            {
                new object[] { null, null , repairDTOsTC1 },
                new object[] { "Screen", null , repairDTOsTC2 },
                new object[] { null , "Tablet", repairDTOsTC3 },
            };

            return allTests;

        }

        // Test que comprueba que el método GetDevicesParaRepararDTO funciona correctamente con distintos filtros
        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCaseFor_GetRepairParaReparar_OK))]
        public async Task GetRepairParaReparar_OK(string? name,  string? scale, List<RepairParaRepararDTO> expectedRepairs)
        {
            // Arrange: se crea el controlador con el contexto de base de datos en memoria
            var controller = new RepairsController(_context, null);
            // Act: se llama al método a testear
            var result = await controller.GetDevicesParaRepararDTO(name, scale);
            //Assert: se comprueba que el resultado es correcto
            var okResult = Assert.IsType<OkObjectResult>(result);
            // Extraemos los datos devueltos por el controlador y verificamos que sean una lista de DTOs
            var repairDTOsActual = Assert.IsType<List<RepairParaRepararDTO>>(okResult.Value);
            // Finalmente comparamos que la lista devuelta sea igual a la esperada
            Assert.Equal(expectedRepairs, repairDTOsActual);
        }
    }
}
