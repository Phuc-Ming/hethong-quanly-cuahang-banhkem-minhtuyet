using Microsoft.AspNetCore.Mvc;
using MinhTuyetBakery.MoHinh;
using MinhTuyetBakery.KhoXuLy;

namespace MinhTuyetBakery.Controllers;

public class SanPhamController : Controller
{
    private readonly XuLyCuaHang _xuLy;

    public SanPhamController(XuLyCuaHang xuLy)
    {
        _xuLy = xuLy;
    }

    public async Task<IActionResult> Index(string? tuKhoa, string? loai)
    {
        if (HttpContext.Session.GetString("TaiKhoan") == null) return RedirectToAction("Index", "DangNhap");
        ViewBag.TuKhoa = tuKhoa;
        ViewBag.Loai = loai;
        return View(await _xuLy.LaySanPhamAsync(tuKhoa, loai));
    }

    public IActionResult Them()
    {
        if (HttpContext.Session.GetString("TaiKhoan") == null) return RedirectToAction("Index", "DangNhap");
        return View(new SanPham { MaBanh = "B" + DateTime.Now.ToString("HHmmss"), TrangThai = "Đang bán" });
    }

    [HttpPost]
    public async Task<IActionResult> Them(SanPham sp)
    {
        await _xuLy.ThemSanPhamAsync(sp);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Sua(int id)
    {
        var sp = await _xuLy.LayMotSanPhamAsync(id);
        if (sp == null) return NotFound();
        return View(sp);
    }

    [HttpPost]
    public async Task<IActionResult> Sua(SanPham sp)
    {
        await _xuLy.SuaSanPhamAsync(sp);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Xoa(int id)
    {
        await _xuLy.XoaSanPhamAsync(id);
        return RedirectToAction("Index");
    }
}
