using Core.Configuration;
using Core.Domain;
using Core.Interfaces.Repository;
using Core.Interfaces.Services;
using Core.Models;
using Core.Models.Request;
using Core.Models.Responses;
using Core.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    /// <summary>
    /// Service to handle the business logic for create, delete a vehicle and get all vehicle availables by range dates
    /// </summary>
    public class VehicleService : IVehicleService
    {
        private readonly IRepository unitOfWork;
        private readonly ILogger<VehicleService> logger;

        public VehicleService(IRepository unitOfWork, ILogger<VehicleService> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        /// <summary>
        /// Logical delete of a vehicle by id
        /// </summary>
        /// <param name="id"></param>
        public Response Delete(int id)
        {
            var response = new Response();

            logger.LogInformation($"Calling vehicle repository to find vehicle with id {id}");

            var entity = unitOfWork.VehicleRepository.Find(id);

            if(entity == null)
            {
                response.AddError(Constants.VEHICLE_NOT_FOUND, "Vehicle not found");
                return response;
            }

            try
            {
                unitOfWork.VehicleRepository.Delete(entity);
                unitOfWork.Save();

                response.AddSuccess(Constants.VEHICLE_DELETED, "Vehicle removed succesfully");
            }
            catch (Exception e)
            {
                ExceptionUtils.HandleGeneralError(response, logger, e);
            }

            return response;
        }

        /// <summary>
        /// Create new vehicle
        /// </summary>
        /// <param name="request"></param>
        public Response Add(VehicleRequestModel request)
        {
            var response = new Response();

            logger.LogInformation("Starting request validation");

            if (string.IsNullOrWhiteSpace(request.Brand)) response.AddError(Constants.BRAND_EMPTY, "The field brand is required");
            if(!request.PricePerDay.HasValue || request.PricePerDay <= 0) response.AddError(Constants.PRICE_PER_DAY_INVALID, "The field pricePerDay is required");

            if (response.HasErrors()) return response;

            logger.LogInformation("Request validated success");

            try
            {
                var domain = new Vehicle { Brand = request.Brand, Year = request.Year, PricePerDay = request.PricePerDay.Value, Active = true };

                logger.LogInformation("Calling vehicle repository to save new vehicle");

                unitOfWork.VehicleRepository.Add(domain);
                unitOfWork.Save();

                response.AddSuccess(Constants.VEHICLE_SAVED, "Vehicle added succesfully");
            }
            catch (Exception e)
            {
                ExceptionUtils.HandleGeneralError(response, logger, e);
            }

            return response;
        }

        /// <summary>
        /// Get available vehicles by range date
        /// </summary>
        /// <param name="request"></param>
        public Response<IList<VehicleAvailableResponseModel>> GetAvailables(SearchAvailableVehiclesRequestModel request)
        {
            var response = new Response<IList<VehicleAvailableResponseModel>>();

            logger.LogInformation("Starting request validation");

            DateUtils.ValidateRangeDates(response, request.StartDate, request.EndDate);

            if (response.HasErrors()) return response;

            try
            {
                logger.LogInformation("Calling rental repository to find availables vehicles by range dates");

                var vehicleAvailables = unitOfWork.RentalRepository.GetVehiclesAvailables(request.StartDate.Value, request.EndDate.Value);

                logger.LogInformation($"{vehicleAvailables.Count()} vehicles founds");

                response.Data = vehicleAvailables.Select(x => new VehicleAvailableResponseModel { Id = x.Id, Brand = x.Brand, PricePerDay = x.PricePerDay, Year = x.Year }).ToList();

                if (!response.Data.Any()) response.AddWarning(Constants.NO_VEHICLE_AVAILABLES, "No vehicles availables for selected dates");
            }
            catch (Exception e)
            {
                ExceptionUtils.HandleGeneralError(response, logger, e);
            }

            return response;
        }
    }
}
