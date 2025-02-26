using DHP_Manager.Data;
using DHP_Manager.Models.Entities;
using DHP_Manager.Repositories.Impl;
using DHP_Manager.Repositories.Interface;
using DHP_Manager.Services.impl;
using DHP_Manager.Services.Interface;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICompanyCustomerRepository, CompanyCustomerRepository>();
builder.Services.AddScoped<INormalCustomerRepository, NormalCustomerRepository>();
builder.Services.AddScoped<INormalCustomerService, NormalCustomerService>();
builder.Services.AddScoped<ICompanyCustomerService, CompanyCustomerService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // Add the URL of your frontend
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Use CORS
app.UseCors("AllowFrontend");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction()) // Ensure it's available
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
