using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AppForSEII2526.UIT.PurchaseDevices
{
    public class CreatePurchase_PO : PageObject
    {

        private By _nameSurnameBy = By.Id("Surname");
        private IWebElement _nameSurname() => _driver.FindElement(_nameSurnameBy);

        private By _nameBy = By.Id("Name");
        private IWebElement _name() => _driver.FindElement(_nameBy);
        private IWebElement _deliveryAddress() => _driver.FindElement(By.Id("DeliveryAddress"));
        private IWebElement _paymentMethod() => _driver.FindElement(By.Id("PaymentMethod"));

        public CreatePurchase_PO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output)
        {
        }


        public void FillInPurchaseInfo(string nameSurname, string name, string deliveryAddress)
        {
            WaitForBeingVisible(_nameBy);
            _name().SendKeys(name);
            WaitForBeingVisible(_nameSurnameBy);
            _nameSurname().SendKeys(nameSurname);
            
            _deliveryAddress().SendKeys(deliveryAddress);

            //create select element object 
            SelectElement selectElement = new SelectElement(_paymentMethod());


        }

        public void FillInPurchaseDescription(string purchaseDescription, int deviceId) // esto
        {
            _driver.FindElement(By.Id("description_" + deviceId)).SendKeys(purchaseDescription);


        }

        public void PressPurchaseYourDevices()
        {
            _driver.FindElement(By.Id("Submit")).Click();
        }

        public void PressModifyDevices()
        {
            _driver.FindElement(By.Id("ModifyDevices")).Click();
        }

        public bool CheckListOfPurchaseItems(List<string[]> expectedPurchaseItems)
        {
            return CheckBodyTable(expectedPurchaseItems, By.Id("TableOfPurchaseItems"));
        }

        public bool CheckValidationError(string expectedError)
        {
            return _driver.PageSource.Contains(expectedError);
        }
    }
}
