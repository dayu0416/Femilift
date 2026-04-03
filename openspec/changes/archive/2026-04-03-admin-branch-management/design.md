# Design: Admin Branch Management System

## Architecture Overview

```
┌─────────────────────────────────────────────────────────┐
│                  IIS (虛擬主機)                           │
│                                                         │
│  ASP.NET MVC 5 Application                              │
│  ┌─────────────────────────────────────────────────┐    │
│  │                                                 │    │
│  │  PUBLIC                      ADMIN              │    │
│  │  ──────                      ─────              │    │
│  │  GET /Branch                 /Admin/Login        │    │
│  │  GET / → index.html          /Admin/Branches     │    │
│  │  GET /*.html (static)        /Admin/Password     │    │
│  │  GET /assets/**                                  │    │
│  │  GET /uploads/**                                 │    │
│  │                                                 │    │
│  └────────────────────┬────────────────────────────┘    │
│                       │                                  │
│                       ▼                                  │
│              ┌──────────────────┐                        │
│              │   SQL Server     │                        │
│              │   (帳密認證)      │                        │
│              └──────────────────┘                        │
│                                                         │
│  磁碟                                                    │
│  ├── bin/                                                │
│  ├── Views/                                              │
│  ├── Web.config                                          │
│  ├── index.html          ← 靜態頁面                      │
│  ├── intelligence.html                                   │
│  ├── blog_*.html                                         │
│  ├── media.html                                          │
│  ├── question.html                                       │
│  ├── assets/             ← CSS, JS, 靜態圖片             │
│  └── uploads/                                            │
│      └── branches/       ← 後台上傳的院所圖片             │
└─────────────────────────────────────────────────────────┘
```

## Project Structure

```
FemiliftAdmin/                          ← Solution root
├── FemiliftAdmin.sln
├── FemiliftAdmin/                      ← MVC Project
│   ├── App_Start/
│   │   ├── RouteConfig.cs
│   │   └── FilterConfig.cs
│   ├── Controllers/
│   │   ├── BranchController.cs         ← 前台：GET /Branch
│   │   └── Admin/
│   │       ├── AccountController.cs    ← 登入、登出、改密碼
│   │       └── BranchesController.cs   ← 院所 CRUD、排序、顯隱
│   ├── Models/
│   │   ├── Branch.cs                   ← Entity
│   │   ├── AdminUser.cs               ← Entity
│   │   ├── FemiliftContext.cs          ← DbContext
│   │   └── ViewModels/
│   │       ├── LoginViewModel.cs
│   │       ├── ChangePasswordViewModel.cs
│   │       └── BranchEditViewModel.cs
│   ├── Migrations/
│   │   └── Configuration.cs           ← Seed data
│   ├── Filters/
│   │   └── AdminAuthAttribute.cs      ← Session 驗證 ActionFilter
│   ├── Views/
│   │   ├── Shared/
│   │   │   ├── _Layout.cshtml         ← 前台 layout（沿用現有 HTML 結構）
│   │   │   └── _AdminLayout.cshtml    ← 後台 layout（側邊選單）
│   │   ├── Branch/
│   │   │   └── Index.cshtml           ← 前台院所列表
│   │   └── Admin/
│   │       ├── Account/
│   │       │   ├── Login.cshtml
│   │       │   └── ChangePassword.cshtml
│   │       └── Branches/
│   │           ├── Index.cshtml       ← 院所列表（含排序、toggle）
│   │           ├── Create.cshtml
│   │           └── Edit.cshtml
│   ├── Content/
│   │   ├── admin.css                  ← 後台樣式
│   │   └── SeedImages/               ← Seed 用的 13 張院所圖
│   ├── Scripts/
│   │   └── admin.js                   ← SortableJS + Ajax 邏輯
│   ├── uploads/
│   │   └── branches/                  ← 運行時圖片上傳目錄
│   ├── Web.config
│   └── Global.asax.cs
```

## Database Schema

```sql
CREATE TABLE Branches (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    Name        NVARCHAR(100)  NOT NULL,        -- 院所名稱
    Phone       NVARCHAR(50)   NULL,            -- 電話
    Hours       NVARCHAR(500)  NULL,            -- 營業時間（textarea, \n→<br>）
    Address     NVARCHAR(200)  NULL,            -- 地址
    MapUrl      NVARCHAR(500)  NULL,            -- Google Maps 連結
    ImagePath   NVARCHAR(300)  NULL,            -- uploads/branches/xxx.jpg
    IsVisible   BIT            NOT NULL DEFAULT 1,
    SortOrder   INT            NOT NULL DEFAULT 0,
    CreatedAt   DATETIME2      NOT NULL DEFAULT GETDATE(),
    UpdatedAt   DATETIME2      NOT NULL DEFAULT GETDATE()
);

CREATE TABLE AdminUsers (
    Id            INT IDENTITY(1,1) PRIMARY KEY,
    Username      NVARCHAR(50)  NOT NULL,
    PasswordHash  NVARCHAR(256) NOT NULL        -- BCrypt (salt 內含)
);
```

## Authentication Flow

```
  Browser                              Server
    │                                    │
    │  GET /Admin/Branches               │
    │───────────────────────────────────▶│
    │                                    │  [AdminAuth] filter
    │                                    │  Session["AdminUserId"] == null
    │  302 → /Admin/Account/Login        │
    │◀───────────────────────────────────│
    │                                    │
    │  POST /Admin/Account/Login         │
    │  { Username, Password }            │
    │───────────────────────────────────▶│
    │                                    │  BCrypt.Verify(pwd, hash) ✓
    │                                    │  Session["AdminUserId"] = id
    │  302 → /Admin/Branches             │
    │◀───────────────────────────────────│
    │                                    │
    │  GET /Admin/Branches               │
    │  Cookie: ASP.NET_SessionId         │
    │───────────────────────────────────▶│
    │                                    │  [AdminAuth] ✓
    │  200 OK                            │
    │◀───────────────────────────────────│
```

