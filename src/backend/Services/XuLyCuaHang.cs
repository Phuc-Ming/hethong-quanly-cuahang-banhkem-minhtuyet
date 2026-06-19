using MinhTuyetBakery.Data;
using MinhTuyetBakery.Models;
using MinhTuyetBakery.ViewModels;
using MySqlConnector;
using System.Data;

namespace MinhTuyetBakery.Services;

public class XuLyCuaHang
{
    private readonly KetNoiCSDL _ketNoi;

    public XuLyCuaHang(KetNoiCSDL ketNoi)
    {
        _ketNoi = ketNoi;
    }

    private static string LayChuoi(object value) => value == DBNull.Value ? "" : Convert.ToString(value) ?? "";
    private static int LaySoNguyen(object value) => value == DBNull.Value ? 0 : Convert.ToInt32(value);
    private static decimal LaySoThapPhan(object value) => value == DBNull.Value ? 0 : Convert.ToDecimal(value);
    private static DateTime LayNgay(object value) => value == DBNull.Value ? DateTime.Today : Convert.ToDateTime(value);

    public async Task<NhanVien?> DangNhapAsync(string taiKhoan, string matKhau)
    {
        await using var conn = _ketNoi.TaoKetNoi();
        await conn.OpenAsync();
        const string sql = @"select * from nhan_vien where tai_khoan = @taiKhoan and mat_khau = @matKhau and trang_thai = 'Hoạt động' limit 1";
        await using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@taiKhoan", taiKhoan);
        cmd.Parameters.AddWithValue("@matKhau", matKhau);
        await using var r = await cmd.ExecuteReaderAsync();
        return await r.ReadAsync() ? DocNhanVien(r) : null;
    }

    public async Task<List<SanPham>> LaySanPhamAsync(string? tuKhoa = null, string? loaiBanh = null)
    {
        var ds = new List<SanPham>();
        await using var conn = _ketNoi.TaoKetNoi();
        await conn.OpenAsync();
        var sql = "select * from san_pham where 1=1";
        if (!string.IsNullOrWhiteSpace(tuKhoa)) sql += " and ten_banh like @tuKhoa";
        if (!string.IsNullOrWhiteSpace(loaiBanh)) sql += " and loai_banh = @loaiBanh";
        sql += " order by id desc";
        await using var cmd = new MySqlCommand(sql, conn);
        if (!string.IsNullOrWhiteSpace(tuKhoa)) cmd.Parameters.AddWithValue("@tuKhoa", $"%{tuKhoa}%");
        if (!string.IsNullOrWhiteSpace(loaiBanh)) cmd.Parameters.AddWithValue("@loaiBanh", loaiBanh);
        await using var r = await cmd.ExecuteReaderAsync();
        while (await r.ReadAsync()) ds.Add(DocSanPham(r));
        return ds;
    }

