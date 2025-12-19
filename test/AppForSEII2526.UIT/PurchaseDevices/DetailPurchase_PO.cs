using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.PurchaseDevices
{
    public class DetailPurchase_PO : PageObject
    {

        public DetailPurchase_PO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output)
        {}

        public bool CheckPurchaseDetail(string name, string delivery)
        {
            WaitForBeingVisible(By.Id("TotalPrice")); // Ojo: asegúrate que este ID existe, en tu razor era "TotalPrice" (correcto)
            bool result = true;

            // ERROR ANTERIOR: Buscabas By.Id("Name") y By.Id("Surname") que no existen.

            // CORRECCIÓN: Buscamos el elemento combinado "NameSurname"
            string nameAndSurnameText = _driver.FindElement(By.Id("NameSurname")).Text;

            // Verificamos que el texto contenga el nombre Y el apellido
            result = result && nameAndSurnameText.Contains(name);
            
            result = result && _driver.FindElement(By.Id("DeliveryAddress")).Text.Contains(delivery);


            return result;
        }

        public bool CheckListOfDevices(List<string[]> expectedPurchaseItems)
        {
            return CheckBodyTable(expectedPurchaseItems, By.Id("PurchaseDevices"));
        }

       


    }
}
