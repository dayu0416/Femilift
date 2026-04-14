# Authentication — Delta

## ADDED Requirements

### Requirement: Super Admin 備援帳號
系統 SHALL 在 Seed 階段建立一組備援管理員帳號 `superadmin`，使用 22 字元以上的強密碼，僅存 BCrypt hash（cost 12），明文不進版控。

#### Scenario: 全新資料庫首次 Seed
- **WHEN** EF AutoMigration 執行 `Seed()` 且 `AdminUsers` 表中不存在 `superadmin`
- **THEN** 系統建立 `superadmin` 帳號，`PasswordHash` 為預先計算好的 BCrypt hash 字串常數

#### Scenario: 既有環境重啟（superadmin 已存在）
- **WHEN** `Seed()` 執行且 `AdminUsers` 表中已存在 `superadmin`（例如已被使用者改過密碼）
- **THEN** 系統不覆寫其 `PasswordHash`，保留使用者目前設定

#### Scenario: superadmin 登入
- **WHEN** 使用者於 `/Admin/Account/Login` 以 `superadmin` + 正確明文密碼登入
- **THEN** BCrypt 驗證通過，Session 設定 `AdminUserId` / `AdminUsername`，導向 `/Admin/Branches`

#### Scenario: superadmin 擁有與 admin 相同權限
- **WHEN** 已登入的 `superadmin` 存取 `/Admin/*` 任一受 `AdminAuth` 保護的頁面
- **THEN** 視同一般 admin 放行，無額外權限差異

## MODIFIED Requirements

### Requirement: 預設帳號
Seed SHALL 建立兩組管理員帳號（`admin`、`superadmin`），兩者在系統內權限完全相同（無角色概念），並且 MUST 透過 `AdminAuthAttribute` 驗證 Session。

#### Scenario: 列出預設帳號
- **WHEN** 查詢全新環境 Seed 完成後的 `AdminUsers` 表
- **THEN** 包含 `admin`（歷史預設，密碼 `admin123`，部署後應立即修改）與 `superadmin`（備援強密碼帳號，明文由開發者安全交付，不存在於 repo）

#### Scenario: Seed 的建立條件
- **WHEN** Seed 判斷是否要建立某組預設帳號
- **THEN** 使用 per-username 判斷（`!context.AdminUsers.Any(u => u.Username == "<name>")`），已上線環境自行改過的密碼不會被覆寫
