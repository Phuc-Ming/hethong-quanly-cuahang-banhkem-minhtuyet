using Microsoft.AspNetCore.Mvc;
using MinhTuyetBakery.Models;
using MinhTuyetBakery.Services;

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
        if (!DaDangNhap())
        {
            return RedirectToAction("Index", "DangNhap");
        }

        return View(await _xuLy.LayNguyenLieuAsync());
    }

    [HttpPost]
    public async Task<IActionResult> Them(NguyenLieu model)
    {
        if (!DaDangNhap())
        {
            return RedirectToAction("Index", "DangNhap");
        }

        if (string.IsNullOrWhiteSpace(model.MaNL))
        {
            model.MaNL = "NL" + DateTime.Now.ToString("HHmmss");
        }

        if (string.IsNullOrWhiteSpace(model.TenNL) ||
            string.IsNullOrWhiteSpace(model.DonVi))
        {
            TempData["Loi"] = "Vui lòng nhập đầy đủ tên nguyên liệu và đơn vị.";
            return RedirectToAction("Index");
        }

        if (model.SoLuongTon <= 10)
        {
            model.TrangThai = "Sắp hết";
        }
        else if (string.IsNullOrWhiteSpace(model.TrangThai))
        {
            model.TrangThai = "Đủ hàng";
        }

        await _xuLy.ThemNguyenLieuAsync(model);

        TempData["ThanhCong"] = "Thêm nguyên liệu thành công.";
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Sua(string maNL)
    {
        if (!DaDangNhap())
        {
            return RedirectToAction("Index", "DangNhap");
        }

        if (string.IsNullOrWhiteSpace(maNL))
        {
            return RedirectToAction("Index");
        }

        var nguyenLieu = await _xuLy.LayNguyenLieuTheoMaAsync(maNL);

        if (nguyenLieu == null)
        {
            TempData["Loi"] = "Không tìm thấy nguyên liệu cần sửa.";
            return RedirectToAction("Index");
        }

        return View(nguyenLieu);
    }

    [HttpPost]
    public async Task<IActionResult> Sua(NguyenLieu model)
    {
        if (!DaDangNhap())
        {
            return RedirectToAction("Index", "DangNhap");
        }

        if (string.IsNullOrWhiteSpace(model.MaNL) ||
            string.IsNullOrWhiteSpace(model.TenNL) ||
            string.IsNullOrWhiteSpace(model.DonVi))
        {
            TempData["Loi"] = "Vui lòng nhập đầy đủ mã nguyên liệu, tên nguyên liệu và đơn vị.";
            return View(model);
        }

        if (model.SoLuongTon <= 10)
        {
            model.TrangThai = "Sắp hết";
        }
        else if (string.IsNullOrWhiteSpace(model.TrangThai))
        {
            model.TrangThai = "Đủ hàng";
        }

        await _xuLy.CapNhatNguyenLieuAsync(model);

        TempData["ThanhCong"] = "Cập nhật nguyên liệu thành công.";
        return RedirectToAction("Index");
    }

    private bool DaDangNhap()
    {
        return HttpContext.Session.GetString("TaiKhoan") != null;
    }
}