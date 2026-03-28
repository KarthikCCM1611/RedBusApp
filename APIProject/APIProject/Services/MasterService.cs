using APIProject.Models;
using WebAPI.Models;

namespace APIProject.Services
{
    public interface IMaster
    {
        ApiResponse<List<TrendingRoute>> TrendingRoutes();
        ApiResponse<IEnumerable<BusModel>> SearchBuses(string fromLocation, string toLocation);
        ApiResponse<List<Location>> GetAllLocations();
        ApiResponse<IEnumerable<BusModel>> GetAllBuses();
        ApiResponse<BusModel> GetBusDetailsById(string id);

    }

    public class MasterService : IMaster
    {
        private readonly object _lock = new();
        private readonly IDataService _dataService;
        public MasterService(IDataService dataService)
        {
            _dataService = dataService;
        }
        public ApiResponse<List<TrendingRoute>> TrendingRoutes()
        {
            ApiResponse<List<TrendingRoute>> response = new ApiResponse<List<TrendingRoute>>();
            try
            {
                List<Booking> bookings = _dataService.GetBookings();
                List<Location> locations = _dataService.GetLocations();
                bool undirected = false;
                int topRoutes = 4;
                // var trendingRoutes = bookings.Select(x => Normalize(x.FromLocation, x.ToLocation, undirected))
                //                         .GroupBy(r => r)
                //                         .Select(g => (Route: g.Key, Count: g.Count()))
                //                         .OrderByDescending(x => x.Count)
                //                         .Take(topRoutes);

                IEnumerable<(DirectedRouteKey Route, int Count)> trendingRoutesData = bookings.Select(u => NormalizeDirected(u.FromLocationId, u.ToLocationId))
                                                                            .GroupBy(k => k)
                                                                            .Select(g => (Route: g.Key, Count: g.Count()))
                                                                            .OrderByDescending(x => x.Count)
                                                                            .Take(topRoutes);
                // List<DirectedRouteKey> keys = trendingRoutesData.Select(t => t.Route).ToList();
                // IEnumerable<TrendingRoute> trendingRoute = from k in keys
                //                                            join lf in locations on k.From equals lf.Id
                //                                            join lt in locations on k.To equals lt.Id
                //                                            select new TrendingRoute
                //                                            {
                //                                                FromLocationId = k.From,
                //                                                FromLocationName = lf.Name,
                //                                                ToLocationId = k.To,
                //                                                ToLocationName = lt.Name,
                //                                            };


                // var trending = trendingRoutesData
                //     .Select(t =>
                //     {
                //         var fromKey = Canon(t.Route.From);
                //         var toKey = Canon(t.Route.To);

                //         var hasFrom = fr.TryGetValue(fromKey, out var lf);
                //         var hasTo = locById.TryGetValue(toKey, out var lt);

                //         if (!hasFrom || !hasTo)
                //             return null; // skip routes that can't be resolved

                //         return new TrendingRoute
                //         {
                //             FromLocationId = lf.Id,
                //             FromLocationName = lf.Name,
                //             ToLocationId = lt.Id,
                //             ToLocationName = lt.Name,
                //             Count = t.Count // keep the count!
                //         };
                //     })
                //     .Where(x => x != null)
                //     .Select(x => x!)
                //     .OrderByDescending(x => x.Count) // sort by popularity
                //     .ToList();



                List<TrendingRoute> trending =
                    trendingRoutesData
                        .Join(
                            locations,
                            t => Canon(t.Route.From),
                            l => Canon(l.Id),
                            (t, lf) => new { t, lf }
                        )
                        .Join(
                            locations,
                            x => Canon(x.t.Route.To),
                            l => Canon(l.Id),
                            (x, lt) => new TrendingRoute
                            {
                                FromLocationId = x.lf.Id,
                                FromLocationName = x.lf.Name,
                                ToLocationId = lt.Id,
                                ToLocationName = lt.Name,
                                Count = x.t.Count
                            }
                        )
                        .OrderByDescending(r => r.Count)
                        .ToList();

                if (trending.Count() == 0)
                {
                    response.StatusCode = 404;
                    response.StatusMessage = "No Trending Route Found";
                    response.Data = trending;
                    return response;
                }
                response.StatusCode = 200;
                response.StatusMessage = "Trending Route Fetched Successfully";
                response.Data = trending;
                return response;
            }
            catch (Exception ex)
            {
                response.StatusCode = 400;
                response.StatusMessage = ex.Message;
                return response;
            }
        }

        public ApiResponse<IEnumerable<BusModel>> SearchBuses(string fromLocationId, string toLocationId)
        {
            ApiResponse<IEnumerable<BusModel>> response = new ApiResponse<IEnumerable<BusModel>>();
            try
            {
                List<Location> locations = _dataService.GetLocations();
                List<Bus> buses = _dataService.GetBuses().FindAll(bus => bus.FromLocationId == fromLocationId &&
                                                            bus.ToLocationId == toLocationId);
                if (buses.Count == 0)
                {
                    response.StatusCode = 404;
                    string fromLoc = locations.FirstOrDefault(loc => loc.Id == fromLocationId)?.Name;
                    string toLoc = locations.FirstOrDefault(loc => loc.Id == toLocationId)?.Name;
                    response.StatusMessage = $"No buses found between {fromLoc} and {toLoc}. Try different route.";
                    return response;
                }
                IEnumerable<BusModel> existingBuses =
                        from b in buses
                        join lf in locations on b.FromLocationId equals lf.Id   // lf = from location
                        join lt in locations on b.ToLocationId equals lt.Id     // lt = to location
                        select new BusModel
                        {

                            Id = b.Id,
                            Name = b.Name,
                            ArriveTime = b.ArriveTime,
                            DepartTime = b.DepartTime,
                            TotalCapacity = b.TotalCapacity,
                            Price = b.Price,
                            FromLocationName = lf.Name,
                            FromLocationId = lf.Id,
                            ToLocationId = lt.Id,
                            ToLocationName = lt.Name
                        };
                response.StatusCode = 200;
                response.StatusMessage = $"Buses searched successfully";
                response.Data = existingBuses;
                return response;
            }
            catch (Exception ex)
            {
                response.StatusCode = 400;
                response.StatusMessage = ex.Message;
                return response;
            }
        }

