using DeviceFleetManager.API.Repositories;
using DeviceFleetManager.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Text;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

// Configurare MongoDB
var mongoConnectionString = builder.Configuration["MongoDB:ConnectionString"];
var mongoDatabaseName = builder.Configuration["MongoDB:DatabaseName"];

var mongoClient = new MongoClient(mongoConnectionString);
var mongoDatabase = mongoClient.GetDatabase(mongoDatabaseName);

// Test conexiune
try
{
    mongoClient.ListDatabaseNames();
    Console.WriteLine("✅ Conexiune MongoDB reusita!");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Eroare conexiune MongoDB: {ex.Message}");
}

// Inregistrare servicii
builder.Services.AddSingleton<IMongoDatabase>(mongoDatabase);
builder.Services.AddScoped<DeviceRepository>();
builder.Services.AddScoped<DeviceService>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();

// Configurare JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
builder.Services.AddOpenApi();
builder.Services.AddHttpClient<AiService>()
    .AddPolicyHandler(HttpPolicyExtensions
        .HandleTransientHttpError() // Prinde erori retea
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.TooManyRequests) // Prinde eroarea 429 (am avut o de mutle ori)
        .WaitAndRetryAsync(
            retryCount: 3, 
            sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // wait 2, 4, 8 sec
            onRetry: (outcome, timespan, retryAttempt, context) =>
            {
                // mes err
                Console.WriteLine($"[Avertisment] Eroare 429 - Prea multe cereri. Se asteapta {timespan.TotalSeconds}s ininte de incercare {retryAttempt}...");
            }));


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();