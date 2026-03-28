using APIProject.Models;
using APIProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace APIProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly IAdmin _adminSrc;
        public AdminController(IAdmin adminSrc) { 
            _adminSrc = adminSrc;
        }

        [HttpPost("AddLocation")]
        public ApiResponse<Location> AddLocation(Location locationObj)
        {
            return _adminSrc.AddLocation(locationObj);
        }

        [HttpPut("UpdateLocation")]
        public ApiResponse<Location> UpdateLocation(Location locationObj)
        {
            return _adminSrc.UpdateLocation(locationObj);
        }

        [HttpDelete("DeleteLocation")]
        public ApiResponse<Location> DeleteLocation(string id)
        {
            return _adminSrc.DeleteLocation(id);
        }

        [HttpPost("AddBus")]
        public ApiResponse<Bus> AddBus(Bus busObj)
        {
            return _adminSrc.AddBus(busObj);
        }

        [HttpPut("UpdateBus")]
        public ApiResponse<Bus> UpdateBus(Bus busObj)
        {
            return _adminSrc.UpdateBus(busObj);
        }

        [HttpDelete("DeleteBus")]
        public ApiResponse<Bus> DeleteBus(string id)
        {
            return _adminSrc.DeleteBus(id);
        }        
    }
}
