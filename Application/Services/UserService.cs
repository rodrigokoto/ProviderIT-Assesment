using System.Linq;
using System.Threading.Tasks;
using Application.InterfaceServices;
using Core.Models;
using Infrastructure.Context;
using Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;

namespace Application.Services

{
	public class UserService : IUserService
	{
		private readonly BaseContextProvider _context;
		private readonly IUserRepository _userRepository;


		public UserService(BaseContextProvider context, IUserRepository userRepository)
		{

			_context = context;
			_userRepository = userRepository;
		}

		public async Task<User> AuthenticateAsync(string username, string password)
		{
			var user = await _context.Users
				.Include(u => u.UserRoles)
				.ThenInclude(ur => ur.Role)
				.FirstOrDefaultAsync(u => u.UserName == username && u.PasswordHash == password);

			if (user == null)
				return null;

			return user;
		}

		public async Task<User> CreateUserAsync(string username, string password, IEnumerable<string> roles)
		{
			var existingUser = _context.Users
				.FirstOrDefaultAsync(u => u.UserName == username);

			// Verifica se o usuário já existe

			if (existingUser != null)
			{
				throw new Exception("Usuário já existe.");
			}

			// Cria o usuário
			var user = new User
			{
				UserName = username,
				PasswordHash = password
			};


			var createdUser = await _userRepository.AddAsync(user);

			return createdUser;

		}
	}
}
