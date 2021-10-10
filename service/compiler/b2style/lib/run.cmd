
setlocal
path %PATH%;..\..\..\resource\gen\;
zipgen.exe b2style_lib b2style_h b2style.h b2style_operators_h b2style_operators.h b2style_stdio_h b2style_stdio.h b2style_ufloat_h b2style_ufloat.h b2style_types_h b2style_types.h > b2style_lib.vb
move /Y *.vb ..\
endlocal
