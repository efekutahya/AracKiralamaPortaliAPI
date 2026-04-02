using AracKiralamaAPI.DTOs.Auth;
using AracKiralamaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AracKiralamaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser>      _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration            _config;

        public AuthController(UserManager<AppUser> um, RoleManager<IdentityRole> rm, IConfiguration cfg)
        { _userManager = um; _roleManager = rm; _config = cfg; }

        // POST api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (await _userManager.FindByEmailAsync(dto.Email) != null)
                return BadRequest(new { message = "Bu e-posta zaten kullanılıyor." });

            var user = new AppUser
            {
                FirstName      = dto.FirstName,
                LastName       = dto.LastName,
                Email          = dto.Email,
                UserName       = dto.Email,
                PhoneNumber    = dto.PhoneNumber,
                Address        = dto.Address,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return BadRequest(new { errors = result.Errors.Select(e => e.Description) });

            if (!await _roleManager.RoleExistsAsync("User"))
                await _roleManager.CreateAsync(new IdentityRole("User"));

            await _userManager.AddToRoleAsync(user, "User");
            return Ok(new { message = "Kayıt başarılı! Giriş yapabilirsiniz." });
        }

        // POST api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                return Unauthorized(new { message = "Geçersiz e-posta veya şifre." });

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(new TokenResponseDto
            {
                Token      = GenerateJwt(user, roles),
                Email      = user.Email!,
                FullName   = $"{user.FirstName} {user.LastName}",
                Roles      = roles,
                Expiration = DateTime.UtcNow.AddHours(2)
            });
        }

        // GET api/auth/me
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMe()
        {
            var uid  = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(uid!);
            if (user == null) return NotFound();
            var roles = await _userManager.GetRolesAsync(user);
            return Ok(new { user.Id, user.FirstName, user.LastName, user.Email, user.PhoneNumber, user.Address, user.CreatedAt, Roles = roles });
        }

        // GET api/auth/users  (Admin)
        [HttpGet("users")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetUsers()
        {
            return Ok(_userManager.Users.ToList().Select(u => new
            { u.Id, u.FirstName, u.LastName, u.Email, u.PhoneNumber, u.CreatedAt }));
        }

        // DELETE api/auth/users/{id}  (Admin)
        [HttpDelete("users/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            await _userManager.DeleteAsync(user);
            return NoContent();
        }

        // ── JWT Token Üretici ─────────────────────────────────
        private string GenerateJwt(AppUser user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Email,          user.Email!),
                new(ClaimTypes.Name,           $"{user.FirstName} {user.LastName}"),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var key   = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer:            _config["JWT:Issuer"],
                audience:          _config["JWT:Audience"],
                claims:            claims,
                expires:           DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
