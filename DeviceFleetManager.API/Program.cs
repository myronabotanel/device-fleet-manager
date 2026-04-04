using MongoDB.Driver;

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
    Console.WriteLine("Conexiune MongoDB reusita!");
}
catch (Exception ex)
{
    Console.WriteLine($"Eroare conexiune MongoDB: {ex.Message}");
}

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();