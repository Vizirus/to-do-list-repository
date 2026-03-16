using AutoMapper;
using BusinessLayer.BusinessLayer.Models;
using BusinessLayer.BusinessLayer.Services;
using DataLayer.DataLayer.Interfaces;
using Moq;
using WebApi.BusinessLayer.Models;
using TaskEntity = DataLayer.DataLayer.Entities.Task;

namespace BusinessTest.Services;

public class TaskServiceValidationTests
{
    [Fact]
    public async Task AddAsyncNewTaskWithZeroIdShouldPersist()
    {
        var tasksRepo = new Mock<ITaskRepository>(MockBehavior.Strict);
        tasksRepo.Setup(r => r.AddAsync(It.IsAny<TaskEntity>())).Returns(System.Threading.Tasks.Task.CompletedTask);

        var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
        unitOfWork.SetupGet(u => u.tasksRepository).Returns(tasksRepo.Object);
        unitOfWork.Setup(u => u.SaveAsync()).Returns(System.Threading.Tasks.Task.CompletedTask);

        var mapper = new Mock<IMapper>(MockBehavior.Strict);
        mapper.Setup(m => m.Map<TaskEntity>(It.IsAny<TaskModel>())).Returns(new TaskEntity());

        var service = new TaskService(unitOfWork.Object, mapper.Object);

        var model = new TaskModel(
            1,
            listId: 1,
            taskName: "Do thing",
            taskDescription: "Desc",
            taskStartDate: DateTime.UtcNow,
            taskFinishDate: DateTime.UtcNow.AddDays(1),
            statusId: 1);

        var result = await service.AddAsync(model);

        Assert.True(result);
        tasksRepo.Verify(r => r.AddAsync(It.IsAny<TaskEntity>()), Times.Once);
        unitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task AddAsyncMissingTaskNameShouldBeRejected()
    {
        var tasksRepo = new Mock<ITaskRepository>(MockBehavior.Strict);
        var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
        unitOfWork.SetupGet(u => u.tasksRepository).Returns(tasksRepo.Object);

        var mapper = new Mock<IMapper>(MockBehavior.Strict);
        var service = new TaskService(unitOfWork.Object, mapper.Object);

        var model = new TaskModel(
            0,
            listId: 1,
            taskName: string.Empty,
            taskDescription: "Desc",
            taskStartDate: DateTime.UtcNow,
            taskFinishDate: DateTime.UtcNow.AddDays(1),
            statusId: 1);

        var result = await service.AddAsync(model);

        Assert.False(result);
        tasksRepo.VerifyNoOtherCalls();
        unitOfWork.Verify(u => u.SaveAsync(), Times.Never);
        mapper.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task UpdateAsyncStartAfterFinishShouldBeRejected()
    {
        var tasksRepo = new Mock<ITaskRepository>(MockBehavior.Strict);
        tasksRepo.Setup(r => r.Update(It.IsAny<TaskEntity>()));

        var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
        unitOfWork.SetupGet(u => u.tasksRepository).Returns(tasksRepo.Object);
        unitOfWork.Setup(u => u.SaveAsync()).Returns(System.Threading.Tasks.Task.CompletedTask);

        var mapper = new Mock<IMapper>(MockBehavior.Strict);
        mapper.Setup(m => m.Map<TaskEntity>(It.IsAny<TaskModel>())).Returns(new TaskEntity());

        var service = new TaskService(unitOfWork.Object, mapper.Object);

        var model = new TaskModel(
            id: 1,
            listId: 1,
            taskName: "Bad dates",
            taskDescription: "Desc",
            taskStartDate: new DateTime(2026, 3, 15, 0, 0, 0, DateTimeKind.Utc),
            taskFinishDate: new DateTime(2026, 3, 14, 0, 0, 0, DateTimeKind.Utc),
            statusId: 1);

        var result = await service.UpdateAsync(model);

        Assert.False(result);
        tasksRepo.Verify(r => r.Update(It.IsAny<TaskEntity>()), Times.Never);
        unitOfWork.Verify(u => u.SaveAsync(), Times.Never);
    }
    [Fact]
    public async Task AddAsyncTaskWithZeroIdShouldBeRejected()
    {
        var tasksRepo = new Mock<ITaskRepository>(MockBehavior.Strict);
        var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
        unitOfWork.SetupGet(u => u.tasksRepository).Returns(tasksRepo.Object);

        var mapper = new Mock<IMapper>(MockBehavior.Strict);
        var service = new TaskService(unitOfWork.Object, mapper.Object);

        var model = new TaskModel(
            id: 0,
            listId: 1,
            taskName: "Do thing",
            taskDescription: "Desc",
            taskStartDate: DateTime.UtcNow,
            taskFinishDate: DateTime.UtcNow.AddDays(1),
            statusId: 1);

        var result = await service.AddAsync(model);

        Assert.False(result);
        tasksRepo.VerifyNoOtherCalls();
        unitOfWork.Verify(u => u.SaveAsync(), Times.Never);
        mapper.VerifyNoOtherCalls();
    }
}
