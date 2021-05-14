using Core.Domain;
using Core.Enums;
using Core.Interfaces.Repository;
using Core.Interfaces.Services;
using Core.Models;
using Core.Models.Request;
using Core.Utils;
using System;
using System.Linq;

namespace Service
{
    public class RentalService : IRentalService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RentalService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Response Add(CreateRentalRequestModel request)
        {
            var response = new Response();

            DateUtils.ValidateRangeDates(response, request.StartDate, request.EndDate);

            var vehicle = ValidateVehicle(request, response);

            ValidateClient(request, response);

            if (response.HasErrors()) return response;

            var vehicleAvailables = _unitOfWork.RentalRepository.GetVehiclesAvailables(request.StartDate.Value, request.EndDate.Value);

            if (!vehicleAvailables.Any(x => x.Id == request.VehicleId))
            {
                response.AddError("Vehicle is not available");
                return response;
            }

            try
            {
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

                _unitOfWork.RentalRepository.Add(rental);
                _unitOfWork.Save();

                response.AddSuccess("A car was reserved succesfully");
            }
            catch (Exception e)
            {
                response.AddError("An error has ocurred");
            }

            return response;
        }

        public Response Cancel(int id)
        {
            var response = new Response();

            var entity = _unitOfWork.RentalRepository.Find(id);

            if(entity == null)
            {
                response.AddError("Rental not found");
                return response;
            }

            try
            {
                entity.Status = RentalStatus.Cancelled;
                _unitOfWork.RentalRepository.Update(entity);
                _unitOfWork.Save();

                response.AddSuccess("The rental car was succesfully cancelled");
            }
            catch (Exception e)
            {
                response.AddError("An error has ocurred");
            }

            return response;
        }

        private void ValidateClient(CreateRentalRequestModel request, Response response)
        {
            if (!request.ClientId.HasValue)
            {
                response.AddError("The field ClientId is required");
            }
            else
            {
                var client = _unitOfWork.ClientRepository.Find(request.ClientId.Value);

                if (client == null || !client.Active) response.AddError("Client not found");
            }
        }

        private Vehicle ValidateVehicle(CreateRentalRequestModel request, Response response)
        {
            if (!request.VehicleId.HasValue)
            {
                response.AddError("The field VehicleId is required");
                return null;
            }
            else
            {
                var vehicle = _unitOfWork.VehicleRepository.Find(request.VehicleId.Value);

                if (vehicle == null || !vehicle.Active) response.AddError("Vehicle not found");

                return vehicle;
            }
        }
    }
}
