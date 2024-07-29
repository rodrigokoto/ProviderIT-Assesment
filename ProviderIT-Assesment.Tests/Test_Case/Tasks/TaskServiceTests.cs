using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.InterfaceServices;
using Core.Enums;
using Core.Models;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace ProviderIT_Assesment.Tests
{
	public class TaskServiceTests
	{
		private readonly Mock<ITaskService> _mockTaskService;
		private readonly BaseContextProvider _context;

		public TaskServiceTests()
		{
			var options = new DbContextOptionsBuilder<BaseContextProvider>()
				.UseInMemoryDatabase(databaseName: "TestDatabase")
				.Options;

			_context = new BaseContextProvider(options);
			_mockTaskService = new Mock<ITaskService>();

			SeedDatabase();
		}

		private void SeedDatabase()
		{
			var user = new User { UserName = "TestUser", PasswordHash = "TestPassword" };
			_context.Users.Add(user);

			_context.Tasks.AddRange(
				new Core.Models.Tasks { Title = "Task1", Description = "Description1", Status = ProviderTaskStatus.Pending, UserId = user.Id },
				new Core.Models.Tasks { Title = "Task2", Description = "Description2", Status = ProviderTaskStatus.InProgress , UserId = user.Id }
			);

			_context.SaveChanges();
		}

		[Fact]
		public async Task GetTasksByUserIdAsync_ShouldReturnTasksForUser()
		{
			// Arrange
			var userId = _context.Users.First().Id;
			_mockTaskService.Setup(service => service.GetTasksByUserIdAsync(userId))
				.ReturnsAsync(_context.Tasks.Where(t => t.UserId == userId).ToList());

			// Act
			var tasks = await _mockTaskService.Object.GetTasksByUserIdAsync(userId);

			// Assert
			Assert.Equal(2, tasks.Count());
		}

		[Fact]
		public async Task CreateTaskAsync_ShouldAddTask()
		{
			// Arrange
			var newTask = new Core.Models.Tasks { Title = "New Task", Description = "New Description", Status = ProviderTaskStatus.Pending , UserId = 1 };
			_mockTaskService.Setup(service => service.CreateTaskAsync(newTask)).ReturnsAsync(newTask);

			// Act
			var createdTask = await _mockTaskService.Object.CreateTaskAsync(newTask);

			// Assert
			Assert.Equal("New Task", createdTask.Title);
		}

		[Fact]
		public async Task UpdateTaskAsync_ShouldUpdateTask()
		{
			// Arrange
			var task = _context.Tasks.First();
			task.Status = ProviderTaskStatus.Completed;
			_mockTaskService.Setup(service => service.UpdateTaskAsync(task)).Returns(Task.CompletedTask);

			// Act
			await _mockTaskService.Object.UpdateTaskAsync(task);
			var updatedTask = await _context.Tasks.FindAsync(task.Id);

			// Assert
			Assert.Equal(ProviderTaskStatus.Completed  , updatedTask.Status);
		}

		[Fact]
		public async Task DeleteTaskAsync_ShouldDeleteTask()
		{
			// Arrange
			var taskId = _context.Tasks.First().Id;
			_mockTaskService.Setup(service => service.DeleteTaskAsync(taskId)).Returns(Task.CompletedTask);

			// Act
			await _mockTaskService.Object.DeleteTaskAsync(taskId);
			var tasks = await _context.Tasks.ToListAsync();

			// Assert
			Assert.DoesNotContain(tasks, t => t.Id == taskId);
		}
	}
}