    public async Task<SanPham?> LaySanPhamTheoIdAsync(int id)
    {
        await using var conn = _ketNoi.TaoKetNoi();
        await conn.OpenAsync();
        await using var cmd = new MySqlCommand("select * from san_pham where id=@id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        await using var r = await cmd.ExecuteReaderAsync();
        return await r.ReadAsync() ? DocSanPham(r) : null;
    }

    public async Task ThemSanPhamAsync(SanPham sp)
    {
        await using var conn = _ketNoi.TaoKetNoi();
        await conn.OpenAsync();
        const string sql = @"insert into san_pham(ma_banh, ten_banh, loai_banh, gia_ban, so_luong, trang_thai)
                             values(@ma, @ten, @loai, @gia, @sl, @trangthai)";
        await using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@ma", sp.MaBanh);
        cmd.Parameters.AddWithValue("@ten", sp.TenBanh);
        cmd.Parameters.AddWithValue("@loai", sp.LoaiBanh);
        cmd.Parameters.AddWithValue("@gia", sp.GiaBan);
        cmd.Parameters.AddWithValue("@sl", sp.SoLuong);
        cmd.Parameters.AddWithValue("@trangthai", sp.TrangThai);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task CapNhatSanPhamAsync(SanPham sp)
    {
        await using var conn = _ketNoi.TaoKetNoi();
        await conn.OpenAsync();
        const string sql = @"update san_pham set ma_banh=@ma, ten_banh=@ten, loai_banh=@loai, gia_ban=@gia, so_luong=@sl, trang_thai=@trangthai where id=@id";
        await using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", sp.Id);
        cmd.Parameters.AddWithValue("@ma", sp.MaBanh);
        cmd.Parameters.AddWithValue("@ten", sp.TenBanh);
        cmd.Parameters.AddWithValue("@loai", sp.LoaiBanh);
        cmd.Parameters.AddWithValue("@gia", sp.GiaBan);
        cmd.Parameters.AddWithValue("@sl", sp.SoLuong);
        cmd.Parameters.AddWithValue("@trangthai", sp.TrangThai);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task XoaSanPhamAsync(int id)
    {
        await using var conn = _ketNoi.TaoKetNoi();
        await conn.OpenAsync();
        await using var cmd = new MySqlCommand("update san_pham set trang_thai='Hết hàng' where id=@id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<List<KhachHang>> LayKhachHangAsync(string? tuKhoa = null)
    {
        var ds = new List<KhachHang>();
        await using var conn = _ketNoi.TaoKetNoi();
        await conn.OpenAsync();

        var sql = @"
            select kh.id, kh.ma_kh, kh.ho_ten, kh.sdt, kh.dia_chi, kh.tong_don,
                   (select count(*) from hoa_don hd where hd.id_khach_hang = kh.id) as tong_hoa_don
            from khach_hang kh
            where 1=1";

        if (!string.IsNullOrWhiteSpace(tuKhoa))
        {
            sql += " and (kh.ho_ten like @tuKhoa or kh.sdt like @tuKhoa or kh.ma_kh like @tuKhoa)";
        }

        sql += " order by kh.id desc";

        await using var cmd = new MySqlCommand(sql, conn);

        if (!string.IsNullOrWhiteSpace(tuKhoa))
        {
            cmd.Parameters.AddWithValue("@tuKhoa", $"%{tuKhoa}%");
        }

        await using var r = await cmd.ExecuteReaderAsync();

        while (await r.ReadAsync())
        {
            ds.Add(new KhachHang
            {
                Id = LaySoNguyen(r["id"]),
                MaKH = LayChuoi(r["ma_kh"]),
                HoTen = LayChuoi(r["ho_ten"]),
                SDT = LayChuoi(r["sdt"]),
                DiaChi = LayChuoi(r["dia_chi"]),
                TongDon = LaySoNguyen(r["tong_hoa_don"])
            });
        }

        return ds;
    }

    public async Task ThemKhachHangAsync(KhachHang kh)
    {
        await using var conn = _ketNoi.TaoKetNoi();
        await conn.OpenAsync();

        if (string.IsNullOrWhiteSpace(kh.MaKH))
        {
            kh.MaKH = await TaoMaKhachHangMoiAsync(conn);
        }

        const string sql = @"
            insert into khach_hang(ma_kh, ho_ten, sdt, dia_chi, tong_don)
            values(@ma, @ten, @sdt, @diachi, 0)";

        await using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@ma", kh.MaKH.Trim());
        cmd.Parameters.AddWithValue("@ten", kh.HoTen.Trim());
        cmd.Parameters.AddWithValue("@sdt", kh.SDT.Trim());
        cmd.Parameters.AddWithValue("@diachi", kh.DiaChi?.Trim() ?? "");
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<List<NhanVien>> LayNhanVienAsync(string? tuKhoa = null)
    {
        var ds = new List<NhanVien>();
        await using var conn = _ketNoi.TaoKetNoi();
        await conn.OpenAsync();
        var sql = "select * from nhan_vien where 1=1";
        if (!string.IsNullOrWhiteSpace(tuKhoa)) sql += " and (ho_ten like @tuKhoa or tai_khoan like @tuKhoa or ma_nv like @tuKhoa)";
        sql += " order by id";
        await using var cmd = new MySqlCommand(sql, conn);
        if (!string.IsNullOrWhiteSpace(tuKhoa)) cmd.Parameters.AddWithValue("@tuKhoa", $"%{tuKhoa}%");
        await using var r = await cmd.ExecuteReaderAsync();
        while (await r.ReadAsync()) ds.Add(DocNhanVien(r));
        return ds;
    }

    public async Task ThemNhanVienAsync(NhanVien nv)
    {
        await using var conn = _ketNoi.TaoKetNoi();
        await conn.OpenAsync();
        const string sql = "insert into nhan_vien(ma_nv, ho_ten, sdt, chuc_vu, tai_khoan, mat_khau, trang_thai) values(@ma,@ten,@sdt,@chucvu,@tk,@mk,@tt)";
        await using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@ma", nv.MaNV);
        cmd.Parameters.AddWithValue("@ten", nv.HoTen);
        cmd.Parameters.AddWithValue("@sdt", nv.SDT);
        cmd.Parameters.AddWithValue("@chucvu", nv.ChucVu);
        cmd.Parameters.AddWithValue("@tk", nv.TaiKhoan);
        cmd.Parameters.AddWithValue("@mk", nv.MatKhau);
        cmd.Parameters.AddWithValue("@tt", nv.TrangThai);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<string> TaoMaNhanVienMoiAsync()
    {
        await using var conn = _ketNoi.TaoKetNoi();
        await conn.OpenAsync();

        const string sql = @"
            select ma_nv
            from nhan_vien
            where ma_nv like 'NV%'
            order by cast(substring(ma_nv, 3) as unsigned) desc
            limit 1";

        await using var cmd = new MySqlCommand(sql, conn);
        var ketQua = await cmd.ExecuteScalarAsync();

        if (ketQua == null || ketQua == DBNull.Value)
        {
            return "NV001";
        }

        string maCu = ketQua.ToString() ?? "NV000";
        string phanSo = maCu.Replace("NV", "");

        if (!int.TryParse(phanSo, out int so))
        {
            so = 0;
        }

        int soMoi = so + 1;

        return "NV" + soMoi.ToString("D3");
    }

    public async Task<NhanVien?> LayNhanVienTheoMaAsync(string maNV)
    {
        await using var conn = _ketNoi.TaoKetNoi();
        await conn.OpenAsync();

        const string sql = @"
            select *
            from nhan_vien
            where ma_nv = @MaNV
            limit 1";

        await using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@MaNV", maNV);

        await using var r = await cmd.ExecuteReaderAsync();

        return await r.ReadAsync() ? DocNhanVien(r) : null;
    }

    public async Task CapNhatNhanVienAsync(NhanVien nv)
    {
        await using var conn = _ketNoi.TaoKetNoi();
        await conn.OpenAsync();

        const string sql = @"
            update nhan_vien
            set 
                ho_ten = @ten,
                sdt = @sdt,
                chuc_vu = @chucvu,
                tai_khoan = @tk,
                mat_khau = @mk,
                trang_thai = @tt
            where ma_nv = @ma";

        await using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@ma", nv.MaNV);
        cmd.Parameters.AddWithValue("@ten", nv.HoTen);
        cmd.Parameters.AddWithValue("@sdt", nv.SDT ?? "");
        cmd.Parameters.AddWithValue("@chucvu", nv.ChucVu ?? "Nhân viên");
        cmd.Parameters.AddWithValue("@tk", nv.TaiKhoan);
        cmd.Parameters.AddWithValue("@mk", nv.MatKhau ?? "123456");
        cmd.Parameters.AddWithValue("@tt", nv.TrangThai ?? "Hoạt động");

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<List<NguyenLieu>> LayNguyenLieuAsync()
    {
        var ds = new List<NguyenLieu>();
        await using var conn = _ketNoi.TaoKetNoi();
        await conn.OpenAsync();
        await using var cmd = new MySqlCommand("select * from nguyen_lieu order by id", conn);
        await using var r = await cmd.ExecuteReaderAsync();
        while (await r.ReadAsync()) ds.Add(DocNguyenLieu(r));
        return ds;
    }

    public async Task ThemNguyenLieuAsync(NguyenLieu nl)
    {
        await using var conn = _ketNoi.TaoKetNoi();
        await conn.OpenAsync();
        const string sql = "insert into nguyen_lieu(ma_nl, ten_nl, don_vi, so_luong_ton, trang_thai) values(@ma,@ten,@dv,@sl,@tt)";
        await using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@ma", nl.MaNL);
        cmd.Parameters.AddWithValue("@ten", nl.TenNL);
        cmd.Parameters.AddWithValue("@dv", nl.DonVi);
        cmd.Parameters.AddWithValue("@sl", nl.SoLuongTon);
        cmd.Parameters.AddWithValue("@tt", nl.SoLuongTon <= 10 ? "Sắp hết" : nl.TrangThai);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<NguyenLieu?> LayNguyenLieuTheoMaAsync(string maNL)
    {
        await using var conn = _ketNoi.TaoKetNoi();
        await conn.OpenAsync();

        const string sql = @"
            select *
            from nguyen_lieu
            where ma_nl = @MaNL
            limit 1";

        await using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@MaNL", maNL);

        await using var r = await cmd.ExecuteReaderAsync();

        return await r.ReadAsync() ? DocNguyenLieu(r) : null;
    }

    public async Task CapNhatNguyenLieuAsync(NguyenLieu nl)
    {
        await using var conn = _ketNoi.TaoKetNoi();
        await conn.OpenAsync();

        if (nl.SoLuongTon <= 10)
        {
            nl.TrangThai = "Sắp hết";
        }
        else if (string.IsNullOrWhiteSpace(nl.TrangThai))
        {
            nl.TrangThai = "Đủ hàng";
        }

        const string sql = @"
            update nguyen_lieu
            set 
                ten_nl = @ten,
                don_vi = @dv,
                so_luong_ton = @sl,
                trang_thai = @tt
            where ma_nl = @ma";

        await using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@ma", nl.MaNL);
        cmd.Parameters.AddWithValue("@ten", nl.TenNL);
        cmd.Parameters.AddWithValue("@dv", nl.DonVi);
        cmd.Parameters.AddWithValue("@sl", nl.SoLuongTon);
        cmd.Parameters.AddWithValue("@tt", nl.TrangThai);

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<List<HoaDon>> LayHoaDonAsync(string? tuKhoa = null, string? trangThai = null)
    {
        var ds = new List<HoaDon>();
        await using var conn = _ketNoi.TaoKetNoi();
        await conn.OpenAsync();
        var sql = @"select hd.*, kh.ho_ten as ho_ten_khach_hang, kh.sdt, nv.ho_ten as ho_ten_nhan_vien
                    from hoa_don hd
                    join khach_hang kh on hd.id_khach_hang = kh.id
                    join nhan_vien nv on hd.id_nhan_vien = nv.id
                    where 1=1";
        if (!string.IsNullOrWhiteSpace(tuKhoa)) sql += " and (hd.ma_hd like @tuKhoa or kh.ho_ten like @tuKhoa or kh.sdt like @tuKhoa)";
        if (!string.IsNullOrWhiteSpace(trangThai)) sql += " and hd.trang_thai = @trangThai";
        sql += " order by hd.id desc";
        await using var cmd = new MySqlCommand(sql, conn);
        if (!string.IsNullOrWhiteSpace(tuKhoa)) cmd.Parameters.AddWithValue("@tuKhoa", $"%{tuKhoa}%");
        if (!string.IsNullOrWhiteSpace(trangThai)) cmd.Parameters.AddWithValue("@trangThai", trangThai);
        await using var r = await cmd.ExecuteReaderAsync();
        while (await r.ReadAsync()) ds.Add(DocHoaDon(r));
        return ds;
    }

    public async Task<List<ChiTietHoaDon>> LayChiTietHoaDonAsync(string maHD)
    {
        var ds = new List<ChiTietHoaDon>();
        await using var conn = _ketNoi.TaoKetNoi();
        await conn.OpenAsync();
        const string sql = @"select ct.*, sp.ten_banh from chi_tiet_hoa_don ct join san_pham sp on ct.ma_banh = sp.ma_banh where ct.ma_hd=@maHD";
        await using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@maHD", maHD);
        await using var r = await cmd.ExecuteReaderAsync();
        while (await r.ReadAsync())
        {
            ds.Add(new ChiTietHoaDon
            {
                MaCTHD = LaySoNguyen(r["ma_cthd"]),
                MaHD = LayChuoi(r["ma_hd"]),
                MaBanh = LayChuoi(r["ma_banh"]),
                TenBanh = LayChuoi(r["ten_banh"]),
                SoLuong = LaySoNguyen(r["so_luong"]),
                DonGia = LaySoThapPhan(r["don_gia"]),
                ThanhTien = LaySoThapPhan(r["thanh_tien"])
            });
        }
        return ds;
    }

    public async Task TaoHoaDonAsync(TaoHoaDonViewModel model, int idNhanVien)
    {
        await using var conn = _ketNoi.TaoKetNoi();
        await conn.OpenAsync();
        await using var tran = await conn.BeginTransactionAsync();
        try
        {
            int idKhachHang = await TimHoacThemKhachHangAsync(conn, tran, model);
            SanPham? sp = await LaySanPhamTheoIdTrongTranAsync(conn, tran, model.SanPhamId);
            if (sp == null) throw new InvalidOperationException("Không tìm thấy sản phẩm.");
            if (model.SoLuong <= 0) throw new InvalidOperationException("Số lượng phải lớn hơn 0.");
            if (sp.SoLuong < model.SoLuong) throw new InvalidOperationException("Số lượng sản phẩm trong kho không đủ.");

            string maHD = await TaoMaHoaDonMoiAsync(conn, tran);
            decimal thanhTien = sp.GiaBan * model.SoLuong;
            const string sqlHD = @"insert into hoa_don(ma_hd, id_khach_hang, id_nhan_vien, ngay_lap, ngay_nhan, ghi_chu, tong_tien, trang_thai)
                                  values(@ma,@kh,@nv,now(),@ngayNhan,@ghiChu,@tongTien,'Chờ xử lý')";
            await using (var cmd = new MySqlCommand(sqlHD, conn, tran))
            {
                cmd.Parameters.AddWithValue("@ma", maHD);
                cmd.Parameters.AddWithValue("@kh", idKhachHang);
                cmd.Parameters.AddWithValue("@nv", idNhanVien);
                cmd.Parameters.AddWithValue("@ngayNhan", model.NgayNhan.Date);
                cmd.Parameters.AddWithValue("@ghiChu", model.GhiChu ?? "");
                cmd.Parameters.AddWithValue("@tongTien", thanhTien);
                await cmd.ExecuteNonQueryAsync();
            }

            const string sqlCT = @"insert into chi_tiet_hoa_don(ma_hd, ma_banh, so_luong, don_gia, thanh_tien)
                                  values(@maHD,@maBanh,@sl,@gia,@tt)";
            await using (var cmd = new MySqlCommand(sqlCT, conn, tran))
            {
                cmd.Parameters.AddWithValue("@maHD", maHD);
                cmd.Parameters.AddWithValue("@maBanh", sp.MaBanh);
                cmd.Parameters.AddWithValue("@sl", model.SoLuong);
                cmd.Parameters.AddWithValue("@gia", sp.GiaBan);
                cmd.Parameters.AddWithValue("@tt", thanhTien);
                await cmd.ExecuteNonQueryAsync();
            }

            await using (var cmd = new MySqlCommand("update san_pham set so_luong = so_luong - @sl, trang_thai = if(so_luong - @sl <= 0, 'Hết hàng', trang_thai) where id=@id", conn, tran))
            {
                cmd.Parameters.AddWithValue("@sl", model.SoLuong);
                cmd.Parameters.AddWithValue("@id", sp.Id);
                await cmd.ExecuteNonQueryAsync();
            }

            await using (var cmd = new MySqlCommand("update khach_hang set tong_don = tong_don + 1 where id=@id", conn, tran))
            {
                cmd.Parameters.AddWithValue("@id", idKhachHang);
                await cmd.ExecuteNonQueryAsync();
            }

            await tran.CommitAsync();
        }
        catch
        {
            await tran.RollbackAsync();
            throw;
        }
    }

    public async Task CapNhatTrangThaiHoaDonAsync(string maHD, string trangThai)
    {
        await using var conn = _ketNoi.TaoKetNoi();
        await conn.OpenAsync();
        await using var cmd = new MySqlCommand("update hoa_don set trang_thai=@tt where ma_hd=@ma", conn);
        cmd.Parameters.AddWithValue("@tt", trangThai);
        cmd.Parameters.AddWithValue("@ma", maHD);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task XoaHoaDonAsync(string maHD)
    {
        await using var conn = _ketNoi.TaoKetNoi();
        await conn.OpenAsync();
        await using var tran = await conn.BeginTransactionAsync();

        try
        {
            int idKhachHang = 0;

            await using (var cmd = new MySqlCommand("select id_khach_hang from hoa_don where ma_hd=@ma limit 1", conn, tran))
            {
                cmd.Parameters.AddWithValue("@ma", maHD);
                var value = await cmd.ExecuteScalarAsync();
                if (value != null && value != DBNull.Value)
                {
                    idKhachHang = Convert.ToInt32(value);
                }
            }

            await using (var cmd = new MySqlCommand(@"
                update san_pham sp
                inner join chi_tiet_hoa_don ct on sp.ma_banh = ct.ma_banh
                set sp.so_luong = sp.so_luong + ct.so_luong,
                    sp.trang_thai = 'Đang bán'
                where ct.ma_hd = @ma", conn, tran))
            {
                cmd.Parameters.AddWithValue("@ma", maHD);
                await cmd.ExecuteNonQueryAsync();
            }

            await using (var cmd = new MySqlCommand("delete from chi_tiet_hoa_don where ma_hd=@ma", conn, tran))
            {
                cmd.Parameters.AddWithValue("@ma", maHD);
                await cmd.ExecuteNonQueryAsync();
            }

            await using (var cmd = new MySqlCommand("delete from hoa_don where ma_hd=@ma", conn, tran))
            {
                cmd.Parameters.AddWithValue("@ma", maHD);
                await cmd.ExecuteNonQueryAsync();
            }

            if (idKhachHang > 0)
            {
                await using var cmd = new MySqlCommand(@"
                    update khach_hang
                    set tong_don = greatest(tong_don - 1, 0)
                    where id = @id", conn, tran);
                cmd.Parameters.AddWithValue("@id", idKhachHang);
                await cmd.ExecuteNonQueryAsync();
            }

            await tran.CommitAsync();
        }
        catch
        {
            await tran.RollbackAsync();
            throw;
        }
    }

    public async Task<TrangChuViewModel> LayTrangChuAsync()
    {
        var vm = new TrangChuViewModel();
        await using var conn = _ketNoi.TaoKetNoi();
        await conn.OpenAsync();
        vm.DoanhThuHomNay = await LayDecimalAsync(conn, "select coalesce(sum(tong_tien),0) from hoa_don where date(ngay_lap)=curdate() and trang_thai <> 'Đã hủy'");
        vm.HoaDonHomNay = await LayIntAsync(conn, "select count(*) from hoa_don where date(ngay_lap)=curdate()");
        vm.SanPhamDangBan = await LayIntAsync(conn, "select count(*) from san_pham where trang_thai='Đang bán'");
        vm.NguyenLieuSapHet = await LayIntAsync(conn, "select count(*) from nguyen_lieu where trang_thai='Sắp hết'");
        vm.HoaDonGanDay = (await LayHoaDonAsync()).Take(5).ToList();
        return vm;
    }

    public async Task<ThongKeViewModel> LayThongKeAsync()
    {
        var vm = new ThongKeViewModel();
        await using var conn = _ketNoi.TaoKetNoi();
        await conn.OpenAsync();
        vm.TongDoanhThu = await LayDecimalAsync(conn, "select coalesce(sum(tong_tien),0) from hoa_don where trang_thai <> 'Đã hủy'");
        vm.SoHoaDon = await LayIntAsync(conn, "select count(*) from hoa_don");
        vm.KhachHangMoi = await LayIntAsync(conn, "select count(*) from khach_hang");
        vm.SanPhamBanChay = await LayStringAsync(conn, @"select sp.ten_banh from chi_tiet_hoa_don ct join san_pham sp on ct.ma_banh=sp.ma_banh group by sp.ten_banh order by sum(ct.so_luong) desc limit 1") ?? "Chưa có dữ liệu";
        for (int i = 4; i >= 0; i--)
        {
            await using var cmd = new MySqlCommand("select coalesce(sum(tong_tien),0) from hoa_don where date(ngay_lap)=date_sub(curdate(), interval @soNgay day) and trang_thai <> 'Đã hủy'", conn);
            cmd.Parameters.AddWithValue("@soNgay", i);
            var value = await cmd.ExecuteScalarAsync();
            vm.DoanhThuTuan.Add(value == null || value == DBNull.Value ? 0 : Convert.ToDecimal(value));
        }
        return vm;
    }

    private async Task<int> TimHoacThemKhachHangAsync(MySqlConnection conn, MySqlTransaction tran, TaoHoaDonViewModel model)
    {
        await using (var cmd = new MySqlCommand("select id from khach_hang where sdt=@sdt limit 1", conn, tran))
        {
            cmd.Parameters.AddWithValue("@sdt", model.SDT);
            var found = await cmd.ExecuteScalarAsync();
            if (found != null && found != DBNull.Value) return Convert.ToInt32(found);
        }

        string maKH = await TaoMaKhachHangMoiAsync(conn, tran);
        await using (var cmd = new MySqlCommand("insert into khach_hang(ma_kh, ho_ten, sdt, dia_chi, tong_don) values(@ma,@ten,@sdt,@dc,0); select last_insert_id();", conn, tran))
        {
            cmd.Parameters.AddWithValue("@ma", maKH);
            cmd.Parameters.AddWithValue("@ten", model.HoTen);
            cmd.Parameters.AddWithValue("@sdt", model.SDT);
            cmd.Parameters.AddWithValue("@dc", model.DiaChi ?? "");
            return Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }
    }

    private async Task<SanPham?> LaySanPhamTheoIdTrongTranAsync(MySqlConnection conn, MySqlTransaction tran, int id)
    {
        await using var cmd = new MySqlCommand("select * from san_pham where id=@id for update", conn, tran);
        cmd.Parameters.AddWithValue("@id", id);
        await using var r = await cmd.ExecuteReaderAsync();
        return await r.ReadAsync() ? DocSanPham(r) : null;
    }

    private static async Task<int> LayIntAsync(MySqlConnection conn, string sql)
    {
        await using var cmd = new MySqlCommand(sql, conn);
        var value = await cmd.ExecuteScalarAsync();
        return value == null || value == DBNull.Value ? 0 : Convert.ToInt32(value);
    }

    private static async Task<decimal> LayDecimalAsync(MySqlConnection conn, string sql)
    {
        await using var cmd = new MySqlCommand(sql, conn);
        var value = await cmd.ExecuteScalarAsync();
        return value == null || value == DBNull.Value ? 0 : Convert.ToDecimal(value);
    }

    private static async Task<string?> LayStringAsync(MySqlConnection conn, string sql)
    {
        await using var cmd = new MySqlCommand(sql, conn);
        var value = await cmd.ExecuteScalarAsync();
        return value == null || value == DBNull.Value ? null : Convert.ToString(value);
    }

    private static SanPham DocSanPham(MySqlDataReader r) => new()
    {
        Id = LaySoNguyen(r["id"]),
        MaBanh = LayChuoi(r["ma_banh"]),
        TenBanh = LayChuoi(r["ten_banh"]),
        LoaiBanh = LayChuoi(r["loai_banh"]),
        GiaBan = LaySoThapPhan(r["gia_ban"]),
        SoLuong = LaySoNguyen(r["so_luong"]),
        TrangThai = LayChuoi(r["trang_thai"])
    };

    private static KhachHang DocKhachHang(MySqlDataReader r) => new()
    {
        Id = LaySoNguyen(r["id"]),
        MaKH = LayChuoi(r["ma_kh"]),
        HoTen = LayChuoi(r["ho_ten"]),
        SDT = LayChuoi(r["sdt"]),
        DiaChi = LayChuoi(r["dia_chi"]),
        TongDon = LaySoNguyen(r["tong_don"])
    };

    private static NhanVien DocNhanVien(MySqlDataReader r) => new()
    {
        Id = LaySoNguyen(r["id"]),
        MaNV = LayChuoi(r["ma_nv"]),
        HoTen = LayChuoi(r["ho_ten"]),
        SDT = LayChuoi(r["sdt"]),
        ChucVu = LayChuoi(r["chuc_vu"]),
        TaiKhoan = LayChuoi(r["tai_khoan"]),
        MatKhau = LayChuoi(r["mat_khau"]),
        TrangThai = LayChuoi(r["trang_thai"])
    };

    private static NguyenLieu DocNguyenLieu(MySqlDataReader r) => new()
    {
        Id = LaySoNguyen(r["id"]),
        MaNL = LayChuoi(r["ma_nl"]),
        TenNL = LayChuoi(r["ten_nl"]),
        DonVi = LayChuoi(r["don_vi"]),
        SoLuongTon = LaySoNguyen(r["so_luong_ton"]),
        TrangThai = LayChuoi(r["trang_thai"])
    };

    private static HoaDon DocHoaDon(MySqlDataReader r) => new()
    {
        Id = LaySoNguyen(r["id"]),
        MaHD = LayChuoi(r["ma_hd"]),
        IDKhachHang = LaySoNguyen(r["id_khach_hang"]),
        IDNhanVien = LaySoNguyen(r["id_nhan_vien"]),
        NgayLap = LayNgay(r["ngay_lap"]),
        NgayNhan = LayNgay(r["ngay_nhan"]),
        GhiChu = LayChuoi(r["ghi_chu"]),
        TongTien = LaySoThapPhan(r["tong_tien"]),
        TrangThai = LayChuoi(r["trang_thai"]),
        HoTenKhachHang = LayChuoi(r["ho_ten_khach_hang"]),
        SDT = LayChuoi(r["sdt"]),
        HoTenNhanVien = LayChuoi(r["ho_ten_nhan_vien"])
    };

    private async Task<string> TaoMaHoaDonMoiAsync(MySqlConnection conn, MySqlTransaction tran)
    {
        const string sql = @"
            select ma_hd
            from hoa_don
            where ma_hd like 'HD%'
            order by cast(substring(ma_hd, 3) as unsigned) desc
            limit 1";

        await using var cmd = new MySqlCommand(sql, conn, tran);
        var ketQua = await cmd.ExecuteScalarAsync();

        if (ketQua == null || ketQua == DBNull.Value)
        {
            return "HD001";
        }

        string maCu = ketQua.ToString() ?? "HD000";
        string phanSo = maCu.Replace("HD", "");

        if (!int.TryParse(phanSo, out int so))
        {
            so = 0;
        }

        int soMoi = so + 1;

        return "HD" + soMoi.ToString("D3");
    }

    private async Task<string> TaoMaKhachHangMoiAsync(MySqlConnection conn, MySqlTransaction? tran = null)
    {
        const string sql = @"
            select ma_kh
            from khach_hang
            where ma_kh like 'KH%'
            order by cast(substring(ma_kh, 3) as unsigned) desc
            limit 1";

        await using var cmd = new MySqlCommand(sql, conn, tran);
        var ketQua = await cmd.ExecuteScalarAsync();

        if (ketQua == null || ketQua == DBNull.Value)
        {
            return "KH001";
        }

        string maCu = ketQua.ToString() ?? "KH000";
        string phanSo = maCu.Replace("KH", "");

        if (!int.TryParse(phanSo, out int so))
        {
            so = 0;
        }

        int soMoi = so + 1;

        return "KH" + soMoi.ToString("D3");
    }

    public async Task<string?> XoaKhachHangAsync(string maKH)
    {
        await using var conn = _ketNoi.TaoKetNoi();
        await conn.OpenAsync();

        const string sqlKiemTra = @"
            select count(*)
            from hoa_don hd
            inner join khach_hang kh on hd.id_khach_hang = kh.id
            where kh.ma_kh = @MaKH";

        await using (var cmdKiemTra = new MySqlCommand(sqlKiemTra, conn))
        {
            cmdKiemTra.Parameters.AddWithValue("@MaKH", maKH);
            int soHoaDon = Convert.ToInt32(await cmdKiemTra.ExecuteScalarAsync());

            if (soHoaDon > 0)
            {
                return "Không thể xóa khách hàng này vì đã có hóa đơn trong hệ thống.";
            }
        }

        const string sqlXoa = @"
            delete from khach_hang
            where ma_kh = @MaKH";

        await using var cmdXoa = new MySqlCommand(sqlXoa, conn);
        cmdXoa.Parameters.AddWithValue("@MaKH", maKH);

        int soDong = await cmdXoa.ExecuteNonQueryAsync();

        if (soDong == 0)
        {
            return "Không tìm thấy khách hàng cần xóa.";
        }

        return null;
    }
}
