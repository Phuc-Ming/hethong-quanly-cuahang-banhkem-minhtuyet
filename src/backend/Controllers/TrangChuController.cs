using Microsoft.AspNetCore.Mvc;
using MinhTuyetBakery.Services;

namespace MinhTuyetBakery.Controllers;

public class TrangChuController : Controller
{
    private readonly XuLyCuaHang _xuLy;
    public TrangChuController(XuLyCuaHang xuLy) => _xuLy = xuLy;

    public async Task<IActionResult> Index()
    {
        if (!DaDangNhap()) return RedirectToAction("Index", "DangNhap");
        return View(await _xuLy.LayTrangChuAsync());
    }

    public IActionResult Loi() => View();
    private bool DaDangNhap() => HttpContext.Session.GetString("TaiKhoan") != null;
}
