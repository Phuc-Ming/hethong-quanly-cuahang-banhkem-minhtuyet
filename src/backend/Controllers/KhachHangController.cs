using Microsoft.AspNetCore.Mvc;
using MinhTuyetBakery.MoHinh;
using MinhTuyetBakery.KhoXuLy;

namespace MinhTuyetBakery.Controllers;

public class KhachHangController : Controller
{
    private readonly XuLyCuaHang _xuLy;

    public KhachHangController(XuLyCuaHang xuLy)
    {
        _xuLy = xuLy;
    }

    public async Task<IActionResult> Index(string? tuKhoa)
    {
        if (HttpContext.Session.GetString("TaiKhoan") == null) return RedirectToAction("Index", "DangNhap");
        ViewBag.TuKhoa = tuKhoa;
        return View(await _xuLy.LayKhachHangAsync(tuKhoa));
    }

    public IActionResult Them()
    {
        return View(new KhachHang { MaKH = "KH" + DateTime.Now.ToString("HHmmss") });
    }

    [HttpPost]
    public async Task<IActionResult> Them(KhachHang kh)
    {
        await _xuLy.ThemKhachHangAsync(kh);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Sua(int id)
    {
        var kh = await _xuLy.LayMotKhachHangAsync(id);
        if (kh == null) return NotFound();
        return View(kh);
    }

    [HttpPost]
    public async Task<IActionResult> Sua(KhachHang kh)
    {
        await _xuLy.SuaKhachHangAsync(kh);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Xoa(int id)
    {
        await _xuLy.XoaKhachHangAsync(id);
        return RedirectToAction("Index");
    }
}
