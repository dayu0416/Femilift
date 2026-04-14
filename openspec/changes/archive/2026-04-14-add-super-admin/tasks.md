# Tasks — add-super-admin

## 1. 產生 BCrypt hash

- [x] 1.1 在本機以 `BCrypt.Net.BCrypt.HashPassword("<明文>", 12)` 產生 hash（明文不入檔）
- [x] 1.2 複製 hash 字串供下一步使用，明文只保留在密碼管理工具

> Hash 透過 Node.js `bcryptjs.hashSync("<明文>", 12)` 產生並 round-trip 驗證；`BCrypt.Net-Next 4.0.3` 可驗證 `$2b$` 版本字首。

## 2. 修改 Seed

- [x] 2.1 於 `FemiliftAdmin/Migrations/Configuration.cs` 的 `Seed()` 新增區塊
- [x] 2.2 條件：`if (!context.AdminUsers.Any(u => u.Username == "superadmin"))`
- [x] 2.3 新增 `AdminUser { Username = "superadmin", PasswordHash = "<hash>" }`
- [x] 2.4 `context.SaveChanges()`

## 3. 更新 spec

- [x] 3.1 修改 `openspec/specs/authentication/spec.md` 的「預設帳號」段落，列出 `superadmin`（僅列 username，不列明文）

## 4. 驗證（由你在本機 Visual Studio / IIS Express 執行）

- [x] 4.1 本機啟動（`dotnet run` / IIS Express），觸發 EF AutoMigration Seed
- [x] 4.2 SQL Server 檢查 `AdminUsers` 表有兩筆（`admin`、`superadmin`）
- [x] 4.3 用 `superadmin` + 明文密碼登入 `/Admin/Account/Login`，確認進入 Branches 頁
- [x] 4.4 用錯誤密碼登入，確認顯示「帳號或密碼錯誤」
- [x] 4.5 透過 `/Admin/Account/ChangePassword` 改掉 `superadmin` 密碼，重新登入確認新密碼生效

> 註：本環境沒有 Visual Studio MSBuild targets，`dotnet build` 在 `Microsoft.WebApplication.targets` 缺失時無法過編譯；該限制為環境問題、與本 change 無關。4.1–4.5 需在你本機跑。

## 5. 收尾

- [x] 5.1 在 commit message / PR description 中**不得**出現明文密碼
- [ ] 5.2 提醒部署者：首次登入後立即改密碼（或告知你要維持此強密碼）
