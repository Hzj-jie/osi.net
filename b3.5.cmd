
if not "x%*"=="x" (
    pushd %1
)
call "%~dp0\bd3.5.cmd"
call "%~dp0\br3.5.cmd"

if not "x%*"=="x" (
    popd
)

