
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.constants

Public Module _object_extensions
    <Extension()> Public Function get_type_or_null(Of T)(ByVal this As T) As Type
        Return If(this Is Nothing, Nothing, this.GetType())
    End Function

    <Extension()> Public Function is_null_or_true(Of T)(ByVal this As T) As Boolean
        Dim r As Boolean = Nothing
        Return this Is Nothing OrElse
               (direct_cast(this, r) AndAlso r)
    End Function

    <Extension()> Public Function is_not_null_and_true(Of T)(ByVal this As T) As Boolean
        Dim r As Boolean = Nothing
        Return this IsNot Nothing AndAlso
               direct_cast(this, r) AndAlso
               r
    End Function

    Private Interface is_null_should_be_logged
    End Interface

    <Extension()> Public Function is_null(Of T)(ByVal i As T) As Boolean
#If DEBUG Then
        If type_info(Of T).is_valuetype AndAlso
           typed_once_action(Of joint_type(Of is_null_should_be_logged, T)).first() Then
            raise_error(error_type.performance, "is_null(Of ", type_info(Of T).fullname, ") is not necessary.")
        End If
#End If
        Return i Is Nothing
    End Function
End Module
