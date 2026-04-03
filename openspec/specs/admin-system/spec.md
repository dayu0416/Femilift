# Admin System

## Overview
菲蜜莉雷射網站後台管理系統。

## Tech Stack
- ASP.NET MVC 5 / .NET Framework 4.8
- SQL Server（虛擬主機帳密認證）
- Entity Framework 6 + AutoMigration
- Bootstrap 5（後台 UI）
- jQuery 3.7
- SortableJS（拖曳排序）

## Project Structure
```
FemiliftAdmin/FemiliftAdmin/
├── Controllers/Admin/     ← 後台 controllers
├── Controllers/           ← 前台 controllers
├── Models/                ← Entity + DbContext
├── Models/ViewModels/     ← 表單 ViewModels
├── Filters/               ← AdminAuthAttribute
├── Migrations/            ← EF Seed
├── Views/Admin/           ← 後台頁面
├── Views/Branch/          ← 前台院所頁
├── Views/Shared/          ← _AdminLayout + _Layout
├── Content/               ← admin.css
├── Scripts/               ← admin.js
├── uploads/branches/      ← 院所圖片上傳目錄
└── *.html + assets/       ← 靜態頁面
```

## Deployment
- 虛擬主機（Windows Server + IIS）
- FTP / Web Deploy
- Web.Debug.config → LocalDB（開發）
- Web.Release.config → 虛擬主機 SQL Server
- 首次啟動自動建表 + Seed
