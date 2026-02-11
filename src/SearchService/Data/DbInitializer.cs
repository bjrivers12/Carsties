using System.Text.Json;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Data;

public static class DbInitializer
{
    public static async Task InitDb(DB db)
    {
        await db.Index<Item>()
            .Key(x => x.Make, KeyType.Text)
            .Key(x => x.Model, KeyType.Text)
            .Key(x => x.Color, KeyType.Text)
            .CreateAsync();

        if (await db.CountAsync<Item>() == 0)
        {
            var json = await File.ReadAllTextAsync("Data/auctions.json");

            var items = JsonSerializer.Deserialize<List<Item>>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            await db.SaveAsync(items);
        }
    }
}
