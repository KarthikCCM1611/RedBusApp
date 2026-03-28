using APIProject.Models;
using APIProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace APIProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUser _userSrc;
        public UserController(IUser userSrc) { 
            _userSrc = userSrc;
        }

        [HttpGet("GetAllBookingsByUserId")]
        public ApiResponse<IEnumerable<BookingModel>> GetAllBookingsByUserId(string userId)
        {
            return _userSrc.GetAllBookingsByUserId(userId);
        }

        [HttpPost("CreateBooking")]
        public ApiResponse<Booking> CreateBooking(Booking bookingObj)
        {
            return _userSrc.CreateBooking(bookingObj);
        }

        [HttpDelete("CancelBooking")]
        public ApiResponse<Booking> CancelBooking(string bookingId)
        {
            return _userSrc.CancelBooking(bookingId);
        }
    }
}
