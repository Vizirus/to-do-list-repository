using Microsoft.EntityFrameworkCore;
using DataLayer.DataLayer.Entities;

namespace DataLayer.DataLayer.ContextData;

public class ToDoListAppDbContext : DbContext
{
    public ToDoListAppDbContext(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<Lists> lists { get; set; }

    public DbSet<SharedLists> sharedLists { get; set; }

    public DbSet<Tags> tags { get; set; }

    public DbSet<TaskComments> taskComments { get; set; }

    public DbSet<Entities.Task> tasks { get; set; }

    public DbSet<TaskStatuses> taskStatuses { get; set; }

    public DbSet<TaskTags> taskTags { get; set; }

    public DbSet<User> users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        base.OnModelCreating(modelBuilder);

        _ = modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Username = "admin", Email = "admin@email.com", PasswordHash = "hash1", CreatedDate = DateTime.Now },
            new User { Id = 2, Username = "john", Email = "john@email.com", PasswordHash = "hash2", CreatedDate = DateTime.Now },
            new User { Id = 3, Username = "sarah", Email = "sarah@email.com", PasswordHash = "hash3", CreatedDate = DateTime.Now }
        );

        // Task Statuses
        _ = modelBuilder.Entity<TaskStatuses>().HasData(
            new TaskStatuses { Id = 1, Name = "New" },
            new TaskStatuses { Id = 2, Name = "In Progress" },
            new TaskStatuses { Id = 3, Name = "Completed" },
            new TaskStatuses { Id = 4, Name = "Blocked" }
        );

        // Lists
        _ = modelBuilder.Entity<Lists>().HasData(
            new Lists { Id = 1, ListName = "Work Tasks", CreatedByUser = 1, CreatedDate = DateTime.Now },
            new Lists { Id = 2, ListName = "Home Tasks", CreatedByUser = 1, CreatedDate = DateTime.Now },
            new Lists { Id = 3, ListName = "Study Tasks", CreatedByUser = 2, CreatedDate = DateTime.Now }
        );

        // Tags
        _ = modelBuilder.Entity<Tags>().HasData(
            new Tags { Id = 1, Name = "Urgent" },
            new Tags { Id = 2, Name = "Low Priority" },
            new Tags { Id = 3, Name = "Bug" },
            new Tags { Id = 4, Name = "Feature" },
            new Tags { Id = 5, Name = "Research" }
        );

        // Tasks
        _ = modelBuilder.Entity<Entities.Task>().HasData(
            new Entities.Task
            {
                Id = 1,
                ListId = 1,
                TaskName = "Prepare report",
                TaskDescription = "Prepare monthly financial report",
                TaskStartDate = new DateTime(2026, 3, 10, 1, 1, 1, DateTimeKind.Utc),
                TaskFinishDate = new DateTime(2026, 3, 15, 1, 1, 1, DateTimeKind.Utc),
                StatusId = 2,
                AssigndUserId = 1
            },
            new Entities.Task
            {
                Id = 2,
                ListId = 1,
                TaskName = "Fix login bug",
                TaskDescription = "Resolve authentication issue",
                TaskStartDate = new DateTime(2026, 3, 11, 1, 1, 1, DateTimeKind.Utc),
                TaskFinishDate = new DateTime(2026, 3, 12, 1, 1, 1, DateTimeKind.Utc),
                StatusId = 1,
                AssigndUserId = 2
            },
            new Entities.Task
            {
                Id = 3,
                ListId = 2,
                TaskName = "Buy groceries",
                TaskDescription = "Milk, bread, vegetables",
                TaskStartDate = new DateTime(2026, 3, 12, 1, 1, 1, DateTimeKind.Utc),
                TaskFinishDate = new DateTime(2026, 3, 12, 1, 1, 1, DateTimeKind.Utc),
                StatusId = 1,
                AssigndUserId = 1
            }
        );

        // TaskTags
        _ = modelBuilder.Entity<TaskTags>().HasData(
            new TaskTags { Id = 1, TaskId = 1, TagId = 1 },
            new TaskTags { Id = 2, TaskId = 1, TagId = 4 },
            new TaskTags { Id = 3, TaskId = 2, TagId = 3 },
            new TaskTags { Id = 4, TaskId = 3, TagId = 2 }
        );

        // Comments
        _ = modelBuilder.Entity<TaskComments>().HasData(
            new TaskComments { Id = 1, TaskId = 1, UserId = 1, CommentText = "Initial draft completed", CreatedDate = DateTime.Now },
            new TaskComments { Id = 2, TaskId = 1, UserId = 2, CommentText = "Need to verify numbers", CreatedDate = DateTime.Now },
            new TaskComments { Id = 3, TaskId = 2, UserId = 2, CommentText = "Bug likely caused by token expiration", CreatedDate = DateTime.Now }
        );

        // Shared Lists
        _ = modelBuilder.Entity<SharedLists>().HasData(
            new SharedLists { Id = 1, ToDoListId = 1, UserWhoAssignsIs = 1, AssignedUserId = 2 },
            new SharedLists { Id = 2, ToDoListId = 2, UserWhoAssignsIs = 1, AssignedUserId = 3 }
        );
    }
}
