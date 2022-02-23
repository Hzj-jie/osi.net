
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.constants

Public Module _copy
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function copy_clone(Of T)(ByRef dest As T, ByVal src As T) As Boolean
#If DEBUG Then
        assert(type_info(Of T).new_object_clone() IsNot Nothing)
        assert(src IsNot Nothing)
#End If
        Return type_info(Of T).new_object_clone()(src, dest)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function copy(Of T)(ByRef dest As T, ByVal src As T) As Boolean
        If src Is Nothing Then
            dest = Nothing
            Return False
        End If

        Return type_info(Of T).dominated_clone()(src, dest)
    End Function

    Private Function copy(Of T)(ByVal i As T, ByVal report_not_cloneable As Boolean) As T
        If type_info(Of T).new_object_clone() Is Nothing AndAlso report_not_cloneable AndAlso isdebugmode() Then
            raise_error(error_type.exclamation, "input is not cloneable, type ", GetType(T).full_name())
        End If
        Dim r As T = Nothing
        copy(r, i)
        Return r
    End Function

    Public Function copy(Of T)(ByVal i As T) As T
        Return copy(i, True)
    End Function

    Public Function copy_no_error(Of T)(ByVal i As T) As T
        Return copy(i, False)
    End Function
End Module
