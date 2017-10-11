
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports System.Threading
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
        Return Not this Is Nothing AndAlso
               direct_cast(this, r) AndAlso
               r
    End Function

    Private NotInheritable Class is_null_should_log(Of T)
        Private Const logged As Int32 = 1
        Private Const not_logged As Int32 = 0
        Private Shared v As Int32 = not_logged

        Public Shared Function [get]() As Boolean
            Return Interlocked.CompareExchange(v, logged, not_logged) = not_logged
        End Function

        Private Sub New()
        End Sub
    End Class

    <Extension()> Public Function is_null(Of T)(ByVal i As T) As Boolean
#If DEBUG Then
        If type_info(Of T).is_valuetype AndAlso is_null_should_log(Of T).get() Then
            raise_error(error_type.performance, "is_null(Of ", type_info(Of T).fullname, ") is not necessary.")
        End If
#End If
        Return i Is Nothing
    End Function
End Module
