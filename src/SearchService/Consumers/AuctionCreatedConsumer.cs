using System;
using AutoMapper;
using Contracts;
using MassTransit;
using SearchService.Models;
using MongoDB.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace SearchService;

public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
{
    private readonly IMapper _mapper;
    private readonly DB _db;

    public AuctionCreatedConsumer(IMapper mapper, DB db)
    {
        _mapper = mapper;
        _db = db;
    }
    public async Task Consume(ConsumeContext<AuctionCreated> context)
    {
        Console.WriteLine("--> Consuming auction created: " + context.Message.Id);

        var item = _mapper.Map<Item>(context.Message);

        if (item.Model == "Foo") throw new ArgumentException("Cannot Sell car named Foo");
        await _db.SaveAsync(item);
    }
}
