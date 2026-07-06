# HỆ THỐNG QUẢN LÝ CLB PICKLEBALL "VỢT THỦ PHỐ NÚI" (PCM)
 
-Đây là một dự án Fullstack bao gồm Backend sử dụng ASP.NET Core Web API và Frontend sử dụng Vue.js để quản lý các hoạt động của một câu lạc bộ Pickleball.
+> **Pickleball Club Management (PCM)** là giải pháp phần mềm toàn diện giúp quản lý vận hành câu lạc bộ Pickleball, từ việc quản lý hội viên, sân bãi đến việc đặt lịch và báo cáo thống kê.
+
+Dự án được xây dựng theo mô hình **Fullstack** hiện đại, tách biệt hoàn toàn giữa Backend (API) và Frontend (Client).
+
+## 🌟 Tính năng chính (Features)
+
+Hệ thống bao gồm các phân hệ chính phục vụ cho Admin và Hội viên:
+
+### 1. Quản trị viên (Admin)
+-   **Quản lý Hội viên:** Xem danh sách, thêm, sửa, khóa tài khoản hội viên.
+-   **Quản lý Sân (Courts):** Quản lý danh sách sân, trạng thái sân (đang bảo trì, trống, đang hoạt động).
+-   **Quản lý Đặt sân:** Xem lịch đặt sân, duyệt hoặc hủy lịch đặt.
+-   **Báo cáo & Thống kê:** Thống kê doanh thu, tần suất sử dụng sân.
+
+### 2. Hội viên (Members)
+-   **Đăng ký/Đăng nhập:** Xác thực bảo mật qua JWT.
+-   **Đặt sân trực tuyến:** Kiểm tra khung giờ trống và đặt sân nhanh chóng.
+-   **Lịch sử hoạt động:** Xem lại lịch sử đặt sân và giao dịch cá nhân.
+-   **Thông tin cá nhân:** Cập nhật hồ sơ, đổi mật khẩu.
 
 ## Công nghệ sử dụng
 
-### Backend
--   ASP.NET Core 8 Web API
--   Entity Framework Core 8
--   SQL Server
--   ASP.NET Core Identity for user management
--   JWT (JSON Web Token) for Authentication & Authorization
+### Backend (.NET Ecosystem)
+-   **Core:** ASP.NET Core 8 Web API
+-   **ORM:** Entity Framework Core 8 (Code First)
+-   **Database:** SQL Server
+-   **Auth:** ASP.NET Core Identity & JWT (JSON Web Token)
+-   **Documentation:** Swagger UI
 
-### Frontend
--   Vue.js 3 (với Composition API)
--   Vite
--   Pinia for state management
--   Vue Router
--   Axios
+### Frontend (Vue Ecosystem)
+-   **Framework:** Vue.js 3 (Composition API + Script Setup)
+-   **Build Tool:** Vite
+-   **State Management:** Pinia
+-   **Routing:** Vue Router
+-   **HTTP Client:** Axios
+-   **UI/Styling:** CSS/SCSS (hoặc Tailwind/Bootstrap tùy cấu hình)
+
+## 📂 Cấu trúc dự án
+
+```text
+PCM-Project/
+├── PCM.Api/                # Backend Solution
+│   ├── PCM.Api/            # Main Web API Project
+│   │   ├── Controllers/    # API Endpoints
+│   │   ├── Data/           # DbContext & Migrations
+│   │   ├── Models/         # Entity Models
+│   │   ├── Services/       # Business Logic
+│   │   └── appsettings.json
+│   └── PCM.Api.sln
+│
+└── PCM.Frontend/           # Frontend Application
+    ├── src/
+    │   ├── api/            # Axios configuration & API calls
+    │   ├── components/     # Reusable Vue components
+    │   ├── stores/         # Pinia stores
+    │   ├── views/          # Page components
+    │   └── App.vue
+    ├── package.json
+    └── vite.config.js
+```
 
 ## Hướng dẫn cài đặt và chạy dự án
 
 ### Yêu cầu
 -   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
 -   [Node.js](https://nodejs.org/) (LTS version)
 -   [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Express, Developer, hoặc phiên bản khác)
 -   [Git](https://git-scm.com/)
 
 ### 1. Cài đặt Backend (PCM.Api)
 
 1.  **Clone repository:**
     ```bash
     git clone <your-repository-url>
-    cd <repository-folder>
+    # Di chuyển vào thư mục gốc của dự án
     ```
 
 2.  **Cấu hình Connection String:**
     -   Mở project `PCM.Api` trong Visual Studio hoặc VS Code.
     -   Tìm và mở file `appsettings.json`.
     -   Chỉnh sửa chuỗi `DefaultConnection` để trỏ tới instance SQL Server của bạn.
 
     ```json
     "ConnectionStrings": {
       "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=PCM_Db;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true"
     }
     ```
+    > **Lưu ý:** Thay `YOUR_SERVER_NAME` bằng tên server của bạn (ví dụ: `.` hoặc `localhost` hoặc `.\SQLEXPRESS`).
 
 3.  **Áp dụng Database Migrations:**
     -   Mở terminal trong thư mục `PCM.Api/PCM.Api`.
     -   Chạy lệnh sau để tạo và seeding cơ sở dữ liệu:
     ```bash
     dotnet ef database update
     ```
     *Lệnh này sẽ tự động tạo các bảng và chèn dữ liệu mẫu (admin, members, courts...).*
 
 4.  **Chạy Backend:**
     -   Bạn có thể chạy project từ Visual Studio (bấm F5) hoặc dùng lệnh trong thư mục `PCM.Api/PCM.Api`:
     ```bash
     dotnet run
     ```
-    -   API sẽ có sẵn tại `https://localhost:7183`.
-    -   Tài liệu API (Swagger) có thể được truy cập tại `https://localhost:7183/swagger`.
+    -   **API URL:** `https://localhost:7183`
+    -   **Swagger Docs:** `https://localhost:7183/swagger` (Dùng để test API trực tiếp).
 
 ### 2. Cài đặt Frontend (PCM.Frontend)
 
 1.  **Cài đặt dependencies:**
     -   Mở một terminal khác và di chuyển vào thư mục `PCM.Frontend`:
     ```bash
     cd PCM.Frontend
     ```
     -   Chạy lệnh sau:
     ```bash
     npm install
     ```
 
 2.  **Chạy Frontend:**
     -   Sau khi cài đặt xong, chạy development server:
     ```bash
     npm run dev
     ```
-    -   Ứng dụng web sẽ có sẵn tại `http://localhost:5173`.
+    -   Truy cập ứng dụng tại: `http://localhost:5173`
 
 ## Tài khoản đăng nhập mặc định
 
 Hệ thống đã được seeding sẵn một tài khoản Admin để bạn có thể đăng nhập và sử dụng các tính năng quản trị ngay lập tức.
 
-| Role | Email | Password |
-| :--- | :--- | :--- |
-| **Admin** | `admin@pcm.com` | `Admin@123` |
-
--   **Email:** `admin@pcm.com`
--   **Password:** `Admin@123`

