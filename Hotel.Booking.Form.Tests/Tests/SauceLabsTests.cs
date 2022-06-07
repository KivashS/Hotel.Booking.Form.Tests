using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using Hotel.Booking.Form.Tests.Helpers;

namespace Hotel.Booking.Form.Tests.Tests
{
    [TestFixture]
    internal class SauceLabsTests : HelperMethods
    {
        public IWebDriver sauceDrv { get; set; }

        [SetUp]
        public void Setup()
        {
            var browserOptions = new ChromeOptions();
            browserOptions.PlatformName = "Windows 10";
            browserOptions.BrowserVersion = "latest";
            var sauceOptions = new Dictionary<string, object>();
            sauceOptions.Add("name", "Hotel Booking Form Test ");
            sauceOptions.Add("username", "oauth-kivashs-f74c6");
            sauceOptions.Add("accesskey", "29e67a90-0489-4ce7-81f5-f1536b5b0eb1");
            browserOptions.AddAdditionalOption("sauce:options", sauceOptions);
            var saucelabsurl = new Uri("https://oauth-kivashs-f74c6:29e67a90-0489-4ce7-81f5-f1536b5b0eb1@ondemand.eu-central-1.saucelabs.com:443/wd/hub");
            sauceDrv = new RemoteWebDriver(saucelabsurl, browserOptions);
        }
        [Test]
        public async Task CreateNewBooking()
        {
            //Arrange
            string firstName = "Saucy";
            string surname = "Test";
            string price = "10";
            string deposit = "true";
            string checkIn = "/html/body/div[2]/table/tbody/tr[2]/td[3]/a";
            string checkOut = "/html/body/div[2]/table/tbody/tr[2]/td[4]/a";

            //Act
            sauceDrv.Navigate().GoToUrl("http://hotel-test.equalexperts.io/");

            WebDriverWait wait = new(sauceDrv, TimeSpan.FromSeconds(10));

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("checkout")));

            var firstNameField = sauceDrv.FindElement(By.Id("firstname"));
            var surnameField = sauceDrv.FindElement(By.Id("lastname"));
            var priceBox = sauceDrv.FindElement(By.Id("totalprice"));
            var depositSelector = sauceDrv.FindElement(By.Id("depositpaid"));
            var checkInPicker = sauceDrv.FindElement(By.Id("checkin"));
            var checkOutPicker = sauceDrv.FindElement(By.Id("checkout"));
            var saveBtn = sauceDrv.FindElement(By.XPath("/html/body/div/div[3]/div/div[7]/input"));

            firstNameField.Click();
            firstNameField.SendKeys(firstName);

            surnameField.Click();
            surnameField.SendKeys(surname);

            priceBox.Click();
            priceBox.SendKeys(price);

            var select = new SelectElement(depositSelector);
            select.SelectByText(deposit);

            checkInPicker.Click();
            var checkInDate = sauceDrv.FindElement(By.XPath(checkIn));
            checkInDate.Click();

            checkOutPicker.Click();
            var checkOutDate = sauceDrv.FindElement(By.XPath(checkOut));
            checkOutDate.Click();

            saveBtn.Click();

            //Assert
            int lastBookingId = await GetLastBookingId();
            string name = await GetSingleBooking(lastBookingId.ToString());

            if (name.Equals("Saucy"))
            {
                Assert.Pass();
            }
        }

        [TearDown]
        public void Teardown()
        {
            var isPassed = TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed;
            var script = "sauce:job-result" + (isPassed ? "passed" : "failed");
            ((IJavaScriptExecutor)sauceDrv).ExecuteScript(script);
            sauceDrv.Quit();
        }
    }
}
