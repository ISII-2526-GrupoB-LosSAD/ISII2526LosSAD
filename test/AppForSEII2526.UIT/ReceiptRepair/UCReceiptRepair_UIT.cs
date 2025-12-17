using AppForMovies.UIT.Shared;
using AppForSEII2526.UIT.ReviewDevices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AppForSEII2526.UIT.ReceiptRepair
{
    public class UCReceiptRepair_UIT : UC_UIT
    {
        // Datos constantes de pruebas

        private const int repairId_1 = 1;
        private const string name_1 = "Screen Replacement";
        private const string scale_1 = "Small Device";
        private const string descriptio_1 = "Replace cracked or broken screen";
        private const string price_1 = "120,5";

        private const int repairId_2 = 2;
        private const string name_2 = "Battery Replacement";
        private const string scale_2 = "Medium Device";
        private const string descriptio_2 = "Install a new battery";
        private const string price_2 = "75";

        // Page Object que maneja la lista de reparaciones
        private ListRepairsForReceipts_PO listrepairs;

        // Constructor de la clase de pruebas
        // Inicializa el navegador y la página de reparaciones
        public UCReceiptRepair_UIT(ITestOutputHelper output) : base(output)
        {
            Initial_step_opening_the_web_page();
            listrepairs = new ListRepairsForReceipts_PO(_driver, _output);
        }


        //private void Precondition_perform_login()
        //{
        //    Perform_login("elena@uclm.es", "Password1234%");
        //}


        // Abrir pantalla de recibos
        private void InitialStepsForReceiptRepairs_UIT()
        {
            
            Thread.Sleep(500);
            listrepairs.WaitForBeingVisible(By.Id("CreateReceipt"));
            _driver.FindElement(By.Id("CreateReceipt")).Click();
        }
        // UC4.1 - Reparación correcto
        [Theory]
        [InlineData("Elena", "Navarro Martínez", "Calle 123 Main St, New York", "CreditCard")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC4_1_BasicFlow_ContractRepair(string name, string surname, string deliveryAddress, string paymentMethod)
        {
            //ARRANGE
            var createReceipt = new CreateReceipt_PO(_driver, _output);
            var detailReceipt = new DetailReceipt_PO(_driver, _output);

            // Datos esperados para la reparación
            string modelToRepair = "iPhone X"; // Modelo de ejemplo

            // Calculamos el precio esperado formateado (asumiendo formato español F2)
            // Si price_1 es "120,5", el total será "120,50" (o similar según cultura)
            // Para asegurar el test, buscaremos la raíz del número
            string expectedPriceLabel = price_1;

            //ACT
            InitialStepsForReceiptRepairs_UIT();

            //Seleccionar reparación 
            listrepairs.WaitForBeingVisible(By.Id("TableOfRepairs"));
            listrepairs.SelectRepairs(name_1);

            //Ir a contratar
            listrepairs.ReceiptRepairs();

            //Rellenar formulario
            createReceipt.FillInReceiptInfo(name, surname, deliveryAddress);
            createReceipt.FillInModelInfo(repairId_1, modelToRepair);

            //Guardar y Confirmar
            createReceipt.PressSubmitReceipt();
            createReceipt.PressOkModalDialog();

            //ASSERT: Verificamos cabecera del recibo (Nombre Completo, Dirección, Precio)
            string fullName = name + " " + surname;

            Assert.True(detailReceipt.CheckReceiptDetail(
                fullName,
                deliveryAddress,
                expectedPriceLabel),
                "Error: Los detalles del recibo no son los esperados");

            // Verificamos la tabla de items
            var expectedRentalItems = new List<string[]>
    {
        new string[] { name_1, scale_1, price_1 + " €", modelToRepair }
    };

            Assert.True(detailReceipt.CheckListOfRepairs(expectedRentalItems),
                "Error: La lista de reparaciones en el recibo no es correcta");
        }

        // UC4.2 - No hay reparaciones disponibles tras filtrar
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC4_2_NoRepairsAvailable()
        {
            // Arrange: acceder a la pantalla de reparaciones
            InitialStepsForReceiptRepairs_UIT();
            listrepairs.WaitForBeingVisible(By.Id("TableOfRepairs"));

            // Act: aplicar un filtro con un nombre inexistente
            string nombreInventado = "NombreErroneo";
            listrepairs.FilterRepairs(nombreInventado, "All");

            // Assert: comprobar que aparece el mensaje de "sin resultados"
            Assert.True(listrepairs.IsNoRepairsMessageShown(),
                "No repairs were found.");
        }


        // UC4.3-4 - Filtrado por nombre o escala
        [Theory]
        [InlineData(name_1, scale_1, descriptio_1, price_1, "Screen Replacement", "")]
        [InlineData(name_2, scale_2, descriptio_2, price_2, "", "Medium Device")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC4_3_4_filteringbyTitleandGenre(string name, string scale,
            string desciption, string price, string filterName, string filterScale)
        {
            //Arrange: lista esperada tras el filtrado
            var expectedRepair = new List<string[]> { new string[] { name, scale, desciption, price }, };

            //Act: acceder y aplicar filtros
            InitialStepsForReceiptRepairs_UIT();

            Thread.Sleep(500);
            listrepairs.FilterRepairs(filterName, filterScale);
            Thread.Sleep(500);


            //Assert: comprobar que la tabla contiene solo la reparación esperada
            Assert.True(listrepairs.CheckListOfRepairs(expectedRepair));

        }

        // UC4.4 - Modificar carrito de reparaciones
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC4_5_ModifyCart()
        {
            // Arrange: abrir pantalla de reparaciones
            InitialStepsForReceiptRepairs_UIT();
            listrepairs.WaitForBeingVisible(By.Id("TableOfRepairs"));

            // Act: Añadir dos reparaciones
            listrepairs.SelectRepairs(name_1); // 120,5
            listrepairs.SelectRepairs(name_2); // 75

            // Assert: Verificar suma inicial (195,50) en la misma pantalla
            string precioInicial = listrepairs.GetCartTotalText();
            Assert.Contains("195,5", precioInicial);

            // Act: Eliminar una reparación (la de 120,5)
            listrepairs.ModifyReceiptingCart(name_1);

            // Assert: Verificar que el precio baja automáticamente (debe quedar 75)
            string precioActualizado = listrepairs.GetCartTotalText();
            Assert.Contains("75", precioActualizado);
            Assert.DoesNotContain("195,5", precioActualizado);
        }

        // UC4.5 - Carrito vacío: no se puede continuar
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC4_6_EmptyCart_NoContinue()
        {

            // Arrange: abrir pantalla de reparaciones
            InitialStepsForReceiptRepairs_UIT();
            Thread.Sleep(1000);
            // Act
            listrepairs.WaitForBeingVisible(By.Id("TableOfRepairs"));
            // Assert: El botón de continuar debe estar deshabilitado o directamente no visible si el carrito está vacío
            Assert.True(listrepairs.ChekReceiptRepairsDisabled() || !listrepairs._receiptButton().Displayed);
        }

        // UC4.6 - Datos incompletos
        [Theory]
        [InlineData("", "Navarro Martínez", "Calle 123 Main St, New York", "iPhone 13", "Name")]
        [InlineData("Elena", "", "Calle 123 Main St, New York", "iPhone 13", "Surname")]
        [InlineData("Elena", "Navarro Martínez", "", "iPhone 13", "Address")]
        [InlineData("Elena", "Navarro Martínez", "Calle 123 Main St, New York", "", "Model")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_7_8_9_10_ValidateMandatoryFields(string username, string usersurname, string deliveryadress, string model, string expectedMessageError)
        {
            //ARRANGE: Instanciamos el PO adaptado
            var createReceipt = new CreateReceipt_PO(_driver, _output);

            //Navegar y limpiar el carrito para empezar desde cero
            InitialStepsForReceiptRepairs_UIT();
            listrepairs.WaitForBeingVisible(By.Id("TableOfRepairs"));

            //Seleccionar una reparación (Screen Replacement - ID 1) para habilitar el botón
            listrepairs.SelectRepairs(name_1);

            //Ir a la pantalla de Crear Recibo (Paso 4)
            listrepairs.ReceiptRepairs();

            //ACT
            //Rellenar información personal con los datos de prueba
            createReceipt.FillInReceiptInfo(username, usersurname, deliveryadress);

            //Rellenar el modelo (usando repairId_1 = 1)
            createReceipt.FillInModelInfo(repairId_1, model);

            //Intentar Enviar
            createReceipt.PressSubmitReceipt();

            //ASSERT: Verificar que el error esperado aparece en la página
            Assert.True(createReceipt.CheckValidationError(expectedMessageError),
                $"Error esperado: No se encontró el mensaje de validación conteniendo '{expectedMessageError}'");
        }

        // UC4.7 - Modificar reparaciones seleccionada
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_11_ModifySelection_PreserveData()
        {
            //ARRANGE
            var createReceipt = new CreateReceipt_PO(_driver, _output);
            string testName = "Elena";
            string testSurname = "Navarro Martínez";
            string testAddress = "Calle 123 Main St, New York";
            string testModel = "iPhone 13";
            InitialStepsForReceiptRepairs_UIT();
            listrepairs.WaitForBeingVisible(By.Id("TableOfRepairs"));

            // Act: Seleccionamos reparación ID 1, Vamos al formulario
            listrepairs.SelectRepairs(name_1);
            listrepairs.ReceiptRepairs();

            // Rellenamos datos usando el PO adaptado
            createReceipt.FillInReceiptInfo(testName, testSurname, testAddress);
            createReceipt.FillInModelInfo(repairId_1, testModel);

            // Volvemos atrás
            createReceipt.PressModifyRepairs();

            // Esperamos a ver la tabla y volvemos a entrar
            listrepairs.WaitForBeingVisible(By.Id("TableOfRepairs"));
            listrepairs.ReceiptRepairs();

            // Assert: Usamos el método de validación que añadimos al PO
            Assert.True(createReceipt.CheckFormDataPreserved(testName, testSurname, testAddress, repairId_1, testModel),
                "Los datos introducidos deberían conservarse tras navegar atrás y volver.");
        }

    }
}
