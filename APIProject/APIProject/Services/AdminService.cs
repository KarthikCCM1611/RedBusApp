using APIProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Text.Json;
using WebAPI.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace APIProject.Services
{
    public interface IAdmin
    {

        ApiResponse<Location> AddLocation(Location locationObj);
        ApiResponse<Location> UpdateLocation(Location locationObj);
        ApiResponse<Location> DeleteLocation(string id);
        ApiResponse<Bus> AddBus(Bus busObj);
        ApiResponse<Bus> UpdateBus(Bus busObj);
        ApiResponse<Bus> DeleteBus(string id);

    }

    public class AdminService : IAdmin
    {
        private readonly object _lock = new();

        private readonly IDataService _dataService;
        public AdminService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public ApiResponse<Location> AddLocation(Location locationObj)
        {
            ApiResponse<Location> response = new ApiResponse<Location>();
            try
            {
                lock (_lock)
                {
                    List<Location> locations = _dataService.GetLocations();
                    Location? existingLocation = locations.FirstOrDefault(location => location.Name == locationObj.Name);
                    if (existingLocation != null)
                    {
                        response.StatusCode = 409;
                        response.StatusMessage = "Location already exist";
                        return response;
                    }
                    Location location = new Location();
                    location.Name = locationObj.Name;
                    _dataService.AddLocation(location);
                    response.Data = location;
                    response.StatusCode = 200;
                    response.StatusMessage = "Location Added Successfully";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 100;
                response.StatusMessage = $"Error adding the location. Message: {ex.Message}";
            }
            return response;
        }

        public ApiResponse<Location> UpdateLocation(Location locationObj)
        {
            ApiResponse<Location> response = new ApiResponse<Location>();
            try
            {
                lock (_lock)
                {
                    List<Location> locations = _dataService.GetLocations();
                    int index = locations.FindIndex(location => location.Id == locationObj.Id);
                    if (index == -1)
                    {
                        response.StatusCode = 404;
                        response.StatusMessage = "Location doesn't exist";
                        return response;
                    }
                    _dataService.AddLocation(locationObj);
                    response.Data = locationObj;
                    response.StatusCode = 200;
                    response.StatusMessage = "Location Updated Successfully";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 100;
                response.StatusMessage = $"Error updating the location. Message: {ex.Message}";
            }
            return response;
        }

        public ApiResponse<Location> DeleteLocation(string id)
        {
            ApiResponse<Location> response = new ApiResponse<Location>();
            try
            {
                lock (_lock)
                {
                    List<Location> locations = _dataService.GetLocations();
                    Location? location = locations.FirstOrDefault(x => x.Id == id);
                    if (location == null)
                    {
                        response.StatusCode = 404;
                        response.StatusMessage = "Location doesn't exist";
                        return response;
                    }
                    _dataService.DeleteLocation(location);
                    response.StatusCode = 200;
                    response.StatusMessage = "Location Deleted Successfully";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 100;
                response.StatusMessage = $"Error deleting the location. Message: {ex.Message}";
            }
            return response;
        }

        public ApiResponse<Bus> AddBus(Bus busObj)
        {
            ApiResponse<Bus> response = new ApiResponse<Bus>();
            try
            {
                lock (_lock)
                {
                    List<Bus> buses = _dataService.GetBuses();
                    Bus? existingBus = buses.FirstOrDefault(bus => bus.Name == busObj.Name);
                    if (existingBus != null)
                    {
                        response.StatusCode = 409;
                        response.StatusMessage = "Bus Name already exist";
                        return response;
                    }
                    Bus bus = new Bus();
                    bus.Name = busObj.Name;
                    bus.FromLocationId = busObj.FromLocationId;
                    bus.ToLocationId = busObj.ToLocationId;
                    bus.DepartTime = busObj.DepartTime;
                    bus.ArriveTime = busObj.ArriveTime;
                    bus.TotalCapacity = busObj.TotalCapacity;
                    bus.Price = busObj.Price;
                    _dataService.AddBus(bus);
                    response.Data = bus;
                    response.StatusCode = 200;
                    response.StatusMessage = "Bus Added Successfully";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 100;
                response.StatusMessage = $"Error adding the bus. Message: {ex.Message}";
            }
            return response;
        }

        public ApiResponse<Bus> UpdateBus(Bus busObj)
        {
            ApiResponse<Bus> response = new ApiResponse<Bus>();
            try
            {
                lock (_lock)
                {
                    List<Bus> buses = _dataService.GetBuses();
                    int index = buses.FindIndex(location => location.Id == busObj.Id);
                    if (index == -1)
                    {
                        response.StatusCode = 404;
                        response.StatusMessage = "Bus doesn't exist";
                        return response;
                    }
                    _dataService.UpdateBus(busObj);
                    response.Data = busObj;
                    response.StatusCode = 200;
                    response.StatusMessage = "Bus Updated Successfully";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 100;
                response.StatusMessage = $"Error updating the bus. Message: {ex.Message}";
            }
            return response;
        }

        public ApiResponse<Bus> DeleteBus(string id)
        {
            ApiResponse<Bus> response = new ApiResponse<Bus>();
            try
            {
                lock (_lock)
                {
                    List<Bus> buses = _dataService.GetBuses();
                    Bus? bus = buses.FirstOrDefault(x => x.Id == id);
                    if (bus == null)
                    {
                        response.StatusCode = 404;
                        response.StatusMessage = "Bus doesn't exist";
                        return response;
                    }
                    _dataService.DeleteBus(bus);
                    response.StatusCode = 200;
                    response.StatusMessage = "Bus Deleted Successfully";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 100;
                response.StatusMessage = $"Error deleting the bus. Message: {ex.Message}";
            }
            return response;
        }
    }
}
