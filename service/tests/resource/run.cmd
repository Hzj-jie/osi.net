
setlocal
cd /d "%~dp0"
path %PATH%;..\..\resource\gen;
gen _data data data > data.do_not_parse_as_vb
zipgen _zipdata zipdata data > zipdata.do_not_parse_as_vb
endlocal

