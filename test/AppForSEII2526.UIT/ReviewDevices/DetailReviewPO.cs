using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.ReviewDevices
{
    internal class DetailReviewPO : PageObject
    {
        public DetailReviewPO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }
        public bool CheckReviewDetail(string name, string reviewTitle, string county, DateTime reviewDate)
        {
            WaitForBeingVisible(By.Id("ReviewTitle"));
            bool result = true;
            result = result && _driver.FindElement(By.Id("NameSurname")).Text.Contains(name);
            result = result && _driver.FindElement(By.Id("Country")).Text.Contains(county);
            result = result && _driver.FindElement(By.Id("ReviewTitle")).Text.Contains(reviewTitle);
            result = result && _driver.FindElement(By.Id("ReviewDate")).Text.Contains(reviewDate.ToString("dd MMMM yyyy"));

            return result;

        }

        public bool CheckListOfMovies(List<string[]> expectedReviewItems)
        {
            return CheckBodyTable(expectedReviewItems, By.Id("ReviewdDevices"));
        }
    }
}