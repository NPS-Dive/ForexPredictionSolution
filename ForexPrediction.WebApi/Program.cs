using ForexPrediction.Application.Commands;
using ForexPrediction.Domain.Entities;
using ForexPrediction.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.PowerBI.Api;
using Microsoft.Rest;
using System.Text;
using ForexPrediction.Application.Commands.UploadData;
using ForexPrediction.Infrastructure.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ForexDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ForexDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
        {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
});
builder.Services.AddAuthorization();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UploadDataCommand).Assembly));
builder.Services.AddScoped<ForexPrediction.Domain.Interfaces.IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ForexPrediction.Domain.Interfaces.IPredictionService, SsaPredictionService>();
builder.Services.AddScoped<ForexPrediction.Domain.Interfaces.IDataService, DataService>();
builder.Services.AddScoped<ForexPrediction.Domain.Interfaces.IIdentityService, IdentityService>();
builder.Services.AddHttpClient<AlphaVantageService>();
builder.Services.AddScoped<IPowerBIClient>(sp =>
{
    var config = builder.Configuration.GetSection("PowerBI");
    var credentials = new TokenCredentials("YOUR_ACCESS_TOKEN", "Bearer"); // Replace with real auth
    return new PowerBIClient(new Uri("https://api.powerbi.com/"), credentials);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
    {
    app.UseSwagger();
    app.UseSwaggerUI();
    }

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();