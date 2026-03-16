using AutoMapper;
using BusinessLayer.BusinessLayer.Models;
using DataLayer.DataLayer.Entities;
using WebApi.BusinessLayer.Models;

namespace WebApi.BusinessLayer;

public class BusinessLayerProfile : Profile
{
    public BusinessLayerProfile()
    {
        _ = this.CreateMap<DataLayer.DataLayer.Entities.Task, TaskModel>()
            .ForMember(t => t.Id, tc => tc.MapFrom(tm => tm.Id))
            .ForMember(t => t.StatusId, tc => tc.MapFrom(tm => tm.StatusId))
            .ForMember(t => t.ListId, tc => tc.MapFrom(tm => tm.ListId))
            .ForMember(t => t.TaskComments, tc => tc.MapFrom(tm => tm.TaskComments))
            .ForMember(t => t.TaskDescription, tc => tc.MapFrom(tm => tm.TaskDescription))
            .ForMember(t => t.TaskStartDate, tc => tc.MapFrom(tm => tm.TaskStartDate))
            .ForMember(t => t.TaskFinishDate, tc => tc.MapFrom(tm => tm.TaskFinishDate))
            .ForMember(t => t.TaskName, tc => tc.MapFrom(tm => tm.TaskName))
            .ReverseMap();

        _ = this.CreateMap<Lists, ListsModel>()
            .ForMember(t => t.Id, tc => tc.MapFrom(tm => tm.Id))
            .ForMember(t => t.CreatedByUser, tc => tc.MapFrom(tm => tm.CreatedByUser))
            .ForMember(t => t.CreatedDate, tc => tc.MapFrom(tm => tm.CreatedDate))
            .ForMember(t => t.ListName, tc => tc.MapFrom(tm => tm.ListName))
            .ReverseMap();

        _ = this.CreateMap<SharedLists, SharedListsModel>()
            .ForMember(t => t.Id, tc => tc.MapFrom(tm => tm.Id))
            .ForMember(t => t.ToDoListId, tc => tc.MapFrom(tm => tm.ToDoListId))
            .ForMember(t => t.UserWhoAssignsIs, tc => tc.MapFrom(tm => tm.UserWhoAssignsIs))
            .ForMember(t => t.AssignedUserId, tc => tc.MapFrom(tm => tm.AssignedUserId))
            .ReverseMap();

        _ = this.CreateMap<Tags, TagsModel>()
            .ForMember(t => t.Id, tc => tc.MapFrom(tm => tm.Id))
            .ForMember(t => t.Name, tc => tc.MapFrom(tm => tm.Name))
            .ReverseMap();

        _ = this.CreateMap<TaskComments, TaskCommentsModel>()
            .ForMember(t => t.Id, tc => tc.MapFrom(tm => tm.Id))
            .ForMember(t => t.CreatedDate, tc => tc.MapFrom(tm => tm.CreatedDate))
            .ForMember(t => t.CommentText, tc => tc.MapFrom(tm => tm.CommentText))
            .ForMember(t => t.TaskId, tc => tc.MapFrom(tm => tm.TaskId))
            .ForMember(t => t.UserId, tc => tc.MapFrom(tm => tm.UserId))
            .ReverseMap();

        _ = this.CreateMap<TaskStatuses, TaskStatusesModel>()
            .ForMember(t => t.Id, tc => tc.MapFrom(tm => tm.Id))
            .ForMember(t => t.Name, tc => tc.MapFrom(tm => tm.Name))
            .ReverseMap();

        _ = this.CreateMap<TaskTags, TaskTagsModel>()
            .ForMember(t => t.Id, tc => tc.MapFrom(tm => tm.Id))
            .ForMember(t => t.TaskId, tc => tc.MapFrom(tm => tm.TaskId))
            .ForMember(t => t.TagId, tc => tc.MapFrom(tm => tm.TagId))
            .ReverseMap();

        _ = this.CreateMap<User, UserModel>()
            .ForMember(t => t.Id, tc => tc.MapFrom(tm => tm.Id))
            .ForMember(t => t.Email, tc => tc.MapFrom(tm => tm.Email))
            .ForMember(t => t.CreatedDate, tc => tc.MapFrom(tm => tm.CreatedDate))
            .ForMember(t => t.Username, tc => tc.MapFrom(tm => tm.Username))
            .ReverseMap();

    }
}
