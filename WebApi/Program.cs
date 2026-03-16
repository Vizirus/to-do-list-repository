using BusinessLayer.BusinessLayer.Interfaces;
using BusinessLayer.BusinessLayer.Services;
using DataLayer.DataLayer.ContextData;
using DataLayer.DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using WebApi.BusinessLayer;

namespace WebApi;

internal static class Program
{
    internal static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        _ = builder.Services.AddDbContext<ToDoListAppDbContext>(options =>
                options.UseInMemoryDatabase("ToDoDb"));

        _ = builder.Services.AddControllers();
        _ = builder.Services.AddEndpointsApiExplorer();
        _ = builder.Services.AddSwaggerGen();
        _ = builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        _ = builder.Services.AddScoped<ITaskService, TaskService>();
        _ = builder.Services.AddScoped<IListService, ListService>();
        _ = builder.Services.AddScoped<ITagsService, TagsService>();
        _ = builder.Services.AddScoped<ITaskAdditionalService, TaskAdditionalService>();
        _ = builder.Services.AddScoped<IUserService, UserService>();
        _ = builder.Services.AddAutoMapper(cfg => cfg.AddProfile<BusinessLayerProfile>());

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ToDoListAppDbContext>();
            _ = context.Database.EnsureCreated();
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            _ = app.UseDeveloperExceptionPage();
            _ = app.UseSwagger();
            _ = app.UseSwaggerUI();
        }

        _ = app.UseHttpsRedirection();

        _ = app.UseAuthorization();

        _ = app.MapControllers();

        app.Run();
    }
}
