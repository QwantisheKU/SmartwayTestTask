using Data;
using SmartwayTestTask.Repositories;
using SmartwayTestTask.Repositories.Interfaces;
using SmartwayTestTask.Services;
using SmartwayTestTask.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<DbContext>();

// Register Repositories
builder.Services.AddTransient<IEmployeeRepository, EmployeeRepository>();

// Register Services
builder.Services.AddTransient<IEmployeeService, EmployeeService>();

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers().AddNewtonsoftJson();	

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
