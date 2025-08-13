using NUnit.Framework;
using NUnit.Framework.Legacy;
using Moq;
using System;
using System.Collections.Generic;
using Railway_Reservation_System_Project.Interfaces.Repositories;
using Railway_Reservation_System_Project.Services;
using Railway_Reservation_System_Project.Models;
using Railway_Reservation_System_Project.Exceptions;

namespace NUnit_Testing_Project
{
    [TestFixture]
    public class AdminServiceTests
    {
        private Mock<IAdminRepository> mockRepo;
        private AdminService service;

        [SetUp]
        public void ArrangeObjects()
        {
            mockRepo = new Mock<IAdminRepository>();
            service = new AdminService(mockRepo.Object);
        }

        [Test]
        public void Login_WithValidAdmin_ReturnsTrue()
        {
            var admin = new Admin();
            mockRepo.Setup(r => r.Login(admin)).Returns(true);

            var result = service.Login(admin);

            ClassicAssert.IsTrue(result);
        }

        [Test]
        public void Login_WithInvalidAdmin_ThrowsLoginException()
        {
            var admin = new Admin();
            mockRepo.Setup(r => r.Login(admin)).Returns(false);

            ClassicAssert.Throws<LoginException>(() => service.Login(admin));
        }

        [Test]
        public void Login_WhenRepoThrowsException_ThrowsLoginException()
        {
            var admin = new Admin();
            mockRepo.Setup(r => r.Login(admin)).Throws(new Exception("DB Error"));

            ClassicAssert.Throws<LoginException>(() => service.Login(admin));
        }

        [Test]
        public void AddTrain_WhenRepoSucceeds_ReturnsTrue()
        {
            var train = new Train();
            mockRepo.Setup(r => r.InsertTrain(train)).Returns(true);

            var result = service.AddTrain(train);

            ClassicAssert.IsTrue(result);
        }

        [Test]
        public void AddTrain_WhenRepoFails_ThrowsTrainOperationException()
        {
            var train = new Train();
            mockRepo.Setup(r => r.InsertTrain(train)).Returns(false);

            ClassicAssert.Throws<TrainOperationException>(() => service.AddTrain(train));
        }

        [Test]
        public void AddTrain_WhenRepoThrowsException_ThrowsTrainOperationException()
        {
            var train = new Train();
            mockRepo.Setup(r => r.InsertTrain(train)).Throws(new Exception("DB Error"));

            ClassicAssert.Throws<TrainOperationException>(() => service.AddTrain(train));
        }

        [Test]
        public void EditTrain_Success_ReturnsTrue()
        {
            var train = new Train();
            mockRepo.Setup(r => r.UpdateTrain(train)).Returns(true);

            var result = service.EditTrain(train);

            ClassicAssert.IsTrue(result);
        }

        [Test]
        public void EditTrain_Fails_ThrowsTrainOperationException()
        {
            var train = new Train();
            mockRepo.Setup(r => r.UpdateTrain(train)).Returns(false);

            ClassicAssert.Throws<TrainOperationException>(() => service.EditTrain(train));
        }

        [Test]
        public void EditTrain_WhenRepoThrowsException_ThrowsTrainOperationException()
        {
            var train = new Train();
            mockRepo.Setup(r => r.UpdateTrain(train)).Throws(new Exception("DB Error"));

            ClassicAssert.Throws<TrainOperationException>(() => service.EditTrain(train));
        }


      
        [Test]
        public void RemoveTrain_WhenRepoThrowsException_ThrowsTrainOperationException()
        {
            string trainNo = "123";
            mockRepo.Setup(r => r.DeleteTrain(trainNo)).Throws(new Exception("DB Error"));

            ClassicAssert.Throws<TrainOperationException>(() => service.RemoveTrain(trainNo));
        }

        [Test]
        public void GenerateBookingRevenueReport_WithValidData_ReturnsReport()
        {
            DateTime sd = DateTime.Today, ed = DateTime.Today.AddDays(1);
            var report = new BookingRevenueReport();
            mockRepo.Setup(r => r.GetTotalBookingsAndRevenue(sd, ed)).Returns(report);

            var result = service.GenerateBookingRevenueReport(sd, ed);

            ClassicAssert.AreSame(report, result);
        }

