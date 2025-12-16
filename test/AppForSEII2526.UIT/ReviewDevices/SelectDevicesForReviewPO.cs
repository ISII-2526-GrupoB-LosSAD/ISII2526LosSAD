using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.ReviewDevices
{
    internal class SelectDevicesForReviewPO : PageObject{
        private By inputBrand = By.Id("inputBrand");
        private By inputYear = By.Id("inputYear");
        private By searchDevices = By.Id("searchDevices");
        private By _ShowReviewCartBy = By.Id("showReviewCart");
        private By _reviewButtonBy = By.Id("DevicesReviewButton");

        private By TableOfDevices = By.Id("TableOfDevices");
        private By DevicesReviewButton = By.Id("DevicesReviewButton");

        private IWebElement _deviceBrand() => _driver.FindElement(inputBrand);
        private IWebElement _deviceYear() => _driver.FindElement(inputYear);
        private IWebElement _searchDevices() => _driver.FindElement(searchDevices);
        private IWebElement _TableOfDevices() => _driver.FindElement(TableOfDevices);
        private IWebElement _DevicesReviewButton() => _driver.FindElement(DevicesReviewButton);
        private IWebElement _showReviewCartButton() => _driver.FindElement(_ShowReviewCartBy);
        private IWebElement _reviewButton() => _driver.FindElement(_reviewButtonBy);
        public SelectDevicesForReviewPO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }
        public void SearchDevices(string brand, int year){
            WaitForBeingVisible(inputBrand);
            _deviceBrand().SendKeys(brand);
            _driver.FindElement(searchDevices).Click();

            WaitForBeingVisible(inputYear);
            _deviceYear().SendKeys(year.ToString());
            _driver.FindElement(searchDevices).Click();

            System.Threading.Thread.Sleep(2000);

        }

        public void SelectDevices(List<string> deviceids)
        {
            //we wait for till the movies are available to be selected 
            foreach (var deviceId in deviceids)
            {
                WaitForBeingVisible(By.Id($"deviceToReview_{deviceId}"));
                _driver.FindElement(By.Id($"deviceToReview_{deviceId}")).Click();
            }
        }

        public void ReviewDevices()
        {
            WaitForBeingClickable(DevicesReviewButton);
            _DevicesReviewButton().Click();
        }
        public void ModifyReviewCart(string name)
        {
            Thread.Sleep(500);
            WaitForBeingVisible(By.Id($"removeDevice_{name}"));
            _driver.FindElement(By.Id($"removeDevice_{name}")).Click();

        }
        public bool CheckListOfDevices(List<string[]> expectedDevices)
        {

            return CheckBodyTable(expectedDevices, TableOfDevices);
        }

        //public bool CheckRentDevicesDisabled()
        //{
        //we return true if the button is disabled
        //  return !(_DevicesReviewButton().Enabled);
        //}

        public bool CheckReviewDeviceDisabled()
        {
            //we return true if the button is disabled
            return (_reviewButton().Enabled);
        }
        public bool CheckShoppingCart(string deviceName)
        {
            Thread.Sleep(500);

            try
            {
                // Verificar si el carrito está visible
                var cartContainer = _driver.FindElement(By.CssSelector("div.col-2"));

                if (!cartContainer.Displayed || cartContainer.GetAttribute("hidden") != null)
                {
                    return false;
                }

                // Buscar directamente por el ID completo del botón
                var button = cartContainer.FindElement(By.Id($"removeDevice_{deviceName}"));
                return button != null && button.Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

    }
}
