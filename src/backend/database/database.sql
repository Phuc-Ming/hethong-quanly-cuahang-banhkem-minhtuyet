create database if not exists minhtuyet_bakery
character set utf8mb4
collate utf8mb4_unicode_ci;

use minhtuyet_bakery;

drop table if exists chi_tiet_don_hang;
drop table if exists don_hang;
drop table if exists san_pham;
drop table if exists khach_hang;
drop table if exists nhan_vien;
drop table if exists nguyen_lieu;

create table san_pham
(
    id int auto_increment primary key,
    ma_banh varchar(20) not null unique,
    ten_banh varchar(100) not null,
    loai_banh varchar(50) not null,
    gia_ban decimal(12,2) not null,
    so_luong int not null default 0,
    mo_ta text null,
    trang_thai varchar(50) not null default 'Đang bán'
);

create table khach_hang
(
    id int auto_increment primary key,
    ma_kh varchar(20) not null unique,
    ho_ten varchar(100) not null,
    sdt varchar(20) not null,
    dia_chi varchar(255) null,
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
    trang_thai varchar(50) not null default 'Hoạt động'
);

create table nguyen_lieu
(
    id int auto_increment primary key,
    ma_nl varchar(20) not null unique,
    ten_nl varchar(100) not null,
    don_vi varchar(30) not null,
    so_luong_ton int not null default 0,
    muc_canh_bao int not null default 10
);

create table don_hang
(
    id int auto_increment primary key,
    ma_don varchar(20) not null unique,
    khach_hang_id int not null,
    nhan_vien_id int null,
    ngay_dat datetime not null,
    ngay_nhan date not null,
    ghi_chu text null,
    tong_tien decimal(12,2) not null default 0,
    trang_thai varchar(50) not null default 'Chờ xử lý',
    constraint fk_don_hang_khach_hang foreign key (khach_hang_id) references khach_hang(id),
    constraint fk_don_hang_nhan_vien foreign key (nhan_vien_id) references nhan_vien(id)
);

create table chi_tiet_don_hang
(
    id int auto_increment primary key,
    don_hang_id int not null,
    san_pham_id int not null,
    so_luong int not null,
    don_gia decimal(12,2) not null,
    thanh_tien decimal(12,2) not null,
    constraint fk_ctdh_don_hang foreign key (don_hang_id) references don_hang(id),
    constraint fk_ctdh_san_pham foreign key (san_pham_id) references san_pham(id)
);

insert into nhan_vien(ma_nv, ho_ten, sdt, chuc_vu, tai_khoan, mat_khau, trang_thai) values
('NV001', 'Trương Truyền Phúc Minh', '0846011105', 'Quản lý', 'admin', '123456', 'Hoạt động'),
('NV002', 'Trần Trung Tín', '0394484984', 'Nhân viên', 'nhanvien1', '123456', 'Hoạt động'),
('NV003', 'Trương Văn Toàn', '0945123678', 'Nhân viên', 'nhanvien2', '123456', 'Đã nghỉ');

insert into san_pham(ma_banh, ten_banh, loai_banh, gia_ban, so_luong, mo_ta, trang_thai) values
('B001', 'Bánh kem dâu', 'Bánh kem', 250000, 20, 'Bánh kem vị dâu, phù hợp sinh nhật.', 'Đang bán'),
('B002', 'Bánh mì bơ', 'Bánh mì', 340000, 15, 'Bánh mì bơ thơm, mềm.', 'Đang bán'),
('B003', 'Bánh tiramisu', 'Bánh ngọt', 120000, 0, 'Bánh tiramisu vị cà phê.', 'Hết hàng'),
('B004', 'Bánh socola', 'Bánh kem', 300000, 12, 'Bánh kem socola trang trí đơn giản.', 'Đang bán');

insert into khach_hang(ma_kh, ho_ten, sdt, dia_chi, tong_don) values
('KH001', 'Trương Truyền Phúc Minh', '0846011105', 'Trà Vinh', 5),
('KH002', 'Trần Trung Tín', '0394484984', 'Hòa Thuận', 2),
('KH003', 'Trương Văn Toàn', '0945123678', 'Duyên Hải', 3);

insert into nguyen_lieu(ma_nl, ten_nl, don_vi, so_luong_ton, muc_canh_bao) values
('NL001', 'Bột mì', 'kg', 20, 10),
('NL002', 'Kem tươi', 'hộp', 8, 10),
('NL003', 'Socola', 'kg', 35, 10);

insert into don_hang(ma_don, khach_hang_id, nhan_vien_id, ngay_dat, ngay_nhan, ghi_chu, tong_tien, trang_thai) values
('DH001', 1, 1, now(), curdate(), 'Ghi chữ Chúc mừng sinh nhật', 350000, 'Đang làm'),
('DH002', 2, 1, now(), curdate(), 'Giao buổi chiều', 460000, 'Chờ xử lý');

insert into chi_tiet_don_hang(don_hang_id, san_pham_id, so_luong, don_gia, thanh_tien) values
(1, 1, 1, 250000, 250000),
(1, 3, 1, 100000, 100000),
(2, 2, 1, 340000, 340000),
(2, 3, 1, 120000, 120000);
