# Hướng dẫn chạy dự án

## Chuẩn bị

- Cài .NET SDK 9.0
- Cài XAMPP
- Start Apache và MySQL trong XAMPP

## Import cơ sở dữ liệu

Vào phpMyAdmin, chọn tab Import và chọn file:

```text
src/backend/database/database.sql
```

Database được tạo tên là:

```text
minhtuyet_bakery
```

## Chạy chương trình

```bash
cd src/backend
dotnet restore
dotnet run
```

Đăng nhập:

```text
admin / 123456
```
