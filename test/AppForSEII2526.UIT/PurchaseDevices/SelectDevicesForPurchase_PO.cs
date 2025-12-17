using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.PurchaseDevices
{
    public class SelectDevicesForPurchase_PO : PageObject
    {

        private By _deviceNameBy = By.Id("inputName");
        private By _deviceColorBy = By.Id("inputColor");
        private By _fromBy = By.Id("fromDate");
        private By _toBy = By.Id("toDate");

        private By _ShowPurchasingCartBy = By.Id("showPurchasingCart");
        private By _searchDevicesBy = By.Id("searchDevices");
        private By _rentButtonBy = By.Id("purchaseDevicesButton");
        private By _purchaseButtonBy = By.Id("purchaseRepairButton");

        private By _tableOfDevicesBy = By.Id("TableOfPurchase");
        private By _modalBy = By.Id("DialogOKSaveDelete");
        private By _noPurchaseMessageBy = By.Id("NoDevicesMessage");
        private By _cartTotalBy = By.Id("CartTotalPrice");

        private IWebElement _deviceColor() => _driver.FindElement(_deviceColorBy);
        private IWebElement _showPurchasingCarButton() => _driver.FindElement(_ShowPurchasingCartBy);
        private IWebElement _searchDevicesButton() => _driver.FindElement(_searchDevicesBy);
        private IWebElement _rentButton() => _driver.FindElement(_rentButtonBy);
        public IWebElement _purchaseButton() => _driver.FindElement(_purchaseButtonBy);

        public SelectDevicesForPurchase_PO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output)
        {
        }

        public void FilterDevices(string deviceName, string deviceColor, string from, string to)
        {
            WaitForBeingVisible(_deviceNameBy);
            _driver.FindElement(_deviceNameBy).Clear();
            _driver.FindElement(_deviceNameBy).SendKeys(deviceName);
            _deviceColor().Clear();
            _deviceColor().SendKeys(deviceColor);
            _driver.FindElement(_fromBy).Clear();
            _driver.FindElement(_fromBy).SendKeys(from);
            _driver.FindElement(_toBy).Clear();
            _driver.FindElement(_toBy).SendKeys(to);
            _searchDevicesButton().Click();
        }

        public void SelectDevices(List<string> deviceNames)
        {

            foreach (var deviceName in deviceNames)
            {
                WaitForBeingVisible(By.Id($"movieToRent_{deviceName}"));
                _driver.FindElement(By.Id($"movieToRent_{deviceName}")).Click();
            }
        }


        public void PurchaseDevices()
        {
            WaitForBeingClickable(_rentButtonBy);
            _rentButton().Click();
        }

        public void ModifyPurchasingCart(string model)
        {
            Thread.Sleep(500);
            WaitForBeingVisible(By.Id($"removeDevice_{model}"));
            _driver.FindElement(By.Id($"removeDevice_{model}")).Click();

        }

        public bool CheckListOfDevices(List<string[]> expectedDevices)
        {


            return CheckBodyTable(expectedDevices, _tableOfDevicesBy);

        }

        public bool CheckPurchaseDevicesDisabled()
        {
            return !_rentButton().Enabled;
        }

        public bool CheckShoppingCart(string price)
        {

            return _showPurchasingCarButton().Text.Contains(price);

        }

        public bool CheckMessageErrorNotAvaibleDevices(string expectedError)
        {

            return _driver.PageSource.Contains(expectedError);

        }

        public bool CheckMessageError(string expectedError)
        {
            return CheckModalBodyText(expectedError, _modalBy);
        }

        public bool IsNoDevicesMessageShown()
        {
            try
            {
                // Esperamos un poco a que aparezca
                WaitForBeingVisible(_noPurchaseMessageBy);
                return _driver.FindElement(_noPurchaseMessageBy).Displayed;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public bool ChekPurchaseDevicesDisabled()
        {
            try
            {
                var button = _driver.FindElement(_rentButtonBy);
                if (!button.Displayed)
                {
                    return true;
                }
                return !_rentButton().Enabled;
            }
            catch (NoSuchElementException)
            {
                // Si no lo encuentra (porque está hidden y Selenium no lo ve en el DOM interactuable), es correcto para este test.
                return true;
            }
        }

        public string GetCartTotalText()
        {
            WaitForBeingVisible(_cartTotalBy);
            return _driver.FindElement(_cartTotalBy).Text;
        }
    }
}
