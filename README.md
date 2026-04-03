# Femilift AI Coding Workspace

這一層是 Femilift 的 AI-coding 專案工作區。

開發時請直接把這一層資料夾拉進 VS Code，並依 `openspec` 的 AI-coding 流程進行：

1. 先在 `openspec/` 內整理需求、提案、設計與任務。
2. 再依規格進行實作與調整。
3. 開發過程中的協作、技能與輔助指令，集中放在 `.claude/`。

## 目錄說明

- `.claude/`：AI-coding 相關技能與指令
- `openspec/`：OpenSpec 規格、變更與任務記錄
- `FemiliftAdmin/`：主要專案程式碼

## 開發方式

- 用 VS Code 開啟這一層作為工作根目錄。
- 以 OpenSpec 驅動需求拆解、設計與實作。
- 變更完成後再檢查 git diff、commit、push。
