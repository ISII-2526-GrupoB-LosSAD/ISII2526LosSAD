using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.ReceiptRepair
{
    internal class ListRepairsForReceipts_PO : PageObject
    {
        private By _repairNameBy = By.Id("inputName");
        private By _repairScaleBy = By.Id("selectScale");

        //private By _showReceiptBy = By.Id("")
        private By _searchRepairBy = By.Id("searchRepairs");
        private By _receiptButtonBy = By.Id("receiptRepairButton");

        private By _tableOfReceiptsBy = By.Id("TableOfRepairs");
        private By _modalBy = By.Id("DialogOKSaveDelete");
        private By _cartTotalBy = By.Id("CartTotalPrice");

        private By _noRepairsMessageBy = By.Id("NoRepairsMessage");

        private IWebElement _repairName() => _driver.FindElement(_repairNameBy);
        private IWebElement _repairScale() => _driver.FindElement(_repairScaleBy);
        private IWebElement _searchRepair() => _driver.FindElement(_searchRepairBy);
        public IWebElement _receiptButton() => _driver.FindElement(_receiptButtonBy);
        public ListRepairsForReceipts_PO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output)
        {
        }

        public void FilterRepairs(string nameFilter, string scaleSelected)
        {
            WaitForBeingClickable(_repairNameBy);
            System.Threading.Thread.Sleep(2000);

            _repairName().SendKeys(nameFilter);
            if (scaleSelected == "") scaleSelected = "All";
            SelectElement selectElement = new SelectElement(_repairScale());

            selectElement.SelectByText(scaleSelected);
            _searchRepair().Click();
            System.Threading.Thread.Sleep(2000);
        }

        public void SelectRepairs(string repairNames)
        {
            WaitForBeingClickable(By.Id($"repairToReceipt_{repairNames}"));
            _driver.FindElement(By.Id($"repairToReceipt_{repairNames}")).Click();
        }

        public void ModifyReceiptingCart(string repairNames)
        {
            WaitForBeingClickable(By.Id($"removeReapir_{repairNames}"));
            _driver.FindElement(By.Id($"removeReapir_{repairNames}")).Click();

        }

        public bool IsNoRepairsMessageShown()
        {
            try
            {
                // Esperamos un poco a que aparezca
                WaitForBeingVisible(_noRepairsMessageBy);
                return _driver.FindElement(_noRepairsMessageBy).Displayed;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public string GetCartTotalText()
        {
            WaitForBeingVisible(_cartTotalBy);
            return _driver.FindElement(_cartTotalBy).Text;
        }

        public void ReceiptRepairs()
        {
            WaitForBeingClickable(_receiptButtonBy);
            _receiptButton().Click();
            System.Threading.Thread.Sleep(500);
        }
        
        
        public bool CheckListOfRepairs(List<string[]> expectedRepairs)
        {
            return CheckBodyTable(expectedRepairs, _tableOfReceiptsBy);
        }

        public bool ChekReceiptRepairsDisabled()
        {
            WaitForBeingVisible(_tableOfReceiptsBy);
            return !_receiptButton().Enabled;
        }

        //public bool CheckRepairsCart(string price)
        //{
        //    return _showReceipt
        //}

        public bool CheckShoppingCart(string name)
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
                var button = cartContainer.FindElement(By.Id($"removeReceipt_{name}"));
                return button != null && button.Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public bool CheckMessageErrorNotAvaibleRepairs()
        {
            return _driver.FindElement(_receiptButtonBy).Displayed == false;

        }

        public bool CheckMessageError(string expectedError)
        {
            return CheckModalBodyText(expectedError, _modalBy);
        }

        // Método correcto para verificar si el botón está OCULTO
        public bool IsReceiptButtonHidden()
        {
            try
            {
                // Buscamos el botón
                var button = _driver.FindElement(_receiptButtonBy);
                // Devolvemos TRUE si NO está visible
                return !button.Displayed;
            }
            catch (NoSuchElementException)
            {
                // Si ni siquiera lo encuentra en el HTML, es que está oculto/no existe
                return true;
            }
        }
        }
    }
