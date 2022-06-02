using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Text.Json;

namespace Hotel.Booking.Form.Tests.Helpers
{
    internal class HelperMethods
    {
        //Create the globals that will be used for the various methods
        public IWebDriver drv = new ChromeDriver();
        public List<int> allBookingIds = new();
        const string url = "http://hotel-test.equalexperts.io/";
        const string getAllBookingIdsUrl = "http://hotel-test.equalexperts.io/booking/";
        public HttpClient httpClient = new();

        public void CreateBooking(string firstname, string surname, string price, string deposit, string checkIn, string checkOut)
        {
            //Start up the Chrome window and navigate to the Hotel Booking Form
            drv.Manage().Window.Maximize();
            drv.Navigate().GoToUrl(url);
            Thread.Sleep(1000);
            WebDriverWait wait = new(drv, TimeSpan.FromSeconds(10));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("checkout")));
            FormElementsHelper formElements = new(drv);

            //Enter all of the supplied details and click Save
            formElements?.EnterFirstName(firstname);
            formElements?.EnterSurname(surname);
            formElements?.EnterPrice(price);
            formElements?.SelectDeposit(deposit);
            formElements?.SelectCheckInDate(checkIn);
            formElements?.SelectCheckOut(checkOut);
            formElements?.SaveBooking();
            Thread.Sleep(3000);
        }

        public async Task<string> GetSingleBooking(string bookingId)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, getAllBookingIdsUrl + bookingId);
            request.Headers.Add("Accept", "*/*");
            HttpResponseMessage response = await httpClient.SendAsync(request);
            var fullBooking = await response.Content.ReadAsStringAsync();
            JsonDocument parsedFullBooking = JsonDocument.Parse(fullBooking);
            string firstname = null;

            var singleBookingId = parsedFullBooking.RootElement;
            firstname = singleBookingId.GetProperty("firstname").ToString();

            return firstname;
        }

        public async Task<int> GetLastBookingId()
        {
            //Retrieve all of the previous bookings
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, getAllBookingIdsUrl);
            HttpResponseMessage response = await httpClient.SendAsync(request);
            var allBookings = await response.Content.ReadAsStringAsync();
            JsonDocument parsedAllBookings = JsonDocument.Parse(allBookings);

            //Extract just the bookingIds from the object
            for (int i = 0; i < parsedAllBookings.RootElement.GetArrayLength(); i++)
            {
                var singleBookingId = parsedAllBookings.RootElement[i];
                var bookingId = singleBookingId.GetProperty("bookingid");
                allBookingIds.Add(int.Parse(bookingId.ToString()));
            }

            return allBookingIds.Last();
        }

        public void DeleteLastBooking(string bookingForDeletion)
        {
            FormElementsHelper formElements = new(drv);

            WebDriverWait wait = new(drv, TimeSpan.FromSeconds(5));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id(bookingForDeletion)));

            formElements?.DeleteLastBooking(bookingForDeletion);
            Thread.Sleep(3000);
        }

        public void CaptureScreenshot(string screenshotName)
        {
            Thread.Sleep(1000);
            ITakesScreenshot screenshotDriver = drv as ITakesScreenshot;
            Screenshot screenshot = screenshotDriver.GetScreenshot();
            screenshot.SaveAsFile(Directory.GetCurrentDirectory() + "/" + screenshotName);
        }

        public void TeardownSession()
        {
            drv.Quit();
        }
    }
}
