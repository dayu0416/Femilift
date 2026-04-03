# Branch Management（院所管理）

## DB Schema
```
Branches: Id, Name, Phone, Hours, Address, MapUrl, ImagePath, IsVisible, SortOrder, CreatedAt, UpdatedAt
```

## 後台功能
- **列表** GET /Admin/Branches — 所有院所，按 SortOrder 排序
- **新增** GET/POST /Admin/Branches/Create — 含圖片上傳
- **編輯** GET/POST /Admin/Branches/Edit/{id} — 含圖片替換/移除、IsVisible toggle
- **刪除** POST /Admin/Branches/Delete/{id} — 含刪除圖片
- **排序** POST /Admin/Branches/Reorder — Ajax，接收 id 陣列
- **顯隱** POST /Admin/Branches/Toggle/{id} — Ajax，翻轉 IsVisible

## 圖片上傳
- 存放：uploads/branches/{guid}{ext}
- 限制：jpg/png/webp，上限 2MB
- 替換時刪除舊檔

## 前台
- GET /Branch — 只顯示 IsVisible=true，按 SortOrder 排序
- 營業時間用 `<pre>` 保留換行
- 沿用原站 CSS class + AOS 動畫
