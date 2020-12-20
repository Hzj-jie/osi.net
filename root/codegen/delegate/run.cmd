
call ..\buildcpp.cmd delegate.cpp
call "delegate.exe" "delegate.vb"
move /y "delegate.vb" ..\..\connector\delegate\

