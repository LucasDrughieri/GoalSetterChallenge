using Core.Domain;
using Core.Interfaces.Repository;
using Core.Interfaces.Services;
using Core.Models;
using Core.Models.Request;
using Core.Utils;
using Microsoft.Extensions.Logging;
using System;

namespace Service
{
    /// <summary>
    /// Service to handle the business logic for create and delete a client
    /// </summary>
    public class ClientService : IClientService
    {
        private readonly IRepository unitOfWork;
        private readonly ILogger<ClientService> logger;

        public ClientService(IRepository unitOfWork, ILogger<ClientService> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        /// <summary>
        /// Create a new client
        /// </summary>
        /// <param name="request"></param>
        public Response Add(ClientRequestModel request)
        {
            var response = new Response();

            logger.LogInformation("Starting request validation");

            if (string.IsNullOrWhiteSpace(request.Name)) response.AddError(Constants.NAME_EMPTY, "The field name is required");
            if (string.IsNullOrWhiteSpace(request.Phone)) response.AddError(Constants.PHONE_EMPTY, "The field phone is required");

            if (response.HasErrors()) return response;

            logger.LogInformation("Request validated success");

            try
            {
                var domain = new Client { Name = request.Name, Phone = request.Phone, Active = true };

                logger.LogInformation("Calling client repository to save new client");

                unitOfWork.ClientRepository.Add(domain);
                unitOfWork.Save();

                response.AddSuccess(Constants.CLIENT_SAVED, "Client added succesfully");
            }
            catch (Exception e)
            {
                ExceptionUtils.HandleGeneralError(response, logger, e);
            }

            return response;
        }

        /// <summary>
        /// Logical delete of a client by id
        /// </summary>
        /// <param name="id"></param>
        public Response Delete(int id)
        {
            var response = new Response();

            logger.LogInformation($"Calling client repository to find client with id {id}");

            var entity = unitOfWork.ClientRepository.Find(id);

            if (entity == null)
            {
                response.AddError(Constants.CLIENT_NOT_FOUND, "Client not found");
                return response;
            }

            try
            {
                logger.LogInformation("Calling client repository to delete client");

                unitOfWork.ClientRepository.Delete(entity);
                unitOfWork.Save();

                response.AddSuccess(Constants.CLIENT_DELETED, "Client removed succesfully");
            }
            catch (Exception e)
            {
                ExceptionUtils.HandleGeneralError(response, logger, e);
            }

            return response;
        }
    }
}
