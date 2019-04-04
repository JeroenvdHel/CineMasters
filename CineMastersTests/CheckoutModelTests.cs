using CineMasters.Areas.Shows.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CineMastersTests
{
    public class CheckoutModelTests
    {
        [Fact]
        public void CanGetTotalPrice()
        {
            // Arrange
            Show show = new Show();
            Ticket t1 = new Ticket(show) { TicketPrice = 11.5M };
            Ticket t2 = new Ticket(show) { TicketPrice = 17.0M };
            Checkout checkout = new Checkout();
            checkout.Tickets = new List<Ticket>();
            checkout.Tickets.Add(t1);
            checkout.Tickets.Add(t2);
            checkout.CouponDiscount = 5M;

            // Act
            decimal result = checkout.GetTotalPrice();

            // Assert
            Assert.Equal(23.5M, result);
        }
    }
}
