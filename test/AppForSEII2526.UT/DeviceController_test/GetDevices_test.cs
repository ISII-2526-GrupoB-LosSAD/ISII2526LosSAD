using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.DevicesDTO;
using AppForSEII2526.API.DTOS.DevicesDTO;
using AppForSEII2526.UT;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AppForSEII2526.UT.DevicesController_test
{
    public class GetDevices_test : AppForSEII25264SqliteUT
    {
        public GetDevices_test()
        {
            var models = new List<Model>()
            {
                new Model { NameModel = "Galaxy S Series" },
                new Model { NameModel = "iPhone 15" },
                new Model { NameModel = "Pixel 8" }
            };
            var devices = new List<Device>()
{
                new Device {
                    Name = "Galaxy S24 Ultra",
                    Brand = "Samsung",
                    Color = "Black",
                    Description = "High-end Android smartphone with dynamic AMOLED display",
                    Quality = "A+",
                    Year = 2023,
                    Model = models[0],
                    priceForPurchase = 1199.99
                },
                new Device {
                    Name = "iPhone 15 Pro Max",
                    Brand = "Apple",
                    Color = "Silver",
                    Description = "Flagship iOS smartphone with advanced camera system",
                    Quality = "A+",
                    Year = 2024,
                    Model = models[1],
                    priceForPurchase = 1399
                },
                new Device {
                    Name = "Pixel 8 Pro",
                    Brand = "Google",
                    Color = "Blue",
                    Description = "Google’s latest smartphone featuring Tensor G3 processor",
                    Quality = "A",
                    Year = 2025,
                    Model = models[2],
                    priceForPurchase = 999
                }
            };

            //ApplicationUser user = new ApplicationUser
            //{
            //    Id = "1",
            //    UserName = "Elena",
            //    CustomerUserSurname = "García",
            //    Email = "elena@uclm.es"
            //};
            //var review = new Review(1, DateTime.Now, 5, 1, "Good", new List<ReviewItem>(), user);
            //var reviewItems = new ReviewItem("Excellent device", 1, devices[0], 5, 1, review);
            //review.ReviewItems.Add(reviewItems);


            _context.AddRange(models);
            _context.AddRange(devices);
            _context.SaveChanges();


        }
        // dani 
        public static IEnumerable<object[]> TestCasesFor_GetDevicesForReview_OK()
        {
            var deviceDTOs = new List<DevicesparareseniaDTO>() {
                new DevicesparareseniaDTO ( 1,  "Samsung", "Black","Galaxy S24 Ultra",  2023, "Galaxy S Series" ),
                new DevicesparareseniaDTO (2,"Apple", "Silver","iPhone 15 Pro Max", 2024, "iPhone 15" ),
                new DevicesparareseniaDTO ( 3,  "Google", "Blue","Pixel 8 Pro", 2025, "Pixel 8" )
            };
            // Creamos varios conjuntos esperados según los filtros que se probarán
            var deviceDTOsTC1 = new List<DevicesparareseniaDTO>() { deviceDTOs[0], deviceDTOs[1], deviceDTOs[2] };
            var deviceDTOsTC2 = new List<DevicesparareseniaDTO>() { deviceDTOs[0] };
            var deviceDTOsTC3 = new List<DevicesparareseniaDTO>() { deviceDTOs[1] };

            // Aquí se definen los parámetros con los que se va a llamar al método y el resultado esperado
            var allTests = new List<object[]>
            {
                new object[] { null, null , deviceDTOsTC1 },
                new object[] { "Samsung", null , deviceDTOsTC2 },
                new object[] { null , 2024, deviceDTOsTC3 },
            };

            return allTests;

        }

        // Test que comprueba que el método GetDevicesParaRepararDTO funciona correctamente con distintos filtros
        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_GetDevicesForReview_OK))]
        public async Task GetDevicesForReview_OK(string? brand, int? year, List<DevicesparareseniaDTO> expectedDevice)
        {
            // Arrange: se crea el controlador con el contexto de base de datos en memoria
            var controller = new DevicesController(null, _context);
            // Act: se llama al método a testear
            var result = await controller.GetDevicesparareseniaDTO(brand, year);
            //Assert: se comprueba que el resultado es correcto
            var okResult = Assert.IsType<OkObjectResult>(result);
            // Extraemos los datos devueltos por el controlador y verificamos que sean una lista de DTOs
            var reviewDTOsActual = Assert.IsType<List<DevicesparareseniaDTO>>(okResult.Value);
            // Finalmente comparamos que la lista devuelta sea igual a la esperada
            Assert.Equal(expectedDevice, reviewDTOsActual);
        }


















        // sandra
        public static IEnumerable<object[]> TestCasesFor_GetDevicesForPurchase_OK()
        {
            var deviceDTOs = new List<DevicesParaComprarDTO>() {
                new DevicesParaComprarDTO ( 1,  "Galaxy S24 Ultra", 1199.99, "Samsung", "Galaxy S Series", "Black" ),
                new DevicesParaComprarDTO (2, "iPhone 15 Pro Max", 1399, "Apple", "iPhone 15", "Silver"),
                new DevicesParaComprarDTO (3, "Pixel 8 Pro", 999, "Google", "Pixel 8", "Blue")
            };
            // Creamos varios conjuntos esperados según los filtros que se probarán
            var deviceDTOsTC1 = new List<DevicesParaComprarDTO>() { deviceDTOs[0], deviceDTOs[1], deviceDTOs[2] };
            var deviceDTOsTC2 = new List<DevicesParaComprarDTO>() { deviceDTOs[0] };
            var deviceDTOsTC3 = new List<DevicesParaComprarDTO>() { deviceDTOs[2] };

            // Aquí se definen los parámetros con los que se va a llamar al método y el resultado esperado
            var allTests = new List<object[]>
            {
                new object[] { null, null , deviceDTOsTC1 },
                new object[] { "Galaxy", null , deviceDTOsTC2 },
                new object[] { null , "Blue", deviceDTOsTC3 },
            };

            return allTests;

        }

        // Test que comprueba que el método GetDevicesParaPurchaseDTO funciona correctamente con distintos filtros
        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_GetDevicesForPurchase_OK))]
        public async Task GetDevicesForPurchase_OK(string? name, string? color, List<DevicesParaComprarDTO> expectedDevice)
        {
            // Arrange: se crea el controlador con el contexto de base de datos en memoria
            var controller = new DevicesController(null, _context);
            // Act: se llama al método a testear
            var result = await controller.GetDevicesParaComprarDTO(name, color);
            //Assert: se comprueba que el resultado es correcto
            var okResult = Assert.IsType<OkObjectResult>(result);
            // Extraemos los datos devueltos por el controlador y verificamos que sean una lista de DTOs
            var purchaseDTOsActual = Assert.IsType<List<DevicesParaComprarDTO>>(okResult.Value);
            // Finalmente comparamos que la lista devuelta sea igual a la esperada
            Assert.Equal(expectedDevice, purchaseDTOsActual);
        }

    }

}