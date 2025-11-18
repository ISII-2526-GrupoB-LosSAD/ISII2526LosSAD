using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.PurchaseDTO;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace AppForSEII2526.UT.PurchaseController_test
{
    public class GetPurchase_test : AppForSEII25264SqliteUT
    {
        public GetPurchase_test()
        {
            // 1. Datos base (Modelos y Dispositivos)
            var model1 = new Model { NameModel = "Galaxy S Series" };
            var model2 = new Model { NameModel = "iPhone 15" };

            var device1 = new Device
            {
                Name = "Galaxy S24 Ultra",
                priceForPurchase = 1200,
                quauntityForPurchase = 2,
                Brand = "Samsung",
                Color = "Black",
                Model = model1,
                Description = "High-end Samsung phone",
                Id = 1,
                Quality = "A+" // <--- NECESARIO para evitar error NOT NULL en BD
            };

            var device2 = new Device
            {
                Name = "iPhone 15 Pro Max",
                priceForPurchase = 1300,
                quauntityForPurchase = 1,
                Brand = "Apple",
                Color = "Silver",
                Model = model2,
                Description = "Apple flagship phone",
                Id = 2,
                Quality = "A+" // <--- NECESARIO
            };

            // 2. Usuario
            var user = new ApplicationUser("Carlos", "Martínez López", "carlos@uclm.es");

            // 3. Compra
            var purchase = new Purchase("Av. de España 45", PurchasePaymentMethodTypes.Paypal, user);
            purchase.ReceiptDate = new DateTime(2024, 5, 15);

            // 4. Items de la compra
            // Nota: Pasamos el objeto 'purchase' para establecer la relación
            var purchaseItem1 = new PurchaseItem("Samsung Galaxy", device1.priceForPurchase, device1.quauntityForPurchase, device1, purchase)
            {
                DeviceId = device1.Id
            };

            var purchaseItem2 = new PurchaseItem("iPhone 15 Pro Max", device2.priceForPurchase, device2.quauntityForPurchase, device2, purchase)
            {
                DeviceId = device2.Id
            };

            // 5. Asignamos items y calculamos totales
            purchase.PurchaseItems.Add(purchaseItem1);
            purchase.PurchaseItems.Add(purchaseItem2);

            // IMPORTANTE: Calculamos el total para que se guarde en BD
            // 1200*2 + 1300*1 = 3700
            purchase.TotalPrice = (device1.priceForPurchase * device1.quauntityForPurchase) +
                                  (device2.priceForPurchase * device2.quauntityForPurchase);

            purchase.TotalQuantity = device1.quauntityForPurchase + device2.quauntityForPurchase; // Total 3

            // 6. Guardar en Contexto
            _context.Models.AddRange(model1, model2);
            _context.Devices.AddRange(device1, device2);
            _context.Users.Add(user);
            _context.Purchases.Add(purchase);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetPurchase_NotFound_test()
        {
            // Arrange
            var mock = new Mock<ILogger<PurchaseController>>();
            var controller = new PurchaseController(_context, mock.Object);

            // Act
            var result = await controller.GetPurchase(0); // ID 0 no existe

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetPurchase_Found_test()
        {
            // Arrange
            var mock = new Mock<ILogger<PurchaseController>>();
            var controller = new PurchaseController(_context, mock.Object);

            // DTO esperado:
            // Precio Total: 3700 (2400 del Samsung + 1300 del iPhone)
            // Cantidad Total: 3 (2 Samsung + 1 iPhone)
            var expectedPurchase = new PurchaseDetailDTO(
                "Carlos",
                "Av. de España 45",
                new DateTime(2024, 5, 15),
                3700,
                3,
                new List<PurchaseItemDTO>()
            );

            // Añadimos los items esperados en el orden correcto
            expectedPurchase.PurchaseItems.Add(new PurchaseItemDTO(
                "High-end Samsung phone", 1200, 2, "Samsung", "Black", "Galaxy S Series"
            ));
            expectedPurchase.PurchaseItems.Add(new PurchaseItemDTO(
                "Apple flagship phone", 1300, 1, "Apple", "Silver", "iPhone 15"
            ));

            // Act
            var result = await controller.GetPurchase(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var purchaseDTOActual = Assert.IsType<PurchaseDetailDTO>(okResult.Value);

            // ¡Ahora esto funcionará perfecto porque ambos DTOs tienen Equals!
            Assert.Equal(expectedPurchase, purchaseDTOActual);
        }
    }
}