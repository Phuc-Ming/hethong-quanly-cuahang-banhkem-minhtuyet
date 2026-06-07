namespace MinhTuyetBakery.MoHinh;

public class TrangChuThongKe
{
    public decimal DoanhThuHomNay { get; set; }
    public int DonHangHomNay { get; set; }
    public int SanPhamDangBan { get; set; }
    public int NguyenLieuSapHet { get; set; }
    public List<DonHang> DonHangGanDay { get; set; } = new();
}

public class BaoCaoThongKe
{
    public decimal TongDoanhThu { get; set; }
    public int SoDonHang { get; set; }
    public string SanPhamBanChay { get; set; } = "";
    public int KhachHangMoi { get; set; }
    public List<decimal> DoanhThuTuan { get; set; } = new();
}
