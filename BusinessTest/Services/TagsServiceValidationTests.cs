using AutoMapper;
using BusinessLayer.BusinessLayer.Services;
using DataLayer.DataLayer.Entities;
using DataLayer.DataLayer.Interfaces;
using Moq;
using WebApi.BusinessLayer.Models;

namespace BusinessTest.Services;

public class TagsServiceValidationTests
{
    [Fact]
    public async System.Threading.Tasks.Task AddAsyncNewTagWithNameShouldPersist()
    {
        var tagsRepo = new Mock<ITagsRepository>(MockBehavior.Strict);
        tagsRepo.Setup(r => r.AddAsync(It.IsAny<Tags>())).Returns(System.Threading.Tasks.Task.CompletedTask);

        var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
        unitOfWork.SetupGet(u => u.tagsRepository).Returns(tagsRepo.Object);
        unitOfWork.Setup(u => u.SaveAsync()).Returns(System.Threading.Tasks.Task.CompletedTask);

        var mapper = new Mock<IMapper>(MockBehavior.Strict);
        mapper.Setup(m => m.Map<Tags>(It.IsAny<TagsModel>())).Returns(new Tags());

        var service = new TagsService(unitOfWork.Object, mapper.Object);

        var model = new TagsModel(1, "Urgent");
        var result = await service.AddAsync(model);

        Assert.True(result);
        tagsRepo.Verify(r => r.AddAsync(It.IsAny<Tags>()), Times.Once);
        unitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async System.Threading.Tasks.Task AddAsyncEmptyNameShouldBeRejectedEvenIfIdIsNonZero()
    {
        var tagsRepo = new Mock<ITagsRepository>(MockBehavior.Strict);
        tagsRepo.Setup(r => r.AddAsync(It.IsAny<Tags>())).Returns(System.Threading.Tasks.Task.CompletedTask);

        var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
        unitOfWork.SetupGet(u => u.tagsRepository).Returns(tagsRepo.Object);
        unitOfWork.Setup(u => u.SaveAsync()).Returns(System.Threading.Tasks.Task.CompletedTask);

        var mapper = new Mock<IMapper>(MockBehavior.Strict);
        mapper.Setup(m => m.Map<Tags>(It.IsAny<TagsModel>())).Returns(new Tags());

        var service = new TagsService(unitOfWork.Object, mapper.Object);

        var model = new TagsModel(1, string.Empty);
        var result = await service.AddAsync(model);

        Assert.False(result);
        tagsRepo.Verify(r => r.AddAsync(It.IsAny<Tags>()), Times.Never);
        unitOfWork.Verify(u => u.SaveAsync(), Times.Never);
    }

    [Fact]
    public async System.Threading.Tasks.Task UpdateAsyncNegativeIdShouldBeRejected()
    {
        var tagsRepo = new Mock<ITagsRepository>(MockBehavior.Strict);
        tagsRepo.Setup(r => r.Update(It.IsAny<Tags>()));

        var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
        unitOfWork.SetupGet(u => u.tagsRepository).Returns(tagsRepo.Object);
        unitOfWork.Setup(u => u.SaveAsync()).Returns(System.Threading.Tasks.Task.CompletedTask);

        var mapper = new Mock<IMapper>(MockBehavior.Strict);
        mapper.Setup(m => m.Map<Tags>(It.IsAny<TagsModel>())).Returns(new Tags());

        var service = new TagsService(unitOfWork.Object, mapper.Object);

        var model = new TagsModel(-1, string.Empty);
        var result = await service.UpdateAsync(model);

        Assert.False(result);
        tagsRepo.Verify(r => r.Update(It.IsAny<Tags>()), Times.Never);
        unitOfWork.Verify(u => u.SaveAsync(), Times.Never);
    }
    [Fact]
    public async System.Threading.Tasks.Task AddAsyncTagWithZeroIdShouldBeRejected()
    {
        var tagsRepo = new Mock<ITagsRepository>(MockBehavior.Strict);
        var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
        unitOfWork.SetupGet(u => u.tagsRepository).Returns(tagsRepo.Object);

        var mapper = new Mock<IMapper>(MockBehavior.Strict);
        var service = new TagsService(unitOfWork.Object, mapper.Object);

        var model = new TagsModel(0, "Urgent");
        var result = await service.AddAsync(model);

        Assert.False(result);
        tagsRepo.VerifyNoOtherCalls();
        unitOfWork.Verify(u => u.SaveAsync(), Times.Never);
        mapper.VerifyNoOtherCalls();
    }
}
