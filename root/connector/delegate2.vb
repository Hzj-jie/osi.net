
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices

Public Module _delegate2
    Public Function delegate_is_pinning_object(ByVal d As [Delegate], ByVal o As Object) As Boolean
        Return Not d Is Nothing AndAlso
               Not o Is Nothing AndAlso
               Not d.Target() Is Nothing AndAlso
               (object_compare(d.Target(), o) = 0)
    End Function

    <Extension()> Public Function is_pinning_object(ByVal d As [Delegate], ByVal o As Object) As Boolean
        Return delegate_is_pinning_object(d, o)
    End Function

    <Extension()> Public Function type_restore(Of T) _
                                              (ByVal v As Action(Of T),
                                               Optional ByVal assert_on_cast_failure As Boolean = True) _
                                              As Action(Of Object)
        If v Is Nothing Then
            Return Nothing
        Else
            Return Sub(ByVal x As Object)
                       v(direct_cast(Of T)(x, assert_on_cast_failure))
                   End Sub
        End If
    End Function

    <Extension()> Public Function safe_invoke(ByVal method As [Delegate], ByVal args() As Object) As Object
        If method Is Nothing Then
            Return Nothing
        Else
            Return do_(AddressOf method.DynamicInvoke, args, Nothing)
        End If
    End Function

    ' This should be rarely used, a typical scenario is to run clean up only.
    <Extension()> Public Sub ignore_exceptions(ByVal v As Action)
        If Not v Is Nothing Then
            Try
                v()
            Catch
            End Try
        End If
    End Sub
End Module
