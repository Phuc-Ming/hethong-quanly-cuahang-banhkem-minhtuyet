using Microsoft.AspNetCore.Mvc;
using MinhTuyetBakery.Models;
using MinhTuyetBakery.Services;

namespace MinhTuyetBakery.Controllers;

public class NhanVienController : Controller
{
    private readonly XuLyCuaHang _xuLy;
    public NhanVienController(XuLyCuaHang xuLy) => _xuLy = xuLy;

    public async Task<IActionResult> Index(string? tuKhoa)
    {
        if (!DaDangNhap()) return RedirectToAction("Index", "DangNhap");
        ViewBag.TuKhoa = tuKhoa;
        return View(await _xuLy.LayNhanVienAsync(tuKhoa));
    }

    [HttpPost]
    public async Task<IActionResult> Them(NhanVien model)
    {
        if (!DaDangNhap()) return RedirectToAction("Index", "DangNhap");
        if (string.IsNullOrWhiteSpace(model.MaNV)) model.MaNV = "NV" + DateTime.Now.ToString("HHmmss");
        await _xuLy.ThemNhanVienAsync(model);
        return RedirectToAction("Index");
    }

    private bool DaDangNhap() => HttpContext.Session.GetString("TaiKhoan") != null;
}
