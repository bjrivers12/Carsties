using System;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public class AuctionFinishedConsumer : IConsumer<AuctionFinished>
{
    private readonly DB _db;

    public AuctionFinishedConsumer(DB db)
    {
        _db = db;
    }
    public async Task Consume(ConsumeContext<AuctionFinished> context)
    {
        var auction = await _db.Find<Item>().OneAsync(context.Message.AuctionId);

        if (context.Message.ItemSold)
        {
            auction.Winner = context.Message.Winner;
            auction.SoldAmount = (int)context.Message.Amount;
        }
        auction.Status = "Finished";

        await _db.SaveAsync(auction);
    }

}
