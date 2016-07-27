﻿
Imports osi.root.constants

Public Module _raise_error
    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
    Public Sub raise_error(ByVal errmsg As String,
                           Optional ByVal errtype As error_type = error_type.information,
                           Optional ByVal errtype_char As Char = character.null,
                           Optional ByVal additional_jump As Int32 = 0)
        error_event.R(errtype, errtype_char, {errmsg}, additional_jump + 1)
    End Sub

    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
    Public Sub raise_error(ByVal errtype As error_type,
                           ByVal errtype_char As Char,
                           ByVal additional_jump As Int32,
                           ByVal ParamArray os() As Object)
        error_event.R(errtype, errtype_char, os, additional_jump + 1)
    End Sub

    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
    Public Sub raise_error(ByVal err_type As error_type,
                           ByVal errtype_char As Char,
                           ByVal ParamArray os() As Object)
        error_event.R(err_type, errtype_char, os, 1)
    End Sub

    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
    Public Sub raise_error(ByVal err_type As error_type,
                           ByVal ParamArray os() As Object)
        error_event.R(err_type, character.null, os, 1)
    End Sub

    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
    Public Sub raise_error(ByVal ParamArray os() As Object)
        error_event.R(error_type.information, character.null, os, 1)
    End Sub
End Module
