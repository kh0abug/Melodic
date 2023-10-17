using AspNetCoreHero.ToastNotification.Abstractions;
using Melodic.Domain.Entities;
using Melodic.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace Melodic.Web.Areas.Admin.Controllers;
[Area("Admin")]
public class EVoucherController : Controller
{
    private readonly ApplicationDbContext _db;
    public INotyfService _notyfService { get; }
    public EVoucherController(ApplicationDbContext db, INotyfService notyfService)
    {
        _db = db;
        _notyfService = notyfService;
    }
    public IActionResult Index()
    {
        IEnumerable<EVoucher> evoucherList = _db.EVouchers;
        return View(evoucherList);
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
