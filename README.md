# FU News Management System - Assignment 01 (PRN232)

---

## 📌 1. THÔNG TIN SINH VIÊN & ĐỀ TÀI
- **Môn học**: PRN232 - Lập trình .NET nâng cao (Advanced Cross-Platform .NET)
- **Bài tập**: Assignment 01
- **Sinh viên**: Phùng Đức Anh
- **Số thứ tự (Student No)**: 22
- **Tên Solution**: `22_PhungDucAnh_Assignment01.sln`
- **Đường dẫn thư mục dự án**: `D:\PRN232\22_PhungDucAnh_Assignment01`

---

## 🏗️ 2. TỔNG QUAN KIẾN TRÚC HỆ THỐNG

Hệ thống được xây dựng theo mô hình tách biệt **BackEnd (RESTful / OData Web API)** và **FrontEnd (Client Web MVC)**:

```
22_PhungDucAnh_Assignment01/
├── 22_PhungDucAnh_Assignment01.sln
│
├── 22_PhungDucAnh_Assignment01_BackEnd/       (Port: http://localhost:5100)
│   ├── Controllers/                          (ODataControllers & API Controllers)
│   ├── DataAccess/                           (DAOs áp dụng Singleton Pattern)
│   ├── Repositories/                         (Repository Pattern - Interface & Implementation)
│   ├── Models/                               (Entity Framework Core Models & DbContext)
│   ├── appsettings.json                      (Cấu hình ConnectionString & Tài khoản Admin)
│   └── Program.cs                            (Đăng ký dịch vụ OData đầy đủ, CORS, DI)
│
└── 22_PhungDucAnh_Assignment01_FrontEnd/      (Port: http://localhost:5200)
    ├── Controllers/                          (MVC Controllers xử lý giao diện & phiên làm việc)
    ├── Services/                             (ApiService giao tiếp với BackEnd qua HttpClient)
    ├── Models/                               (ViewModels dữ liệu cho View)
    ├── Views/                                (Razor Views với giao diện Popup Dialogs / Modals)
    └── Program.cs                            (Cấu hình Session, BaseAddress HttpClient)
```

### Điểm nhấn kỹ thuật chuẩn chỉnh:
1. **Kiến trúc 3 lớp (3-Layer Architecture)**:
   - `Presentation Layer (Controllers)`: Tiếp nhận request OData/REST, trả về kết quả. Tuyệt đối **không gọi trực tiếp DbContext**.
   - `Business Logic Layer (Repositories)`: `ICategoryRepository`, `INewsArticleRepository`, `ISystemAccountRepository`, `ITagRepository`.
   - `Data Access Layer (DAOs)`: `CategoryDAO`, `NewsArticleDAO`, `SystemAccountDAO`, `TagDAO` được cài đặt theo chuẩn **Singleton Pattern** (`Instance`).
2. **Cấu hình OData đầy đủ nhất**:
   - Khai báo route prefix: `/odata`
   - Đăng ký trọn vẹn các tính năng OData: `.Select().Filter().OrderBy().Expand().Count().SetMaxTop(100).SkipToken()`.
3. **Giao diện người dùng hiện đại**:
   - Tối ưu trải nghiệm với **Bootstrap 5**.
   - Toàn bộ thao tác Thêm, Sửa, Xóa đều sử dụng **Popup Dialog (Modal)**, không load lại trang trắng.

---

## 🗄️ 3. CƠ SỞ DỮ LIỆU (`FUNewsManagement`)

Hệ thống kết nối cơ sở dữ liệu SQL Server gồm các bảng:
- **`Category`**: Danh mục tin tức (`CategoryID`, `CategoryName`, `CategoryDesciption`, `ParentCategoryID`, `IsActive`).
- **`NewsArticle`**: Bài viết tin tức (`NewsArticleID`, `NewsTitle`, `Headline`, `CreatedDate`, `NewsContent`, `NewsSource`, `CategoryID`, `NewsStatus`, `CreatedByID`, `UpdatedByID`, `ModifiedDate`).
- **`Tag`**: Thẻ phân loại bài viết (`TagID`, `TagName`, `Note`).
- **`NewsTag`**: Bảng liên kết n-n giữa `NewsArticle` và `Tag`.
- **`SystemAccount`**: Tài khoản người dùng (`AccountID`, `AccountName`, `AccountEmail`, `AccountRole`, `AccountPassword`).

---

## 👥 4. MA TRẬN PHÂN QUYỀN (ROLES & PERMISSIONS)

| Vai trò (Role) | Nguồn lưu trữ | Quyền hạn & Chức năng |
| :--- | :--- | :--- |
| **Guest** (Khách vãng lai) | Không cần đăng nhập | - Xem danh sách bài viết đang kích hoạt (`NewsStatus = true`).<br>- Tìm kiếm bài viết theo từ khóa tiêu đề.<br>- Lọc bài viết theo danh mục.<br>- Xem chi tiết nội dung và các tags đính kèm qua Popup Modal. |
| **Staff** (`AccountRole = 1`) | Database (`SystemAccount`) | - **Quản lý danh mục (Category)**: Xem danh sách, Thêm, Sửa, Xóa bằng Popup Modal. *(Ràng buộc: Không được xóa danh mục nếu đang có bài viết thuộc danh mục đó)*.<br>- **Quản lý bài viết (NewsArticle)**: Xem danh sách, Thêm bài viết mới, Sửa bài viết, Xóa bài viết (hỗ trợ tích chọn nhiều Tags).<br>- **Lịch sử đăng bài (My History)**: Xem các bài viết do chính tài khoản mình tạo.<br>- **Hồ sơ cá nhân (Profile)**: Xem và cập nhật thông tin tên, email, mật khẩu cá nhân. |
| **Admin** | `appsettings.json` | - **Quản lý tài khoản (Account Management)**: Xem danh sách, Thêm tài khoản mới, Cập nhật thông tin, Xóa tài khoản bằng Popup Modal. *(Ràng buộc: Không được xóa tài khoản nếu người này đã từng tạo bài viết)*.<br>- **Báo cáo thống kê (Reports)**: Lọc bài viết theo khoảng ngày (`StartDate` đến `EndDate`), hiển thị tổng số bài viết và danh sách sắp xếp giảm dần theo ngày tạo. |
| **Lecturer** (`AccountRole = 2`) | Database (`SystemAccount`) | - Đăng nhập vào hệ thống để đọc tin tức active. |

