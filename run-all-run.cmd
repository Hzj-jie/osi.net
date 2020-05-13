
@echo off
setlocal
for /f "delims=" %%x in ('dir run.cmd /s /b') do (
  echo.%%x | find /i "\.git\" 1>nul || (
  echo.%%x | find /i "\.vs\" 1>nul || (
  echo.%%x | find /i "\bin\" 1> nul || (
  echo.%%x | find /i "\obj\" 1> nul || (
  echo.%%x | find /i "%cd%\run.cmd" 1> nul || (
  echo.%%x | find /i "\production\" 1> nul || (
    echo Running in %%~dpx
    pushd "%%~dpx"
    call run.cmd
    popd
  ))))))
)
endlocal
