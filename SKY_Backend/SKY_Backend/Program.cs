using DAL;
using Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "Cors", policy =>
    {
        policy.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

// Dependencies 

builder.Services.AddTransient<IGroupService, GroupService>();
builder.Services.AddTransient<IRoomService, RoomService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IRoomAccess, RoomAccess>();
builder.Services.AddTransient<IGroupAccess, GroupAccess>();
builder.Services.AddTransient<IUserAcess, UserAccess>();
builder.Services.AddTransient<IBookingAccess, BookingAccess>();
builder.Services.AddTransient<IBookingService, BookingService>();
builder.Services.AddTransient<IDateConverter, DateConverter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Cors");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
