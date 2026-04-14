# Design — add-super-admin

## 決策 1：不新增角色欄位
現有 `AdminUsers` 僅有 `Id / Username / PasswordHash`，所有管理員權限相同。此 change 的 `superadmin` 僅是「另一組強密碼管理員」，不代表更高權限。

**Why:** 最小破壞面積；目前後台功能對不同角色沒有差異化需求，加 `Role` 欄位是過度設計。
**Trade-off:** 日後若出現「只有超管能改別人密碼」這類需求，需另開 change 引入角色。

## 決策 2：Seed 只存 BCrypt hash，不存明文
`Configuration.cs` 現行寫法 `BCrypt.HashPassword("admin123")` 把明文留在原始碼。`superadmin` 改為直接寫入**預先計算好的 hash 字串常數**，明文永不進 Git。

**Why:** `Web.config` 未被 `.gitignore` 排除；appSettings / 原始碼內的任何明文都等同公開。寫死 hash 是在不增加部署流程的前提下避免明文外洩的最簡解。
**Trade-off:** 若密碼需要輪換，必須重新產生 hash 並改 Seed + 提交；但備援帳號本就不該頻繁輪換，首次登入後使用者也應立即透過 `ChangePassword` 自行改掉，之後 hash 就跟 Seed 脫鉤。

### Hash 產生方式（實作時）
使用 BCrypt.Net 相同的套件（專案已引用）：
```csharp
// 一次性執行（例如 LINQPad、單元測試、或 dotnet script），取得輸出字串後貼到 Seed
var hash = BCrypt.Net.BCrypt.HashPassword("F3mi!Lift@Admin#2026_X9", workFactor: 12);
```
- workFactor 預設 10；這裡可提到 12 強化成本（現有 `admin` 可維持 10，不動）。
- 明文不寫入任何檔案；僅透過聊天／密碼管理工具交付。

## 決策 3：Seed 僅在 username 不存在時建立
```csharp
if (!context.AdminUsers.Any(u => u.Username == "superadmin"))
{
    context.AdminUsers.Add(new AdminUser
    {
        Username = "superadmin",
        PasswordHash = "<pre-computed-bcrypt-hash>"
    });
    context.SaveChanges();
}
```

**Why:** 與現行 `admin` Seed 一致（`!context.AdminUsers.Any()`）；保證已上線環境不會被 Seed 覆寫使用者自行改過的密碼。
**Trade-off:** 注意目前 `admin` Seed 用的是 `Any()`（無任何使用者時才建），`superadmin` 改用 `Any(u => u.Username == ...)`（更精準）。兩種判斷可同時存在，互不干擾。

## 決策 4：不改 Login / 權限流程
`AccountController.Login` 既有 BCrypt 驗證流程對 `superadmin` 自然適用；`AdminAuthAttribute` 只看 Session 是否有 `AdminUserId`。故實作只需改 Seed。

## 風險
| 風險 | 緩解 |
|------|------|
| 明文被貼到 commit/PR description | 實作階段不把明文寫入任何檔案；PR 說明只提 username |
| Hash 被離線暴力破解 | 用 22 字元高強度密碼 + BCrypt cost 12，實務上不可行 |
| 部署者不知道密碼 | 由開發者透過安全管道交付（1Password、加密訊息等），spec 註明出處不在 repo |
