# HỆ THỐNG QUẢN LÝ CLB PICKLEBALL "VỢT THỦ PHỐ NÚI" (PCM)

Đây là một dự án Fullstack bao gồm Backend sử dụng ASP.NET Core Web API và Frontend sử dụng Vue.js để quản lý các hoạt động của một câu lạc bộ Pickleball.

## Công nghệ sử dụng

### Backend
-   ASP.NET Core 8 Web API
-   Entity Framework Core 8
-   SQL Server
-   ASP.NET Core Identity for user management
-   JWT (JSON Web Token) for Authentication & Authorization

### Frontend
-   Vue.js 3 (với Composition API)
-   Vite
-   Pinia for state management
-   Vue Router
-   Axios

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
    cd <repository-folder>
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
    -   API sẽ có sẵn tại `https://localhost:7183`.
    -   Tài liệu API (Swagger) có thể được truy cập tại `https://localhost:7183/swagger`.

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
    -   Ứng dụng web sẽ có sẵn tại `http://localhost:5173`.

## Tài khoản đăng nhập mặc định

Hệ thống đã được seeding sẵn một tài khoản Admin để bạn có thể đăng nhập và sử dụng các tính năng quản trị ngay lập tức.

-   **Email:** `admin@pcm.com`
-   **Password:** `Admin@123`