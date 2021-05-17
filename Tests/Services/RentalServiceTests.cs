using Core.Domain;
using Core.Enums;
using Core.Interfaces.Repository;
using Core.Models;
using Core.Models.Request;
using Core.Utils;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Service;
using System;
using System.Linq;

namespace Tests.Services
{
    public class RentalServiceTests
    {
        private Mock<IRepository> repositoryMock;
        private Mock<ILogger<RentalService>> loggerMock;
        private Mock<IRentalRepository> rentalRepositoryMock;
        private Mock<IVehicleRepository> vehicleRepositoryMock;
        private Mock<IClientRepository> clientRepositoryMock;

        private RentalService rentalService;

        [SetUp]
        public void Setup()
        {
            repositoryMock = new Mock<IRepository>();
            loggerMock = new Mock<ILogger<RentalService>>();
            rentalRepositoryMock = new Mock<IRentalRepository>();
            vehicleRepositoryMock = new Mock<IVehicleRepository>();
            clientRepositoryMock = new Mock<IClientRepository>();

            repositoryMock.Setup(x => x.RentalRepository).Returns(rentalRepositoryMock.Object);
            repositoryMock.Setup(x => x.VehicleRepository).Returns(vehicleRepositoryMock.Object);
            repositoryMock.Setup(x => x.ClientRepository).Returns(clientRepositoryMock.Object);

            rentalService = new RentalService(repositoryMock.Object, loggerMock.Object);
        }

