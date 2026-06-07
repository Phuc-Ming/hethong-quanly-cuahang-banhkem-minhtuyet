using Microsoft.AspNetCore.Mvc;
using MinhTuyetBakery.MoHinh;
using MinhTuyetBakery.KhoXuLy;

namespace MinhTuyetBakery.Controllers;

public class DonHangController : Controller
{
    private readonly XuLyCuaHang _xuLy;

    public DonHangController(XuLyCuaHang xuLy)
    {
        _xuLy = xuLy;
    }

    public async Task<IActionResult> Index(string? tuKhoa, string? trangThai)
    {
        if (HttpContext.Session.GetString("TaiKhoan") == null) return RedirectToAction("Index", "DangNhap");
        ViewBag.TuKhoa = tuKhoa;
        ViewBag.TrangThai = trangThai;
        return View(await _xuLy.LayDonHangAsync(tuKhoa, trangThai));
    }

    public async Task<IActionResult> Tao()
    {
        var vm = new TaoDonHang
        {
            DanhSachSanPham = await _xuLy.LaySanPhamAsync()
        };
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Tao(TaoDonHang model)
    {
        await _xuLy.TaoDonHangAsync(model);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> CapNhatTrangThai(int id, string trangThai)
    {
        await _xuLy.CapNhatTrangThaiDonAsync(id, trangThai);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Xoa(int id)
    {
        await _xuLy.XoaDonHangAsync(id);
        return RedirectToAction("Index");
    }
}
