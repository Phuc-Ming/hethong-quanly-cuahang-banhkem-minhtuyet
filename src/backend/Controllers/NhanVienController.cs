using Microsoft.AspNetCore.Mvc;
using MinhTuyetBakery.MoHinh;
using MinhTuyetBakery.KhoXuLy;

namespace MinhTuyetBakery.Controllers;

public class NhanVienController : Controller
{
    private readonly XuLyCuaHang _xuLy;

    public NhanVienController(XuLyCuaHang xuLy)
    {
        _xuLy = xuLy;
    }

    public async Task<IActionResult> Index(string? tuKhoa)
    {
        if (HttpContext.Session.GetString("TaiKhoan") == null) return RedirectToAction("Index", "DangNhap");
        ViewBag.TuKhoa = tuKhoa;
        return View(await _xuLy.LayNhanVienAsync(tuKhoa));
    }

    public IActionResult Them()
    {
        return View(new NhanVien { MaNV = "NV" + DateTime.Now.ToString("HHmmss"), MatKhau = "123456", TrangThai = "Hoạt động" });
    }

    [HttpPost]
    public async Task<IActionResult> Them(NhanVien nv)
    {
        await _xuLy.ThemNhanVienAsync(nv);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Sua(int id)
    {
        var nv = await _xuLy.LayMotNhanVienAsync(id);
        if (nv == null) return NotFound();
        return View(nv);
    }

    [HttpPost]
    public async Task<IActionResult> Sua(NhanVien nv)
    {
        await _xuLy.SuaNhanVienAsync(nv);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Xoa(int id)
    {
        await _xuLy.XoaNhanVienAsync(id);
        return RedirectToAction("Index");
    }
}
