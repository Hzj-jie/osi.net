
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports osi.root.connector

Public NotInheritable Class invoker
    Inherits invocable(Of Func(Of Object(), Object))

    Public Delegate Sub undefined_delegate_type()
    Private ReadOnly impl As invoker(Of undefined_delegate_type)
    Private ReadOnly m As Func(Of Object(), Object)

    Public Shared Function [New](ByVal t As Type,
                                 ByVal binding_flags As BindingFlags,
                                 ByVal obj As Object,
                                 ByVal name As String,
                                 ByVal suppress_error As Boolean,
                                 ByRef o As invoker) As Boolean
        Dim impl As invoker(Of undefined_delegate_type) = Nothing
        If invoker(Of undefined_delegate_type).[New](t, binding_flags, obj, name, suppress_error, impl) Then
            assert(impl.invoke_only())
            Dim m As Func(Of Object(), Object) = Nothing
            If impl.instance_invocable() Then
                m = AddressOf impl.instance_invoke
            ElseIf impl.static_invocable() Then
                m = AddressOf impl.static_invoke
            End If
            o = New invoker(impl, m)
            Return True
        End If
        Return False
    End Function

    Public Shared Function [New](ByVal t As Type,
                                 ByVal binding_flags As BindingFlags,
                                 ByVal obj As Object,
                                 ByVal name As String,
                                 ByVal suppress_error As Boolean) As invoker
        Dim o As invoker = Nothing
        assert([New](t, binding_flags, obj, name, suppress_error, o))
        Return o
    End Function

    Private Sub New(ByVal impl As invoker(Of undefined_delegate_type),
                    ByVal m As Func(Of Object(), Object))
        assert(impl IsNot Nothing)
        Me.impl = impl
        assert(impl.invoke_only())
        Me.m = m
    End Sub

    Public Overrides Function [static]() As Boolean
        Return impl.static()
    End Function

    Public Overrides Function pre_binding() As Boolean
        Return m IsNot Nothing
    End Function

    Public Overrides Function pre_bind(ByRef d As Func(Of Object(), Object)) As Boolean
        If pre_binding() Then
            assert(m IsNot Nothing)
            d = m
            Return True
        End If
        Return False
    End Function

    Public Overrides Function post_binding() As Boolean
        Return m Is Nothing
    End Function

    Public Overrides Function post_bind(ByVal obj As Object, ByRef d As Func(Of Object(), Object)) As Boolean
        If post_binding() Then
            d = Function(ByVal params() As Object) As Object
                    Return impl.invoke(obj, params)
                End Function
            Return True
        End If
        Return False
    End Function

    Public Overrides Function invoke(ByVal obj As Object, ParamArray params() As Object) As Object
        Return impl.invoke(obj, params)
    End Function

    Public Overrides Function method_info() As MethodInfo
        Return impl.method_info()
    End Function
End Class
