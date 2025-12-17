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
        public bool CheckReviewDetail(string name, string reviewTitle, string county)
        {
            WaitForBeingVisible(By.Id("ReviewTitle"));
            bool result = true;
            result = result && _driver.FindElement(By.Id("NameSurname")).Text.Contains(name);
            result = result && _driver.FindElement(By.Id("Country")).Text.Contains(county);
            result = result && _driver.FindElement(By.Id("ReviewTitle")).Text.Contains(reviewTitle);
            // Opción recomendada: usar una cultura invariable o solo comparar día y año
            string reviewDate = DateTime.Now.ToString("dd") + " " + DateTime.Now.ToString("yyyy");
            // O simplemente verificar que el elemento contiene el año actual
            result = result && _driver.FindElement(By.Id("ReviewDate")).Text.Contains(DateTime.Now.Year.ToString());

            return result;

        }

        public bool CheckListOfDevices(List<string[]> expectedReviewItems)
        {
            return CheckBodyTable(expectedReviewItems, By.Id("ReviewdDevices"));
        }
    }
}