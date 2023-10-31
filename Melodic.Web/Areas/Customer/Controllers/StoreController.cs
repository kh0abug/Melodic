using Melodic.Application.ExtensionMethods;
using Melodic.Application.Parameters;
using Melodic.Domain.Entities;
using Melodic.Infrastructure.Persistence;
using Melodic.Web.Areas.Customer.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using NuGet.Packaging.Signing;
using System;
using System.Data;

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

        //var billDetail = _context.BillProducts
        //    .GroupBy(b => b.SpeakerId)
        //    .Select(
        //    b => new BillProduct
        //    {
        //        SpeakerId = b.First().SpeakerId,
        //        Quantity = b.Sum(sp => sp.Quantity)
        //    });

        var billDetail = from bill in _context.OrderDetails
                         /*group bill by bill.SpeakerId into speakerSold
                         select
                          new OrderDetail
                          {
                              SpeakerId = speakerSold.First().SpeakerId,
                              Quantity = speakerSold.Sum(sp => sp.Quantity)
                          }*/;

        //var speakers = from speaker in _context.Speakers
        //               join bill in billDetail
        //                on speaker.Id equals bill.SpeakerId
        //               select new SpeakerViewModel()
        //               {
        //                   Id = speaker.Id,
        //                   Name = speaker.Name,
        //                   BrandId = speaker.BrandId,
        //                   Price = speaker.Price,
        //                   Img = speaker.Img,
        //                   QuantitySold = bill.Quantity,
        //                   Created = speaker.Created,
        //                   Decription = speaker.Decription
        //               };

        //    var staffData =
        //from ps in dc.PromotionSlots
        // join b in dc.Bookings on ps.StaffID = b.StaffID into slots
        // from slot in slots.DefaultIfEmpty()
        // group new { ps, slot } by ps.StaffFName into NewGroup
        // select new dataView
        // {
        //     StaffFName = NewGroup.Key,
        //     Total = NewGroup.Sum(a => a.ps != null ? a.ps.Max_Occupancy : 0),

        //     //problem:
        //     Occupied = NewGroup.Select(x => x.slot.Quantity).Distinct().Sum()
        // }

        var speakers = from speaker in _context.Speakers
                       join orderDetail in _context.OrderDetails on speaker.Id equals orderDetail.SpeakerId into slots
                       from slot in slots.DefaultIfEmpty()
                       group new { speaker, slot } by speaker.Id into NewGroup
                       select new SpeakerViewModel()
                       {
                           //Id = NewGroup.Key,
                           //Name = NewGroup.First().,
                           //BrandId = speaker.BrandId,
                           //Price = speaker.Price,
                           //Img = speaker.Img,
                           //QuantitySold = bill.Quantity,
                           //Created = speaker.Created,
                           //Decription = speaker.Decription
                       };

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
            "bestseller" => speakers.OrderByDescending(s => s.QuantitySold),
            "newest" => speakers.OrderByDescending(s => s.Created),
            _ => speakers.OrderBy(s => s.Price),
        };

        var result = await speakers.AsNoTracking().PaginatedListAsync(parameter.PageNumber ?? 1, parameter.PageSize);
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

