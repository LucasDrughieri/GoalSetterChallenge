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
    public class VehicleService : IVehicleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<VehicleService> _logger;

        public VehicleService(IUnitOfWork unitOfWork, ILogger<VehicleService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public Response Delete(int id)
        {
            var response = new Response();

            _logger.LogInformation($"Calling vehicle repository to find vehicle with id {id}");

            var entity = _unitOfWork.VehicleRepository.Find(id);

            if(entity == null)
            {
                response.AddError(Constants.VEHICLE_NOT_FOUND, "Vehicle not found");
                return response;
            }

            try
            {
                _unitOfWork.VehicleRepository.Delete(entity);
                _unitOfWork.Save();

                response.AddSuccess(Constants.VEHICLE_DELETED, "Vehicle removed succesfully");
            }
            catch (Exception e)
            {
                ExceptionUtils.HandleGeneralError(response, _logger, e);
            }

            return response;
        }

        public Response Add(VehicleRequestModel request)
        {
            var response = new Response();

            _logger.LogInformation("Starting request validation");

            if (string.IsNullOrWhiteSpace(request.Brand)) response.AddError(Constants.BRAND_EMPTY, "The field brand is required");
            if(!request.PricePerDay.HasValue || request.PricePerDay <= 0) response.AddError(Constants.PRICE_PER_DAY_INVALID, "The field pricePerDay is required");

            if (response.HasErrors()) return response;

            _logger.LogInformation("Request validated success");

            try
            {
                var domain = new Vehicle { Brand = request.Brand, Year = request.Year, PricePerDay = request.PricePerDay.Value, Active = true };

                _logger.LogInformation("Calling vehicle repository to save new vehicle");

                _unitOfWork.VehicleRepository.Add(domain);
                _unitOfWork.Save();

                response.AddSuccess(Constants.VEHICLE_SAVED, "Vehicle added succesfully");
            }
            catch (Exception e)
            {
                ExceptionUtils.HandleGeneralError(response, _logger, e);
            }

            return response;
        }

        public Response<IList<VehicleAvailableResponseModel>> GetAvailables(SearchAvailableVehiclesRequestModel request)
        {
            var response = new Response<IList<VehicleAvailableResponseModel>>();

            _logger.LogInformation("Starting request validation");

            DateUtils.ValidateRangeDates(response, request.StartDate, request.EndDate);

            if (response.HasErrors()) return response;

            _logger.LogInformation("Calling rental repository to find availables vehicles by range dates");

            var vehicleAvailables = _unitOfWork.RentalRepository.GetVehiclesAvailables(request.StartDate.Value, request.EndDate.Value);

            _logger.LogInformation($"{vehicleAvailables.Count()} vehicles founds");

            response.Data = vehicleAvailables.Select(x => new VehicleAvailableResponseModel { Id = x.Id, Brand = x.Brand, PricePerDay = x.PricePerDay, Year = x.Year }).ToList();

            if (!response.Data.Any()) response.AddWarning(Constants.NO_VEHICLE_AVAILABLES, "No vehicles availables for selected dates");

            return response;
        }
    }
}
