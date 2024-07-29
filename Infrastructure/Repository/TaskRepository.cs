using Core.Models;
using Infrastructure.Context;
using Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
	public class TaskRepository : BaseRepository<Tasks>, ITaskRepository
	{
		public TaskRepository(BaseContextProvider context) : base(context)
		{
		}

		public  async Task<List<Tasks>> GetUserTasksAsync(int userId)
		{
			return await  _context.Tasks.Where(x => x.UserId == userId).ToListAsync();
		}
	}
}