        [Test]
        public void ShouldAddRental()
        {
            var request = new CreateRentalRequestModel { StartDate = new DateTime(2022, 1, 1), EndDate = new DateTime(2022, 1, 3), ClientId = 1, VehicleId = 1 };

            vehicleRepositoryMock.Setup(x => x.Find(It.IsAny<int>())).Returns(new Vehicle { Active = true, Id = 1, Brand = "brand", PricePerDay = 10, Year = 2022 });
            clientRepositoryMock.Setup(x => x.Find(It.IsAny<int>())).Returns(new Client { Active = true, Id = 1, Name = "name", Phone = "1243" });
            rentalRepositoryMock.Setup(x => x.VerifyIfVehicleIsAvailableByRangeDates(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(true);

            Response response = rentalService.Add(request);

            rentalRepositoryMock.Verify(x => x.VerifyIfVehicleIsAvailableByRangeDates(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
            rentalRepositoryMock.Verify(x => x.Add(It.IsAny<Rental>()));
            repositoryMock.Verify(x => x.Save(), Times.Once);

            Assert.False(response.HasErrors());
            Assert.AreEqual(response.Messages.Count(x => x.Type == MessageType.Error), 0);
            Assert.AreEqual(response.Messages.Count(x => x.Type == MessageType.Success), 1);
            Assert.AreEqual(response.Messages.Count(x => x.Code == Constants.RENTAL_SAVED), 1);
        }

        [Test]
        public void ShouldNotSaveRentalWhenExceptionIsThrown()
        {
            var request = new CreateRentalRequestModel { StartDate = new DateTime(2022, 1, 1), EndDate = new DateTime(2022, 1, 3), ClientId = 1, VehicleId = 1 };

            vehicleRepositoryMock.Setup(x => x.Find(It.IsAny<int>())).Returns(new Vehicle { Active = true, Id = 1, Brand = "brand", PricePerDay = 10, Year = 2022 });
            clientRepositoryMock.Setup(x => x.Find(It.IsAny<int>())).Returns(new Client { Active = true, Id = 1, Name = "name", Phone = "1243" });
            rentalRepositoryMock.Setup(x => x.VerifyIfVehicleIsAvailableByRangeDates(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(true);
            repositoryMock.Setup(x => x.Save()).Throws(new Exception());

            Response response = rentalService.Add(request);

            rentalRepositoryMock.Verify(x => x.VerifyIfVehicleIsAvailableByRangeDates(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
            rentalRepositoryMock.Verify(x => x.Add(It.IsAny<Rental>()));
            repositoryMock.Verify(x => x.Save(), Times.Once);

            Assert.True(response.HasErrors());
            Assert.AreEqual(response.Messages.Count(x => x.Type == MessageType.Error), 1);
            Assert.AreEqual(response.Messages.Count(x => x.Type == MessageType.Success), 0);
            Assert.AreEqual(response.Messages.Count(x => x.Code == Constants.GENERAL_ERROR), 1);
        }

        [Test]
        public void ShouldValidateRequest()
        {
            var request = new CreateRentalRequestModel { StartDate = new DateTime(2020, 1, 1), EndDate = new DateTime(2020, 1, 3), ClientId = null, VehicleId = null };

            Response response = rentalService.Add(request);

            rentalRepositoryMock.Verify(x => x.VerifyIfVehicleIsAvailableByRangeDates(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);
            repositoryMock.Verify(x => x.Save(), Times.Never);

            Assert.True(response.HasErrors());
            Assert.AreEqual(response.Messages.Count(x => x.Type == MessageType.Error), 4);
            Assert.AreEqual(response.Messages.Count(x => x.Code == Constants.START_DATE_INVALID), 1);
            Assert.AreEqual(response.Messages.Count(x => x.Code == Constants.END_DATE_INVALID), 1);
            Assert.AreEqual(response.Messages.Count(x => x.Code == Constants.VEHICLEID_EMPTY), 1);
            Assert.AreEqual(response.Messages.Count(x => x.Code == Constants.CLIENTID_EMPTY), 1);
        }

        [Test]
        public void ShouldValidateClientAndVehicleNotFound()
        {
            var request = new CreateRentalRequestModel { StartDate = new DateTime(2022, 1, 1), EndDate = new DateTime(2022, 1, 3), ClientId = 1, VehicleId = 1 };

            vehicleRepositoryMock.Setup(x => x.Find(It.IsAny<int>())).Returns((Vehicle)null);
            clientRepositoryMock.Setup(x => x.Find(It.IsAny<int>())).Returns((Client)null);

            Response response = rentalService.Add(request);

            rentalRepositoryMock.Verify(x => x.VerifyIfVehicleIsAvailableByRangeDates(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);
            repositoryMock.Verify(x => x.Save(), Times.Never);

            Assert.True(response.HasErrors());
            Assert.AreEqual(response.Messages.Count(x => x.Type == MessageType.Error), 2);
            Assert.AreEqual(response.Messages.Count(x => x.Code == Constants.CLIENT_NOT_FOUND), 1);
            Assert.AreEqual(response.Messages.Count(x => x.Code == Constants.VEHICLE_NOT_FOUND), 1);
        }

        [Test]
        public void ShouldValidateClientAndVehicleInactives()
        {
            var request = new CreateRentalRequestModel { StartDate = new DateTime(2022, 1, 1), EndDate = new DateTime(2022, 1, 3), ClientId = 1, VehicleId = 1 };

            vehicleRepositoryMock.Setup(x => x.Find(It.IsAny<int>())).Returns(new Vehicle { Active = false });
            clientRepositoryMock.Setup(x => x.Find(It.IsAny<int>())).Returns(new Client { Active = false });

            Response response = rentalService.Add(request);

            rentalRepositoryMock.Verify(x => x.VerifyIfVehicleIsAvailableByRangeDates(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);
            repositoryMock.Verify(x => x.Save(), Times.Never);

            Assert.True(response.HasErrors());
            Assert.AreEqual(response.Messages.Count(x => x.Type == MessageType.Error), 2);
            Assert.AreEqual(response.Messages.Count(x => x.Code == Constants.CLIENT_NOT_FOUND), 1);
            Assert.AreEqual(response.Messages.Count(x => x.Code == Constants.VEHICLE_NOT_FOUND), 1);
        }

        [Test]
        public void ShouldValidateVehicleNotAvailable()
        {
            var request = new CreateRentalRequestModel { StartDate = new DateTime(2022, 1, 1), EndDate = new DateTime(2022, 1, 3), ClientId = 1, VehicleId = 1 };

            vehicleRepositoryMock.Setup(x => x.Find(It.IsAny<int>())).Returns(new Vehicle { Active = true, Id = 1 });
            clientRepositoryMock.Setup(x => x.Find(It.IsAny<int>())).Returns(new Client { Active = true, Id = 1 });
            rentalRepositoryMock.Setup(x => x.VerifyIfVehicleIsAvailableByRangeDates(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(false);

            Response response = rentalService.Add(request);

            rentalRepositoryMock.Verify(x => x.VerifyIfVehicleIsAvailableByRangeDates(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
            repositoryMock.Verify(x => x.Save(), Times.Never);

            Assert.True(response.HasErrors());
            Assert.AreEqual(response.Messages.Count(x => x.Type == MessageType.Error), 1);
            Assert.AreEqual(response.Messages.Count(x => x.Code == Constants.VEHICLE_NOT_AVAILABLE), 1);
        }

        [Test]
        public void ShouldCancelRental()
        {
            rentalRepositoryMock.Setup(x => x.Find(It.IsAny<int>())).Returns(new Rental { Id = 1, Status = RentalStatus.Reserved });

            var response = rentalService.Cancel(1);

            rentalRepositoryMock.Verify(x => x.Update(It.IsAny<Rental>()), Times.Once);
            repositoryMock.Verify(x => x.Save(), Times.Once);
            Assert.False(response.HasErrors());
            Assert.AreEqual(response.Messages.Count(x => x.Type == MessageType.Success), 1);
            Assert.AreEqual(response.Messages.Count(x => x.Code == Constants.RENTAL_CANCELLED), 1);
        }

        [Test]
        public void ShouldNotCancelRentalNotFound()
        {
            rentalRepositoryMock.Setup(x => x.Find(It.IsAny<int>())).Returns((Rental)null);

            var response = rentalService.Cancel(1);

            rentalRepositoryMock.Verify(x => x.Update(It.IsAny<Rental>()), Times.Never);
            repositoryMock.Verify(x => x.Save(), Times.Never);
            Assert.True(response.HasErrors());
            Assert.AreEqual(response.Messages.Count(x => x.Type == MessageType.Error), 1);
            Assert.AreEqual(response.Messages.Count(x => x.Code == Constants.RENTAL_NOT_FOUND), 1);
        }

        [Test]
        public void ShouldNotCancelRentalWhenExceptionIsThrown()
        {
            rentalRepositoryMock.Setup(x => x.Find(It.IsAny<int>())).Returns(new Rental { Id = 1, Status = RentalStatus.Reserved });
            repositoryMock.Setup(x => x.Save()).Throws(new Exception());

            var response = rentalService.Cancel(1);

            rentalRepositoryMock.Verify(x => x.Update(It.IsAny<Rental>()), Times.Once);
            repositoryMock.Verify(x => x.Save(), Times.Once);
            Assert.True(response.HasErrors());
            Assert.AreEqual(response.Messages.Count(x => x.Type == MessageType.Error), 1);
            Assert.AreEqual(response.Messages.Count(x => x.Code == Constants.GENERAL_ERROR), 1);
        }
    }
}
