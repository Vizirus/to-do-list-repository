using System;
using Microsoft.EntityFrameworkCore;
using DataLayer.DataLayer.ContextData;
using DataLayer.DataLayer.Interfaces;

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
