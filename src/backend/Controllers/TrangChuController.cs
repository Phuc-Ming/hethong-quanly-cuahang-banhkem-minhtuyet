using Microsoft.AspNetCore.Mvc;
using MinhTuyetBakery.KhoXuLy;

namespace MinhTuyetBakery.Controllers;

public class TrangChuController : Controller
{
    private readonly XuLyCuaHang _xuLy;

    public TrangChuController(XuLyCuaHang xuLy)
    {
        _xuLy = xuLy;
    }

    public async Task<IActionResult> Index()
    {
        if (HttpContext.Session.GetString("TaiKhoan") == null) return RedirectToAction("Index", "DangNhap");
        return View(await _xuLy.LayTrangChuAsync());
    }
}
