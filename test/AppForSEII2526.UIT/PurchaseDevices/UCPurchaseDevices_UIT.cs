using AppForMovies.UIT.Shared;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.PurchaseDevices
{
    public class UCPurchaseDevices_UIT : UC_UIT
    {

        public UCPurchaseDevices_UIT(ITestOutputHelper output): base( output)
        {

            Initial_step_opening_the_web_page();
            selectDevices = new SelectDevicesForPurchase_PO(_driver, _output);   

        }

        private const int deviceID1 = 4;
        public const string deviceName1 = "Pixel 8 Pro";
        public const string deciveModel1 = "Pixel 8";
        public const string deciveBrand1 = "Google";
        private const string deviceColor1 = "Blue";
        private const string devicePrice1 = "999";

        private const int deviceID2 = 1;
        private const string deviceName2 = "Galaxy S24 Ultra";

        private const string deciveModel2 = "Galaxy S Series";
        private const string deciveBrand2 = "Samsung";
        private const string deviceColor2 = "Black";
        private const string devicePrice2 = "1199,99";

        private SelectDevicesForPurchase_PO selectDevices;

        private void InitialStepsForPurchaseDevices_UIT() {

            selectDevices.WaitForBeingVisibleIgnoringExeptionTypes(By.Id("CreatePurchasing"));
            _driver.FindElement(By.Id("CreatePurchasing")).Click();

        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_2()
        { //Esc – 2 Dispositivos no disponibles 
            Thread.Sleep(1000);
            // Arrange: acceder a la pantalla de reparaciones
            InitialStepsForPurchaseDevices_UIT();
            selectDevices.WaitForBeingVisible(By.Id("TableOfPurchase"));

            // Act: aplicar un filtro con un nombre inexistente
            string nombreInventado = "NombreErroneo";
            selectDevices.FilterDevices(nombreInventado, "", "01/01/2024", "02/01/2024");

            // Assert: comprobar que aparece el mensaje de "sin resultados"
            Assert.True(selectDevices.IsNoDevicesMessageShown(),
                "No purchase were found.");
        }

       


            [Theory]
        [InlineData(deviceName1, deciveBrand1, deciveModel1,  deviceColor1, devicePrice1, "", "Blue")]
        [InlineData(deviceName2, deciveBrand2, deciveModel2,  deviceColor2, devicePrice2, "Galaxy S24 Ultra", "")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_3_4(string name,  string brand, string model, string color, string price, string filterName, string filterColor) {
            //Filtrar dispositivos por nombre y color
            Thread.Sleep(1000);
            var form = DateTime.Today.AddDays(2);
            var to = DateTime.Today.AddDays(3);
            var expectedDevices = new List<string[]> { new string[] { name, brand, model,  color, price.ToString() }, };

            InitialStepsForPurchaseDevices_UIT();
            Thread.Sleep(1000);
            selectDevices.FilterDevices(filterName, filterColor, form.ToString("dd/MM/yyyy"), to.ToString("dd/MM/yyyy"));
            Thread.Sleep(1000);
            Assert.True(selectDevices.CheckListOfDevices(expectedDevices));

        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_5()
        {   //Modificar el carrito de compras y verificar el precio total

            // Arrange: abrir pantalla de dispositivos
            InitialStepsForPurchaseDevices_UIT();
            selectDevices.WaitForBeingVisible(By.Id("TableOfPurchase"));

            // Act: Añadir dos reparaciones
            selectDevices.SelectDevices(new List<string> { deviceName1, deviceName2 });
            // Assert: Verificar que el precio total del carrito es correcto (debe ser 999+1199,99=2198,99)
            string precioInicial = selectDevices.GetCartTotalText();
            Assert.Contains("2198,99", precioInicial);

            // Act: Eliminar un dispositivo del carrito (el primer dispositivo)
            selectDevices.ModifyPurchasingCart(deciveModel1);

            // Assert: Verificar que el precio baja (debe ser 1199,99 al quitar el primer dispositivo)
            string precioActualizado = selectDevices.GetCartTotalText();
            Assert.Contains("1199,99", precioActualizado);
            Assert.DoesNotContain("2198,99", precioActualizado);
        }


        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_6()
        { //No hay compras en el carrito no deja continuar

            // Arrange: abrir pantalla de reparaciones
            InitialStepsForPurchaseDevices_UIT();

            Thread.Sleep(1000);
            // Act
            selectDevices.WaitForBeingVisible(By.Id("TableOfPurchase"));
            // Assert: El botón de continuar debe estar deshabilitado o directamente no visible si el carrito está vacío
            Assert.True(selectDevices.ChekPurchaseDevicesDisabled() || !selectDevices._purchaseButton().Displayed);


        }










        [Theory]
        [InlineData("", "Navarro Martínez", "123 Elm St, NY", 1, "Pixel 8 Pro(x1)", "CustomerUserName field is required")]
        [InlineData("Elena", "", "123 Elm St, NY", 1, "Pixel 8 Pro(x1)", "The UserSurname field is required.")]
        [InlineData("Elena", "Navarro Martínez", "", 1, "Pixel 8 Pro(x1)", "")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC3_6_7_8_9_testingErrorsMandatorydata(string username,string surname, string delivery, int Quantity, string device,
          string expectedMessageError)
        {
            //Arrange

            var createpurchase = new CreatePurchase_PO(_driver, _output);

            //Act
            InitialStepsForPurchaseDevices_UIT();

            selectDevices.SelectDevices(new List<string> { deviceName1 });
            selectDevices.PurchaseDevices();
            Thread.Sleep(1000);
            createpurchase.FillInPurchaseInfo(surname, username,  delivery);
            Thread.Sleep(1000);

            createpurchase.PressPurchaseYourDevices();

            //Assert
            //the expected error is shown in the view
            Assert.True(createpurchase.CheckValidationError(expectedMessageError), $"Expected error: {expectedMessageError}");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC3_10_ModifyRentalItems()
        {
            //Arrange

            var createpurchase = new CreatePurchase_PO(_driver, _output);

            //Act
            InitialStepsForPurchaseDevices_UIT();


            selectDevices.SelectDevices(new List<string> { deviceName1 });
            selectDevices.PurchaseDevices();
            Thread.Sleep(1000);
            createpurchase.PressModifyDevices();
            Thread.Sleep(1000);

            //Assert
            //the list of movies must change
            Assert.True(selectDevices.CheckShoppingCart(deviceName1));
        }


     //   [Theory]
     //   [InlineData("Elena", "Navarro Martínez", "123 Elm St, NY", 1, "Pixel 8 Pro(x1)")]
     //   [Trait("LevelTesting", "Funcional Testing")]
     //   public void UC2_1_BasicFlow(string username, string surname, string delivery, int Quantity, string device)
     //   {
     //       //Arrange

     //       var createpurchase = new CreatePurchase_PO(_driver, _output);
     //       var detailPurchase = new DetailPurchase_PO(_driver, _output);



     //       //Act
     //       InitialStepsForPurchaseDevices_UIT();

     //       selectDevices.SelectDevices(new List<string> { deviceName1 });
     //       selectDevices.PurchaseDevices();

     //       createpurchase.FillInPurchaseInfo(surname, username, delivery);
     //       Thread.Sleep(500);
            

     //       createpurchase.PressPurchaseYourDevices();
     //       Thread.Sleep(500);
     //       createpurchase.PressOkModalDialog();
     //       Thread.Sleep(500);

     //       //Assert
     //       //the expected error is shown in the view
     //       Assert.True(detailPurchase.CheckPurchaseDetail(username, surname, delivery),
     //"Error: detail review is not as expected");

     //       var expectedPurchaseItems = new List<string[]>
     //               { new string[] { deviceName1, deciveModel1, deviceColor1, devicePrice1}, };

     //       Assert.True(detailPurchase.CheckListOfDevices(expectedPurchaseItems),
     //           "Error: rental items are not as expected");

     //   }

    }
}
