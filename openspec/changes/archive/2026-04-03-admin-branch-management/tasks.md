# Tasks: Admin Branch Management System

## Phase 1: Project Scaffolding
- [x] 1.1 建立 ASP.NET MVC 5 專案（FemiliftAdmin），設定 .NET Framework 4.x
- [x] 1.2 安裝 NuGet 套件：EntityFramework 6、BCrypt.Net-Next
- [x] 1.3 建立專案資料夾結構（Controllers/Admin/, Views/Admin/, Models/, Filters/, ViewModels/）
- [x] 1.4 將現有靜態檔案（*.html, assets/）搬入專案根目錄，設定 StaticFiles serving
- [x] 1.5 設定 Web.config connection string（SQL Server 帳密認證）
- [x] 1.6 設定 RouteConfig：確保靜態檔案優先、/Branch 和 /Admin/* routing 正確

## Phase 2: Database & Models
- [x] 2.1 建立 Entity Models：Branch.cs、AdminUser.cs
- [x] 2.2 建立 FemiliftContext.cs（DbContext）
- [x] 2.3 設定 Global.asax.cs 的 MigrateDatabaseToLatestVersion initializer
- [x] 2.4 建立 ViewModels：LoginViewModel、ChangePasswordViewModel、BranchEditViewModel
- [x] 2.5 建立 Seed 資料：14 筆院所 + 預設 admin 帳號
- [x] 2.6 準備 SeedImages/（複製現有 14 張院所圖片），Seed 時複製到 uploads/branches/

## Phase 3: Authentication
- [x] 3.1 建立 AdminAuthAttribute（ActionFilter，檢查 Session）
- [x] 3.2 建立 Admin/AccountController：Login GET/POST、Logout、ChangePassword GET/POST
- [x] 3.3 建立 Views/Admin/Account/Login.cshtml
- [x] 3.4 建立 Views/Admin/Account/ChangePassword.cshtml

## Phase 4: Admin Layout & UI
- [x] 4.1 建立 _AdminLayout.cshtml（側邊選單：院所管理、改密碼、登出）
- [x] 4.2 引入 Bootstrap（後台用）+ 基本 admin.css 樣式
- [x] 4.3 引入 SortableJS（CDN）

## Phase 5: Branch CRUD (Backend)
- [x] 5.1 建立 Admin/BranchesController：Index（列表）
- [x] 5.2 建立 Create GET/POST（新增院所 + 圖片上傳）
- [x] 5.3 建立 Edit GET/POST（編輯院所 + 圖片替換/移除）
- [x] 5.4 建立 Delete POST（刪除院所 + 刪除圖片）
- [x] 5.5 建立 Toggle POST（Ajax 切換 IsVisible）
- [x] 5.6 建立 Reorder POST（Ajax 更新排序）

## Phase 6: Admin Views
- [x] 6.1 建立 Views/Admin/Branches/Index.cshtml（列表 + SortableJS + Toggle UI）
- [x] 6.2 建立 Views/Admin/Branches/Create.cshtml（新增表單 + 圖片上傳）
- [x] 6.3 建立 Views/Admin/Branches/Edit.cshtml（編輯表單 + 圖片 preview/替換）
- [x] 6.4 建立 admin.js（SortableJS init、Reorder Ajax、Toggle Ajax、Delete confirm）

## Phase 7: Frontend Branch Page
- [x] 7.1 從現有 branch.html 抽取 _Layout.cshtml（header, footer, CSS/JS references）
- [x] 7.2 建立 BranchController.Index()（查詢 IsVisible + SortOrder）
- [x] 7.3 建立 Views/Branch/Index.cshtml（Razor foreach render，沿用現有 CSS class）
- [x] 7.4 移除或重新命名原 branch.html，避免路由衝突
- [x] 7.5 測試前台頁面：視覺一致性、AOS 動畫、RWD

## Phase 8: Testing & Deployment Prep
- [x] 8.1 本機完整測試：登入、CRUD、排序、顯隱、圖片上傳、前台顯示
- [x] 8.2 確認 Web.config 的 connection string 可切換（開發 vs 虛擬主機）
- [x] 8.3 Publish 設定（FTP 或 Web Deploy profile）
- [x] 8.4 部署到虛擬主機，確認 uploads/ 資料夾有寫入權限
- [x] 8.5 首次啟動驗證：AutoMigration + Seed 執行成功
- [x] 8.6 修改預設 admin 密碼
