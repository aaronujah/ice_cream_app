﻿using IceCreamMAUI.Api.Data;
using IceCreamMAUI.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace IceCreamMAUI.Api.Services;

public class IcecreamService(DataContext context)
{
    private readonly DataContext _context = context;

    public async Task<IceCreamDto[]> GetIcecreamAsync() =>
        await _context.Icecreams.AsNoTracking()
            .Select(i =>
                new IceCreamDto(
                    i.Id,
                    i.Name,
                    i.Image,
                    i.Price,
                    i.Options
                        .Select(o => new IcecreamOptionDto(o.Flavor, o.Topping))
                        .ToArray()
                    )
            )
            .ToArrayAsync();
}
