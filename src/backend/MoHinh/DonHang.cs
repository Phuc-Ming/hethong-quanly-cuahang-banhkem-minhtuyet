namespace MinhTuyetBakery.MoHinh;

public class DonHang
{
    public int Id { get; set; }
    public string MaDon { get; set; } = "";
    public int KhachHangId { get; set; }
    public string TenKhachHang { get; set; } = "";
    public string SDT { get; set; } = "";
    public DateTime NgayDat { get; set; }
    public DateTime NgayNhan { get; set; }
    public string GhiChu { get; set; } = "";
    public decimal TongTien { get; set; }
    public string TrangThai { get; set; } = "Chờ xử lý";
}

public class TaoDonHang
{
    public string TenKhachHang { get; set; } = "";
    public string SDT { get; set; } = "";
    public string DiaChi { get; set; } = "";
    public DateTime NgayNhan { get; set; } = DateTime.Today;
    public string GhiChu { get; set; } = "";
    public int SanPhamId { get; set; }
    public int SoLuong { get; set; } = 1;
    public List<SanPham> DanhSachSanPham { get; set; } = new();
}
