using MinhTuyetBakery.Models;

namespace MinhTuyetBakery.ViewModels;

public class TrangChuViewModel
{
    public decimal DoanhThuHomNay { get; set; }
    public int HoaDonHomNay { get; set; }
    public int SanPhamDangBan { get; set; }
    public int NguyenLieuSapHet { get; set; }
    public List<HoaDon> HoaDonGanDay { get; set; } = new();
}
