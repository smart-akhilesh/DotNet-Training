using NUnit.Framework;
using NUnit.Framework.Legacy;
using Moq;
using System;
using System.Collections.Generic;
using Railway_Reservation_System_Project.Models;
using Railway_Reservation_System_Project.Repositories;
using Railway_Reservation_System_Project.Services;


namespace NUnit_Testing_Project
{
    [TestFixture]
    public class CustomerServiceTests
    {
        private Mock<ICustomerRepository> mockRepo;
        private CustomerService service;

        [SetUp]
        public void ArrangeObjects()
        {
            mockRepo = new Mock<ICustomerRepository>();
            service = new CustomerService(mockRepo.Object);
        }

        [Test]
        public void RegisterCustomer_ReturnsCustomerId()
        {
            // Arrange
            var customer = new Customer { EmailId = "test@example.com" };
            mockRepo.Setup(r => r.RegisterCustomer(customer)).Returns(1);

            // Act
            var result = service.RegisterCustomer(customer);

            // Assert
            ClassicAssert.AreEqual(1, result);
        }

        [Test]
        public void Login_WithValidCredentials_ReturnsTrue()
        {
          
            int customerId;
            mockRepo.Setup(r => r.Login("email", "pass", out It.Ref<int>.IsAny))
                    .Returns(true);

            
            var result = service.Login("email", "pass", out customerId);

       
            ClassicAssert.IsTrue(result);
        }

        [Test]
        public void ViewTrainsByRoute_ReturnsListOfTrains()
        {
          
            var trains = new List<Train> { new Train { TrainNo = 123 } };
            mockRepo.Setup(r => r.ViewTrainsByRoute("A", "B", It.IsAny<DateTime>()))
                    .Returns(trains);

         
            var result = service.ViewTrainsByRoute("A", "B", DateTime.Now);

       
            ClassicAssert.AreEqual(1, result.Count);
        }

        [Test]
        public void GetAvailableSeatsAndPrice_ReturnsCorrectValues()
        {
       
            mockRepo.Setup(r => r.GetAvailableSeatsAndPrice("123", It.IsAny<DateTime>(), "AC"))
                    .Returns((50, 1000m));

     
            var result = service.GetAvailableSeatsAndPrice("123", DateTime.Now, "AC");

        
            ClassicAssert.AreEqual(50, result.availableSeats);
            ClassicAssert.AreEqual(1000m, result.price);
        }

        [Test]
        public void BookTicket_ReturnsPNR()
        {
          
            var passengers = new List<Passenger> { new Passenger { PassengerName = "John" } };
            mockRepo.Setup(r => r.BookTicket(1, "123", "AC", It.IsAny<DateTime>(), 1, "Card", passengers))
                    .Returns("PNR123");

            var result = service.BookTicket(1, "123", "AC", DateTime.Now, 1, "Card", passengers);

        
            ClassicAssert.AreEqual("PNR123", result);
        }

        [Test]
        public void CancelPassengerTicket_ReturnsRefundAmount()
        {
           
            mockRepo.Setup(r => r.CancelPassengerTicket("PNR123", 1))
                    .Returns(200m);

        
            var result = service.CancelPassengerTicket("PNR123", 1);

          
            ClassicAssert.AreEqual(200m, result);
        }
    }
}
