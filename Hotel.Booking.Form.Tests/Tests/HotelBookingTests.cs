using FluentAssertions;
using Hotel.Booking.Form.Tests.Helpers;


namespace Hotel.Booking.Form.Tests.HotelBookingTests
{
    internal class HotelBookingTests : HelperMethods
    {
        
        [Test]
        public void CreateNewBooking()
        {
            //Arrange
            string firstName = "Tester";
            string surname = "Chester";
            string price = "50";
            string deposit = "true";
            string checkIn = "/html/body/div[2]/table/tbody/tr[2]/td[3]/a";
            string checkOut = "/html/body/div[2]/table/tbody/tr[2]/td[4]/a";

            //Act
            CreateBooking(firstName, surname, price, deposit, checkIn, checkOut);
            CaptureScreenshot("CreateBooking.jpg");

            //Assert
            Assert.Pass();
        }
        [Test]
        public async Task DeleteLatestBooking()
        {
            //Arrange
            int bookingIdForDeletion = await GetLastBookingId();

            //Act
            DeleteLastBooking(bookingIdForDeletion.ToString());
            CaptureScreenshot("BookingDeleted.jpg");

            //Assert
            var assertId = await GetLastBookingId();
            assertId.Should().NotBe(bookingIdForDeletion);
            TeardownSession();
        }
    }
}