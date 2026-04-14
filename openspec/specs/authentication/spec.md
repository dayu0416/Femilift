# Authentication（後台驗證）

## DB Schema
```
AdminUsers: Id, Username, PasswordHash(BCrypt)
```

## 功能
- **登入** GET/POST /Admin/Account/Login — BCrypt 驗證，Session["AdminUserId"]
- **登出** GET /Admin/Account/Logout — Session.Abandon()
- **改密碼** GET/POST /Admin/Account/ChangePassword — 驗證舊密碼，新密碼最短 6 字元

## 驗證機制
- AdminAuthAttribute（ActionFilter）檢查 Session
- Session InProc，timeout 60 分鐘
- 所有 /Admin/* 路由需登入（除 Login/Logout）

## 預設帳號
Seed 建立兩組管理員（權限相同、無角色概念，均透過 `AdminAuthAttribute` 驗證 Session）：

- `admin / admin123` — 歷史預設，部署後應立即修改。Seed 條件：`AdminUsers` 表無任何使用者時建立。
- `superadmin` — 備援管理員，22 字元強密碼；原始碼僅存 BCrypt hash（cost 12），明文不進 Git，由開發者透過安全管道交付部署者。Seed 條件：`!AdminUsers.Any(u => u.Username == "superadmin")`，已改過密碼的環境不會被覆寫。
