using DeviceFleetManager.API.Repositories;
using DeviceFleetManager.API.Services;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Conventions;

// Configurare BSON Convention pentru mapare camelCase
var pack = new ConventionPack
{
    new CamelCaseElementNameConvention(),
    new IgnoreExtraElementsConvention(true),
    new IgnoreIfDefaultConvention(true)
};
ConventionRegistry.Register("camelCase", pack, t => true);

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors();
app.MapControllers();

app.Run();