        public ApiResponse<List<Location>> GetAllLocations()
        {
            ApiResponse<List<Location>> response = new ApiResponse<List<Location>>();
            try
            {
                lock (_lock)
                {
                    List<Location> existingLocations = _dataService.GetLocations();
                    if (existingLocations.Count == 0)
                    {
                        response.StatusCode = 404;
                        response.StatusMessage = "Location doesn't exist";
                        return response;
                    }
                    response.Data = existingLocations;
                    response.StatusCode = 200;
                    response.StatusMessage = "Location Fetched Successfully";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 100;
                response.StatusMessage = $"Error fetching the location. Message: {ex.Message}";
            }
            return response;
        }

        public ApiResponse<IEnumerable<BusModel>> GetAllBuses()
        {
            ApiResponse<IEnumerable<BusModel>> response = new ApiResponse<IEnumerable<BusModel>>();
            try
            {
                lock (_lock)
                {
                    List<Location> locations = _dataService.GetLocations();
                    IEnumerable<BusModel> existingBuses =
                        from b in _dataService.GetBuses()
                        join lf in locations on b.FromLocationId equals lf.Id   // lf = from location
                        join lt in locations on b.ToLocationId equals lt.Id     // lt = to location
                        select new BusModel
                        {

                            Id = b.Id,
                            Name = b.Name,
                            ArriveTime = b.ArriveTime,
                            DepartTime = b.DepartTime,
                            TotalCapacity = b.TotalCapacity,
                            Price = b.Price,
                            FromLocationName = lf.Name,
                            FromLocationId = lf.Id,
                            ToLocationId = lt.Id,
                            ToLocationName = lt.Name
                        };

                    if (existingBuses.Count() == 0)
                    {
                        response.StatusCode = 404;
                        response.StatusMessage = "Bus doesn't exist";
                        return response;
                    }
                    response.Data = existingBuses;
                    response.StatusCode = 200;
                    response.StatusMessage = "Buses Fetched Successfully";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 100;
                response.StatusMessage = $"Error fetching the buses. Message: {ex.Message}";
            }
            return response;
        }

        public ApiResponse<BusModel> GetBusDetailsById(string id)
        {
            ApiResponse<BusModel> response = new ApiResponse<BusModel>();
            try
            {
                List<Bus> buses = _dataService.GetBuses().FindAll(bus => bus.Id == id);
                List<Location> locations = _dataService.GetLocations();
                if (buses.Count == 0)
                {
                    response.StatusCode = 404;
                    response.StatusMessage = $"Bus ${id} not found";
                    return response;
                }
                List<Booking> bookings = _dataService.GetBookings().FindAll(b => b.BusId == id);
                IEnumerable<BusModel> existingBuses =
                        from b in buses
                        join lf in locations on b.FromLocationId equals lf.Id   // lf = from location
                        join lt in locations on b.ToLocationId equals lt.Id     // lt = to location
                        select new BusModel
                        {

                            Id = b.Id,
                            Name = b.Name,
                            ArriveTime = b.ArriveTime,
                            DepartTime = b.DepartTime,
                            TotalCapacity = b.TotalCapacity,
                            Price = b.Price,
                            FromLocationName = lf.Name,
                            FromLocationId = lf.Id,
                            ToLocationId = lt.Id,
                            ToLocationName = lt.Name,
                            SeatNos = bookings.SelectMany(b => b.SeatNos).ToArray()
                        };
                response.StatusCode = 200;
                response.StatusMessage = $"Buses details fetched successfully";
                response.Data = existingBuses.ToList()[0];
                return response;
            }
            catch (Exception ex)
            {
                response.StatusCode = 400;
                response.StatusMessage = ex.Message;
                return response;
            }
        }

        // public record Route(string FromId, string ToId);

        // // If your domain treats A→B and B→A as the same route, normalize:
        // static Route Normalize(string fromId, string toId, bool undirected = true)
        // {
        //     if (!undirected) return new Route(fromId, toId);
        //     return fromId <= toId ? new Route(fromId, toId) : new Route(toId, fromId);
        // }


        public record DirectedRouteKey(string From, string To);

        static string Canon(string s) =>
            (s ?? "").Trim().ToUpperInvariant();

        static DirectedRouteKey NormalizeDirected(string from, string to) =>
            new DirectedRouteKey(Canon(from), Canon(to));



        // public record RouteKey(string A, string B);

        // static RouteKey NormalizeUndirected(string from, string to)
        // {
        //     var f = Canon(from);
        //     var t = Canon(to);
        //     return string.Compare(f, t, StringComparison.Ordinal) <= 0
        //         ? new RouteKey(f, t)
        //         : new RouteKey(t, f);
        // }

    }
}
