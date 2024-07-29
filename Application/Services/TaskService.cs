using Application.InterfaceServices;
using Core.Models;
using Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
	public class TaskService : ITaskService
	{
		private readonly ITaskRepository _taskRepository;
		private readonly IUserRepository _userRepository;

		public TaskService(ITaskRepository taskRepository , IUserRepository userRepository)
		{
			_taskRepository = taskRepository;
			_userRepository = userRepository;
		}

		public async Task<IEnumerable<Tasks>> GetTasksByUserIdAsync(int userId)
		{
			return await _taskRepository.GetUserTasksAsync(userId);
		}

		public async Task<Tasks> CreateTaskAsync(Tasks task)
		{
			return await _taskRepository.AddAsync(task);
		}

		public async Task UpdateTaskAsync(Tasks task)
		{
			await _taskRepository.UpdateAsync(task);
		}

		public async Task DeleteTaskAsync(int taskId)
		{
			await _taskRepository.DeleteAsync(taskId);
		}
	}
}

