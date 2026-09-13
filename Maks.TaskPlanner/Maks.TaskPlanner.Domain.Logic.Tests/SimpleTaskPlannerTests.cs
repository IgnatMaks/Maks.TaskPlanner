using Maks.TaskPlanner.DataAccess.Abstractions;
using Maks.TaskPlanner.Domain.Logic;
using Maks.TaskPlanner.Domain.Models;
using Maks.TaskPlanner.Domain.Models.Enums;
using Moq;

namespace Maks.TaskPlanner.Domain.Logic.Tests;

public class SimpleTaskPlannerTests
{
    [Fact]
    public void CreatePlan_ShouldSortWorkItemsCorrectly()
    {
        var repositoryMock = new Mock<IWorkItemsRepository>();

        var items = new[]
        {
            new WorkItem
            {
                Id = Guid.NewGuid(),
                Title = "Task C",
                DueDate = new DateTime(2026, 9, 20),
                Priority = Priority.Low,
                IsCompleted = false
            },
            new WorkItem
            {
                Id = Guid.NewGuid(),
                Title = "Task B",
                DueDate = new DateTime(2026, 9, 18),
                Priority = Priority.High,
                IsCompleted = false
            },
            new WorkItem
            {
                Id = Guid.NewGuid(),
                Title = "Task A",
                DueDate = new DateTime(2026, 9, 15),
                Priority = Priority.High,
                IsCompleted = false
            }
        };

        repositoryMock
            .Setup(repository => repository.GetAll())
            .Returns(items);

        var planner = new SimpleTaskPlanner(repositoryMock.Object);

        WorkItem[] result = planner.CreatePlan();

        Assert.Equal(3, result.Length);
        Assert.Equal("Task A", result[0].Title);
        Assert.Equal("Task B", result[1].Title);
        Assert.Equal("Task C", result[2].Title);
    }

    [Fact]
    public void CreatePlan_ShouldIncludeAllNotCompletedWorkItems()
    {
        var repositoryMock = new Mock<IWorkItemsRepository>();

        var items = new[]
        {
            new WorkItem
            {
                Id = Guid.NewGuid(),
                Title = "Task 1",
                DueDate = DateTime.Now.AddDays(1),
                Priority = Priority.Medium,
                IsCompleted = false
            },
            new WorkItem
            {
                Id = Guid.NewGuid(),
                Title = "Task 2",
                DueDate = DateTime.Now.AddDays(2),
                Priority = Priority.Low,
                IsCompleted = false
            }
        };

        repositoryMock
            .Setup(repository => repository.GetAll())
            .Returns(items);

        var planner = new SimpleTaskPlanner(repositoryMock.Object);

        WorkItem[] result = planner.CreatePlan();

        Assert.Equal(2, result.Length);
        Assert.Contains(result, item => item.Title == "Task 1");
        Assert.Contains(result, item => item.Title == "Task 2");
    }

    [Fact]
    public void CreatePlan_ShouldNotIncludeCompletedWorkItems()
    {
        var repositoryMock = new Mock<IWorkItemsRepository>();

        var items = new[]
        {
            new WorkItem
            {
                Id = Guid.NewGuid(),
                Title = "Not completed",
                DueDate = DateTime.Now.AddDays(1),
                Priority = Priority.High,
                IsCompleted = false
            },
            new WorkItem
            {
                Id = Guid.NewGuid(),
                Title = "Completed",
                DueDate = DateTime.Now.AddDays(2),
                Priority = Priority.Urgent,
                IsCompleted = true
            }
        };

        repositoryMock
            .Setup(repository => repository.GetAll())
            .Returns(items);

        var planner = new SimpleTaskPlanner(repositoryMock.Object);

        WorkItem[] result = planner.CreatePlan();

        Assert.Single(result);
        Assert.Equal("Not completed", result[0].Title);
    }
}