using AutoMapper;
using BusinessLayer.BusinessLayer.Services;
using DataLayer.DataLayer.Entities;
using DataLayer.DataLayer.Interfaces;
using Moq;
using WebApi.BusinessLayer.Models;

namespace BusinessTest.Services;

public class TaskAdditionalServiceValidationTests
{
    [Fact]
    public async System.Threading.Tasks.Task AddAsyncNewCommentWithZeroIdShouldPersist()
    {
        var commentsRepo = new Mock<ITaskCommentsRepository>(MockBehavior.Strict);
        commentsRepo.Setup(r => r.AddAsync(It.IsAny<TaskComments>())).Returns(System.Threading.Tasks.Task.CompletedTask);

        var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
        unitOfWork.SetupGet(u => u.tasksCommentsRepository).Returns(commentsRepo.Object);
        unitOfWork.Setup(u => u.SaveAsync()).Returns(System.Threading.Tasks.Task.CompletedTask);

        var mapper = new Mock<IMapper>(MockBehavior.Strict);
        mapper.Setup(m => m.Map<TaskComments>(It.IsAny<TaskCommentsModel>())).Returns(new TaskComments());

        var service = new TaskAdditionalService(unitOfWork.Object, mapper.Object);

        var model = new TaskCommentsModel(1, taskId: 1, userId: 1, commentText: "Hi", createdDate: DateTime.UtcNow);
        var result = await service.AddAsync(model);

        Assert.True(result);
        commentsRepo.Verify(r => r.AddAsync(It.IsAny<TaskComments>()), Times.Once);
        unitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async System.Threading.Tasks.Task AddTaskStatusAsyncNewStatusWithZeroIdShouldPersist()
    {
        var statusesRepo = new Mock<ITaskStatusesRepository>(MockBehavior.Strict);
        statusesRepo.Setup(r => r.AddAsync(It.IsAny<TaskStatuses>())).Returns(System.Threading.Tasks.Task.CompletedTask);

        var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
        unitOfWork.SetupGet(u => u.taskStatusesRepository).Returns(statusesRepo.Object);
        unitOfWork.Setup(u => u.SaveAsync()).Returns(System.Threading.Tasks.Task.CompletedTask);

        var mapper = new Mock<IMapper>(MockBehavior.Strict);
        mapper.Setup(m => m.Map<TaskStatuses>(It.IsAny<TaskStatusesModel>())).Returns(new TaskStatuses());

        var service = new TaskAdditionalService(unitOfWork.Object, mapper.Object);

        var model = new TaskStatusesModel(1, "In Review");
        var result = await service.AddTaskStatusAsync(model);

        Assert.True(result);
        statusesRepo.Verify(r => r.AddAsync(It.IsAny<TaskStatuses>()), Times.Once);
        unitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async System.Threading.Tasks.Task AddTaskStatusAsyncEmptyNameShouldBeRejected()
    {
        var statusesRepo = new Mock<ITaskStatusesRepository>(MockBehavior.Strict);
        var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
        unitOfWork.SetupGet(u => u.taskStatusesRepository).Returns(statusesRepo.Object);

        var mapper = new Mock<IMapper>(MockBehavior.Strict);
        var service = new TaskAdditionalService(unitOfWork.Object, mapper.Object);

        var model = new TaskStatusesModel(0, string.Empty);
        var result = await service.AddTaskStatusAsync(model);

        Assert.False(result);
        statusesRepo.VerifyNoOtherCalls();
        unitOfWork.Verify(u => u.SaveAsync(), Times.Never);
        mapper.VerifyNoOtherCalls();
    }

    [Fact]
    public async System.Threading.Tasks.Task AddAsyncCommentWithZeroIdShouldBeRejected()
    {
        var commentsRepo = new Mock<ITaskCommentsRepository>(MockBehavior.Strict);
        var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
        unitOfWork.SetupGet(u => u.tasksCommentsRepository).Returns(commentsRepo.Object);

        var mapper = new Mock<IMapper>(MockBehavior.Strict);
        var service = new TaskAdditionalService(unitOfWork.Object, mapper.Object);

        var model = new TaskCommentsModel(0, taskId: 1, userId: 1, commentText: "Hi", createdDate: DateTime.UtcNow);
        var result = await service.AddAsync(model);

        Assert.False(result);
        commentsRepo.VerifyNoOtherCalls();
        unitOfWork.Verify(u => u.SaveAsync(), Times.Never);
        mapper.VerifyNoOtherCalls();
    }
}


