namespace MinhTuyetBakery.Models;

public class ChiTietHoaDon
{
    public int MaCTHD { get; set; }
    public string MaHD { get; set; } = "";
    public string MaBanh { get; set; } = "";
    public string TenBanh { get; set; } = "";
    public int SoLuong { get; set; }
    public decimal DonGia { get; set; }
    public decimal ThanhTien { get; set; }
}
