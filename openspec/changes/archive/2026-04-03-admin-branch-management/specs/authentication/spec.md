# Spec: Authentication

## Overview
後台使用 Session Cookie 驗證，單一管理員帳號。

## Requirements

### 登入 (GET/POST /Admin/Account/Login)
- 表單：Username + Password
- 驗證：BCrypt.Verify 比對 AdminUsers 表
- 成功：Session["AdminUserId"] = user.Id，redirect 到 /Admin/Branches
- 失敗：顯示「帳號或密碼錯誤」，不透露是哪個錯
- 已登入狀態訪問 Login 頁面，自動 redirect 到 /Admin/Branches

### 登出 (GET /Admin/Account/Logout)
- Session.Abandon()
- Redirect 到 Login 頁面

### 改密碼 (GET/POST /Admin/Account/ChangePassword)
- 需登入才能存取
- 表單：目前密碼、新密碼、確認新密碼
- 驗證目前密碼正確
- 新密碼與確認需一致
- 新密碼最短 6 字元
- 成功後更新 PasswordHash，顯示成功訊息

### AdminAuth Filter
- 自訂 ActionFilterAttribute
- 檢查 Session["AdminUserId"] 是否存在
- 不存在則 redirect 到 /Admin/Account/Login
- 套用在所有 Admin 下的 controller（除了 AccountController 的 Login/Logout）

### Seed 預設帳號
- Username: admin
- Password: 預設密碼（首次部署後應立即更改）
- 儲存為 BCrypt hash

## Security
- BCrypt hash（BCrypt.Net-Next NuGet）
- Anti-forgery token on all POST forms
- Session timeout: 60 minutes
- 登入失敗不透露具體原因
