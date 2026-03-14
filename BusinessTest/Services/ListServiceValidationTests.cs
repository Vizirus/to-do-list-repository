using AutoMapper;
using BusinessLayer.BusinessLayer.Services;
using DataLayer.DataLayer.Entities;
using DataLayer.DataLayer.Interfaces;
using Moq;
using WebApi.BusinessLayer.Models;

namespace BusinessTest.Services;

public class ListServiceValidationTests
{
    [Fact]
    public async System.Threading.Tasks.Task AddAsyncValidModelWithZeroIdShouldPersist()
    {
        var listsRepo = new Mock<IListsRepository>(MockBehavior.Strict);
        listsRepo.Setup(r => r.AddAsync(It.IsAny<Lists>())).Returns(System.Threading.Tasks.Task.CompletedTask);

        var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
        unitOfWork.SetupGet(u => u.listsRepository).Returns(listsRepo.Object);
        unitOfWork.Setup(u => u.SaveAsync()).Returns(System.Threading.Tasks.Task.CompletedTask);

        var mapper = new Mock<IMapper>(MockBehavior.Strict);
        mapper.Setup(m => m.Map<Lists>(It.IsAny<ListsModel>())).Returns(new Lists());

        var service = new ListService(unitOfWork.Object, mapper.Object);

        var model = new ListsModel(1, "Work", 1, DateTime.UtcNow);
        var result = await service.AddAsync(model);

        Assert.True(result);
        listsRepo.Verify(r => r.AddAsync(It.IsAny<Lists>()), Times.Once);
        unitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async System.Threading.Tasks.Task AddAsyncEmptyListNameShouldNotCallRepository()
    {
        var listsRepo = new Mock<IListsRepository>(MockBehavior.Strict);
        var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
        unitOfWork.SetupGet(u => u.listsRepository).Returns(listsRepo.Object);

        var mapper = new Mock<IMapper>(MockBehavior.Strict);
        var service = new ListService(unitOfWork.Object, mapper.Object);

        var model = new ListsModel(0, string.Empty, 1, DateTime.UtcNow);
        var result = await service.AddAsync(model);

        Assert.False(result);
        listsRepo.VerifyNoOtherCalls();
        unitOfWork.Verify(u => u.SaveAsync(), Times.Never);
        mapper.VerifyNoOtherCalls();
    }

    [Fact]
    public async System.Threading.Tasks.Task AddAsyncListWithZeroIdShouldBeRejected()
    {
        var listsRepo = new Mock<IListsRepository>(MockBehavior.Strict);
        var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
        unitOfWork.SetupGet(u => u.listsRepository).Returns(listsRepo.Object);

        var mapper = new Mock<IMapper>(MockBehavior.Strict);
        var service = new ListService(unitOfWork.Object, mapper.Object);

        var model = new ListsModel(0, "Work", 1, DateTime.UtcNow);
        var result = await service.AddAsync(model);

        Assert.False(result);
        listsRepo.VerifyNoOtherCalls();
        unitOfWork.Verify(u => u.SaveAsync(), Times.Never);
        mapper.VerifyNoOtherCalls();
    }
}



