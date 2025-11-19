using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ReceiptDTOs;
using AppForSEII2526.API.DTOs.ReviewDTOs;
using AppForSEII2526.API.Models;
using Humanizer.Localisation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.ReviewsController_test
{
    // Clase de pruebas unitarias para el controlador ReviewsController
    public class CreateReview_test : AppForSEII25264SqliteUT
    {
        //  Variables constantes usadas en los tests
        private const string _CustomerUserName1 = "Elena";
        private const string _CustomerUserSurname1 = "Navarro Martinez";
        private const string _DeliveryAddress1 = "123 Main St";
        private const string _CustomerUserName2 = "Sandra";
        private const string _CustomerUserSurname2 = "Garcia Alcolea";
        private const string _Country = "España";
        private const string _ReviewTitle1 = "Excellent Performance";
        private const string _ReviewTitle2 = "Title2";

        // Guardamos la fecha actual para los tests
        public DateTime today = DateTime.Today;

        // Constructor: inicializa los datos base de prueba
        public CreateReview_test()
        {

            // Se crean diferentes modelos de dispositivos
            var models = new List<Model>() {
                new Model{ NameModel= "Model A" },
                new Model{ NameModel= "Model B" },
            };

            // Se crean diferentes dispositivos asociados a los modelos
            var devices = new List<Device>(){
                new Device{ Name= "Device A1", Quality= "High", Year=2022, Brand="BrandA", Color="Black", Description="DescriptionA", priceForPurchase=500, priceForRent=50, quauntityForPurchase=10, quauntityForRent=5, Model=models[0]},
                new Device{ Name= "Device B1", Quality= "Medium", Year=2021, Brand="BrandB", Color="White", Description="DescriptionB", priceForPurchase=300, priceForRent=30, quauntityForPurchase=8, quauntityForRent=4, Model=models[1]},
                new Device{ Name= "Device A2", Quality= "Low", Year=2020, Brand="BrandA", Color="Blue", Description="DescriptionC", priceForPurchase=200, priceForRent=20, quauntityForPurchase=5, quauntityForRent=2, Model=models[0]},
            };

            // Se añaden los modelos y dispositivos al contexto (base de datos en memoria)
            _context.AddRange(models);
            _context.AddRange(devices);

            // Se crea un usuario de aplicación
            ApplicationUser user = new ApplicationUser(_CustomerUserName1, _CustomerUserSurname1, _Country);
            _context.Add(user);

            // Se crea una review de ejemplo asociada al usuario
            var review = new Review (1, today, 5, 1, "Good", new List<ReviewItem>(), user);
            // Se añade un ítem a la review
            review.ReviewItems.Add(new ReviewItem( "Great device!", devices[0].Id, devices[0] ,4, review.ReviewId, review));


            // Se añade la review al contexto
            _context.Add(review);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_CreateReview() //Casos de prueba con errores esperados
        {
            // Lista de ítems de ejemplo que se incluyen en los recibos
            IList<ReviewItemDTO> Reviewitems = new List<ReviewItemDTO>() {
                new ReviewItemDTO("Device A1","Model A",2022,4,"Muy eficiente")
            };

            // Casos de prueba: recibos con errores (titulo, pais)
            ReviewForCreateDTO reviewNoTitle = new ReviewForCreateDTO(null, _Country, null, Reviewitems
            );

            // Caso con país nulo
            ReviewForCreateDTO receiptNoCountry = new ReviewForCreateDTO(
                _ReviewTitle2, null, null, Reviewitems
            );
            


            // Se definen los errores esperados para cada caso
            var allTests = new List<object[]>
            {
                new object[] { reviewNoTitle, "Error! Title is required" },
                new object[] { receiptNoCountry, "Error! Country is required" },
            };

            return allTests;
        }

        //Test: casos con errores (validaciones fallidas)
        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_CreateReview))]
        public async Task CreateReview_Error_test(ReviewForCreateDTO reviewForCreateDTO, string errorExpected)
        {
            // Arrange: se crea un mock del logger y el controlador
            var mock = new Mock<ILogger<ReviewController>>();
            ILogger<ReviewController> logger = mock.Object;
            var controller = new ReviewController(_context,logger);

            // Act: se ejecuta el método CreateReview con los datos del test
            var result = await controller.CreateReview(reviewForCreateDTO);

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
        public async Task CreateReview_Success_test()
        {
            // Arrange: se prepara el controlador con mock de logger
            var mock = new Mock<ILogger<ReviewController>>();
            ILogger<ReviewController> logger = mock.Object;
            var controller = new ReviewController(_context, logger);

            // Se crea un DTO de review válido
            ReviewForCreateDTO reviewDTO = new ReviewForCreateDTO(
                _ReviewTitle1,
                _Country,
                _CustomerUserName1,
                new List<ReviewItemDTO>()
            );

            // Se añade un ítem al recibo
            reviewDTO.ReviewItems.Add(
                new ReviewItemDTO("Device A1", "Model A", 2022, 5, "Excellent Performance")
            );

            // Se define el resultado esperado del recibo creado
            // (ID 2 porque ya existe uno en la base de datos)
            ReviewDetailDTO expectedreviewDetailDTO = new ReviewDetailDTO(
                _CustomerUserName1,
                _Country,
                today,
                _ReviewTitle1,
                new List<ReviewItemDTO>()
                {
                    new ReviewItemDTO("Device A1", "Model A", 2022, 5, "Excellent Performance")
                }
            );

            // Act: se llama al método CreateReview
            var result = await controller.CreateReview(reviewDTO);

            // Assert:
            //Se verifica que la respuesta sea "CreatedAtActionResult"
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);

            //Se obtiene el objeto del cuerpo de la respuesta
            var actualReviewDetailDTO = Assert.IsType<ReviewDetailDTO>(createdResult.Value);

            //Se compara el DTO esperado con el obtenido
            Assert.Equal(expectedreviewDetailDTO, actualReviewDetailDTO);
        }
    }
}
