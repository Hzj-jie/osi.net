
setlocal
cd /d "%~dp0"
path %PATH%;..\..\resource\gen;
gen _data data data > data.vb
zipgen _zipdata zipdata data > zipdata.vb
endlocal

