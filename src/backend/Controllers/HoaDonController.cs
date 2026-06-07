using Microsoft.AspNetCore.Mvc;
using MinhTuyetBakery.Services;
using MinhTuyetBakery.ViewModels;

namespace MinhTuyetBakery.Controllers;

public class HoaDonController : Controller
{
    private readonly XuLyCuaHang _xuLy;
    public HoaDonController(XuLyCuaHang xuLy) => _xuLy = xuLy;

    public async Task<IActionResult> Index(string? tuKhoa, string? trangThai)
    {
        if (!DaDangNhap()) return RedirectToAction("Index", "DangNhap");
        ViewBag.TuKhoa = tuKhoa;
        ViewBag.TrangThai = trangThai;
        return View(await _xuLy.LayHoaDonAsync(tuKhoa, trangThai));
    }

    [HttpGet]
    public async Task<IActionResult> Tao()
    {
        if (!DaDangNhap()) return RedirectToAction("Index", "DangNhap");
        return View(new TaoHoaDonViewModel { DanhSachSanPham = await _xuLy.LaySanPhamAsync() });
    }

    [HttpPost]
    public async Task<IActionResult> Tao(TaoHoaDonViewModel model)
    {
        if (!DaDangNhap()) return RedirectToAction("Index", "DangNhap");
        model.DanhSachSanPham = await _xuLy.LaySanPhamAsync();
        if (string.IsNullOrWhiteSpace(model.HoTen) || string.IsNullOrWhiteSpace(model.SDT) || model.SanPhamId <= 0 || model.SoLuong <= 0)
        {
            ViewBag.Loi = "Vui lòng nhập đầy đủ thông tin hóa đơn.";
            return View(model);
        }

        try
        {
            int idNhanVien = HttpContext.Session.GetInt32("NhanVienId") ?? 1;
            await _xuLy.TaoHoaDonAsync(model, idNhanVien);
            TempData["ThongBao"] = "Tạo hóa đơn thành công.";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ViewBag.Loi = ex.Message;
            return View(model);
        }
    }

    public async Task<IActionResult> ChiTiet(string maHD)
    {
        if (!DaDangNhap()) return RedirectToAction("Index", "DangNhap");
        ViewBag.MaHD = maHD;
        return View(await _xuLy.LayChiTietHoaDonAsync(maHD));
    }

    [HttpPost]
    public async Task<IActionResult> CapNhatTrangThai(string maHD, string trangThai)
    {
        if (!DaDangNhap()) return RedirectToAction("Index", "DangNhap");
        await _xuLy.CapNhatTrangThaiHoaDonAsync(maHD, trangThai);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Xoa(string maHD)
    {
        if (!DaDangNhap()) return RedirectToAction("Index", "DangNhap");
        await _xuLy.XoaHoaDonAsync(maHD);
        return RedirectToAction("Index");
    }

    private bool DaDangNhap() => HttpContext.Session.GetString("TaiKhoan") != null;
}
