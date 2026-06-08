namespace MinhTuyetBakery.Models;

public class KhachHang
{
    public int Id { get; set; }
    public string MaKH { get; set; } = "";
    public string HoTen { get; set; } = "";
    public string SDT { get; set; } = "";
    public string DiaChi { get; set; } = "";
    public int TongDon { get; set; }
}