---

## 🔑 5. DANH SÁCH TÀI KHOẢN THỬ NGHIỆM

| Vai trò | Email | Mật khẩu | Ghi chú |
| :--- | :--- | :--- | :--- |
| **Admin** | `admin@FUNewsManagementSystem.org` | `@@abc123@@` | Cấu hình trong `appsettings.json` |
| **Staff** | `IsabellaDavid@FUNewsManagement.org` | `@1` | Tài khoản có sẵn trong Database |
| **Lecturer** | `DavidTaylor@FUNewsManagement.org` | `@1` | Tài khoản có sẵn trong Database |

---

## 🚀 6. HƯỚNG DẪN CÀI ĐẶT & CHẠY DỰ ÁN

### Yêu cầu môi trường
- .NET SDK 8.0 trở lên.
- Microsoft SQL Server (Localhost).
- Visual Studio 2022 (khuyên dùng) hoặc VS Code.

### Bước 1: Khởi động Database
- Đảm bảo database `FUNewsManagement` đã được chạy trên SQL Server:
  - Server: `localhost`
  - User: `sa`
  - Password: `123`
- *(Nếu thông tin SQL Server máy bạn khác, chỉnh sửa chuỗi kết nối tại file `22_PhungDucAnh_Assignment01_BackEnd/appsettings.json`)*.

### Bước 2: Khởi chạy bằng Visual Studio
1. Mở file `22_PhungDucAnh_Assignment01.sln`.
2. Click chuột phải vào Solution ➜ chọn **Configure Startup Projects...**.
3. Chọn **Multiple startup projects**:
   - `22_PhungDucAnh_Assignment01_BackEnd`: Chọn action **Start**.
   - `22_PhungDucAnh_Assignment01_FrontEnd`: Chọn action **Start**.
4. Nhấn **F5** để khởi chạy cả 2 dự án cùng lúc.

### Bước 3: Khởi chạy bằng Dòng lệnh (Terminal)
Mở 2 cửa sổ PowerShell riêng biệt:

- **Terminal 1 - Khởi chạy BackEnd (Port 5100)**:
  ```powershell
  cd D:\PRN232\22_PhungDucAnh_Assignment01\22_PhungDucAnh_Assignment01_BackEnd
  dotnet run --launch-profile http
  ```

- **Terminal 2 - Khởi chạy FrontEnd (Port 5200)**:
  ```powershell
  cd D:\PRN232\22_PhungDucAnh_Assignment01\22_PhungDucAnh_Assignment01_FrontEnd
  dotnet run --launch-profile http
  ```

Mở trình duyệt truy cập:
- **Giao diện Web**: `http://localhost:5200`
- **Swagger API & OData**: `http://localhost:5100`

---

## 📡 7. DANH SÁCH ENDPOINTS CHÍNH

### OData Endpoints (BackEnd)
- `GET /odata/NewsArticles`: Lấy danh sách tin tức.
  - Ví dụ OData: `GET /odata/NewsArticles?$expand=Category,Tags&$filter=NewsStatus eq true&$orderby=CreatedDate desc`
- `GET /odata/NewsArticles({key})`: Lấy chi tiết bài viết kèm Category và Tags.
- `POST /odata/NewsArticles`: Tạo mới bài viết.
- `PUT /odata/NewsArticles({key})`: Cập nhật bài viết.
- `DELETE /odata/NewsArticles({key})`: Xóa bài viết.
- `GET /odata/Categories`: Lấy danh mục.
- `POST /odata/Categories`: Tạo danh mục.
- `PUT /odata/Categories({key})`: Sửa danh mục.
- `DELETE /odata/Categories({key})`: Xóa danh mục (kiểm tra không có bài viết mới cho xóa).
- `GET /odata/SystemAccounts`: Quản lý tài khoản (Admin).
- `DELETE /odata/SystemAccounts({key})`: Xóa tài khoản (kiểm tra chưa từng tạo bài viết).
- `GET /odata/Tags`: Lấy danh sách thẻ tag.

### Custom REST Endpoints
- `POST /api/auth/login`: Xác thực người dùng (hỗ trợ cả Admin cấu hình file và Tài khoản DB).
- `GET /api/reports/statistics?startDate=...&endDate=...`: Thống kê tin tức theo khoảng thời gian.

---

## 📦 8. HƯỚNG DẪN NỘP BÀI GITHUB

Dự án đã được khởi tạo sẵn Git cục bộ. Để đẩy lên GitHub của bạn:
```powershell
cd D:\PRN232\22_PhungDucAnh_Assignment01
git remote add origin <URL_REPO_GITHUB_CUA_BAN>
git branch -M main
git push -u origin main
```
