# Spec: Branch CRUD

## Overview
管理員可透過後台介面對院所資料進行新增、檢視、編輯、刪除操作。

## Requirements

### 列表頁 (GET /Admin/Branches)
- 顯示所有院所（含隱藏的），按 SortOrder 排序
- 每筆顯示：縮圖、名稱、電話、IsVisible 狀態
- 提供「新增院所」按鈕
- 提供每筆的「編輯」連結
- 提供 IsVisible toggle 按鈕（Ajax）
- 提供 drag & drop 排序（SortableJS + Ajax）

### 新增 (GET/POST /Admin/Branches/Create)
- 表單欄位：名稱（必填）、電話、營業時間（textarea）、地址、地圖連結、圖片上傳
- 圖片限制：jpg/png/webp，上限 2MB
- 新增後 SortOrder 設為目前最大值 + 1（排到最後）
- 新增後 IsVisible 預設為 true
- 成功後導回列表頁

### 編輯 (GET/POST /Admin/Branches/Edit/{id})
- 載入現有資料填入表單
- 圖片：顯示目前圖片 preview，可上傳新圖（取代舊圖）或移除
- 上傳新圖時刪除舊圖檔案
- 成功後導回列表頁

### 刪除 (POST /Admin/Branches/Delete/{id})
- 需確認對話框（前端 confirm）
- 刪除 DB 記錄
- 刪除對應的圖片檔案
- 成功後導回列表頁

### 排序 (POST /Admin/Branches/Reorder)
- 接收 JSON：排序後的 id 陣列
- 依序更新每筆的 SortOrder
- 回傳 200 JSON

### 顯隱 (POST /Admin/Branches/Toggle/{id})
- 翻轉該筆的 IsVisible
- 回傳新的 IsVisible 狀態 JSON

## Validation
- Name：必填，最長 100 字
- Phone：最長 50 字
- Hours：最長 500 字
- Address：最長 200 字
- MapUrl：最長 500 字，需為合法 URL 格式
- Image：副檔名白名單（.jpg, .jpeg, .png, .webp），大小 ≤ 2MB
