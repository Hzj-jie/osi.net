
@echo off

setlocal

echo.
echo 'this file is generated by /osi/root/codegen/gitver/gitver.cmd
echo 'so change the script instead of this file
echo.
echo Partial Public NotInheritable Class gitver
echo     Public Shared ReadOnly latest_commit_raw = _
set FORMAT="CommitHash:%%H  |-+-|  ShortCommitHash:%%h  |-+-|  Author:%%an  |-+-|  AuthorEMail:%%ae  |-+-|  AuthorDate:%%ad  |-+-|  Committer:%%cn  |-+-|  CommitterEMail:%%ce  |-+-|  CommitterDate:%%cd  |-+-|  Subject:%%s  |-+-|  Body:%%b"
set gitver=""
if defined NO_GITVER (
  set gitver=""
) else (
  call git fetch origin >nul 2>&1
  for /F "delims=" %%i in ('git log -1 origin/master --pretty^=%FORMAT%') do (
    set gitver="%%i" )
)
echo         %gitver%.Trim()
echo     Public Shared ReadOnly current_commit_raw = _
set gitver=""
if defined NO_GITVER (
  set gitver=""
) else (
  for /F "delims=" %%i in ('git log -1 --pretty^=%FORMAT%') do (
    set gitver="%%i" )
)
echo         %gitver%.Trim()
echo End Class

endlocal
