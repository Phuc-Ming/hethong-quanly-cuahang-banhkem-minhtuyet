using Microsoft.AspNetCore.Mvc;
using MinhTuyetBakery.Services;

namespace MinhTuyetBakery.Controllers;

public class ThongKeController : Controller
{
    private readonly XuLyCuaHang _xuLy;
    public ThongKeController(XuLyCuaHang xuLy) => _xuLy = xuLy;

    public async Task<IActionResult> Index()
    {
        if (!DaDangNhap()) return RedirectToAction("Index", "DangNhap");
        return View(await _xuLy.LayThongKeAsync());
    }

    private bool DaDangNhap() => HttpContext.Session.GetString("TaiKhoan") != null;
}
