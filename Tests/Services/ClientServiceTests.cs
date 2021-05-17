using Core.Domain;
using Core.Enums;
using Core.Interfaces.Repository;
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
    public class Tests
    {
        private Mock<IRepository> repositoryMock;
        private Mock<ILogger<ClientService>> loggerMock;
        private Mock<IClientRepository> clientRepositoryMock;

        private ClientService clientService;

        [SetUp]
        public void Setup()
        {
            repositoryMock = new Mock<IRepository>();
            loggerMock = new Mock<ILogger<ClientService>>();
            clientRepositoryMock = new Mock<IClientRepository>();

            repositoryMock.Setup(x => x.ClientRepository).Returns(clientRepositoryMock.Object);

            clientService = new ClientService(repositoryMock.Object, loggerMock.Object);
        }

        [Test]
        public void ShouldAddClient()
        {
            var clientRequestModel = new ClientRequestModel { Name = "name", Phone = "123" };

            var response = clientService.Add(clientRequestModel);

            clientRepositoryMock.Verify(x => x.Add(It.IsAny<Client>()), Times.Once);
            repositoryMock.Verify(x => x.Save(), Times.Once);
            Assert.False(response.HasErrors());

            Assert.AreEqual(response.Messages.Count(x => x.Code == Constants.CLIENT_SAVED), 1);
        }

        [Test]
        public void ShouldValidateModel()
        {
            var clientRequestModel = new ClientRequestModel { Name = "", Phone = "" };

            var response = clientService.Add(clientRequestModel);

            clientRepositoryMock.Verify(x => x.Add(It.IsAny<Client>()), Times.Never);
            Assert.True(response.HasErrors());
            Assert.AreEqual(response.Messages.Count(x => x.Type == MessageType.Error), 2);
            Assert.AreEqual(response.Messages.Count(x => x.Code == Constants.NAME_EMPTY), 1);
            Assert.AreEqual(response.Messages.Count(x => x.Code == Constants.PHONE_EMPTY), 1);
        }

        [Test]
        public void ShouldNotSave()
        {
            repositoryMock.Setup(x => x.Save()).Throws(new Exception());

            var clientRequestModel = new ClientRequestModel { Name = "name", Phone = "123" };

            var response = clientService.Add(clientRequestModel);

            clientRepositoryMock.Verify(x => x.Add(It.IsAny<Client>()), Times.Once);
            repositoryMock.Verify(x => x.Save(), Times.Once);

            Assert.True(response.HasErrors());
            Assert.AreEqual(response.Messages.Count(x => x.Type == MessageType.Error), 1);
            Assert.AreEqual(response.Messages.Count(x => x.Code == Constants.GENERAL_ERROR), 1);
        }

        [Test]
        public void ShouldDeleteClient()
        {
            clientRepositoryMock.Setup(x => x.Find(It.IsAny<int>())).Returns(new Client { Id = 1 });

            var response = clientService.Delete(1);

            clientRepositoryMock.Verify(x => x.Delete(It.IsAny<Client>()), Times.Once);
            repositoryMock.Verify(x => x.Save(), Times.Once);
            Assert.False(response.HasErrors());
            Assert.AreEqual(response.Messages.Count(x => x.Code == Constants.CLIENT_DELETED), 1);
        }

        [Test]
        public void ShouldNotDeleteClientNotFound()
        {
            clientRepositoryMock.Setup(x => x.Find(It.IsAny<int>())).Returns((Client)null);

            var response = clientService.Delete(1);

            clientRepositoryMock.Verify(x => x.Delete(It.IsAny<Client>()), Times.Never);
            repositoryMock.Verify(x => x.Save(), Times.Never);
            Assert.True(response.HasErrors());
            Assert.AreEqual(response.Messages.Count(x => x.Type == MessageType.Error), 1);
            Assert.AreEqual(response.Messages.Count(x => x.Code == Constants.CLIENT_NOT_FOUND), 1);
        }

        [Test]
        public void ShouldNotDelete()
        {
            clientRepositoryMock.Setup(x => x.Find(It.IsAny<int>())).Returns(new Client { Id = 1 });
            repositoryMock.Setup(x => x.Save()).Throws(new Exception());

            var response = clientService.Delete(1);

            clientRepositoryMock.Verify(x => x.Delete(It.IsAny<Client>()), Times.Once);
            repositoryMock.Verify(x => x.Save(), Times.Once);

            Assert.True(response.HasErrors());
            Assert.AreEqual(response.Messages.Count(x => x.Type == MessageType.Error), 1);
            Assert.AreEqual(response.Messages.Count(x => x.Code == Constants.GENERAL_ERROR), 1);
        }
    }
}