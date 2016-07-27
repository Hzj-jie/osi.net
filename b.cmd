
if not "x%*"=="x" (
    pushd %1
)
call "%~dp0\bd.cmd"
call "%~dp0\br.cmd"

if not "x%*"=="x" (
    popd
)

