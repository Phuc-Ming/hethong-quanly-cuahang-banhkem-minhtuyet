using Microsoft.AspNetCore.Mvc;
using MinhTuyetBakery.Models;
using MinhTuyetBakery.Services;

namespace MinhTuyetBakery.Controllers;

public class KhoNguyenLieuController : Controller
{
    private readonly XuLyCuaHang _xuLy;
    public KhoNguyenLieuController(XuLyCuaHang xuLy) => _xuLy = xuLy;

    public async Task<IActionResult> Index()
    {
        if (!DaDangNhap()) return RedirectToAction("Index", "DangNhap");
        return View(await _xuLy.LayNguyenLieuAsync());
    }

    [HttpPost]
    public async Task<IActionResult> Them(NguyenLieu model)
    {
        if (!DaDangNhap()) return RedirectToAction("Index", "DangNhap");
        if (string.IsNullOrWhiteSpace(model.MaNL)) model.MaNL = "NL" + DateTime.Now.ToString("HHmmss");
        await _xuLy.ThemNguyenLieuAsync(model);
        return RedirectToAction("Index");
    }

    private bool DaDangNhap() => HttpContext.Session.GetString("TaiKhoan") != null;
}
