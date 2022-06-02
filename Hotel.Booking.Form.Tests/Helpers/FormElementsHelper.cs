using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;


namespace Hotel.Booking.Form.Tests.Helpers
{
    internal class FormElementsHelper
    {
        private IWebDriver driver;
        public IWebElement firstNameField { get; set; }
        public IWebElement surnameField { get; set; }
        public IWebElement priceBox { get; set; }
        public IWebElement depositSelector { get; set; }
        public IWebElement checkInPicker { get; set; }
        public IWebElement checkOutPicker { get; set; }
        public IWebElement saveBtn { get; set; }
        public IWebElement deleteBtn { get; set; }

        public FormElementsHelper(IWebDriver driver)
        {
            this.driver = driver;

            //Create the methods to find each field that is going to be intereacted with.
            firstNameField = driver.FindElement(By.Id("firstname"));
            surnameField = driver.FindElement(By.Id("lastname"));
            priceBox = driver.FindElement(By.Id("totalprice"));
            depositSelector = driver.FindElement(By.Id("depositpaid"));
            checkInPicker = driver.FindElement(By.Id("checkin"));
            checkOutPicker = driver.FindElement(By.Id("checkout"));
            saveBtn = driver.FindElement(By.XPath("/html/body/div/div[3]/div/div[7]/input"));
        }

        //Create the interaction methods that will click and enter the details into the required fields
        public void EnterFirstName(string firstNameText)
        {
            firstNameField.Click();
            firstNameField.SendKeys(firstNameText);
        }

        public void EnterSurname(string lastNameText)
        {
            surnameField.Click();
            surnameField.SendKeys(lastNameText);
        }

        public void EnterPrice(string price)
        {
            priceBox.Click();
            priceBox.SendKeys(price);
        }

        public void SelectDeposit(string depositChoice)
        {
            var select = new SelectElement(depositSelector);
            select.SelectByText(depositChoice);
        }

        public void SelectCheckInDate(string xPathCheckInDate)
        {
            checkInPicker.Click();
            var checkInDate = driver.FindElement(By.XPath(xPathCheckInDate));
            checkInDate.Click();
        }

        public void SelectCheckOut(string xPathCheckOutDate)
        {
            checkOutPicker.Click();
            var checkOutDate = driver.FindElement(By.XPath(xPathCheckOutDate));
            checkOutDate.Click();
        }

        public void SaveBooking()
        {
            saveBtn.Click();
        }

        public void DeleteLastBooking(string bookingId)
        {
            var deleteBtn = driver.FindElement(By.XPath("/html/body/div/div[2]/div[@id=" + bookingId + "]/div[7]/input"));
            deleteBtn.Click();
        }
    }
}
