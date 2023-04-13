using Application.DAOInterfaces;
using Application.Logic;
using Application.LogicInterfaces;
using EfcDataAccess;
using EfcDataAccess.DAOs;
using FileData;
using FileData.DAOs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<FileContext>();
builder.Services.AddScoped<IUserDAO, UserEfcDao>();
builder.Services.AddScoped<IUserLogic, UserLogic>();

builder.Services.AddScoped<ITodoDao, ToDoEfcDao>();
builder.Services.AddScoped<ITodoLogic, TodoLogic>();

builder.Services.AddDbContext<ToDoContext>();

//this can be done if we use connection string to connect to the sqlite database
// builder.Services.AddDbContext<ToDoContext>(options =>
//     options.UseSqlite(builder.Configuration.GetConnectionString("DefaultSqliteConnection")));

var app = builder.Build();

//enabling Cross Origin Resource Sharing to access the API
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
