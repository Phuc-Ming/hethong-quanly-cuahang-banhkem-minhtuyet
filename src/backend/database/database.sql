drop database if exists minhtuyet_bakery;
create database minhtuyet_bakery character set utf8mb4 collate utf8mb4_unicode_ci;
use minhtuyet_bakery;

create table san_pham
(
    id int auto_increment primary key,
    ma_banh varchar(20) not null unique,
    ten_banh varchar(100) not null,
    loai_banh varchar(50) not null,
    gia_ban decimal(18,2) not null,
    so_luong int not null default 0,
    mo_ta text null,
    trang_thai varchar(30) not null default 'Đang bán'
);

create table khach_hang
(
    id int auto_increment primary key,
    ma_kh varchar(20) not null unique,
    ho_ten varchar(100) not null,
    sdt varchar(20) not null,
    dia_chi varchar(200) null,
    tong_don int not null default 0
);

create table nhan_vien
(
    id int auto_increment primary key,
    ma_nv varchar(20) not null unique,
    ho_ten varchar(100) not null,
    sdt varchar(20) null,
    chuc_vu varchar(50) not null,
    tai_khoan varchar(50) not null unique,
    mat_khau varchar(100) not null,
    trang_thai varchar(30) not null default 'Hoạt động'
);

create table hoa_don
(
    id int auto_increment primary key,
    ma_hd varchar(30) not null unique,
    id_khach_hang int not null,
    id_nhan_vien int not null,
    ngay_lap datetime not null,
    ngay_nhan date not null,
    ghi_chu text null,
    tong_tien decimal(18,2) not null default 0,
    trang_thai varchar(30) not null default 'Chờ xử lý',
    constraint fk_hoa_don_khach_hang foreign key (id_khach_hang) references khach_hang(id),
    constraint fk_hoa_don_nhan_vien foreign key (id_nhan_vien) references nhan_vien(id)
);

create table chi_tiet_hoa_don
(
    ma_cthd int auto_increment primary key,
    ma_hd varchar(30) not null,
    ma_banh varchar(20) not null,
    so_luong int not null,
    don_gia decimal(18,2) not null,
    thanh_tien decimal(18,2) not null,
    constraint fk_cthd_hoa_don foreign key (ma_hd) references hoa_don(ma_hd),
    constraint fk_cthd_san_pham foreign key (ma_banh) references san_pham(ma_banh)
);

create table nguyen_lieu
(
    id int auto_increment primary key,
    ma_nl varchar(20) not null unique,
    ten_nl varchar(100) not null,
    don_vi varchar(30) not null,
    so_luong_ton int not null default 0,
    trang_thai varchar(30) not null default 'Đủ hàng'
);

insert into san_pham(ma_banh, ten_banh, loai_banh, gia_ban, so_luong, mo_ta, trang_thai) values
('B001', 'Bánh kem dâu', 'Bánh kem', 250000, 40, 'Bánh kem vị dâu phù hợp sinh nhật', 'Đang bán'),
('B002', 'Bánh mì bơ', 'Bánh mì', 340000, 20, 'Bánh mì bơ thơm béo', 'Đang bán'),
('B003', 'Bánh tiramisu', 'Bánh ngọt', 120000, 0, 'Bánh tiramisu mềm, vị cà phê nhẹ', 'Hết hàng'),
('B004', 'Bánh socola', 'Bánh kem', 80000, 30, 'Bánh socola nhỏ', 'Đang bán');

insert into khach_hang(ma_kh, ho_ten, sdt, dia_chi, tong_don) values
('KH001', 'Trương Truyền Phúc Minh', '0846011105', 'Trà Vinh', 5),
('KH002', 'Trần Trung Tín', '0394484984', 'Hòa Thuận', 2),
('KH003', 'Trương Văn Toàn', '0945123678', 'Duyên Hải', 3);

insert into nhan_vien(ma_nv, ho_ten, sdt, chuc_vu, tai_khoan, mat_khau, trang_thai) values
('NV001', 'Trương Truyền Phúc Minh', '0846011105', 'Quản lý', 'admin', '123456', 'Hoạt động'),
('NV002', 'Trần Trung Tín', '0394484984', 'Nhân viên', 'nhanvien1', '123456', 'Hoạt động'),
('NV003', 'Trương Văn Toàn', '0945123678', 'Nhân viên', 'nhanvien2', '123456', 'Đã nghỉ');

insert into nguyen_lieu(ma_nl, ten_nl, don_vi, so_luong_ton, trang_thai) values
('NL001', 'Bột mì', 'kg', 20, 'Đủ hàng'),
('NL002', 'Kem tươi', 'Hộp', 10, 'Sắp hết'),
('NL003', 'Socola', 'kg', 35, 'Đủ hàng');

insert into hoa_don(ma_hd, id_khach_hang, id_nhan_vien, ngay_lap, ngay_nhan, ghi_chu, tong_tien, trang_thai) values
('HD001', 1, 1, '2026-06-07 08:00:00', '2026-06-10', 'Ghi chữ Chúc mừng sinh nhật', 350000, 'Đang làm'),
('HD002', 2, 2, '2026-06-07 09:30:00', '2026-06-12', 'Giao trong buổi sáng', 460000, 'Chờ xử lý');

insert into chi_tiet_hoa_don(ma_hd, ma_banh, so_luong, don_gia, thanh_tien) values
('HD001', 'B001', 1, 250000, 250000),
('HD001', 'B004', 1, 80000, 80000),
('HD002', 'B002', 1, 340000, 340000),
('HD002', 'B003', 1, 120000, 120000);
