using Core.Domain;
using Core.Enums;
using Core.Interfaces.Repository;
using Core.Models;
using Core.Models.Request;
using Core.Models.Responses;
using Core.Utils;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests.Services
{
    public class VehicleServiceTests
    {
        private Mock<IUnitOfWork> unitOfWorkMock;
        private Mock<ILogger<VehicleService>> loggerMock;
        private Mock<IVehicleRepository> vehicleRepositoryMock;
        private Mock<IRentalRepository> rentalRepositoryMock;

        private VehicleService vehicleService;

        [SetUp]
        public void Setup()
        {
            unitOfWorkMock = new Mock<IUnitOfWork>();
            loggerMock = new Mock<ILogger<VehicleService>>();
            vehicleRepositoryMock = new Mock<IVehicleRepository>();
            rentalRepositoryMock = new Mock<IRentalRepository>();

            unitOfWorkMock.Setup(x => x.VehicleRepository).Returns(vehicleRepositoryMock.Object);
            unitOfWorkMock.Setup(x => x.RentalRepository).Returns(rentalRepositoryMock.Object);

            vehicleService = new VehicleService(unitOfWorkMock.Object, loggerMock.Object);
        }

        [Test]
        public void ShouldAddVehicle()
        {
            var vehicleRequestModel = new VehicleRequestModel { Brand = "brand", PricePerDay = 10, Year = 2021 };

            var response = vehicleService.Add(vehicleRequestModel);

            vehicleRepositoryMock.Verify(x => x.Add(It.IsAny<Vehicle>()), Times.Once);
            unitOfWorkMock.Verify(x => x.Save(), Times.Once);
            Assert.False(response.HasErrors());
            Assert.AreEqual(response.Messages.Count(x => x.Code == Constants.VEHICLE_SAVED), 1);
        }

        [Test]
        public void ShouldValidateModel()
        {
            var vehicleRequestModel = new VehicleRequestModel { Brand = "", PricePerDay = null, Year = 2021 };

            var response = vehicleService.Add(vehicleRequestModel);

            vehicleRepositoryMock.Verify(x => x.Add(It.IsAny<Vehicle>()), Times.Never);
            Assert.True(response.HasErrors());
            Assert.AreEqual(response.Messages.Count(x => x.Type == MessageType.Error), 2);
            Assert.AreEqual(response.Messages.Count(x => x.Code == Constants.BRAND_EMPTY), 1);
            Assert.AreEqual(response.Messages.Count(x => x.Code == Constants.PRICE_PER_DAY_INVALID), 1);
        }

        [Test]
        public void ShouldNotSave()
        {
            unitOfWorkMock.Setup(x => x.Save()).Throws(new Exception());

            var vehicleRequestModel = new VehicleRequestModel { Brand = "brand", PricePerDay = 10, Year = 2021 };

            var response = vehicleService.Add(vehicleRequestModel);

            vehicleRepositoryMock.Verify(x => x.Add(It.IsAny<Vehicle>()), Times.Once);
            unitOfWorkMock.Verify(x => x.Save(), Times.Once);

            Assert.True(response.HasErrors());
            Assert.AreEqual(response.Messages.Count(x => x.Type == MessageType.Error), 1);
            Assert.AreEqual(response.Messages.Count(x => x.Code == Constants.GENERAL_ERROR), 1);
        }

        [Test]
        public void ShouldDeleteClient()
        {
            vehicleRepositoryMock.Setup(x => x.Find(It.IsAny<int>())).Returns(new Vehicle { Id = 1 });

            var response = vehicleService.Delete(1);

            vehicleRepositoryMock.Verify(x => x.Delete(It.IsAny<Vehicle>()), Times.Once);
            unitOfWorkMock.Verify(x => x.Save(), Times.Once);
            Assert.False(response.HasErrors());
            Assert.AreEqual(response.Messages.Count(x => x.Code == Constants.VEHICLE_DELETED), 1);
        }

        [Test]
        public void ShouldNotDeleteVehicleNotFound()
        {
            vehicleRepositoryMock.Setup(x => x.Find(It.IsAny<int>())).Returns((Vehicle)null);

            var response = vehicleService.Delete(1);

            vehicleRepositoryMock.Verify(x => x.Delete(It.IsAny<Vehicle>()), Times.Never);
            unitOfWorkMock.Verify(x => x.Save(), Times.Never);
            Assert.True(response.HasErrors());
            Assert.AreEqual(response.Messages.Count(x => x.Type == MessageType.Error), 1);
            Assert.AreEqual(response.Messages.Count(x => x.Code == Constants.VEHICLE_NOT_FOUND), 1);
        }

        [Test]
        public void ShouldNotDelete()
        {
            vehicleRepositoryMock.Setup(x => x.Find(It.IsAny<int>())).Returns(new Vehicle { Id = 1 });
            unitOfWorkMock.Setup(x => x.Save()).Throws(new Exception());

            var response = vehicleService.Delete(1);

            vehicleRepositoryMock.Verify(x => x.Delete(It.IsAny<Vehicle>()), Times.Once);
            unitOfWorkMock.Verify(x => x.Save(), Times.Once);

            Assert.True(response.HasErrors());
            Assert.AreEqual(response.Messages.Count(x => x.Type == MessageType.Error), 1);
            Assert.AreEqual(response.Messages.Count(x => x.Code == Constants.GENERAL_ERROR), 1);
        }

        [Test]
        public void ShouldGetAvailables()
        {
            var request = new SearchAvailableVehiclesRequestModel { StartDate = new DateTime(2022, 1, 1), EndDate = new DateTime(2022, 1, 3) };
            var vehicleAvailables = new List<Vehicle> { new Vehicle { Id = 1, Brand = "brand", PricePerDay = 10, Year = 2021 } };

            rentalRepositoryMock.Setup(x => x.GetVehiclesAvailables(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(vehicleAvailables);

            Response<IList<VehicleAvailableResponseModel>> response = vehicleService.GetAvailables(request);

            Assert.False(response.HasErrors());
            Assert.AreEqual(response.Data.Count, 1);
        }

        [Test]
        public void ShouldValidateRangeDates()
        {
            var request = new SearchAvailableVehiclesRequestModel { StartDate = new DateTime(2020, 1, 1), EndDate = new DateTime(2020, 1, 3) };

            Response<IList<VehicleAvailableResponseModel>> response = vehicleService.GetAvailables(request);

            rentalRepositoryMock.Verify(x => x.GetVehiclesAvailables(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);

            Assert.True(response.HasErrors());
            Assert.AreEqual(response.Data, null);
            Assert.AreEqual(response.Messages.Count(x => x.Type == MessageType.Error), 2);
            Assert.AreEqual(response.Messages.Count(x => x.Code == Constants.START_DATE_INVALID), 1);
            Assert.AreEqual(response.Messages.Count(x => x.Code == Constants.END_DATE_INVALID), 1);
        }

        [Test]
        public void ShouldValidateEndDateLowerThanStartDate()
        {
            var request = new SearchAvailableVehiclesRequestModel { StartDate = new DateTime(2022, 1, 5), EndDate = new DateTime(2022, 1, 3) };

            Response<IList<VehicleAvailableResponseModel>> response = vehicleService.GetAvailables(request);

            rentalRepositoryMock.Verify(x => x.GetVehiclesAvailables(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);

            Assert.True(response.HasErrors());
            Assert.AreEqual(response.Data, null);
            Assert.AreEqual(response.Messages.Count(x => x.Type == MessageType.Error), 1);
            Assert.AreEqual(response.Messages.Count(x => x.Code == Constants.START_DATE_GREATER_THAN_END_DATE), 1);
        }

        [Test]
        public void ShouldReturnAnEmptyListIfThereAreNotVehicleAvailables()
        {
            var request = new SearchAvailableVehiclesRequestModel { StartDate = new DateTime(2022, 1, 1), EndDate = new DateTime(2022, 1, 3) };
            rentalRepositoryMock.Setup(x => x.GetVehiclesAvailables(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(new List<Vehicle>());

            Response<IList<VehicleAvailableResponseModel>> response = vehicleService.GetAvailables(request);

            rentalRepositoryMock.Verify(x => x.GetVehiclesAvailables(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);

            Assert.False(response.HasErrors());
            Assert.AreEqual(response.Data.Count, 0);
            Assert.AreEqual(response.Messages.Count(x => x.Type == MessageType.Warning), 1);
            Assert.AreEqual(response.Messages.Count(x => x.Code == Constants.NO_VEHICLE_AVAILABLES), 1);
        }
    }
}
