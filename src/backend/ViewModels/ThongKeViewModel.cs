namespace MinhTuyetBakery.ViewModels;

public class ThongKeViewModel
{
    public decimal TongDoanhThu { get; set; }
    public int SoHoaDon { get; set; }
    public string SanPhamBanChay { get; set; } = "Chưa có dữ liệu";
    public int KhachHangMoi { get; set; }
    public List<decimal> DoanhThuTuan { get; set; } = new();
}
