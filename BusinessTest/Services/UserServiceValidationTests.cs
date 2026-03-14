using AutoMapper;
using BusinessLayer.BusinessLayer.Services;
using DataLayer.DataLayer.Entities;
using DataLayer.DataLayer.Interfaces;
using Moq;
using WebApi.BusinessLayer.Models;

namespace BusinessTest.Services;

public class UserServiceValidationTests
{
    [Fact]
    public async System.Threading.Tasks.Task AddAsyncNewUserWithZeroIdShouldPersist()
    {
        var userRepo = new Mock<IUserRepository>(MockBehavior.Strict);
        userRepo.Setup(r => r.AddAsync(It.IsAny<User>())).Returns(System.Threading.Tasks.Task.CompletedTask);

        var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
        unitOfWork.SetupGet(u => u.userRepository).Returns(userRepo.Object);
        unitOfWork.Setup(u => u.SaveAsync()).Returns(System.Threading.Tasks.Task.CompletedTask);

        var mapper = new Mock<IMapper>(MockBehavior.Strict);
        mapper.Setup(m => m.Map<User>(It.IsAny<UserModel>())).Returns(new User());

        var service = new UserService(unitOfWork.Object, mapper.Object);

        var model = new UserModel(1, "alice", "alice@example.com", "hash", DateTime.UtcNow);
        var result = await service.AddAsync(model);

        Assert.True(result);
        userRepo.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
        unitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async System.Threading.Tasks.Task AddAsyncMissingUsernameShouldBeRejected()
    {
        var userRepo = new Mock<IUserRepository>(MockBehavior.Strict);
        var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
        unitOfWork.SetupGet(u => u.userRepository).Returns(userRepo.Object);

        var mapper = new Mock<IMapper>(MockBehavior.Strict);
        var service = new UserService(unitOfWork.Object, mapper.Object);

        var model = new UserModel(0, string.Empty, "a@b.com", "hash", DateTime.UtcNow);
        var result = await service.AddAsync(model);

        Assert.False(result);
        userRepo.VerifyNoOtherCalls();
        unitOfWork.Verify(u => u.SaveAsync(), Times.Never);
        mapper.VerifyNoOtherCalls();
    }

    [Fact]
    public async System.Threading.Tasks.Task UpdateAsyncNegativeIdShouldBeRejected()
    {
        var userRepo = new Mock<IUserRepository>(MockBehavior.Strict);
        userRepo.Setup(r => r.Update(It.IsAny<User>()));

        var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
        unitOfWork.SetupGet(u => u.userRepository).Returns(userRepo.Object);
        unitOfWork.Setup(u => u.SaveAsync()).Returns(System.Threading.Tasks.Task.CompletedTask);

        var mapper = new Mock<IMapper>(MockBehavior.Strict);
        mapper.Setup(m => m.Map<User>(It.IsAny<UserModel>())).Returns(new User());

        var service = new UserService(unitOfWork.Object, mapper.Object);

        var model = new UserModel(-1, "alice", "alice@example.com", "hash", DateTime.UtcNow);
        var result = await service.UpdateAsync(model);

        Assert.False(result);
        userRepo.Verify(r => r.Update(It.IsAny<User>()), Times.Never);
        unitOfWork.Verify(u => u.SaveAsync(), Times.Never);
    }
}
