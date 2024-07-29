using Core.Models;

namespace Application.InterfaceServices
{
	public interface ITaskService
	{
		Task<IEnumerable<Tasks>> GetTasksByUserIdAsync(int userId);
		Task<Tasks> CreateTaskAsync(Tasks task);
		Task UpdateTaskAsync(Tasks task);
		Task DeleteTaskAsync(int taskId);
	}
}
