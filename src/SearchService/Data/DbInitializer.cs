using System.Text.Json;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Data;

public static class DbInitializer
{
    public static async Task InitDb(WebApplication app, DB db)
    {
        await db.Index<Item>()
            .Key(x => x.Make, KeyType.Text)
            .Key(x => x.Model, KeyType.Text)
            .Key(x => x.Color, KeyType.Text)
            .CreateAsync();

        var count = await db.CountAsync<Item>();

        using var scope = app.Services.CreateScope();

        var httpClient = scope.ServiceProvider.GetRequiredService<AuctionSvcHttpClient>();

        var items = await httpClient.GetItemsForSearchDb(db);

        Console.WriteLine(items.Count + " returned from the aution service");

        if (items.Count > 0) await db.SaveAsync(items);


    }
}
