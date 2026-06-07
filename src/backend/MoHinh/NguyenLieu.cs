namespace MinhTuyetBakery.MoHinh;

public class NguyenLieu
{
    public int Id { get; set; }
    public string MaNL { get; set; } = "";
    public string TenNL { get; set; } = "";
    public string DonVi { get; set; } = "";
    public int SoLuongTon { get; set; }
    public int MucCanhBao { get; set; } = 10;
    public string TrangThai => SoLuongTon <= MucCanhBao ? "Sắp hết" : "Đủ hàng";
}
