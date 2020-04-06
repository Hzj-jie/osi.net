
setlocal
cd /d "%~dp0"

pushd math\codegen
call run.cmd
popd

endlocal
