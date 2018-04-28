
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports osi.root.connector
Imports osi.root.constants

Public NotInheritable Class typeless_invoker
    Public Shared Function [New](Of delegate_t)(ByVal type_name As String,
                                                ByVal assembly_name As String,
                                                ByVal binding_flags As BindingFlags,
                                                ByVal name As String,
                                                ByVal suppress_error As Boolean,
                                                ByRef r As invoker(Of delegate_t)) As Boolean
        r = typeless_invoker(Of delegate_t).[New](type_name, assembly_name, binding_flags, name, suppress_error)
        Return Not r Is Nothing
    End Function

    Public Shared Function [New](Of delegate_t)(ByVal type_name As String,
                                                ByVal binding_flags As BindingFlags,
                                                ByVal name As String,
                                                ByVal suppress_error As Boolean,
                                                ByRef r As invoker(Of delegate_t)) As Boolean
        r = typeless_invoker(Of delegate_t).[New](type_name, binding_flags, name, suppress_error)
        Return Not r Is Nothing
    End Function

    Public Shared Function [New](Of delegate_t)(ByVal type_name As String,
                                                ByVal assembly_name As String,
                                                ByVal binding_flags As BindingFlags,
                                                ByVal name As String,
                                                ByRef r As invoker(Of delegate_t)) As Boolean
        r = typeless_invoker(Of delegate_t).[New](type_name, assembly_name, binding_flags, name)
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
                                                ByVal assembly_name As String,
                                                ByVal name As String,
                                                ByVal suppress_error As Boolean,
                                                ByRef r As invoker(Of delegate_t)) As Boolean
        r = typeless_invoker(Of delegate_t).[New](type_name, assembly_name, name, suppress_error)
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
                                                ByVal assembly_name As String,
                                                ByVal name As String,
                                                ByRef r As invoker(Of delegate_t)) As Boolean
        r = typeless_invoker(Of delegate_t).[New](type_name, assembly_name, name)
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
    Public Shared Function [New](ByVal type_name As String,
                                 ByVal assembly_name As String,
                                 ByVal binding_flags As BindingFlags,
                                 ByVal name As String,
                                 ByVal suppress_error As Boolean) As invoker(Of delegate_t)
        Dim t As Type = Nothing
        If t.[New](type_name, assembly_name) Then
            Return New invoker(Of delegate_t)(t, binding_flags, name, suppress_error)
        End If
        Return Nothing
    End Function

    Public Shared Function [New](ByVal type_name As String,
                                 ByVal binding_flags As BindingFlags,
                                 ByVal name As String,
                                 ByVal suppress_error As Boolean) As invoker(Of delegate_t)
        Return [New](type_name, default_str, binding_flags, name, suppress_error)
    End Function

    Public Shared Function [New](ByVal type_name As String,
                                 ByVal assembly_name As String,
                                 ByVal binding_flags As BindingFlags,
                                 ByVal name As String) As invoker(Of delegate_t)
        Dim t As Type = Nothing
        If t.[New](type_name, assembly_name) Then
            Return invoker.of(Of delegate_t).
                       with_type(t).
                       with_binding_flags(binding_flags).
                       with_name(name).
                       build()
        End If
        Return Nothing
    End Function

    Public Shared Function [New](ByVal type_name As String,
                                 ByVal binding_flags As BindingFlags,
                                 ByVal name As String) As invoker(Of delegate_t)
        Return [New](type_name, default_str, binding_flags, name)
    End Function

    Public Shared Function [New](ByVal type_name As String,
                                 ByVal assembly_name As String,
                                 ByVal name As String,
                                 ByVal suppress_error As Boolean) As invoker(Of delegate_t)
        Dim t As Type = Nothing
        If t.[New](type_name, assembly_name) Then
            Return invoker.of(Of delegate_t).
                       with_type(t).
                       with_name(name).
                       with_suppress_error(suppress_error).
                       build()
        End If
        Return Nothing
    End Function

    Public Shared Function [New](ByVal type_name As String,
                                 ByVal name As String,
                                 ByVal suppress_error As Boolean) As invoker(Of delegate_t)
        Return [New](type_name, default_str, name, suppress_error)
    End Function

    Public Shared Function [New](ByVal type_name As String,
                                 ByVal assembly_name As String,
                                 ByVal name As String) As invoker(Of delegate_t)
        Dim t As Type = Nothing
        If t.[New](type_name, assembly_name) Then
            Return invoker.of(Of delegate_t).
                       with_type(t).
                       with_name(name).
                       build()
        End If
        Return Nothing
    End Function

    Public Shared Function [New](ByVal type_name As String, ByVal name As String) As invoker(Of delegate_t)
        Return [New](type_name, default_str, name)
    End Function

    Private Sub New()
    End Sub
End Class
