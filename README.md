## 📌 Giới thiệu
APIQuanLyLichChieu là một Web API được phát triển bằng **ASP.NET Core 6** nhằm quản lý việc thêm, xóa, sửa lịch chiếu các media như video, audio một cách tiện lợi, hợp lý
## 🚀 Công nghệ sử dụng
- **.NET 6 (ASP.NET Core Web API)**
- **Entity Framework Core** (Code-First, Fluent API)
- **Microsoft SQL Server** (Lưu trữ dữ liệu)
- **Fluent Validation** (Kiểm tra ràng buộc dữ liệu đầu vào)
- **AutoMapper** (Chuyển đổi dữ liệu giữa DTO và Model)
- **Swagger UI** (Tài liệu API)
- **Repository Pattern** (Quản lý dữ liệu)


## 📚 Cấu trúc thư mục
```
GiftManagement/
│-- root/                # Lưu trữ file ở local
│-- Controllers/         # Xử lý request và trả về response cho client
│-- Data/                # DbContext, Domain Model và cấu hình EF Core
│-- Heplers/             # Các hàm tiện ích
│-- Model/               # Data Transfer Objects
│-- Mapping/             # Cấu hình AutoMapper
│-- Migrations/          # Lưu trữ các migration của database
│-- Properties/          # Cấu hình dự án
│-- Repository/          # Repository Pattern
│-- Services/            # Chứa logic nghiệp vụ
│-- Validation/          # Kiểm tra dữ liệu đầu vào
│-- Program.cs           # Cấu hình ứng dụng
│-- appsettings.json     # Cấu hình database và JWT
```

## 🔑 Chức năng chính
✅ **Quản lý các bản ghi**: Tạo, đọc, cập nhật và xóa các định bản ghi như mp3, mp4, image.                
✅ **Quản lý lịch phát cho các bản ghi**: Cho phép quản lý các thời gian chiếu không trùng và lặp lại theo chu kỳ  
                  
