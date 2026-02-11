using System.Net;
using MongoDB.Driver;
using MongoDB.Entities;
using Polly;
using Polly.Extensions.Http;
using SearchService;
using SearchService.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient<AuctionSvcHttpClient>().AddPolicyHandler(GetPolicy());

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
app.Lifetime.ApplicationStarted.Register(async () =>
{

    try
    {
        await DbInitializer.InitDb(app, db);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }
});


// Register DB for DI

// Init indexes + seed

app.Run();

static IAsyncPolicy<HttpResponseMessage> GetPolicy()
    => HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
        .WaitAndRetryForeverAsync(_ => TimeSpan.FromSeconds(3));
