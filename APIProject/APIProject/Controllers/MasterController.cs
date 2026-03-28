using APIProject.Models;
using APIProject.Services;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace APIProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterController : ControllerBase
    {
        private readonly IMaster _masterSrc;
        public MasterController(IMaster masterSrc)
        {
            _masterSrc = masterSrc;
        }
        [HttpGet("TrendingRoutes")]
        public ApiResponse<List<TrendingRoute>> TrendingRoutes()
        {
            return _masterSrc.TrendingRoutes();
        }
        [HttpGet("GetBusDetailsById")]
        public ApiResponse<BusModel> GetBusDetailsById(string id)
        {
            return _masterSrc.GetBusDetailsById(id);
        }

        [HttpGet("SearchBuses")]
        public ApiResponse<IEnumerable<BusModel>> SearchBuses(string fromLocationId, string toLocationId)
        {
            return _masterSrc.SearchBuses(fromLocationId, toLocationId);
        }


        [HttpGet("GetAllLocations")]
        public ApiResponse<List<Location>> GetAllLocations()
        {
            return _masterSrc.GetAllLocations();
        }

        [HttpGet("GetAllBuses")]
        public ApiResponse<IEnumerable<BusModel>> GetAllBuses()
        {
            return _masterSrc.GetAllBuses();
        }
    }
}
