using Microsoft.AspNetCore.Mvc;
using MinhTuyetBakery.Models;
using MinhTuyetBakery.Services;

namespace MinhTuyetBakery.Controllers;

public class SanPhamController : Controller
{
    private readonly XuLyCuaHang _xuLy;
    public SanPhamController(XuLyCuaHang xuLy) => _xuLy = xuLy;

    public async Task<IActionResult> Index(string? tuKhoa, string? loaiBanh)
    {
        if (!DaDangNhap()) return RedirectToAction("Index", "DangNhap");
        ViewBag.TuKhoa = tuKhoa;
        ViewBag.LoaiBanh = loaiBanh;
        return View(await _xuLy.LaySanPhamAsync(tuKhoa, loaiBanh));
    }

    [HttpGet]
    public IActionResult Them()
    {
        if (!DaDangNhap()) return RedirectToAction("Index", "DangNhap");
        return View(new SanPham { MaBanh = "B" + DateTime.Now.ToString("HHmmss") });
    }

    [HttpPost]
    public async Task<IActionResult> Them(SanPham model)
    {
        if (!DaDangNhap()) return RedirectToAction("Index", "DangNhap");
        if (!ModelState.IsValid || string.IsNullOrWhiteSpace(model.TenBanh))
        {
            ViewBag.Loi = "Vui lòng nhập đầy đủ thông tin sản phẩm.";
            return View(model);
        }
        await _xuLy.ThemSanPhamAsync(model);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Sua(int id)
    {
        if (!DaDangNhap()) return RedirectToAction("Index", "DangNhap");
        var sp = await _xuLy.LaySanPhamTheoIdAsync(id);
        return sp == null ? RedirectToAction("Index") : View(sp);
    }

    [HttpPost]
    public async Task<IActionResult> Sua(SanPham model)
    {
        if (!DaDangNhap()) return RedirectToAction("Index", "DangNhap");
        await _xuLy.CapNhatSanPhamAsync(model);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Xoa(int id)
    {
        if (!DaDangNhap()) return RedirectToAction("Index", "DangNhap");
        await _xuLy.XoaSanPhamAsync(id);
        return RedirectToAction("Index");
    }

    private bool DaDangNhap() => HttpContext.Session.GetString("TaiKhoan") != null;
}
