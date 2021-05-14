using Core.Domain;
using Core.Interfaces.Repository;
using Core.Interfaces.Services;
using Core.Models;
using Core.Models.Request;
using Core.Models.Responses;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class VehicleService : IVehicleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VehicleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Response Delete(int id)
        {
            var response = new Response();

            var entity = _unitOfWork.VehicleRepository.Find(id);

            if(entity == null)
            {
                response.AddError("Vehicle not found");
                return response;
            }

            try
            {
                _unitOfWork.VehicleRepository.Delete(entity);
                _unitOfWork.Save();

                response.AddSuccess("Vehicle removed succesfully");
            }
            catch (Exception e)
            {
                response.AddError("An error has ocurred");
            }

            return response;
        }

        public Response Add(VehicleRequestModel request)
        {
            var response = new Response();

            if (request == null)
            {
                response.AddError("Wrong request");
                return response;
            }

            if (string.IsNullOrWhiteSpace(request.Brand)) response.AddError("The field brand is required");
            if(!request.PricePerDay.HasValue || request.PricePerDay <= 0) response.AddError("The field pricePerDay is required");

            if (response.HasErrors()) return response;

            try
            {
                var domain = new Vehicle { Brand = request.Brand, Year = request.Year, PricePerDay = request.PricePerDay.Value, Active = true };

                _unitOfWork.VehicleRepository.Add(domain);
                _unitOfWork.Save();

                response.AddSuccess("Vehicle added succesfully");
            }
            catch (Exception e)
            {
                response.AddError("An error has ocurred");
            }

            return response;
        }

        public Response<IList<VehicleAvailableResponseModel>> GetAvailables(SearchAvailableVehiclesRequestModel request)
        {
            var response = new Response<IList<VehicleAvailableResponseModel>>();

            DateUtils.ValidateRangeDates(response, request.StartDate, request.EndDate);

            if (response.HasErrors()) return response;

            var vehicleAvailables = _unitOfWork.RentalRepository.GetVehiclesAvailables(request.StartDate.Value, request.EndDate.Value);

            response.Data = vehicleAvailables.Select(x => new VehicleAvailableResponseModel { Id = x.Id, Brand = x.Brand, PricePerDay = x.PricePerDay, Year = x.Year }).ToList();

            if (!response.Data.Any()) response.AddWarning("No vehicles availables for selected dates");

            return response;
        }
    }
}
