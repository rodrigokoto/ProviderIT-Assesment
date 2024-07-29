using Application.InterfaceServices;
using Core.DTO;
using Core.DTO.Requests;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProviderIT_Assesment.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IUserService _userService; // Serviço para verificar usuários
		private readonly string _key;

		public AuthController(IUserService userService, IConfiguration configuration)
		{
			_userService = userService;
			_key = configuration["Jwt:Key"];
		}
		/// <summary>
		/// Metod to login into plataform
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginModel model)
		{
			// Verificar se o usuário existe e a senha está correta
			var user = await _userService.AuthenticateAsync(model.Username, model.Password);

			if (user == null)
				return Unauthorized();

			// Gerar o token
			var tokenHandler = new JwtSecurityTokenHandler();
			var claims = new List<Claim>();
			{
				new Claim(ClaimTypes.Name, user.UserName);
                // Adiciona roles ao token
            };

			foreach (var userRole in user.UserRoles)
			{
				claims.Add(new Claim(ClaimTypes.Role, userRole.Role.Name)); 
			}

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.UtcNow.AddHours(1),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_key)), SecurityAlgorithms.HmacSha256Signature),
				Issuer = "ProviderIT", 
				Audience = "ProviderIT" 
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			var tokenString = tokenHandler.WriteToken(token);

			return Ok(new { Token = tokenString });
		}

		/// <summary>
		/// Create a new user 
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost("create")]
		public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
		{
			try
			{
				var user = await _userService.CreateUserAsync(request.Username, request.Password, request.Roles);
				return Ok(user);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
