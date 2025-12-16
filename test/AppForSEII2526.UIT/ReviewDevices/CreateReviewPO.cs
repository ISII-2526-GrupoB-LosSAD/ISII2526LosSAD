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

        public void FillInReviewComent(string reviewComent, int deviceId)
        {
            _driver.FindElement(By.Id("comentario_" + deviceId)).SendKeys(reviewComent);
        }


        public void PressReviewYourDevices()
        {
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
