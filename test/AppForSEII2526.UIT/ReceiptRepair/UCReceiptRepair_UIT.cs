using AppForMovies.UIT.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public void UC4_4_ModifyCart()
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
        public void UC4_5_EmptyCart_NoContinue()
        {

            // Arrange: abrir pantalla de reparaciones
            InitialStepsForReceiptRepairs_UIT();
            Thread.Sleep(1000);
            // Act
            listrepairs.WaitForBeingVisible(By.Id("TableOfRepairs"));
            // Assert: El botón de continuar debe estar deshabilitado o directamente no visible si el carrito está vacío
            Assert.True(listrepairs.ChekReceiptRepairsDisabled() || !listrepairs._receiptButton().Displayed);
        }

        

    }
}
