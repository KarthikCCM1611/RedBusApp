using APIProject.Models;
using System.Text.Json;
using WebAPI.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace APIProject.Services
{
    public interface IDataService
    {

        List<User> GetUsers();
        List<Bus> GetBuses();
        List<Location> GetLocations();
        List<Booking> GetBookings();
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
        void AddBus(Bus bus);
        void UpdateBus(Bus bus);
        void DeleteBus(Bus bus);
        void AddLocation(Location location);
        void UpdateLocation(Location location);
        void DeleteLocation(Location location);
        void AddBooking(Booking booking);
        void DeleteBooking(Booking booking);
        // void SaveUsers(List<User> users);
        // void SaveBuses(List<Bus> buses);
        // void SaveLocations(List<Location> locations);
        // void SaveBookings(List<Booking> bookings);
        List<RefreshToken> GetTokens();
        void AddToken(RefreshToken token);
        void SaveToken();
    }

    public class DataService : IDataService
    {
        private readonly string _userFilePath;
        private readonly string _busFilePath;
        private readonly string _locationFilePath;
        private readonly string _bookingFilePath;
        private readonly string _tokenFilePath;

        private List<User> _users { get; set; }
        private List<Bus> _buses { get; set; }
        private List<Location> _locations { get; set; }
        private List<Booking> _bookings { get; set; }
        private List<RefreshToken> _tokens { get; set; }

        private readonly object _lock = new();
        private readonly IWebHostEnvironment _hostEnvironment;
        public DataService(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
            var dir = Path.Combine(_hostEnvironment.ContentRootPath, "data/user-data");
            Directory.CreateDirectory(dir);
            _userFilePath = Path.Combine(dir, "users.json");
            if (!File.Exists(_userFilePath)) File.Create(_userFilePath);
            _users = LoadExistingUsers();

            var busDir = Path.Combine(_hostEnvironment.ContentRootPath, "data/bus-data");
            Directory.CreateDirectory(busDir);
            _busFilePath = Path.Combine(busDir, "bus.json");
            if (!File.Exists(_busFilePath)) File.Create(_busFilePath);
            _buses = LoadExistingBuses();

            var locationDir = Path.Combine(_hostEnvironment.ContentRootPath, "data/location-data");
            Directory.CreateDirectory(locationDir);
            _locationFilePath = Path.Combine(locationDir, "locations.json");
            if (!File.Exists(_locationFilePath)) File.Create(_locationFilePath);
            _locations = LoadExistingLocations();

            var bookingDir = Path.Combine(_hostEnvironment.ContentRootPath, "data/booking-data");
            Directory.CreateDirectory(bookingDir);
            _bookingFilePath = Path.Combine(bookingDir, "bookings.json");
            if (!File.Exists(_locationFilePath)) File.Create(_bookingFilePath);
            _bookings = LoadExistingBookings();

            var tokenDir = Path.Combine(_hostEnvironment.ContentRootPath, "data/token-data");
            Directory.CreateDirectory(tokenDir);
            _tokenFilePath = Path.Combine(tokenDir, "token.json");
            if (!File.Exists(_locationFilePath)) File.Create(_tokenFilePath);
            _tokens = LoadExistingTokens();

        }

        private List<User> LoadExistingUsers()
        {
            try
            {
                var json = File.ReadAllText(_userFilePath);
                return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
            }
            catch
            {
                return new List<User>();
            }
        }

        private List<Bus> LoadExistingBuses()
        {
            try
            {
                var json = File.ReadAllText(_busFilePath);
                return JsonSerializer.Deserialize<List<Bus>>(json) ?? new List<Bus>();
            }
            catch
            {
                return new List<Bus>();
            }
        }

        private List<Location> LoadExistingLocations()
        {
            try
            {
                var json = File.ReadAllText(_locationFilePath);
                return JsonSerializer.Deserialize<List<Location>>(json) ?? new List<Location>();
            }
            catch
            {
                return new List<Location>();
            }
        }

        private List<Booking> LoadExistingBookings()
        {
            try
            {
                var json = File.ReadAllText(_bookingFilePath);
                return JsonSerializer.Deserialize<List<Booking>>(json) ?? new List<Booking>();
            }
            catch
            {
                return new List<Booking>();
            }
        }
        private List<RefreshToken> LoadExistingTokens()
        {
            try
            {
                var json = File.ReadAllText(_tokenFilePath);
                return JsonSerializer.Deserialize<List<RefreshToken>>(json) ?? new List<RefreshToken>();
            }
            catch
            {
                return new List<RefreshToken>();
            }
        }

        private void SaveUser()
        {
            var json = JsonSerializer.Serialize(_users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_userFilePath, json);
        }

        private void SaveBus()
        {
            var json = JsonSerializer.Serialize(_buses, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_busFilePath, json);
        }

        private void SaveBooking()
        {
            var json = JsonSerializer.Serialize(_bookings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_bookingFilePath, json);
        }

        private void SaveLocation()
        {
            var json = JsonSerializer.Serialize(_locations, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_locationFilePath, json);
        }



        // public void SaveUsers(List<User> users)
        // {
        //     _users = users;
        //     var json = JsonSerializer.Serialize(_users, new JsonSerializerOptions { WriteIndented = true });
        //     File.WriteAllText(_userFilePath, json);
        // }

        // public void SaveBuses(List<Bus> buses)
        // {
        //     _buses = buses;
        //     var json = JsonSerializer.Serialize(_buses, new JsonSerializerOptions { WriteIndented = true });
        //     File.WriteAllText(_busFilePath, json);
        // }

        // public void SaveBookings(List<Booking> bookings)
        // {
        //     _bookings = bookings;
        //     var json = JsonSerializer.Serialize(_bookings, new JsonSerializerOptions { WriteIndented = true });
        //     File.WriteAllText(_bookingFilePath, json);
        // }

        // public void SaveLocations(List<Location> locations)
        // {
        //     _locations = locations;
        //     var json = JsonSerializer.Serialize(_locations, new JsonSerializerOptions { WriteIndented = true });
        //     File.WriteAllText(_locationFilePath, json);
        // }

        public List<User> GetUsers()
        {
            return _users;
        }

        public List<Bus> GetBuses()
        {
            return _buses;
        }

        public List<Location> GetLocations()
        {
            return _locations;
        }

        public List<Booking> GetBookings()
        {
            return _bookings;
        }

        public void AddToken(RefreshToken token)
        {
            _tokens.Add(token);
        }

        public void SaveToken()
        {
            var json = JsonSerializer.Serialize(_tokens, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_tokenFilePath, json);
        }

        public List<RefreshToken> GetTokens()
        {
            return _tokens;
        }

        public void AddUser(User user)
        {
            _users.Add(user);
            SaveUser();
        }

        public void UpdateUser(User user)
        {
            int index = _users.FindIndex(usr => usr.Id == user.Id);
            _users[index] = new User
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                PhoneNo = user.PhoneNo,
                City = user.City,
                Role = Role.User
            };
            SaveUser();
        }

        public void DeleteUser(User user)
        {
            _users.Remove(user);
            SaveUser();
        }

        public void AddBus(Bus bus)
        {
            _buses.Add(bus);
            SaveBus();
        }

        public void UpdateBus(Bus busObj)
        {
            int index = _buses.FindIndex(location => location.Id == busObj.Id);
            _buses[index] = new Bus
            {
                Id = busObj.Id,
                Name = busObj.Name,
                FromLocationId = busObj.FromLocationId,
                ToLocationId = busObj.ToLocationId,
                DepartTime = busObj.DepartTime,
                ArriveTime = busObj.ArriveTime,
                TotalCapacity = busObj.TotalCapacity,
                Price = busObj.Price,
            };
            SaveBus();
        }

        public void DeleteBus(Bus bus)
        {
            _buses.Remove(bus);
            SaveBus();
        }

        public void AddLocation(Location location)
        {
            _locations.Add(location);
            SaveLocation();
        }

        public void UpdateLocation(Location location)
        {
            int index = _locations.FindIndex(location => location.Id == location.Id);
            _locations[index] = new Location
            {
                Id = location.Id,
                Name = location.Name,
            };
            SaveLocation();
        }

        public void DeleteLocation(Location location)
        {
            _locations.Remove(location);
            SaveLocation();
        }

        public void AddBooking(Booking booking)
        {
            _bookings.Add(booking);
            SaveBooking();
        }

        public void DeleteBooking(Booking booking)
        {
            _bookings.Remove(booking);
            SaveBooking();
        }
    }
}
