
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports osi.root.connector

' Support static functions only, otherwise consumers can get the type from Object.GetType().
Public NotInheritable Class typeless_invoker(Of delegate_t)
    Public Shared Function [New](ByVal assembly As Assembly,
                                 ByVal type_name As String,
                                 ByVal assembly_name As String,
                                 ByVal binding_flags As BindingFlags,
                                 ByVal name As String,
                                 ByVal suppress_error As Boolean,
                                 ByRef o As invoker(Of delegate_t)) As Boolean
        Dim t As Type = Nothing
        Return t.[New](assembly, type_name, assembly_name) AndAlso
               invoker(Of delegate_t).[New](t, binding_flags, Nothing, name, suppress_error, o)
    End Function

    Public Shared Function [New](ByVal assembly As Assembly,
                                 ByVal type_name As String,
                                 ByVal assembly_name As String,
                                 ByVal binding_flags As BindingFlags,
                                 ByVal name As String,
                                 ByVal suppress_error As Boolean) As invoker(Of delegate_t)
        Dim r As invoker(Of delegate_t) = Nothing
        assert([New](assembly, type_name, assembly_name, binding_flags, name, suppress_error, r))
        Return r
    End Function

    Private Sub New()
    End Sub
End Class
