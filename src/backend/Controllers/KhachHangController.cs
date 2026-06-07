using Microsoft.AspNetCore.Mvc;
using MinhTuyetBakery.Models;
using MinhTuyetBakery.Services;

namespace MinhTuyetBakery.Controllers;

public class KhachHangController : Controller
{
    private readonly XuLyCuaHang _xuLy;
    public KhachHangController(XuLyCuaHang xuLy) => _xuLy = xuLy;

    public async Task<IActionResult> Index(string? tuKhoa)
    {
        if (!DaDangNhap()) return RedirectToAction("Index", "DangNhap");
        ViewBag.TuKhoa = tuKhoa;
        return View(await _xuLy.LayKhachHangAsync(tuKhoa));
    }

    [HttpPost]
    public async Task<IActionResult> Them(KhachHang model)
    {
        if (!DaDangNhap()) return RedirectToAction("Index", "DangNhap");

        if (string.IsNullOrWhiteSpace(model.HoTen) || string.IsNullOrWhiteSpace(model.SDT))
        {
            TempData["Loi"] = "Vui lòng nhập tên khách hàng và số điện thoại.";
            return RedirectToAction("Index");
        }

        try
        {
            await _xuLy.ThemKhachHangAsync(model);
            TempData["ThanhCong"] = "Thêm khách hàng thành công.";
        }
        catch (Exception ex)
        {
            TempData["Loi"] = "Không thể thêm khách hàng. " + ex.Message;
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Xoa(string maKH)
    {
        if (!DaDangNhap()) return RedirectToAction("Index", "DangNhap");

        if (string.IsNullOrWhiteSpace(maKH))
        {
            TempData["Loi"] = "Không tìm thấy mã khách hàng cần xóa.";
            return RedirectToAction("Index");
        }

        try
        {
            var loi = await _xuLy.XoaKhachHangAsync(maKH);

            if (!string.IsNullOrWhiteSpace(loi))
            {
                TempData["Loi"] = loi;
            }
            else
            {
                TempData["ThanhCong"] = "Xóa khách hàng thành công.";
            }
        }
        catch (Exception ex)
        {
            TempData["Loi"] = "Không thể xóa khách hàng. " + ex.Message;
        }

        return RedirectToAction("Index");
    }

    private bool DaDangNhap() => HttpContext.Session.GetString("TaiKhoan") != null;
}
