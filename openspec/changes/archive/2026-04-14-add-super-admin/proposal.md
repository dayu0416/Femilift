# Add Super Admin（備援管理員帳號）

## Why
目前 Seed 只建立一組預設帳號 `admin / admin123`（密碼過弱，spec 亦註明「部署後應立即修改」）。若部署者忘記修改、或將 `admin` 密碼改掉後遺失，就失去進入後台的途徑。需要一組**強密碼的備援管理員**，在任何情況下都能登入，不依賴 `admin` 帳號的狀態。

## What Changes
- Seed 額外建立一組管理員：username `superadmin`，密碼為一組 22 字元強密碼（僅存 BCrypt hash，明文不進 Git）。
- 不改動 schema、不引入角色欄位；`superadmin` 與 `admin` 在權限上完全一樣（走現有 `AdminAuth` Filter）。
- 僅在「`superadmin` 不存在時」建立，避免覆蓋已被改過密碼的現有帳號。
- 更新 `authentication` spec 的「預設帳號」段落，列出 `superadmin`。

## Non-goals
- ❌ 不加 `Role` / `IsSuperUser` 欄位。
- ❌ 不做 RBAC（角色權限），一律視為 admin。
- ❌ 不自動淘汰 `admin` 帳號。
- ❌ 不把密碼明文寫進任何被版控的檔案（Web.config、Seed、spec 皆不得含明文）。

## Impact
- 影響 spec：`authentication`
- 影響程式碼：`FemiliftAdmin/Migrations/Configuration.cs`（Seed）
- 部署影響：既有環境啟動後會自動 Seed 出 `superadmin`；密碼須由開發者另行透過安全管道交付給部署者，並要求首次登入後以 `ChangePassword` 立即改掉。
