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
- admin / admin123（Seed 建立，部署後應立即修改）