### AdminAuthAttribute

自訂 `ActionFilterAttribute`，檢查 `Session["AdminUserId"]`，失敗則 redirect 到 Login。套用在所有 Admin controllers 上。

## Admin UI Layout

```
┌──────────────────────────────────────────────────────────┐
│  菲蜜莉雷射 後台管理                              管理員 ▾ │
├────────────┬─────────────────────────────────────────────┤
│            │                                             │
│  📋 院所管理│         主要內容區                           │
│            │                                             │
│  ────────  │    ┌─────────────────────────────────────┐  │
│  🔑 改密碼 │    │                                     │  │
│  🚪 登出   │    │   @RenderBody()                     │  │
│            │    │                                     │  │
│            │    └─────────────────────────────────────┘  │
│            │                                             │
└────────────┴─────────────────────────────────────────────┘
```

## Admin Branches List Page

```
┌─────────────────────────────────────────────────────────┐
│  院所管理                              [+ 新增院所]       │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  ☰  [圖] 尼斯診所           (02)2778-2255   ✅ 顯示  ✏️ │
│  ☰  [圖] 博田國際醫學美容中心  (07)556-2217   ✅ 顯示  ✏️ │
│  ☰  [圖] 高大美杏生醫院       (07)365-8885   ❌ 隱藏  ✏️ │
│  ☰  [圖] 萊佳-台中館         (04)2329-7989   ✅ 顯示  ✏️ │
│  ...                                                    │
│                                                         │
│  ☰ = drag handle (SortableJS)                           │
│  ✅/❌ = IsVisible toggle (Ajax POST)                    │
│  ✏️ = 進入編輯頁                                         │
│                                                         │
└─────────────────────────────────────────────────────────┘
```

## Branch Edit Page

```
┌─────────────────────────────────────────────────────────┐
│  編輯院所：尼斯診所                                       │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  院所名稱    [________________________]                  │
│                                                         │
│  電話        [________________________]                  │
│                                                         │
│  營業時間    [________________________]                  │
│             [________________________]  ← textarea       │
│             [________________________]                  │
│                                                         │
│  地址        [________________________]                  │
│                                                         │
│  地圖連結    [________________________]                  │
│                                                         │
│  院所圖片    ┌────────────┐                              │
│             │  目前圖片    │  [選擇檔案]                  │
│             │  preview    │  [移除圖片]                  │
│             └────────────┘                              │
│             * jpg, png, webp / 上限 2MB                  │
│                                                         │
│              [儲存]  [刪除此院所]  [取消]                  │
│                                                         │
└─────────────────────────────────────────────────────────┘
```

## Key Interactions

### Drag & Drop Reorder
1. 前端：SortableJS 綁定列表，拖曳結束時收集新的 id 順序
2. Ajax POST `/Admin/Branches/Reorder` body: `{ ids: [3, 1, 5, 2, ...] }`
3. Server 端迴圈更新 `SortOrder`，回傳 200

### IsVisible Toggle
1. 點擊 toggle 按鈕
2. Ajax POST `/Admin/Branches/Toggle/{id}`
3. Server 端翻轉 `IsVisible`，回傳新狀態
4. 前端更新按鈕顯示

### Image Upload
1. Edit form 用 `enctype="multipart/form-data"`
2. Controller 接收 `HttpPostedFileBase`
3. 驗證副檔名 + 大小（≤ 2MB）
4. 儲存為 `uploads/branches/{guid}{ext}`
5. 舊圖刪除（如果有替換）
6. DB 更新 ImagePath

## Front-end Branch Page (/Branch)

沿用現有 branch.html 的 HTML 結構與 CSS，改為 Razor View：
- `_Layout.cshtml` 包含 header/footer（從現有 HTML 抽取）
- `Branch/Index.cshtml` 用 `@foreach` 迴圈 render 院所列表
- 只顯示 `IsVisible == true` 的院所
- 按 `SortOrder` 排序
- Hours 欄位用 `@Html.Raw(branch.Hours.Replace("\n", "<br>"))` render

## Configuration (Web.config)

```xml
<connectionStrings>
  <add name="FemiliftDb"
       connectionString="Server=主機商位址;Database=Femilift;
                          User Id=帳號;Password=密碼;"
       providerName="System.Data.SqlClient" />
</connectionStrings>

<system.web>
  <sessionState mode="InProc" timeout="60" />
</system.web>
```

## Seed Strategy

在 `Migrations/Configuration.cs` 的 `Seed()` 方法中：
1. 檢查 Branches 表是否為空
2. 若空，INSERT 13 筆院所資料（名稱、電話、營業時間、地址、地圖連結）
3. 將 `Content/SeedImages/` 中的圖片複製到 `uploads/branches/`
4. INSERT 預設 admin 帳號（admin / 預設密碼，BCrypt hash）

`Global.asax.cs` 中設定：
```csharp
Database.SetInitializer(
    new MigrateDatabaseToLatestVersion<FemiliftContext, Configuration>());
```

## Security Considerations

- BCrypt hash 密碼（使用 BCrypt.Net-Next NuGet）
- 圖片上傳驗證副檔名白名單 + 檔案大小限制
- 上傳檔名用 GUID 避免路徑穿越
- Admin 路由全部經過 `[AdminAuth]` filter
- Web.config 中的 DB 密碼由 IIS 保護不被直接存取
- Anti-forgery token 在所有 POST form 上（`@Html.AntiForgeryToken()`）
