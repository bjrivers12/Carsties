using System;
using AutoMapper;
using Contracts;
using SearchService.Models;
using ZstdSharp.Unsafe;

namespace SearchService.RequestHelpers;

public class MappingProfiles : Profile
{
    private readonly IMapper _mapper;
    public MappingProfiles(IMapper mapper)
    {
        _mapper = mapper;
    }
    public MappingProfiles()
    {
        CreateMap<AuctionCreated, Item>();
    }
}
