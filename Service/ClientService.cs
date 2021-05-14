using Core.Domain;
using Core.Interfaces.Repository;
using Core.Interfaces.Services;
using Core.Models;
using Core.Models.Request;
using System;

namespace Service
{
    public class ClientService : IClientService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClientService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Response Add(ClientRequestModel request)
        {
            var response = new Response();

            if(request == null)
            {
                response.AddError("Wrong request");
                return response;
            }

            if (string.IsNullOrWhiteSpace(request.Name)) response.AddError("The field name is required");
            if (string.IsNullOrWhiteSpace(request.Phone)) response.AddError("The field phone is required");

            if (response.HasErrors()) return response;

            try
            {
                var domain = new Client { Name = request.Name, Phone = request.Phone, Active = true };

                _unitOfWork.ClientRepository.Add(domain);
                _unitOfWork.Save();

                response.AddSuccess("Client added succesfully");
            }
            catch (Exception e)
            {
                response.AddError("An error has ocurred");
            }

            return response;
        }

        public Response Delete(int id)
        {
            var response = new Response();

            var entity = _unitOfWork.ClientRepository.Find(id);

            if (entity == null)
            {
                response.AddError("Client not found");
                return response;
            }

            try
            {
                _unitOfWork.ClientRepository.Delete(entity);
                _unitOfWork.Save();

                response.AddSuccess("Client removed succesfully");
            }
            catch (Exception e)
            {
                response.AddError("An error has ocurred");
            }

            return response;
        }
    }
}
