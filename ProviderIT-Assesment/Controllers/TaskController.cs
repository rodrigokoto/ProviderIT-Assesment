using Application.Services;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProviderIT_Assesment.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class TaskController : ControllerBase
	{
		private readonly TaskService _taskService;

		public TaskController(TaskService taskService)
		{
			_taskService = taskService;
		}
		/// <summary>
		/// Get Tasks from Users
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Authorize]
		public async Task<ActionResult<IEnumerable<Tasks>>> GetUserTasks()
		{
			var userId = int.Parse(User.FindFirst("id").Value);
			var tasks = await _taskService.GetTasksByUserIdAsync(userId);
			return Ok(tasks);
		}
		/// <summary>
		/// Create a new Task
		/// </summary>
		/// <param name="task"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize]
		public async Task<ActionResult<Tasks>> CreateTask(Tasks task)
		{
			var userId = int.Parse(User.FindFirst("id").Value);
			task.UserId = userId;
			var createdTask = await _taskService.CreateTaskAsync(task);
			return CreatedAtAction(nameof(GetUserTasks), new { id = createdTask.Id }, createdTask);
		}
		/// <summary>
		/// Change Tasks
		/// </summary>
		/// <param name="id"></param>
		/// <param name="task"></param>
		/// <returns></returns>
		[HttpPut("{id}")]
		[Authorize]
		public async Task<IActionResult> UpdateTask(int id, Tasks task)
		{
			if (id != task.Id)
			{
				return BadRequest();
			}

			var userId = int.Parse(User.FindFirst("id").Value);
			if (task.UserId != userId && !User.IsInRole("Admin"))
			{
				return Forbid();
			}

			await _taskService.UpdateTaskAsync(task);
			return NoContent();
		}
		/// <summary>
		/// Delete Task With Role
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpDelete("{id}")]
		[Authorize(Roles = "Admin,User")]
		public async Task<IActionResult> DeleteTask(int id)
		{
			var task = await _taskService.GetTasksByUserIdAsync(id);
			var userId = int.Parse(User.FindFirst("id").Value);
			if (task == null || (task.FirstOrDefault().UserId != userId && !User.IsInRole("Admin")))
			{
				return NotFound();
			}

			await _taskService.DeleteTaskAsync(id);
			return NoContent();
		}
	}
}
