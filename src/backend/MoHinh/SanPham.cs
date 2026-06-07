namespace MinhTuyetBakery.Models;

public class SanPham
{
    public int Id { get; set; }
    public string MaBanh { get; set; } = "";
    public string TenBanh { get; set; } = "";
    public string LoaiBanh { get; set; } = "";
    public decimal GiaBan { get; set; }
    public int SoLuong { get; set; }
    public string MoTa { get; set; } = "";
    public string TrangThai { get; set; } = "Đang bán";
}
