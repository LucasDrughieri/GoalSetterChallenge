using Core.Domain;
using Core.Enums;
using Core.Interfaces.Repository;
using Core.Interfaces.Services;
using Core.Models;
using Core.Models.Request;
using Core.Utils;
using Microsoft.Extensions.Logging;
using System;

namespace Service
{
    public class RentalService : IRentalService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RentalService> _logger;

        public RentalService(IUnitOfWork unitOfWork, ILogger<RentalService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public Response Add(CreateRentalRequestModel request)
        {
            var response = new Response();

            try
            {
                _logger.LogInformation("Starting request validation");

                DateUtils.ValidateRangeDates(response, request.StartDate, request.EndDate);

                var vehicle = ValidateVehicle(request, response);

                ValidateClient(request, response);

                if (response.HasErrors()) return response;

                _logger.LogInformation("Request validated success");

                _logger.LogInformation("Calling rental repository to find availables vehicles by range dates");

                var isVehicleAvailable = _unitOfWork.RentalRepository.VerifyIfVehicleIsAvailableByRangeDates(request.VehicleId.Value, request.StartDate.Value, request.EndDate.Value);

                if (!isVehicleAvailable)
                {
                    response.AddError(Constants.VEHICLE_NOT_AVAILABLE, "Vehicle is not available");
                    return response;
                }

                var totalDays = request.EndDate.Value.Subtract(request.StartDate.Value).TotalDays;

                var rental = new Rental
                {
                    ClientId = request.ClientId.Value,
                    VehicleId = request.VehicleId.Value,
                    StartDate = request.StartDate.Value,
                    EndDate = request.EndDate.Value,
                    Price = vehicle.PricePerDay * totalDays,
                    Status = RentalStatus.Reserved
                };

                _logger.LogInformation("Calling rental repository to save new rental");

                _unitOfWork.RentalRepository.Add(rental);
                _unitOfWork.Save();

                response.AddSuccess(Constants.RENTAL_SAVED, "A vehicle was reserved succesfully");
            }
            catch (Exception e)
            {
                ExceptionUtils.HandleGeneralError(response, _logger, e);
            }

            return response;
        }

        public Response Cancel(int id)
        {
            var response = new Response();

            _logger.LogInformation($"Calling rental repository to find rental with id {id}");

            var entity = _unitOfWork.RentalRepository.Find(id);

            if(entity == null)
            {
                response.AddError(Constants.RENTAL_NOT_FOUND, "Rental not found");
                return response;
            }

            try
            {
                entity.Status = RentalStatus.Cancelled;

                _logger.LogInformation("Calling rental repository to cancel rental with id {id}");

                _unitOfWork.RentalRepository.Update(entity);
                _unitOfWork.Save();

                response.AddSuccess(Constants.RENTAL_CANCELLED, "The rental car was succesfully cancelled");
            }
            catch (Exception e)
            {
                ExceptionUtils.HandleGeneralError(response, _logger, e);
            }

            return response;
        }

        private void ValidateClient(CreateRentalRequestModel request, Response response)
        {
            if (!request.ClientId.HasValue)
            {
                response.AddError(Constants.CLIENTID_EMPTY, "The field ClientId is required");
            }
            else
            {
                var client = _unitOfWork.ClientRepository.Find(request.ClientId.Value);

                if (client == null || !client.Active) response.AddError(Constants.CLIENT_NOT_FOUND, "Client not found");
            }
        }

        private Vehicle ValidateVehicle(CreateRentalRequestModel request, Response response)
        {
            if (!request.VehicleId.HasValue)
            {
                response.AddError(Constants.VEHICLEID_EMPTY, "The field VehicleId is required");
                return null;
            }
            else
            {
                var vehicle = _unitOfWork.VehicleRepository.Find(request.VehicleId.Value);

                if (vehicle == null || !vehicle.Active) response.AddError(Constants.VEHICLE_NOT_FOUND, "Vehicle not found");

                return vehicle;
            }
        }
    }
}
