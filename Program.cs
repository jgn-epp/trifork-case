using trifork.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using trifork.Interfaces;
using trifork.Models.Input;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IValidator<CreatePlayerModel>, CreatePlayerModelValidator>();
builder.Services.AddScoped<IValidator<UpdatePlayerModel>, UpdatePlayerModelValidator>();
builder.Services.AddScoped<IValidator<CreateMatchModel>, CreateMatchModelValidator>();
builder.Services.AddScoped<IValidator<UpdateMatchResultModel>, UpdateMatchResultModelValidator>();

builder.Services.AddDbContext<FoosballContext>(opt =>
    opt.UseInMemoryDatabase("FoosballDb")
);

builder.Services.AddScoped<IDataAccess, DataAccess>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
