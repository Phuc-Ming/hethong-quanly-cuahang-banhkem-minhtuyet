using Microsoft.AspNetCore.Mvc;
using MinhTuyetBakery.Models;
using MinhTuyetBakery.Services;

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
        if (!DaDangNhap())
        {
            return RedirectToAction("Index", "DangNhap");
        }

        ViewBag.TuKhoa = tuKhoa;
        return View(await _xuLy.LayNhanVienAsync(tuKhoa));
    }

    [HttpPost]
    public async Task<IActionResult> Them(NhanVien model)
    {
        if (!DaDangNhap())
        {
            return RedirectToAction("Index", "DangNhap");
        }

        if (string.IsNullOrWhiteSpace(model.MaNV))
        {
            model.MaNV = await _xuLy.TaoMaNhanVienMoiAsync();
        }

        if (string.IsNullOrWhiteSpace(model.HoTen) ||
            string.IsNullOrWhiteSpace(model.TaiKhoan) ||
            string.IsNullOrWhiteSpace(model.MatKhau))
        {
            TempData["Loi"] = "Vui lòng nhập đầy đủ họ tên, tài khoản và mật khẩu.";
            return RedirectToAction("Index");
        }

        if (string.IsNullOrWhiteSpace(model.SDT))
        {
            model.SDT = "";
        }

        if (string.IsNullOrWhiteSpace(model.ChucVu))
        {
            model.ChucVu = "Nhân viên";
        }

        if (string.IsNullOrWhiteSpace(model.TrangThai))
        {
            model.TrangThai = "Hoạt động";
        }

        await _xuLy.ThemNhanVienAsync(model);

        TempData["ThanhCong"] = "Thêm nhân viên thành công.";
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Sua(string maNV)
    {
        if (!DaDangNhap())
        {
            return RedirectToAction("Index", "DangNhap");
        }

        if (string.IsNullOrWhiteSpace(maNV))
        {
            return RedirectToAction("Index");
        }

        var nhanVien = await _xuLy.LayNhanVienTheoMaAsync(maNV);

        if (nhanVien == null)
        {
            TempData["Loi"] = "Không tìm thấy nhân viên cần sửa.";
            return RedirectToAction("Index");
        }

        return View(nhanVien);
    }

    [HttpPost]
    public async Task<IActionResult> Sua(NhanVien model)
    {
        if (!DaDangNhap())
        {
            return RedirectToAction("Index", "DangNhap");
        }

        if (string.IsNullOrWhiteSpace(model.MaNV) ||
            string.IsNullOrWhiteSpace(model.HoTen) ||
            string.IsNullOrWhiteSpace(model.TaiKhoan))
        {
            TempData["Loi"] = "Vui lòng nhập đầy đủ mã nhân viên, họ tên và tài khoản.";
            return View(model);
        }

        if (string.IsNullOrWhiteSpace(model.SDT))
        {
            model.SDT = "";
        }

        if (string.IsNullOrWhiteSpace(model.ChucVu))
        {
            model.ChucVu = "Nhân viên";
        }

        if (string.IsNullOrWhiteSpace(model.MatKhau))
        {
            model.MatKhau = "123456";
        }

        if (string.IsNullOrWhiteSpace(model.TrangThai))
        {
            model.TrangThai = "Hoạt động";
        }

        await _xuLy.CapNhatNhanVienAsync(model);

        TempData["ThanhCong"] = "Cập nhật thông tin nhân viên thành công.";
        return RedirectToAction("Index");
    }

    private bool DaDangNhap()
    {
        return HttpContext.Session.GetString("TaiKhoan") != null;
    }
}