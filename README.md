# Hệ thống quản lý cửa hàng bánh kem Minh Tuyết

Dự án C# ASP.NET Core MVC + MySQL/XAMPP, xây dựng theo nội dung báo cáo môn Công nghệ phần mềm.

## Chức năng

- Đăng nhập hệ thống
- Trang chủ tổng quan
- Quản lý sản phẩm: thêm, sửa, tìm kiếm, lọc, ngừng bán
- Quản lý hóa đơn: tìm kiếm, lọc trạng thái, cập nhật trạng thái, xem chi tiết, xóa
- Tạo hóa đơn: nhập khách hàng, chọn sản phẩm, nhập số lượng, tính tiền, lưu hóa đơn
- Quản lý khách hàng
- Quản lý kho nguyên liệu
- Quản lý nhân viên
- Thống kê doanh thu và biểu đồ 5 ngày gần đây

## Công nghệ

- C# ASP.NET Core MVC
- .NET 9.0
- MySQL trong XAMPP
- MySqlConnector
- Razor View, HTML, CSS, JavaScript

## Cách chạy trên Windows bằng VS Code

1. Mở XAMPP và Start Apache + MySQL.
2. Vào phpMyAdmin: http://localhost/phpmyadmin
3. Import file: src/backend/database/database.sql
4. Mở VS Code tại thư mục dự án.
5. Mở Terminal và chạy:

```bash
cd src/backend
dotnet restore
dotnet run
```

6. Mở đường dẫn terminal hiển thị, thường là: http://localhost:5000

## Tài khoản đăng nhập

- Tài khoản: admin
- Mật khẩu: 123456

## Cấu trúc thư mục

```text
src/backend/          Mã nguồn ASP.NET Core MVC
database/             File SQL MySQL
docs/                 Tài liệu dự án
test/                 Kịch bản kiểm thử
```
