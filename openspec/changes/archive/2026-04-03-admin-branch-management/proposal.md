# Admin Branch Management System

## Problem

菲蜜莉雷射網站的「院所資訊」頁面（branch.html）目前是純靜態 HTML，包含 13 間認證院所的資料。每次需要新增、修改、刪除院所時，都必須手動編輯 HTML 原始碼再部署，對非技術人員不友好，也容易出錯。

## Proposal

建立一個簡易的後台管理系統，讓管理員可以透過 Web 介面管理院所資料。採用 ASP.NET MVC 5 + SQL Server + Entity Framework 6 架構，部署在現有的 Windows 虛擬主機上。

### Scope

**In scope:**
- 後台登入系統（單一 admin，Session Cookie 驗證）
- 院所 CRUD（新增、編輯、刪除）
- 院所圖片獨立上傳（存 wwwroot/uploads/branches/）
- 院所排序（SortableJS drag & drop + Ajax）
- 院所顯隱切換（IsVisible toggle）
- 前台 /Branch 頁面由 MVC Razor 動態 render
- 其他頁面維持靜態 HTML 由 StaticFiles serve
- 改密碼功能
- DB Seed（既有 13 筆院所資料 + 圖片）

**Out of scope (future):**
- 部落格管理
- Q&A 管理
- 影音專區管理
- 多使用者 / 角色權限

### Non-goals
- 不做前台的重新設計，維持現有視覺風格
- 不做 API（後台用傳統 form post + 局部 Ajax）
- 不做 CMS 等級的通用內容管理

## Decisions

| 項目 | 決策 | 備註 |
|------|------|------|
| 框架 | ASP.NET MVC 5 / .NET Framework 4.x | 虛擬主機支援 |
| 資料庫 | SQL Server（帳密認證） | 虛擬主機不支援 Integrated Security |
| ORM | Entity Framework 6 + AutoMigration | MigrateDatabaseToLatestVersion，部署時自動建表 |
| 圖片儲存 | wwwroot/uploads/branches/ | IIS 直接 serve，需確認寫入權限 |
| 營業時間 | Textarea，\n → `<br>` render | |
| 排序 | SortableJS drag & drop + Ajax reorder | |
| 顯隱 | IsVisible bit，toggle 按鈕 | |
| 後台驗證 | 單一 admin，Session Cookie | InProc Session，App Pool 回收會踢出登入 |
| 前台 URL | /Branch（MVC routing） | 取代原 branch.html |
| 其他頁面 | 靜態 HTML 放 wwwroot | StaticFiles middleware serve |
| 後台結構 | Area 式分層，_AdminLayout.cshtml | 側邊選單：院所管理 + 改密碼/登出 |
| Seed | EF Seed 13 筆資料 + 複製圖片 | |
| 部署 | FTP / Web Deploy → 虛擬主機 | |
