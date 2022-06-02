using FluentAssertions;
using Hotel.Booking.Form.Tests.Helpers;


namespace Hotel.Booking.Form.Tests.HotelBookingTests
{
    internal class HotelBookingTests : HelperMethods
    {
        
        [Test]
        public async Task CreateNewBooking()
        {
            //Arrange
            string firstName = "UniqueName";
            string surname = "UniqueSurname";
            string price = "50";
            string deposit = "true";
            string checkIn = "/html/body/div[2]/table/tbody/tr[2]/td[3]/a";
            string checkOut = "/html/body/div[2]/table/tbody/tr[2]/td[4]/a";

            //Act
            CreateBooking(firstName, surname, price, deposit, checkIn, checkOut);
            CaptureScreenshot("CreateBooking.jpg");

            //Assert
            int lastBookingId = await GetLastBookingId();
            string name = await GetSingleBooking(lastBookingId.ToString());

            if (name.Equals("UniqueName"))
            {
                Assert.Pass();
            }
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