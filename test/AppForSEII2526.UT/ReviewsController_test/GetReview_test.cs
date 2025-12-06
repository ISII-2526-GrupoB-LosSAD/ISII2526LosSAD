using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ReviewDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.ReviewsController_test
{
    public class GetReview_test : AppForSEII25264SqliteUT
    {
        public GetReview_test()
        {
            // Inicialización de datos de prueba específicos para los tests de ReceiptsController
            var models = new List<Model>()
            {
                new Model { NameModel = "Galaxy S Series" },
                new Model { NameModel = "iPhone 15" },
                new Model { NameModel = "Pixel 8" }
            };
            var devices = new List<Device>()
            {
                new Device {
                        Brand = "Samsung",
                        Color = "Black",
                        Name = "Galaxy S24 Ultra",
                        Description = "High-end Android smartphone with dynamic AMOLED display",
                        Quality = "A+",
                        Year = 2023,
                        Model = models[0]
                    },
                    new Device {
                        Brand = "Apple",
                        Color = "Silver",
                        Name = "iPhone 15 Pro Max",
                        Description = "Flagship iOS smartphone with advanced camera system",
                        Quality = "A+",
                        Year = 2024,
                        Model = models[1]
                    },
                    new Device {
                        Brand = "Google",
                        Color = "Blue",
                        Name = "Pixel 8 Pro",
                        Description = "Google’s latest smartphone featuring Tensor G3 processor",
                        Quality = "A",
                        Year = 2025,
                        Model = models[2]
                    }

            };

            // Añadimos los datos al contexto (la base de datos en memoria que usamos para testear)
            _context.AddRange(models);
            _context.AddRange(devices);

            ApplicationUser user = new ApplicationUser("Elena", "Navarro Martinez", "Spain");

            // Creamos un recibo de ejemplo con una fecha, dirección, método de pago, precio y el usuario anterior
            var review = new Review(1, new DateTime(2011, 10, 20), 5, 1, "Good", new List<ReviewItem>(), user);
            // Añadimos un ítem dentro del recibo (por ejemplo, una reparación de pantalla para un iPhone X)
            review.ReviewItems.Add(new ReviewItem("Excellent device", 1, devices[0], 5, 1, review));


            // Guardamos el usuario y el recibo en la base de datos
            _context.Users.Add(user);
            _context.Add(review);
            _context.SaveChanges();
        }
      
        // Primer test: se prueba que si se busca un recibo con un id que no existe, se devuelve NotFound
        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetReview_ById_OK()
        {
            // Arrange: se crea un mock del logger para pasárselo al controlador
            var mock = new Mock<ILogger<ReviewController>>();
            ILogger<ReviewController> logger = mock.Object;

            // Se crea el controlador con el contexto de prueba
            var controller = new ReviewController(_context, logger);

            // Act: se intenta obtener un recibo con id 0 (que no existe)
            var result = await controller.GetReview(0);
            // Assert: comprobamos que el resultado sea un NotFoundResult
            Assert.IsType<NotFoundResult>(result);
        }

        // Segundo test: se comprueba que el controlador devuelva correctamente una review existente
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetReview_Found_test()
        {
            // Arrange: se prepara el logger (mock) y el controlador
            var mock = new Mock<ILogger<ReviewController>>();
            ILogger<ReviewController> logger = mock.Object;
            var controller = new ReviewController(_context, logger);

            // Se crea el DTO esperado (lo que debería devolver el método del controlador)
            var expectedReview = new ReviewDetailDTO("Elena", "Spain", new DateTime(2011, 10, 20), "Good", new List<ReviewItemDTO>());

            // Se añade al DTO el ítem que esperamos que tenga el recibo
            expectedReview.ReviewItems.Add(new ReviewItemDTO(comments: "Excellent device", rating: 5, name: "Galaxy S24 Ultra", model: "Galaxy S Series", year: 2023));

            // Act: se llama al método del controlador con el id del recibo que sí existe (1)
            var result = await controller.GetReview(1);

            //Assert: comprobamos que la respuesta sea OK y que los datos sean los esperados
            var okResult = Assert.IsType<OkObjectResult>(result);
            var reviewDTOActual = Assert.IsType<ReviewDetailDTO>(okResult.Value);

            // Se compara el DTO esperado con el recibido
            Assert.Equal(expectedReview, reviewDTOActual);
        }
    }
}