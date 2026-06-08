namespace MinhTuyetBakery.Models;

public class NguyenLieu
{
    public int Id { get; set; }
    public string MaNL { get; set; } = "";
    public string TenNL { get; set; } = "";
    public string DonVi { get; set; } = "";
    public int SoLuongTon { get; set; }
    public string TrangThai { get; set; } = "Đủ hàng";
}
