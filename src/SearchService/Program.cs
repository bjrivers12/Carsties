using MongoDB.Driver;
using MongoDB.Entities;
using SearchService;
using SearchService.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient<AuctionSvcHttpClient>();

// Init Mongo once
var db = await DB.InitAsync(
    "SearchDb",
    MongoClientSettings.FromConnectionString(
        builder.Configuration.GetConnectionString("MongoDbConnection"))
);

builder.Services.AddSingleton(db);

var app = builder.Build();

app.UseAuthorization();
app.MapControllers();


// Register DB for DI

// Init indexes + seed
try
{
    await DbInitializer.InitDb(db);
}
catch (Exception e)
{
    Console.WriteLine(e);
}

app.Run();