        [Test]
        public void GenerateBookingRevenueReport_NoData_ThrowsReportGenerationException()
        {
            DateTime sd = DateTime.Today, ed = DateTime.Today.AddDays(1);
            mockRepo.Setup(r => r.GetTotalBookingsAndRevenue(sd, ed)).Returns((BookingRevenueReport)null);

            ClassicAssert.Throws<ReportGenerationException>(() => service.GenerateBookingRevenueReport(sd, ed));
        }

        [Test]
        public void GenerateBookingRevenueReport_WhenRepoThrowsException_ThrowsReportGenerationException()
        {
            DateTime sd = DateTime.Today, ed = DateTime.Today.AddDays(1);
            mockRepo.Setup(r => r.GetTotalBookingsAndRevenue(sd, ed)).Throws(new Exception("DB Error"));

            ClassicAssert.Throws<ReportGenerationException>(() => service.GenerateBookingRevenueReport(sd, ed));
        }

        [Test]
        public void GenerateTrainBookingReport_WithData_ReturnsList()
        {
            DateTime sd = DateTime.Today, ed = DateTime.Today.AddDays(1);
            var list = new List<TrainBookingReport> { new TrainBookingReport() };
            mockRepo.Setup(r => r.GetBookingsPerTrain(sd, ed)).Returns(list);

            var result = service.GenerateTrainBookingReport(sd, ed);

            ClassicAssert.AreEqual(1, result.Count);
        }

        [Test]
        public void GenerateTrainBookingReport_NoData_ThrowsReportGenerationException()
        {
            DateTime sd = DateTime.Today, ed = DateTime.Today.AddDays(1);
            mockRepo.Setup(r => r.GetBookingsPerTrain(sd, ed)).Returns(new List<TrainBookingReport>());

            ClassicAssert.Throws<ReportGenerationException>(() => service.GenerateTrainBookingReport(sd, ed));
        }

        [Test]
        public void GenerateTrainBookingReport_WhenRepoThrowsException_ThrowsReportGenerationException()
        {
            DateTime sd = DateTime.Today, ed = DateTime.Today.AddDays(1);
            mockRepo.Setup(r => r.GetBookingsPerTrain(sd, ed)).Throws(new Exception("DB Error"));

            ClassicAssert.Throws<ReportGenerationException>(() => service.GenerateTrainBookingReport(sd, ed));
        }

        [Test]
        public void GenerateCancellationReport_WithData_ReturnsList()
        {
            DateTime sd = DateTime.Today, ed = DateTime.Today.AddDays(1);
            string trainNo = "123";
            var list = new List<CancellationReport> { new CancellationReport() };
            mockRepo.Setup(r => r.GetCancellationRefunds(sd, ed, trainNo)).Returns(list);

            var result = service.GenerateCancellationReport(sd, ed, trainNo);

            ClassicAssert.AreEqual(1, result.Count);
        }

        [Test]
        public void GenerateCancellationReport_NoData_ThrowsReportGenerationException()
        {
            DateTime sd = DateTime.Today, ed = DateTime.Today.AddDays(1);
            string trainNo = "123";
            mockRepo.Setup(r => r.GetCancellationRefunds(sd, ed, trainNo)).Returns(new List<CancellationReport>());

            ClassicAssert.Throws<ReportGenerationException>(() => service.GenerateCancellationReport(sd, ed, trainNo));
        }

        [Test]
        public void GenerateCancellationReport_WhenRepoThrowsException_ThrowsReportGenerationException()
        {
            DateTime sd = DateTime.Today, ed = DateTime.Today.AddDays(1);
            string trainNo = "123";
            mockRepo.Setup(r => r.GetCancellationRefunds(sd, ed, trainNo)).Throws(new Exception("DB Error"));

            ClassicAssert.Throws<ReportGenerationException>(() => service.GenerateCancellationReport(sd, ed, trainNo));
        }
    }
}
