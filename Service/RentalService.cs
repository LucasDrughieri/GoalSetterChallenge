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
    /// <summary>
    /// Service to handle the business logic for create and cancel a rental
    /// </summary>
    public class RentalService : IRentalService
    {
        private readonly IRepository repository;
        private readonly ILogger<RentalService> logger;

        public RentalService(IRepository repository, ILogger<RentalService> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        /// <summary>
        /// Create new rental
        /// </summary>
        /// <param name="request"></param>
        public Response Add(CreateRentalRequestModel request)
        {
            var response = new Response();

            try
            {
                logger.LogInformation("Starting request validation");

                DateUtils.ValidateRangeDates(response, request.StartDate, request.EndDate);

                var vehicle = ValidateVehicle(request, response);

                ValidateClient(request, response);

                if (response.HasErrors()) return response;

                logger.LogInformation("Request validated success");

                logger.LogInformation("Calling rental repository to find availables vehicles by range dates");

                var isVehicleAvailable = repository.RentalRepository.VerifyIfVehicleIsAvailableByRangeDates(request.VehicleId.Value, request.StartDate.Value, request.EndDate.Value);

                if (!isVehicleAvailable)
                {
                    response.AddError(Constants.VEHICLE_NOT_AVAILABLE, "Vehicle is not available");
                    return response;
                }

                var totalDays = request.EndDate.Value.Subtract(request.StartDate.Value).TotalDays + 1;

                var rental = new Rental
                {
                    ClientId = request.ClientId.Value,
                    VehicleId = request.VehicleId.Value,
                    StartDate = request.StartDate.Value,
                    EndDate = request.EndDate.Value,
                    Price = vehicle.PricePerDay * totalDays,
                    Status = RentalStatus.Reserved
                };

                logger.LogInformation("Calling rental repository to save new rental");

                repository.RentalRepository.Add(rental);
                repository.Save();

                response.AddSuccess(Constants.RENTAL_SAVED, "A vehicle was reserved succesfully");
            }
            catch (Exception e)
            {
                ExceptionUtils.HandleGeneralError(response, logger, e);
            }

            return response;
        }

        /// <summary>
        /// Cancel a rental by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Response Cancel(int id)
        {
            var response = new Response();

            logger.LogInformation($"Calling rental repository to find rental with id {id}");

            var entity = repository.RentalRepository.Find(id);

            if(entity == null)
            {
                response.AddError(Constants.RENTAL_NOT_FOUND, "Rental not found");
                return response;
            }

            try
            {
                entity.Status = RentalStatus.Cancelled;

                logger.LogInformation("Calling rental repository to cancel rental with id {id}");

                repository.RentalRepository.Update(entity);
                repository.Save();

                response.AddSuccess(Constants.RENTAL_CANCELLED, "The rental car was succesfully cancelled");
            }
            catch (Exception e)
            {
                ExceptionUtils.HandleGeneralError(response, logger, e);
            }

            return response;
        }

        /// <summary>
        /// Client validations
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        private void ValidateClient(CreateRentalRequestModel request, Response response)
        {
            if (!request.ClientId.HasValue)
            {
                response.AddError(Constants.CLIENTID_EMPTY, "The field ClientId is required");
            }
            else
            {
                var client = repository.ClientRepository.Find(request.ClientId.Value);

                if (client == null || !client.Active) response.AddError(Constants.CLIENT_NOT_FOUND, "Client not found");
            }
        }

        /// <summary>
        /// Vehicle validations
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        private Vehicle ValidateVehicle(CreateRentalRequestModel request, Response response)
        {
            if (!request.VehicleId.HasValue)
            {
                response.AddError(Constants.VEHICLEID_EMPTY, "The field VehicleId is required");
                return null;
            }
            else
            {
                var vehicle = repository.VehicleRepository.Find(request.VehicleId.Value);

                if (vehicle == null || !vehicle.Active) response.AddError(Constants.VEHICLE_NOT_FOUND, "Vehicle not found");

                return vehicle;
            }
        }
    }
}
