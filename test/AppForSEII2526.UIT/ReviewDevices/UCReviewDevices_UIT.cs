using AppForMovies.UIT.Shared;
using OpenQA.Selenium.DevTools.V141.Security;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AppForSEII2526.UIT.ReviewDevices
{
    public class UCReviewDevices_UIT : UC_UIT {
        public UCReviewDevices_UIT(ITestOutputHelper output) : base(output) {
            Initial_step_opening_the_web_page();
            selectDevices = new SelectDevicesForReviewPO(_driver, _output);
        }
        private const int deviceId1 = 1;
        private const string deviceName1 = "Galaxy S24 Ultra";
        private const string deviceColor1 = "Black";
        private const string deviceBrand1 = "Samsung";
        private const int deviceYear1 = 2024;
        private const string deviceModel1 = "Galaxy S Series";

        private const int deviceId2 = 6;
        private const string deviceName2 = "ThinkPad X1 Gen11";
        private const string deviceColor2 = "Black";
        private const string deviceBrand2 = "Lenovo";
        private const int deviceYear2 = 2023;
        private const string deviceModel2 = "ThinkPad X1";
        private SelectDevicesForReviewPO selectDevices;


        private void Precondition_perform_login()
        {
            Perform_login("elena@uclm.es", "Password1234%");
        }

        private void InitialStepsForReviewDevice_UIT()
        {
            Precondition_perform_login();
            Thread.Sleep(1000);
            
            selectDevices.WaitForBeingVisibleIgnoringExeptionTypes(By.Id("CreateReview"));
            _driver.FindElement(By.Id("CreateReview")).Click();
        }

        [Theory]
        [InlineData(deviceBrand1, deviceColor1, deviceName1, deviceYear1, deviceModel1, "Samsung", 2024)]
        [InlineData(deviceBrand2, deviceColor2, deviceName2, deviceYear2, deviceModel2, "",2023)]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC3_2_3_filteringbyBrandandYear(string brand, string color,
            string name, int year, string model, string filterBrand, int filterYear)
        {
            //Arrange
           
            var expectedDevices = new List<string[]> { new string[] { brand, color,name, year.ToString(), model  }, };
            //Act
            InitialStepsForReviewDevice_UIT();

            selectDevices.SearchDevices(filterBrand, filterYear);

            //Assert            
            Assert.True(selectDevices.CheckListOfDevices(expectedDevices));

        }


        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC3_4_ModifySelecteDevices()
        {
            //Arrange

            //Act
            InitialStepsForReviewDevice_UIT();

            
            selectDevices.SelectDevices(new List<string> { deviceId1.ToString(), deviceId2.ToString() });
            Thread.Sleep(500);
            selectDevices.ModifyReviewCart(deviceName2);


            //Assert            
            Assert.True(selectDevices.CheckShoppingCart(deviceName1));
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC3_5_ReviewButtonNotAvailable()
        {
            //Arrange

            //Act
            InitialStepsForReviewDevice_UIT();

            selectDevices.SelectDevices(new List<string> { deviceId1.ToString() });
            Thread.Sleep(500);
            selectDevices.ModifyReviewCart(deviceName1);
            Thread.Sleep(500);


            //Assert            
            Assert.True(selectDevices.CheckReviewDeviceDisabled(), "You must select at least one device");
        }


        [Theory]
        [InlineData("Elena", "Spain",  "", "Reseña para", 5, "The ReviewTitle field is required.")]
        [InlineData("Elena", "",  "Perfecto rendimiento", "Reseña para", 5, "The Country field is required.")]
        [InlineData("Elena", "Spain", "Perfecto rendimiento", "Reseña para", null, "")]
        [InlineData("Elena", "Spain", "Perfecto rendimiento", "", 5, "")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC3_6_7_8_9_testingErrorsMandatorydata(string username, string country, string reviretitle,string comentario, int rating,
           string expectedMessageError)
        {
            //Arrange

            var createreview = new CreateReviewPO(_driver, _output);

            //Act
            InitialStepsForReviewDevice_UIT();

            selectDevices.SelectDevices(new List<string> { deviceId1.ToString() });
            selectDevices.ReviewDevices();
            createreview.FillInReviewInfo(reviretitle, username, country);
            Thread.Sleep(1000);
            
            createreview.PressReviewYourDevices();

            //Assert
            //the expected error is shown in the view
            Assert.True(createreview.CheckValidationError(expectedMessageError), $"Expected error: {expectedMessageError}");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC3_10_ModifyRentalItems()
        {
            //Arrange

            var createreview = new CreateReviewPO(_driver, _output);

            //Act
            InitialStepsForReviewDevice_UIT();

            
            selectDevices.SelectDevices(new List<string> { deviceId1.ToString()});
            selectDevices.ReviewDevices();
            Thread.Sleep(1000);
            createreview.PressModifyMovies();
            

            //Assert
            //the list of movies must change
            Assert.True(selectDevices.CheckShoppingCart(deviceName1));
        }
        [Theory]
        [InlineData("Elena", "Spain", "Perfecto rendimiento", "Reseña para", -5, "")]
        [InlineData("Elena", "Spain", "Perfecto rendimiento", "No sirve", 5, "")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC3_11_12_testingErrorsvalidationdata(string username, string country, string reviretitle, string comentario, int rating,
          string expectedMessageError)
        {
            //Arrange

            var createreview = new CreateReviewPO(_driver, _output);

            //Act
            InitialStepsForReviewDevice_UIT();

            selectDevices.SelectDevices(new List<string> { deviceId1.ToString() });
            selectDevices.ReviewDevices();
            createreview.FillInReviewInfo(reviretitle, username, country);
            createreview.AddDeviceReviewComent(deviceId1, comentario);
            Thread.Sleep(1000);
            createreview.AddDeviceReviewRating(deviceId1, rating);
            Thread.Sleep(1000);

            createreview.PressReviewYourDevices();

            //Assert
            //the expected error is shown in the view
            Assert.True(createreview.CheckValidationError(expectedMessageError), $"Expected error: {expectedMessageError}");
        }

    }
}
