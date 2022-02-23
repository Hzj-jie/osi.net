
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports osi.root.constants

Public NotInheritable Class delegate_info(Of T)
    Private Shared ReadOnly invoke As MethodInfo = calculate_invoke()

    Private Shared Function calculate_invoke() As MethodInfo
        assert(GetType(T).is(GetType([Delegate])))
        Dim invoke As MethodInfo = GetType(T).GetMethod("Invoke", binding_flags.instance_public_method)
        assert(invoke IsNot Nothing)
        Return invoke
    End Function

    ' Return true if one delegate of T can be created from mi.
    Public Shared Function match(ByVal mi As MethodInfo) As Boolean
        If mi Is Nothing Then
            Return False
        End If

        If array_size(invoke.GetParameters()) <> array_size(mi.GetParameters()) Then
            Return False
        End If

        If Not mi.ReturnType().is(invoke.ReturnType()) Then
            Return False
        End If

        For i As Int32 = 0 To array_size_i(invoke.GetParameters()) - 1
            If Not invoke.GetParameters()(i).ParameterType().is(mi.GetParameters()(i).ParameterType()) Then
                Return False
            End If
        Next

        Return True
    End Function

    Private Sub New()
    End Sub
End Class
