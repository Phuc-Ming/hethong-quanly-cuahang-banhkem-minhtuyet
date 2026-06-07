using Dapper;
using MinhTuyetBakery.DuLieu;
using MinhTuyetBakery.MoHinh;

namespace MinhTuyetBakery.KhoXuLy;

public class XuLyCuaHang
{
    private readonly KetNoiCSDL _db;

    public XuLyCuaHang(KetNoiCSDL db)
    {
        _db = db;
    }

    public async Task<NhanVien?> DangNhapAsync(string taiKhoan, string matKhau)
    {
        using var conn = _db.TaoKetNoi();
        return await conn.QueryFirstOrDefaultAsync<NhanVien>(
            @"select id, ma_nv MaNV, ho_ten HoTen, sdt SDT, chuc_vu ChucVu, tai_khoan TaiKhoan, mat_khau MatKhau, trang_thai TrangThai
              from nhan_vien
              where tai_khoan=@taiKhoan and mat_khau=@matKhau and trang_thai='Hoạt động'",
            new { taiKhoan, matKhau });
    }

    public async Task<TrangChuThongKe> LayTrangChuAsync()
    {
        using var conn = _db.TaoKetNoi();
        var vm = new TrangChuThongKe();

        vm.DoanhThuHomNay = await conn.ExecuteScalarAsync<decimal>("select ifnull(sum(tong_tien),0) from don_hang where date(ngay_dat)=curdate()");
        vm.DonHangHomNay = await conn.ExecuteScalarAsync<int>("select count(*) from don_hang where date(ngay_dat)=curdate()");
        vm.SanPhamDangBan = await conn.ExecuteScalarAsync<int>("select count(*) from san_pham where trang_thai='Đang bán'");
        vm.NguyenLieuSapHet = await conn.ExecuteScalarAsync<int>("select count(*) from nguyen_lieu where so_luong_ton <= muc_canh_bao");

        vm.DonHangGanDay = (await conn.QueryAsync<DonHang>(
            @"select d.id, d.ma_don MaDon, k.id KhachHangId, k.ho_ten TenKhachHang, k.sdt SDT,
                     d.ngay_dat NgayDat, d.ngay_nhan NgayNhan, d.ghi_chu GhiChu,
                     d.tong_tien TongTien, d.trang_thai TrangThai
              from don_hang d
              join khach_hang k on d.khach_hang_id = k.id
              order by d.id desc limit 5")).ToList();

        return vm;
    }

    public async Task<List<SanPham>> LaySanPhamAsync(string? tuKhoa = null, string? loai = null)
    {
        using var conn = _db.TaoKetNoi();
        return (await conn.QueryAsync<SanPham>(
            @"select id, ma_banh MaBanh, ten_banh TenBanh, loai_banh LoaiBanh, gia_ban GiaBan,
                     so_luong SoLuong, mo_ta MoTa, trang_thai TrangThai
              from san_pham
              where (@tuKhoa is null or ten_banh like concat('%',@tuKhoa,'%'))
              and (@loai is null or @loai='' or loai_banh=@loai)
              order by id desc", new { tuKhoa, loai })).ToList();
    }

    public async Task<SanPham?> LayMotSanPhamAsync(int id)
    {
        using var conn = _db.TaoKetNoi();
        return await conn.QueryFirstOrDefaultAsync<SanPham>(
            @"select id, ma_banh MaBanh, ten_banh TenBanh, loai_banh LoaiBanh, gia_ban GiaBan,
                     so_luong SoLuong, mo_ta MoTa, trang_thai TrangThai
              from san_pham where id=@id", new { id });
    }

    public async Task ThemSanPhamAsync(SanPham sp)
    {
        using var conn = _db.TaoKetNoi();
        await conn.ExecuteAsync(
            @"insert into san_pham(ma_banh,ten_banh,loai_banh,gia_ban,so_luong,mo_ta,trang_thai)
              values(@MaBanh,@TenBanh,@LoaiBanh,@GiaBan,@SoLuong,@MoTa,@TrangThai)", sp);
    }

    public async Task SuaSanPhamAsync(SanPham sp)
    {
        using var conn = _db.TaoKetNoi();
        await conn.ExecuteAsync(
            @"update san_pham set ma_banh=@MaBanh, ten_banh=@TenBanh, loai_banh=@LoaiBanh,
              gia_ban=@GiaBan, so_luong=@SoLuong, mo_ta=@MoTa, trang_thai=@TrangThai where id=@Id", sp);
    }

    public async Task XoaSanPhamAsync(int id)
    {
        using var conn = _db.TaoKetNoi();
        await conn.ExecuteAsync("delete from san_pham where id=@id", new { id });
    }

    public async Task<List<KhachHang>> LayKhachHangAsync(string? tuKhoa = null)
    {
        using var conn = _db.TaoKetNoi();
        return (await conn.QueryAsync<KhachHang>(
            @"select id, ma_kh MaKH, ho_ten HoTen, sdt SDT, dia_chi DiaChi, tong_don TongDon
              from khach_hang
              where (@tuKhoa is null or ho_ten like concat('%',@tuKhoa,'%') or sdt like concat('%',@tuKhoa,'%'))
              order by id desc", new { tuKhoa })).ToList();
    }

    public async Task<KhachHang?> LayMotKhachHangAsync(int id)
    {
        using var conn = _db.TaoKetNoi();
        return await conn.QueryFirstOrDefaultAsync<KhachHang>(
            "select id, ma_kh MaKH, ho_ten HoTen, sdt SDT, dia_chi DiaChi, tong_don TongDon from khach_hang where id=@id",
            new { id });
    }

    public async Task ThemKhachHangAsync(KhachHang kh)
    {
        using var conn = _db.TaoKetNoi();
        await conn.ExecuteAsync(
            "insert into khach_hang(ma_kh,ho_ten,sdt,dia_chi,tong_don) values(@MaKH,@HoTen,@SDT,@DiaChi,@TongDon)", kh);
    }

    public async Task SuaKhachHangAsync(KhachHang kh)
    {
        using var conn = _db.TaoKetNoi();
        await conn.ExecuteAsync(
            "update khach_hang set ma_kh=@MaKH, ho_ten=@HoTen, sdt=@SDT, dia_chi=@DiaChi, tong_don=@TongDon where id=@Id", kh);
    }

    public async Task XoaKhachHangAsync(int id)
    {
        using var conn = _db.TaoKetNoi();
        await conn.ExecuteAsync("delete from khach_hang where id=@id", new { id });
    }

    public async Task<List<NhanVien>> LayNhanVienAsync(string? tuKhoa = null)
    {
        using var conn = _db.TaoKetNoi();
        return (await conn.QueryAsync<NhanVien>(
            @"select id, ma_nv MaNV, ho_ten HoTen, sdt SDT, chuc_vu ChucVu, tai_khoan TaiKhoan, mat_khau MatKhau, trang_thai TrangThai
              from nhan_vien
              where (@tuKhoa is null or ho_ten like concat('%',@tuKhoa,'%') or sdt like concat('%',@tuKhoa,'%'))
              order by id desc", new { tuKhoa })).ToList();
    }

    public async Task<NhanVien?> LayMotNhanVienAsync(int id)
    {
        using var conn = _db.TaoKetNoi();
        return await conn.QueryFirstOrDefaultAsync<NhanVien>(
            "select id, ma_nv MaNV, ho_ten HoTen, sdt SDT, chuc_vu ChucVu, tai_khoan TaiKhoan, mat_khau MatKhau, trang_thai TrangThai from nhan_vien where id=@id",
            new { id });
    }

    public async Task ThemNhanVienAsync(NhanVien nv)
    {
        using var conn = _db.TaoKetNoi();
        await conn.ExecuteAsync(
            "insert into nhan_vien(ma_nv,ho_ten,sdt,chuc_vu,tai_khoan,mat_khau,trang_thai) values(@MaNV,@HoTen,@SDT,@ChucVu,@TaiKhoan,@MatKhau,@TrangThai)", nv);
    }

    public async Task SuaNhanVienAsync(NhanVien nv)
    {
        using var conn = _db.TaoKetNoi();
        await conn.ExecuteAsync(
            "update nhan_vien set ma_nv=@MaNV, ho_ten=@HoTen, sdt=@SDT, chuc_vu=@ChucVu, tai_khoan=@TaiKhoan, mat_khau=@MatKhau, trang_thai=@TrangThai where id=@Id", nv);
    }

    public async Task XoaNhanVienAsync(int id)
    {
        using var conn = _db.TaoKetNoi();
        await conn.ExecuteAsync("delete from nhan_vien where id=@id", new { id });
    }

    public async Task<List<NguyenLieu>> LayNguyenLieuAsync()
    {
        using var conn = _db.TaoKetNoi();
        return (await conn.QueryAsync<NguyenLieu>(
            "select id, ma_nl MaNL, ten_nl TenNL, don_vi DonVi, so_luong_ton SoLuongTon, muc_canh_bao MucCanhBao from nguyen_lieu order by id desc")).ToList();
    }

    public async Task<NguyenLieu?> LayMotNguyenLieuAsync(int id)
    {
        using var conn = _db.TaoKetNoi();
        return await conn.QueryFirstOrDefaultAsync<NguyenLieu>(
            "select id, ma_nl MaNL, ten_nl TenNL, don_vi DonVi, so_luong_ton SoLuongTon, muc_canh_bao MucCanhBao from nguyen_lieu where id=@id", new { id });
    }

    public async Task ThemNguyenLieuAsync(NguyenLieu nl)
    {
        using var conn = _db.TaoKetNoi();
        await conn.ExecuteAsync(
            "insert into nguyen_lieu(ma_nl,ten_nl,don_vi,so_luong_ton,muc_canh_bao) values(@MaNL,@TenNL,@DonVi,@SoLuongTon,@MucCanhBao)", nl);
    }

    public async Task SuaNguyenLieuAsync(NguyenLieu nl)
    {
        using var conn = _db.TaoKetNoi();
        await conn.ExecuteAsync(
            "update nguyen_lieu set ma_nl=@MaNL, ten_nl=@TenNL, don_vi=@DonVi, so_luong_ton=@SoLuongTon, muc_canh_bao=@MucCanhBao where id=@Id", nl);
    }

    public async Task XoaNguyenLieuAsync(int id)
    {
        using var conn = _db.TaoKetNoi();
        await conn.ExecuteAsync("delete from nguyen_lieu where id=@id", new { id });
    }

    public async Task<List<DonHang>> LayDonHangAsync(string? tuKhoa = null, string? trangThai = null)
    {
        using var conn = _db.TaoKetNoi();
        return (await conn.QueryAsync<DonHang>(
            @"select d.id, d.ma_don MaDon, k.id KhachHangId, k.ho_ten TenKhachHang, k.sdt SDT,
                     d.ngay_dat NgayDat, d.ngay_nhan NgayNhan, d.ghi_chu GhiChu,
                     d.tong_tien TongTien, d.trang_thai TrangThai
              from don_hang d
              join khach_hang k on d.khach_hang_id = k.id
              where (@tuKhoa is null or d.ma_don like concat('%',@tuKhoa,'%') or k.ho_ten like concat('%',@tuKhoa,'%'))
              and (@trangThai is null or @trangThai='' or d.trang_thai=@trangThai)
              order by d.id desc", new { tuKhoa, trangThai })).ToList();
    }

    public async Task TaoDonHangAsync(TaoDonHang model)
    {
        using var conn = _db.TaoKetNoi();
        await conn.OpenAsync();
        using var tran = await conn.BeginTransactionAsync();

        var sp = await conn.QueryFirstAsync<SanPham>(
            @"select id, ma_banh MaBanh, ten_banh TenBanh, loai_banh LoaiBanh, gia_ban GiaBan,
                     so_luong SoLuong, mo_ta MoTa, trang_thai TrangThai
              from san_pham where id=@id", new { id = model.SanPhamId }, tran);

        var kh = await conn.QueryFirstOrDefaultAsync<KhachHang>(
            "select id, ma_kh MaKH, ho_ten HoTen, sdt SDT, dia_chi DiaChi, tong_don TongDon from khach_hang where sdt=@sdt",
            new { sdt = model.SDT }, tran);

        int khachHangId;
        if (kh == null)
        {
            var maKH = "KH" + DateTime.Now.ToString("HHmmss");
            await conn.ExecuteAsync(
                "insert into khach_hang(ma_kh,ho_ten,sdt,dia_chi,tong_don) values(@ma,@ten,@sdt,@diaChi,1)",
                new { ma = maKH, ten = model.TenKhachHang, sdt = model.SDT, diaChi = model.DiaChi }, tran);
            khachHangId = await conn.ExecuteScalarAsync<int>("select last_insert_id()", transaction: tran);
        }
        else
        {
            khachHangId = kh.Id;
            await conn.ExecuteAsync("update khach_hang set tong_don=tong_don+1 where id=@id", new { id = khachHangId }, tran);
        }

        var tongTien = sp.GiaBan * model.SoLuong;
        var maDon = "DH" + DateTime.Now.ToString("HHmmss");

        await conn.ExecuteAsync(
            @"insert into don_hang(ma_don,khach_hang_id,nhan_vien_id,ngay_dat,ngay_nhan,ghi_chu,tong_tien,trang_thai)
              values(@maDon,@khachHangId,1,now(),@ngayNhan,@ghiChu,@tongTien,'Chờ xử lý')",
            new { maDon, khachHangId, ngayNhan = model.NgayNhan, ghiChu = model.GhiChu, tongTien }, tran);

        var donHangId = await conn.ExecuteScalarAsync<int>("select last_insert_id()", transaction: tran);

        await conn.ExecuteAsync(
            @"insert into chi_tiet_don_hang(don_hang_id,san_pham_id,so_luong,don_gia,thanh_tien)
              values(@donHangId,@sanPhamId,@soLuong,@donGia,@thanhTien)",
            new { donHangId, sanPhamId = sp.Id, soLuong = model.SoLuong, donGia = sp.GiaBan, thanhTien = tongTien }, tran);

        await conn.ExecuteAsync(
            "update san_pham set so_luong = greatest(so_luong - @sl, 0) where id=@id",
            new { sl = model.SoLuong, id = sp.Id }, tran);

        await tran.CommitAsync();
    }

    public async Task CapNhatTrangThaiDonAsync(int id, string trangThai)
    {
        using var conn = _db.TaoKetNoi();
        await conn.ExecuteAsync("update don_hang set trang_thai=@trangThai where id=@id", new { id, trangThai });
    }

    public async Task XoaDonHangAsync(int id)
    {
        using var conn = _db.TaoKetNoi();
        await conn.ExecuteAsync("delete from chi_tiet_don_hang where don_hang_id=@id; delete from don_hang where id=@id;", new { id });
    }

    public async Task<BaoCaoThongKe> LayBaoCaoThongKeAsync()
    {
        using var conn = _db.TaoKetNoi();
        var vm = new BaoCaoThongKe();

        vm.TongDoanhThu = await conn.ExecuteScalarAsync<decimal>("select ifnull(sum(tong_tien),0) from don_hang");
        vm.SoDonHang = await conn.ExecuteScalarAsync<int>("select count(*) from don_hang");
        vm.KhachHangMoi = await conn.ExecuteScalarAsync<int>("select count(*) from khach_hang");
        vm.SanPhamBanChay = await conn.ExecuteScalarAsync<string?>(
            @"select s.ten_banh
              from chi_tiet_don_hang c
              join san_pham s on c.san_pham_id = s.id
              group by s.id, s.ten_banh
              order by sum(c.so_luong) desc
              limit 1") ?? "Chưa có dữ liệu";

        vm.DoanhThuTuan = (await conn.QueryAsync<decimal>(
            @"select ifnull(sum(d.tong_tien),0)
              from (
                select curdate() - interval 4 day as ngay union all
                select curdate() - interval 3 day union all
                select curdate() - interval 2 day union all
                select curdate() - interval 1 day union all
                select curdate()
              ) ngay_tuan
              left join don_hang d on date(d.ngay_dat)=ngay_tuan.ngay
              group by ngay_tuan.ngay
              order by ngay_tuan.ngay")).ToList();

        return vm;
    }
}
