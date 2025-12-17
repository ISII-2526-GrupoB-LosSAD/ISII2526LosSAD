using System;
using System.Collections.Generic;
using AppForMovies.UIT.Shared;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.ReceiptRepair
{
    internal class CreateReceipt_PO : PageObject
    {
        // IDs basados en tu CreateReceipt.razor 
        private By _NameBy = By.Id("Name");
        private IWebElement _Name() => _driver.FindElement(_NameBy);
        private IWebElement _Surname() => _driver.FindElement(By.Id("Surname"));
        private IWebElement _Address() => _driver.FindElement(By.Id("DeliveryAddress"));

        // El ID dinámico para el modelo: "model_1", "model_2", etc. 
        private IWebElement _Model(int deviceId) => _driver.FindElement(By.Id("model_" + deviceId));

        public CreateReceipt_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        // Rellenar los datos personales del recibo
        public void FillInReceiptInfo(string name, string surname, string address)
        {
            WaitForBeingVisible(_NameBy);

            _Name().Clear();
            _Name().SendKeys(name);

            _Surname().Clear();
            _Surname().SendKeys(surname);

            _Address().Clear();
            _Address().SendKeys(address);
        }

        // // Rellenar el modelo del dispositivo reparado
        public void FillInModelInfo(int deviceId, string model)
        {
            try
            {
                // Asegurarse de que el campo está visible
                WaitForBeingVisible(By.Id($"model_{deviceId}"));

                // Añadir modelo
                _Model(deviceId).Clear();
                _Model(deviceId).SendKeys(model);
            }
            catch (NoSuchElementException ex)
            {
                _output.WriteLine($"Error: No se encontró el campo modelo para el dispositivo {deviceId}");
                _output.WriteLine($"Detalles: {ex.Message}");
                throw;
            }
        }

        // Enviar el formulario de creación del recibo
        public void PressSubmitReceipt()
        {
            Thread.Sleep(1000); 
            _driver.FindElement(By.Id("Submit")).Click();
        }

        // Volver a modificar las reparaciones seleccionadas
        public void PressModifyRepairs()
        {
            // Tu botón se llama "ModifyRepairs" 
            _driver.FindElement(By.Id("ModifyRepairs")).Click();
        }

        // Comprobar errores de validación del formulario
        public bool CheckValidationError(string expectedError)
        {
            Thread.Sleep(500); // Esperar a que Blazor muestre el error
            return _driver.PageSource.Contains(expectedError);
        }

        // Verificar que los datos del formulario se mantienen tras volver atrás o tras un error de validación
        public bool CheckFormDataPreserved(string expectedName, string expectedSurname, string expectedAddress, int repairId, string expectedModel)
        {
            // Esperar a que el campo Nombre vuelva a ser visible
            WaitForBeingVisible(_NameBy);

            // Comprobar que los valores introducidos siguen en los campos
            bool nameOk = _Name().GetAttribute("value") == expectedName;
            bool surnameOk = _Surname().GetAttribute("value") == expectedSurname;
            bool addressOk = _Address().GetAttribute("value") == expectedAddress;
            bool modelOk = _Model(repairId).GetAttribute("value") == expectedModel;

            return nameOk && surnameOk && addressOk && modelOk;
        }
    }
}
