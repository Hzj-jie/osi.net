
if not "x%*"=="x" (
    pushd %1
)
call "%~dp0\bd4.cmd"
call "%~dp0\br4.cmd"

if not "x%*"=="x" (
    popd
)

