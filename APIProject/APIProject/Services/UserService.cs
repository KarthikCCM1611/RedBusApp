using APIProject.Models;
using WebAPI.Models;

namespace APIProject.Services
{
    public interface IUser
    {
        ApiResponse<IEnumerable<BookingModel>> GetAllBookingsByUserId(string id);
        ApiResponse<Booking> CreateBooking(Booking bookingObj);
        ApiResponse<Booking> CancelBooking(string bookingId);
    }

    public class UserService : IUser
    {
        private readonly object _lock = new();

        private readonly IDataService _dataService;
        public UserService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public ApiResponse<IEnumerable<BookingModel>> GetAllBookingsByUserId(string userId)
        {
            ApiResponse<IEnumerable<BookingModel>> response = new ApiResponse<IEnumerable<BookingModel>>();
            try
            {
                lock (_lock)
                {
                    List<Location> locations = _dataService.GetLocations();
                    List<Bus> buses = _dataService.GetBuses();
                    List<Booking> bookings = _dataService.GetBookings().FindAll(booking => booking.UserId == userId);
                    IEnumerable<BookingModel> bookingModel =
                                        from b in bookings
                                        join lf in locations on b.FromLocationId equals lf.Id   // lf = from location
                                        join lt in locations on b.ToLocationId equals lt.Id     // lt = to location
                                        join bus in buses on b.BusId equals bus.Id
                                        select new BookingModel
                                        {

                                            Id = b.Id,
                                            BusId = b.BusId,
                                            UserId = b.UserId,
                                            FromLocationId = b.FromLocationId,
                                            FromLocationName = lf.Name,
                                            ToLocationId = b.ToLocationId,
                                            ToLocationName = lt.Name,
                                            SeatNos = b.SeatNos,
                                            TotalPrice = b.TotalPrice,
                                            BusName = bus.Name
                                        };
                    if (bookings.Count == 0)
                    {
                        response.StatusCode = 404;
                        response.StatusMessage = "No Bookings Found";
                        return response;
                    }
                    response.StatusCode = 200;
                    response.StatusMessage = "Booking fetched sucesfully";
                    response.Data = bookingModel;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 100;
                response.StatusMessage = $"Error adding the location. Message: {ex.Message}";
            }
            return response;
        }

        public ApiResponse<Booking> CreateBooking(Booking bookingObj)
        {
            ApiResponse<Booking> response = new ApiResponse<Booking>();
            try
            {
                lock (_lock)
                {
                    Booking booking = new Booking();
                    booking.UserId = bookingObj.UserId;
                    booking.BusId = bookingObj.BusId;
                    booking.FromLocationId = bookingObj.FromLocationId;
                    booking.ToLocationId = bookingObj.ToLocationId;
                    booking.SeatNos = bookingObj.SeatNos;
                    booking.TotalPrice = bookingObj.TotalPrice;
                    _dataService.AddBooking(booking);
                    response.Data = booking;
                    response.StatusCode = 200;
                    response.StatusMessage = "Booking Created Successfully";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 100;
                response.StatusMessage = $"Error creating the booking. Message: {ex.Message}";
            }
            return response;
        }

        public ApiResponse<Booking> CancelBooking(string id)
        {
            ApiResponse<Booking> response = new ApiResponse<Booking>();
            try
            {
                lock (_lock)
                {
                    List<Booking> bookings = _dataService.GetBookings();
                    Booking? booking = bookings.FirstOrDefault(x => x.Id == id);
                    if (booking == null)
                    {
                        response.StatusCode = 404;
                        response.StatusMessage = "Booking doesn't exist";
                        return response;
                    }
                    _dataService.DeleteBooking(booking);
                    response.StatusCode = 200;
                    response.Data = booking;
                    response.StatusMessage = "Booking Cancelld Successfully";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 100;
                response.StatusMessage = $"Error deleting the location. Message: {ex.Message}";
            }
            return response;
        }

    }
}
