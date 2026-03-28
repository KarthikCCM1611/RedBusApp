using APIProject.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Models;

namespace APIProject.Services
{
    public interface IAuth
    {
        string Register(User user);
        ApiResponse<User> Login(Login login);
        User ValidateUserAsync(string email, string passWord);
        User GetUserById(string userId);
        string CreateJwt(User user, out string jti, out DateTime expiresUtc);
        void AddToken(RefreshToken token);
        RefreshToken GetToken(string refreshHash);
        void RevokeRefreshToken(string userId);
    }

    public class AuthService : IAuth
    {
        private readonly IDataService _dataService;
        private readonly IConfiguration _config;

        public AuthService(IDataService dataService, IConfiguration config)
        {
            _dataService = dataService;
            _config = config;
        }

        public ApiResponse<User> Login(Login loginObj)
        {
            ApiResponse<User> response = new ApiResponse<User>();
            try
            {
                List<User> users = _dataService.GetUsers();
                User? existingUser = users.FirstOrDefault(usr => usr.Email == loginObj.Email);
                if (existingUser == null)
                {
                    response.StatusCode = 404;
                    response.StatusMessage = "Email doesn't exist";
                    return response;
                }
                if (loginObj.Password != existingUser.Password)
                {
                    response.StatusCode = 401;
                    response.StatusMessage = "Pasword is incorrect, Please check the entered password";
                    return response;
                }
                response.StatusCode = 200;
                response.StatusMessage = "User Login Success";
                response.Data = existingUser;
                return response;
            }
            catch (Exception ex)
            {
                response.StatusCode = 400;
                response.StatusMessage = ex.Message;
                return response;
            }
        }

        public string Register(User userObj)
        {
            try
            {
                List<User> users = _dataService.GetUsers();
                User? existingUser = users.FirstOrDefault(usr => usr.Email == userObj.Email);
                if (existingUser != null)
                {
                    return "Email Already Exists";
                }
                User user = new User();
                user.Name = userObj.Name;
                user.Email = userObj.Email;
                user.Password = userObj.Password;
                user.PhoneNo = userObj.PhoneNo;
                user.City = userObj.City;
                _dataService.AddUser(user);
                return "User Created Successfully";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public User ValidateUserAsync(string email, string passWord)
        {
            List<User> users = _dataService.GetUsers();
            return users.FirstOrDefault(usr => usr.Email == email && usr.Password == passWord);
        }


        public string CreateJwt(User user, out string jti, out DateTime expiresUtc)
        {
            var jwtSection = _config.GetSection("Jwt");
            var issuer = jwtSection["Issuer"];
            var audience = jwtSection["Audience"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            jti = Guid.NewGuid().ToString();
            expiresUtc = DateTime.UtcNow.AddMinutes(30);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, jti),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("UserId", user.Id)
            };
            var token = new JwtSecurityToken(issuer, audience, claims, expires: expiresUtc, signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public void AddToken(RefreshToken token)
        {
            _dataService.AddToken(token);
            _dataService.SaveToken();
        }

        public RefreshToken GetToken(string refreshHash)
        {
            return _dataService.GetTokens().FirstOrDefault(token => token.TokenHash == refreshHash);
        }

        public User GetUserById(string userId)
        {
            return _dataService.GetUsers().FirstOrDefault(usr => usr.Id == userId);
        }

        public void RevokeRefreshToken(string userId)
        {
            var tokens = _dataService.GetTokens().Where(t => t.UserId == userId && !t.IsRevoked).ToList();

            foreach (var t in tokens) t.RevokedAtUtc = DateTime.UtcNow;
            _dataService.SaveToken();
        }
    }
}
