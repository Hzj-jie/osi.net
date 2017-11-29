
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports osi.root.connector

Public NotInheritable Class typeless_invoker
    Public Shared Function [New](Of delegate_t)(ByVal type_name As String,
                                                ByVal binding_flags As BindingFlags,
                                                ByVal name As String,
                                                ByVal suppress_error As Boolean,
                                                ByRef r As invoker(Of delegate_t)) As Boolean
        r = typeless_invoker(Of delegate_t).[New](type_name, binding_flags, name, suppress_error)
        Return Not r Is Nothing
    End Function

    Public Shared Function [New](Of delegate_t)(ByVal type_name As String,
                                                ByVal binding_flags As BindingFlags,
                                                ByVal name As String,
                                                ByRef r As invoker(Of delegate_t)) As Boolean
        r = typeless_invoker(Of delegate_t).[New](type_name, binding_flags, name)
        Return Not r Is Nothing
    End Function

    Public Shared Function [New](Of delegate_t)(ByVal type_name As String,
                                                ByVal name As String,
                                                ByVal suppress_error As Boolean,
                                                ByRef r As invoker(Of delegate_t)) As Boolean
        r = typeless_invoker(Of delegate_t).[New](type_name, name, suppress_error)
        Return Not r Is Nothing
    End Function

    Public Shared Function [New](Of delegate_t)(ByVal type_name As String,
                                                ByVal name As String,
                                                ByRef r As invoker(Of delegate_t)) As Boolean
        r = typeless_invoker(Of delegate_t).[New](type_name, name)
        Return Not r Is Nothing
    End Function

    Private Sub New()
    End Sub
End Class

' Support static functions only, otherwise consumers can get the type from Object.GetType().
Public NotInheritable Class typeless_invoker(Of delegate_t)
    Private Shared Function [New](ByVal type_name As String,
                                  ByVal r As Func(Of Type, invoker(Of delegate_t))) As invoker(Of delegate_t)
        assert(Not r Is Nothing)
        Dim t As Type = Nothing
        t = Type.GetType(type_name)
        If t Is Nothing Then
            Return Nothing
        Else
            Return r(t)
        End If
    End Function

    Public Shared Function [New](ByVal type_name As String,
                                 ByVal binding_flags As BindingFlags,
                                 ByVal name As String,
                                 ByVal suppress_error As Boolean) As invoker(Of delegate_t)
        Return [New](type_name,
                     Function(ByVal t As Type) As invoker(Of delegate_t)
                         Return New invoker(Of delegate_t)(t, binding_flags, name, suppress_error)
                     End Function)
    End Function

    Public Shared Function [New](ByVal type_name As String,
                                 ByVal binding_flags As BindingFlags,
                                 ByVal name As String) As invoker(Of delegate_t)
        Return [New](type_name,
                     Function(ByVal t As Type) As invoker(Of delegate_t)
                         Return New invoker(Of delegate_t)(t, binding_flags, name)
                     End Function)
    End Function

    Public Shared Function [New](ByVal type_name As String,
                                 ByVal name As String,
                                 ByVal suppress_error As Boolean) As invoker(Of delegate_t)
        Return [New](type_name,
                     Function(ByVal t As Type) As invoker(Of delegate_t)
                         Return New invoker(Of delegate_t)(t, name, suppress_error)
                     End Function)
    End Function

    Public Shared Function [New](ByVal type_name As String, ByVal name As String) As invoker(Of delegate_t)
        Return [New](type_name,
                     Function(ByVal t As Type) As invoker(Of delegate_t)
                         Return New invoker(Of delegate_t)(t, name)
                     End Function)
    End Function

    Private Sub New()
    End Sub
End Class
