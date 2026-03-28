using APIProject.Models;
using APIProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace APIProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuth _authSrc;
        public AuthController(IAuth authSrc)
        {
            _authSrc = authSrc;
        }

        [HttpPost("Login")]
        public IActionResult Login(Login loginObj)
        {
            var user = _authSrc.ValidateUserAsync(loginObj.Email, loginObj.Password);
            if (user is null) return Unauthorized(new { message = "UnAuthorized Error" });

            // Create JWT
            var jwt = _authSrc.CreateJwt(user, out var jti, out var accessExpiresUtc);

            // Create refresh token
            var refreshPlain = TokenHelper.GenerateSecureToken(64);
            var refreshHash = TokenHelper.Sha256(refreshPlain);
            var refresh = new RefreshToken
            {
                UserId = user.Id,
                TokenHash = refreshHash,
                ExpiresAtUtc = DateTime.UtcNow.AddDays(14)
            };
            _authSrc.AddToken(refresh);
            // Send refresh token via HttpOnly cookie (recommended)
            Response.Cookies.Append("refresh_token", refreshPlain, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,             // true in production
                SameSite = SameSiteMode.Strict,
                Expires = refresh.ExpiresAtUtc
            });

            // Return access token in body
            return Ok(new
            {
                accessToken = jwt,
                expiresAt = accessExpiresUtc
            });

        }

        [HttpPost("Register")]
        public IActionResult Register(User registerObj)
        {
            string message = _authSrc.Register(registerObj);
            if (message == "Email Already Exists")
            {
                return Conflict(new { message = message });
            }
            else if (message == "User Created Successfully")
            {
                User user = _authSrc.ValidateUserAsync(registerObj.Email, registerObj.Password);
                // Create JWT
                var jwt = _authSrc.CreateJwt(user, out var jti, out var accessExpiresUtc);

                // Create refresh token
                var refreshPlain = TokenHelper.GenerateSecureToken(64);
                var refreshHash = TokenHelper.Sha256(refreshPlain);
                var refresh = new RefreshToken
                {
                    UserId = user.Id,
                    TokenHash = refreshHash,
                    ExpiresAtUtc = DateTime.UtcNow.AddDays(14)
                };
                _authSrc.AddToken(refresh);
                // Send refresh token via HttpOnly cookie (recommended)
                Response.Cookies.Append("refresh_token", refreshPlain, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,             // true in production
                    SameSite = SameSiteMode.Strict,
                    Expires = refresh.ExpiresAtUtc
                });

                // Return access token in body
                return Ok(new
                {
                    accessToken = jwt,
                    expiresAt = accessExpiresUtc
                });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                errorMsg = message
            });
        }


        [HttpPost("refresh")]
        public IActionResult Refresh()
        {

            if (!Request.Cookies.TryGetValue("refresh_token", out var refreshPlain))
                return Unauthorized(new { message = "UnAuthorized Error" });

            var refreshHash = TokenHelper.Sha256(refreshPlain);

            var existing = _authSrc.GetToken(refreshHash);

            if (existing is null || existing.IsExpired || existing.IsRevoked)
                return Unauthorized(new { message = "UnAuthorized Error" });


            // Load user
            var user = _authSrc.GetUserById(existing.UserId);
            if (user is null) return Unauthorized();

            // Rotate refresh token
            var newRefreshPlain = TokenHelper.GenerateSecureToken(64);
            var newRefreshHash = TokenHelper.Sha256(newRefreshPlain);
            existing.RevokedAtUtc = DateTime.UtcNow;
            existing.ReplacedByTokenHash = newRefreshHash;

            var newRefresh = new RefreshToken
            {
                UserId = user.Id,
                TokenHash = newRefreshHash,
                ExpiresAtUtc = DateTime.UtcNow.AddDays(14)
            };
            _authSrc.AddToken(newRefresh);

            // Issue new access token
            var jwt = _authSrc.CreateJwt(user, out var jti, out var accessExpiresUtc);

            Response.Cookies.Append("refresh_token", newRefreshPlain, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = newRefresh.ExpiresAtUtc
            });

            return Ok(new { accessToken = jwt, expiresAt = accessExpiresUtc });
        }


        [HttpPost("logout")]
        public IActionResult Logout(string id)
        {
            _authSrc.RevokeRefreshToken(id);
            Response.Cookies.Delete("refresh_token");
            return Ok(new { message = "Logged out successfully." });
        }
    }
}
