using DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Service;
using Service.AdminGroupService;
using Service.AdminRomService;
using Service.AdminRoomBookingService;
using Service.DateHandlerService;
using Service.GroupService;
using Service.RoomService;
using Service.ScheduleService;
using Service.SingleBookingService;
using Service.UserService;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Role Based Authentication",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

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
builder.Services.AddTransient<IAdminGroupService, AdminGroupService>();
builder.Services.AddTransient<IRoomService, RoomService>();
builder.Services.AddTransient<IAdminRoomService, AdminRoomService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IUserAcess, UserAccess>();
builder.Services.AddTransient<IBookingAccess, BookingAccess>();
builder.Services.AddTransient<ISingleBookingService, SingleBookingService>();
builder.Services.AddTransient<IAdminRoomBookingService, AdminRoomBookingService>();
builder.Services.AddTransient<IDatehandler, DateHandler>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<IScheduleService, ScheduleService>();

//--------------------------JWT-Token stuff----------------------------------------------------------------------
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
//--------------------------JWT-Token stuff----------------------------------------------------------------------

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
