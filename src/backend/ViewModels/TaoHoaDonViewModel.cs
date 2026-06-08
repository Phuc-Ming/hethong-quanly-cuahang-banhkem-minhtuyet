using MinhTuyetBakery.Models;

namespace MinhTuyetBakery.ViewModels;

public class TaoHoaDonViewModel
{
    public string HoTen { get; set; } = "";
    public string SDT { get; set; } = "";
    public string DiaChi { get; set; } = "";
    public DateTime NgayNhan { get; set; } = DateTime.Today;
    public string GhiChu { get; set; } = "";
    public int SanPhamId { get; set; }
    public int SoLuong { get; set; } = 1;
    public List<SanPham> DanhSachSanPham { get; set; } = new();
}
