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
    public class ClientService : IClientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ClientService> _logger;

        public ClientService(IUnitOfWork unitOfWork, ILogger<ClientService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public Response Add(ClientRequestModel request)
        {
            var response = new Response();

            _logger.LogInformation("Starting request validation");

            if (string.IsNullOrWhiteSpace(request.Name)) response.AddError(Constants.NAME_EMPTY, "The field name is required");
            if (string.IsNullOrWhiteSpace(request.Phone)) response.AddError(Constants.PHONE_EMPTY, "The field phone is required");

            if (response.HasErrors()) return response;

            _logger.LogInformation("Request validated success");

            try
            {
                var domain = new Client { Name = request.Name, Phone = request.Phone, Active = true };

                _logger.LogInformation("Calling client repository to save new client");

                _unitOfWork.ClientRepository.Add(domain);
                _unitOfWork.Save();

                response.AddSuccess(Constants.CLIENT_SAVED, "Client added succesfully");
            }
            catch (Exception e)
            {
                ExceptionUtils.HandleGeneralError(response, _logger, e);
            }

            return response;
        }

        public Response Delete(int id)
        {
            var response = new Response();

            _logger.LogInformation($"Calling client repository to find client with id {id}");

            var entity = _unitOfWork.ClientRepository.Find(id);

            if (entity == null)
            {
                response.AddError(Constants.CLIENT_NOT_FOUND, "Client not found");
                return response;
            }

            try
            {
                _logger.LogInformation("Calling client repository to delete client");

                _unitOfWork.ClientRepository.Delete(entity);
                _unitOfWork.Save();

                response.AddSuccess(Constants.CLIENT_DELETED, "Client removed succesfully");
            }
            catch (Exception e)
            {
                ExceptionUtils.HandleGeneralError(response, _logger, e);
            }

            return response;
        }
    }
}
