
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports osi.root.connector

Partial Public NotInheritable Class typeless_invoker
    Public Shared Function [of](Of delegate_t)() As builder(Of delegate_t)
        Return New builder(Of delegate_t)()
    End Function

    Public Shared Function [of](Of delegate_t)(ByVal invoker As invoker(Of delegate_t)) As builder(Of delegate_t)
        Return [of](Of delegate_t)()
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
                                 ByVal suppress_error As Boolean,
                                 ByRef o As invoker(Of delegate_t)) As Boolean
        Dim t As Type = Nothing
        Return t.[New](type_name, assembly_name) AndAlso
               invoker.of(Of delegate_t).
                   with_type(t).
                   with_binding_flags(binding_flags).
                   with_name(name).
                   with_suppress_error(suppress_error).
                   build(o)
    End Function

    Public Shared Function [New](ByVal type_name As String,
                                 ByVal assembly_name As String,
                                 ByVal binding_flags As BindingFlags,
                                 ByVal name As String,
                                 ByVal suppress_error As Boolean) As invoker(Of delegate_t)
        Dim r As invoker(Of delegate_t) = Nothing
        assert([New](type_name, assembly_name, binding_flags, name, suppress_error, r))
        Return r
    End Function

    Private Sub New()
    End Sub
End Class
