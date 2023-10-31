using AspNetCoreHero.ToastNotification.Abstractions;
using Melodic.Application.ExtensionMethods;
using Melodic.Domain.Entities;
using Melodic.Domain.ValueObjects;
using Melodic.Infrastructure.Identity;
using Melodic.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Melodic.Web.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = ApplicationRole.Role_Admin)]
public class EVoucherController : Controller
{
    private readonly ApplicationDbContext _db;
    public INotyfService _notyfService { get; }
    public EVoucherController(ApplicationDbContext db, INotyfService notyfService)
    {
        _db = db;
        _notyfService = notyfService;
    }
    public async Task<IActionResult> Index(int? pageNumber)
    {
        var paginatedList = await _db.EVouchers.PaginatedListAsync(pageNumber ?? 1, 4);
        return View(paginatedList);
    }
    public async Task<IActionResult> CreateAndUpdate(int? id)
    {
        if (id == null || id == 0)
        {
            EVoucher voucher = new EVoucher();
            return View(voucher);
        }
        else
        {
            EVoucher voucher = await _db.EVouchers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            return View(voucher);
        }
    }
    [HttpPost]
    public async Task<IActionResult> CreateAndUpdate(EVoucher voucher)
    {
        if (ModelState.IsValid)
        {
            //Create new
            if (voucher.Id == 0)
            {
                await _db.EVouchers.AddAsync(voucher);
                _notyfService.Success("EVoucher Added Successfully");
            }
            else
            {
                _db.EVouchers.Update(voucher);
                _notyfService.Success("EVoucher Updated Successfully");
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int? id)
    {
        EVoucher? voucher = await _db.EVouchers.FirstOrDefaultAsync(x => x.Id == id);
        if (id == null || id == 0)
        {
            return NotFound();
        }
        _db.EVouchers.Remove(voucher);
        await _db.SaveChangesAsync();
        _notyfService.Success("Deleted!");
        return RedirectToAction("Index");
    }
}
