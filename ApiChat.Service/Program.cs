using ApiChat.Business.TokenService;
using ApiChat.Business.UseService;
using ApiChat.DataAccess.Context;
using ApiChat.Domain.Handler;
using ApiChat.Domain.UnitOfWork;
using ApiChat.Service.Hubs;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});
builder.Services.AddAuthentication("None").AddScheme<AuthenticationSchemeOptions, JwtTokenValidationHandler>("None", null);

#region Inyeccion de dependencias
builder.Services.AddDbContext<ChatBotContext>(options => options.UseSqlServer("Server = KAKASHI\\SQLEXPRESS; Database = ChatBot;User Id=sa; Password=Kakashi2403*"));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddSignalR();

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder =>
{
    builder
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .WithOrigins();
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/hubs/chat");

app.Run();
