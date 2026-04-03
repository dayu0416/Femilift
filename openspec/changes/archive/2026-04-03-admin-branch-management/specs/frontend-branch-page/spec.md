# Spec: Frontend Branch Page

## Overview
將原靜態 branch.html 改為 MVC Razor View，從 DB 動態 render。

## Requirements

### Route
- GET /Branch → BranchController.Index()
- 取代原本的 branch.html（移除或重新命名原檔避免衝突）

### 資料
- 查詢 Branches 表，WHERE IsVisible = 1，ORDER BY SortOrder
- 傳入 View 作為 model

### View
- 沿用現有 branch.html 的完整 HTML 結構（header, footer, CSS, JS）
- `_Layout.cshtml` 抽取共用的 header/footer/scripts
- `Branch/Index.cshtml` 用 `@foreach` 迴圈 render `.list-item`
- 營業時間：`@Html.Raw(Model.Hours.Replace("\n", "<br>"))`
- 圖片：`<img src="/@Model.ImagePath" alt="@Model.Name">`
- 維持 AOS (fade-up) 動畫效果
- 維持 parallax banner

### 靜態頁面共存
- index.html, intelligence.html, blog_*.html, media.html, question.html 維持靜態
- 在 RouteConfig 中設定 StaticFiles 優先，MVC routing 其次
- 確保 /assets/ 和 /uploads/ 路徑不被 MVC routing 攔截
