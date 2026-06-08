namespace MinhTuyetBakery.Models;

public class HoaDon
{
    public int Id { get; set; }
    public string MaHD { get; set; } = "";
    public int IDKhachHang { get; set; }
    public int IDNhanVien { get; set; }
    public DateTime NgayLap { get; set; }
    public DateTime NgayNhan { get; set; }
    public string GhiChu { get; set; } = "";
    public decimal TongTien { get; set; }
    public string TrangThai { get; set; } = "Chờ xử lý";
    public string HoTenKhachHang { get; set; } = "";
    public string SDT { get; set; } = "";
    public string HoTenNhanVien { get; set; } = "";
}
