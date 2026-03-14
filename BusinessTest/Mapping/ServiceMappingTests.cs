using AutoMapper;
using BusinessTest.TestInfrastructure;
using DataLayer.DataLayer.Entities;
using WebApi.BusinessLayer.Models;
using TaskEntity = DataLayer.DataLayer.Entities.Task;

namespace BusinessTest.Mapping;

public class ServiceMappingTests
{
    private readonly IMapper mapper = MapperFactory.CreateBusinessMapper();

    [Fact]
    public void ListsModelToListsShouldMap()
    {
        var model = new ListsModel(0, "Work", 1, DateTime.UtcNow);
        var entity = this.mapper.Map<Lists>(model);
        Assert.NotNull(entity);
    }

    [Fact]
    public void UserEntityToUserModelShouldMap()
    {
        var entity = new User { Id = 1, Username = "a", Email = "a@b.com", PasswordHash = "h", CreatedDate = DateTime.UtcNow };
        var model = this.mapper.Map<UserModel>(entity);
        Assert.NotNull(model);
    }

    [Fact]
    public void TaskModelToTaskEntityShouldMap()
    {
        var model = new TaskModel(0, 1, "Name", "Desc", DateTime.UtcNow, DateTime.UtcNow.AddDays(1), 1, 1);
        var entity = this.mapper.Map<TaskEntity>(model);
        Assert.NotNull(entity);
    }

    [Fact]
    public void TaskStatusesEntityToTaskStatusesModelShouldMap()
    {
        var entity = new TaskStatuses { Id = 1, Name = "New" };
        var model = this.mapper.Map<TaskStatusesModel>(entity);
        Assert.NotNull(model);
    }

    [Fact]
    public void TaskTagsModelToTaskTagsEntityShouldMap()
    {
        var model = new TaskTagsModel(0, 1, 1);
        var entity = this.mapper.Map<TaskTags>(model);
        Assert.NotNull(entity);
    }

    [Fact]
    public void TagsModelToTagsEntityShouldMap()
    {
        var model = new TagsModel(0, "Urgent");
        var entity = this.mapper.Map<Tags>(model);
        Assert.NotNull(entity);
    }

    [Fact]
    public void TaskCommentsModelToTaskCommentsEntityShouldMap()
    {
        var model = new TaskCommentsModel(0, 1, 1, "Hi", DateTime.UtcNow);
        var entity = this.mapper.Map<TaskComments>(model);
        Assert.NotNull(entity);
    }
}
