using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.ReceiptRepair
{
    internal class DetailReceipt_PO : PageObject
    {

        private By _nameSurnameBy = By.Id("NameSurname");
        private By _deliveryAddressBy = By.Id("DeliveryAddress");
        private By _rentalDateBy = By.Id("RentalDate");
        private By _totalPriceBy = By.Id("TotalPrice");
        private By _tableOfRepairsBy = By.Id("ReceiptRepair");

        public DetailReceipt_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output) { }

        // Comprobar los datos generales del recibo
        public bool CheckReceiptDetail(string expectedNameSurname, string expectedAddress, string expectedPrice)
        {
            WaitForBeingVisible(_nameSurnameBy);

            bool result = true;

            result = result && _driver.FindElement(_nameSurnameBy).Text.Contains(expectedNameSurname);
            result = result && _driver.FindElement(_deliveryAddressBy).Text.Contains(expectedAddress);
            string today = DateTime.Now.ToString("dd/MM/yyyy");
            result = result && _driver.FindElement(_rentalDateBy).Text.Contains(today);

            result = result & _driver.FindElement(_totalPriceBy).Text.Contains(expectedPrice);

            return result;
        }

        // Comprobar la lista de reparaciones del recibo
        public bool CheckListOfRepairs(List<string[]> expectedItems)
        {
            // Verifica las filas de la tabla con ID "ReceiptRepair"
            return CheckBodyTable(expectedItems, _tableOfRepairsBy);
        }
    }
}
