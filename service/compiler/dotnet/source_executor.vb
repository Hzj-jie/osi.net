
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports osi.root.connector
Imports osi.root.utils

Namespace dotnet
    Partial Public Class source_executor
        Private ReadOnly result As Assembly

        Public Enum language
            vbnet
            cs
        End Enum

        Private Sub New(ByVal result As Assembly)
            assert(Not result Is Nothing)
            Me.result = result
        End Sub

        Public Shared Function [New]() As builder
            Return New builder()
        End Function

        Public Function invoker(ByVal type_name As String,
                                ByVal function_name As String,
                                ByRef o As invoker(Of not_resolved_type_delegate)) As Boolean
            Return invoker(Of not_resolved_type_delegate)(type_name, function_name, o)
        End Function

        Public Function invoker(Of delegate_t)(ByVal type_name As String,
                                               ByVal function_name As String,
                                               ByRef o As invoker(Of delegate_t)) As Boolean
            Return typeless_invoker.of(o).
                with_assembly(result).
                with_type_name(type_name).
                with_name(function_name).
                build(o)
        End Function

        Public Function functor(ByVal type_name As String,
                                ByVal function_name As String,
                                ByRef o As Func(Of Object(), Object)) As Boolean
            Dim i As invoker(Of not_resolved_type_delegate) = Nothing
            Return invoker(type_name, function_name, i) AndAlso
                   i.pre_or_post_alloc_raw_bind(o)
        End Function

        Public Function functor(Of T)(ByVal type_name As String,
                                      ByVal function_name As String,
                                      ByRef o As Func(Of Object(), T)) As Boolean
            Dim i As invoker(Of not_resolved_type_delegate) = Nothing
            Return invoker(type_name, function_name, i) AndAlso
                   pre_or_post_alloc_raw_bind(i, o)
        End Function

        Public Function execute(ByVal type_name As String,
                                ByVal function_name As String,
                                ByRef o As Object,
                                ByVal ParamArray parameters() As Object) As Boolean
            Dim i As invoker(Of not_resolved_type_delegate) = Nothing
            If Not invoker(type_name, function_name, i) Then
                Return False
            End If
            Return i.pre_or_post_alloc_invoke(o, parameters)
        End Function

        Public Function execute(ByVal type_name As String,
                                ByVal function_name As String,
                                ByVal ParamArray parameters() As Object) As Object
            Dim o As Object = Nothing
            assert(execute(type_name, function_name, o, parameters))
            Return o
        End Function
    End Class
End Namespace
