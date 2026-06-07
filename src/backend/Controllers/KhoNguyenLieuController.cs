using Microsoft.AspNetCore.Mvc;
using MinhTuyetBakery.MoHinh;
using MinhTuyetBakery.KhoXuLy;

namespace MinhTuyetBakery.Controllers;

public class KhoNguyenLieuController : Controller
{
    private readonly XuLyCuaHang _xuLy;

    public KhoNguyenLieuController(XuLyCuaHang xuLy)
    {
        _xuLy = xuLy;
    }

    public async Task<IActionResult> Index()
    {
        if (HttpContext.Session.GetString("TaiKhoan") == null) return RedirectToAction("Index", "DangNhap");
        return View(await _xuLy.LayNguyenLieuAsync());
    }

    public IActionResult Them()
    {
        return View(new NguyenLieu { MaNL = "NL" + DateTime.Now.ToString("HHmmss"), MucCanhBao = 10 });
    }

    [HttpPost]
    public async Task<IActionResult> Them(NguyenLieu nl)
    {
        await _xuLy.ThemNguyenLieuAsync(nl);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Sua(int id)
    {
        var nl = await _xuLy.LayMotNguyenLieuAsync(id);
        if (nl == null) return NotFound();
        return View(nl);
    }

    [HttpPost]
    public async Task<IActionResult> Sua(NguyenLieu nl)
    {
        await _xuLy.SuaNguyenLieuAsync(nl);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Xoa(int id)
    {
        await _xuLy.XoaNguyenLieuAsync(id);
        return RedirectToAction("Index");
    }
}
