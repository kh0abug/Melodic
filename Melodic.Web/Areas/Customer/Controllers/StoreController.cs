﻿using Melodic.Application.Pagination;
using Melodic.Application.Parameters;
using Melodic.Domain.Entities;
using Melodic.Infrastructure.Persistence;
using Melodic.Web.ViewsModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection.Metadata;

namespace Melodic.Web.Areas.Customer.Controllers;

[Area("Customer")]
public class StoreController : Controller
{
    private readonly ApplicationDbContext _context;

    public StoreController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(SpeakerRequestParameters parameter)
    {
        ViewBag.SearchTerm = parameter.SearchTerm;
        var speakers = _context.Speakers.AsQueryable();
        var brands = _context.Brands.AsNoTracking().Take(10);

        if (!parameter.ValidPriceRange)
        {
            ModelState.AddModelError("maxprice", "Please fill in the appropriate price range.");
        }

        if (!ModelState.IsValid)
        {
            return View(new StoreViewModel
            {
                RequestParameters = parameter,
                Brands = await brands.ToListAsync()
            });
        }
        else
        {
            speakers = speakers.Where(s => s.Name!.Contains(parameter.SearchTerm!));
            speakers = speakers.Where(s => s.Price >= parameter.MinPrice && s.Price <= parameter.MaxPrice);
        }

        if (parameter.BrandId is not null)
        {
            speakers = speakers.Where(s => s.BrandId == parameter.BrandId);
        }

        speakers = parameter.OrderBy?.ToLower() switch
        {
            "price_desc" => speakers.OrderByDescending(s => s.Price),
            //"bestseller" => speakers.OrderByDescending(s => s.Price),
            //"newest" => speakers.Reverse(),
            _ => speakers.OrderBy(s => s.Price),
        };

        var result = await speakers.AsNoTracking().PaginatedListAsync(parameter.PageNumber ?? 1 , parameter.PageSize);
        if (!result.Items.Any())
        {
            result = null;
        }
        return View(new StoreViewModel
        {
            Speakers = result,
            RequestParameters = parameter,
            Brands = await brands.ToListAsync()
        });
    }

    public async Task<IActionResult> Detail(int? id)
    {
        if (id == null && id == 0)
        {
            return NotFound();
        }
        var speaker = await _context.Speakers
            .AsNoTracking()
            .Include(s => s.Brand)
            .Where(s => s.Id == id).FirstAsync();
        return View(speaker);
    }
}

