using Microsoft.AspNetCore.Mvc;
using MinhTuyetBakery.KhoXuLy;

namespace MinhTuyetBakery.Controllers;

public class ThongKeController : Controller
{
    private readonly XuLyCuaHang _xuLy;

    public ThongKeController(XuLyCuaHang xuLy)
    {
        _xuLy = xuLy;
    }

    public async Task<IActionResult> Index()
    {
        if (HttpContext.Session.GetString("TaiKhoan") == null) return RedirectToAction("Index", "DangNhap");
        return View(await _xuLy.LayBaoCaoThongKeAsync());
    }
}
