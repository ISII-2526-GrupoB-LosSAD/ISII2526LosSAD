using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.ReviewDevices
{
    internal class CreateReviewPO : PageObject
    {
        private By _ReviewTitleBy = By.Id("ReviewTitle");
        private IWebElement _reviewTitle() => _driver.FindElement(_ReviewTitleBy);
        private IWebElement _CustomerUserName() => _driver.FindElement(By.Id("CustomerUserName"));
        private IWebElement _Country() => _driver.FindElement(By.Id("Country"));
        private IWebElement _Comentario(int deviceId) => _driver.FindElement(By.Id("comentario_" + deviceId));
        private IWebElement _Rating(int deviceId) => _driver.FindElement(By.Id("rating_" + deviceId));
        public CreateReviewPO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }
        public void FillInReviewInfo(string reviewTitle, string customerUserName, string country)
        {
            WaitForBeingVisible(_ReviewTitleBy);
            _reviewTitle().SendKeys(reviewTitle);
            _CustomerUserName().SendKeys(customerUserName);
            _Country().SendKeys(country);
        }

        public void AddDeviceReviewComent(int deviceId, string comentario)
        {
            try
            {
                // Asegurarse de que los campos están visibles
                WaitForBeingVisible(By.Id($"comentario_{deviceId}"));

                

                // Añadir comentario
                _Comentario(deviceId).Clear();
                _Comentario(deviceId).SendKeys(comentario);

                
            }
            catch (NoSuchElementException ex)
            {
                _output.WriteLine($"Error: No se encontraron los campos para el dispositivo {deviceId}");
                _output.WriteLine($"Detalles: {ex.Message}");
                throw;
            }
        }
        public void AddDeviceReviewRating(int deviceId,  int rating)
        {
            try
            {
                // Asegurarse de que los campos están visibles
                WaitForBeingVisible(By.Id($"rating_{deviceId}"));

                // Añadir rating (asumiendo que es un input numérico o texto)
                _Rating(deviceId).Clear();
                _Rating(deviceId).SendKeys(rating.ToString());

                

            }
            catch (NoSuchElementException ex)
            {
                _output.WriteLine($"Error: No se encontraron los campos para el dispositivo {deviceId}");
                _output.WriteLine($"Detalles: {ex.Message}");
                throw;
            }
        }
        public void FillInReviewComent(string reviewComent, int deviceId)
        {
            _driver.FindElement(By.Id("comentario_" + deviceId)).SendKeys(reviewComent);
        }


        public void PressReviewYourDevices()
        {
            Thread.Sleep(1000);
            _driver.FindElement(By.Id("Submit")).Click();
        }



        public void PressModifyMovies()
        {
            _driver.FindElement(By.Id("ModifyDevices")).Click();
        }

        public bool CheckListOfReviewItems(List<string[]> expectedReviewItems)
        {
            return CheckBodyTable(expectedReviewItems, By.Id("TableOfReviewItems"));
        }

        public bool CheckValidationError(string expectedError)
        {
            return _driver.PageSource.Contains(expectedError);
        }
    }
}
