using Core.Models;
using Infrastructure.Repository;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
	public interface ITaskRepository : IBaseRepository<Tasks>
	{
		Task<List<Tasks>> GetUserTasksAsync(int userId);
	}
}
