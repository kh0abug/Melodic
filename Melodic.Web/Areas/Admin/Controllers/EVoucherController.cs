using AspNetCoreHero.ToastNotification.Abstractions;
using Melodic.Application.Pagination;
using Melodic.Domain.Entities;
using Melodic.Domain.ValueObjects;
using Melodic.Infrastructure.Identity;
using Melodic.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> IndexAsync(int? pageNumber)
    {
        var paginatedList = await _db.EVouchers.PaginatedListAsync(pageNumber ?? 1, 4);
        return View(paginatedList);
    }
    public ActionResult CreateAndUpdate(int? id)
    {
        if (id == null || id == 0)
        {
            EVoucher voucher = new EVoucher();
            return View(voucher);
        }
        else
        {
            EVoucher voucher = _db.EVouchers.FirstOrDefault(x => x.Id == id);
            return View(voucher);
        }
    }
    [HttpPost]
    public ActionResult CreateAndUpdate(EVoucher voucher)
    {
        if (ModelState.IsValid)
        {
            //Create new
            if (voucher.Id == 0)
            {
                _db.EVouchers.Add(voucher);
                _notyfService.Success("EVoucher Added Successfully");
            }
            else
            {
                _db.EVouchers.Update(voucher);
                _notyfService.Success("EVoucher Updated Successfully");
            }
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        return View();
    }

    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        EVoucher? voucher = _db.EVouchers.FirstOrDefault(x => x.Id == id);
        if (voucher == null) { return NotFound(); }
        return View(voucher);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteBrand(int? id)
    {
        EVoucher? voucher = _db.EVouchers.FirstOrDefault(x => x.Id == id);
        if (id == null || id == 0)
        {
            return NotFound();
        }
        _db.EVouchers.Remove(voucher);
        _db.SaveChanges();
        _notyfService.Success("Deleted!");
        return RedirectToAction("Index");
    }
}
