using billing_mangment_system.Models;
using billing_mangment_system_v2.ICollectionService;
using billing_mangment_system_v2.Repo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAuthentication("Bearer")
            .AddJwtBearer(opts =>
            {
                opts.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration.GetValue<string>("Authentication:Issuer"),
                    ValidAudience = builder.Configuration.GetValue<string>("Authentication:Audience"),
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(
                        builder.Configuration.GetValue<string>("Authentication:Key")))
                };
            });

builder.Services.AddCors(options =>
{

    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});


// Add MongoDB services
builder.Services.AddSingleton<IMongoClient>(ServiceProvider =>
{
    return new MongoClient("mongodb://localhost:27017");
});
builder.Services.AddScoped<IMongoDatabase>(ServiceProvider =>
{
    var mongoClient = ServiceProvider.GetRequiredService<IMongoClient>();
    return mongoClient.GetDatabase("ElectricBillingSystem");
});
builder.Services.AddScoped<ICollectionBillService, BillsRepo>();
builder.Services.AddScoped<IUserService, UserRepo>();
builder.Services.AddScoped<IAdminUser, AdminUserRepo>();
builder.Services.AddScoped<IAmount, AmountRepo>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


string localIP = LocalIPAddress();
app.Urls.Add("http://" + localIP + ":8888");
static string LocalIPAddress()
{
    using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
    {
        socket.Connect("192.168.22.196", 8888);
        IPEndPoint? endPoint = socket.LocalEndPoint as IPEndPoint;
        if (endPoint != null)
        {
            return endPoint.Address.ToString();
        }
        else
        {
            return "127.0.0.1";
        }
    }
}
app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
