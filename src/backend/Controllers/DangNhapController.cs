using Microsoft.AspNetCore.Mvc;
using MinhTuyetBakery.Services;

namespace MinhTuyetBakery.Controllers;

public class DangNhapController : Controller
{
    private readonly XuLyCuaHang _xuLy;

    public DangNhapController(XuLyCuaHang xuLy)
    {
        _xuLy = xuLy;
    }

    [HttpGet]
    public IActionResult Index()
    {
        if (HttpContext.Session.GetString("TaiKhoan") != null)
            return RedirectToAction("Index", "TrangChu");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(string taiKhoan, string matKhau)
    {
        var nv = await _xuLy.DangNhapAsync(taiKhoan ?? "", matKhau ?? "");
        if (nv == null)
        {
            ViewBag.Loi = "Tên đăng nhập hoặc mật khẩu không đúng.";
            return View();
        }
        HttpContext.Session.SetInt32("NhanVienId", nv.Id);
        HttpContext.Session.SetString("TaiKhoan", nv.TaiKhoan);
        HttpContext.Session.SetString("HoTen", nv.HoTen);
        HttpContext.Session.SetString("ChucVu", nv.ChucVu);
        return RedirectToAction("Index", "TrangChu");
    }

    public IActionResult DangXuat()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }
}
