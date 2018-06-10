﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports osi.root.connector
Imports osi.root.constants

Public Delegate Sub not_resolved_type_delegate()

Public NotInheritable Class invoker(Of delegate_t)
    Private Shared ReadOnly dt As Type
    Private Shared ReadOnly is_not_resolved_type_delegate As Boolean
    Private ReadOnly s As Boolean
    Private ReadOnly mi As MethodInfo
    Private ReadOnly m As delegate_t
    Private ReadOnly t As Type
    Private ReadOnly name As String
    Private ReadOnly preb As Boolean
    Private ReadOnly postb As Boolean

    Shared Sub New()
        assert(GetType(delegate_t).is(GetType([Delegate])))
        dt = GetType(delegate_t)
        is_not_resolved_type_delegate = (dt Is GetType(not_resolved_type_delegate))
    End Sub

    Public Shared Function [New](ByVal t As Type,
                                 ByVal binding_flags As BindingFlags,
                                 ByVal obj As Object,
                                 ByVal name As String,
                                 ByVal suppress_error As Boolean,
                                 ByRef o As invoker(Of delegate_t)) As Boolean
        o = New invoker(Of delegate_t)(t, binding_flags, obj, name, suppress_error)
        If o.valid() Then
            Return True
        End If
        o = Nothing
        Return False
    End Function

    Public Shared Function [New](ByVal t As Type,
                                 ByVal binding_flags As BindingFlags,
                                 ByVal obj As Object,
                                 ByVal name As String,
                                 ByVal suppress_error As Boolean) As invoker(Of delegate_t)
        Dim r As invoker(Of delegate_t) = Nothing
        assert([New](t, binding_flags, obj, name, suppress_error, r))
        Return r
    End Function

    Private Sub New(ByVal t As Type,
                    ByVal binding_flags As BindingFlags,
                    ByVal obj As Object,
                    ByVal name As String,
                    ByVal suppress_error As Boolean)
        If t Is Nothing Then
            If Not suppress_error Then
                raise_error(error_type.warning, "input type is nothing")
            End If
            Return
        End If
        If String.IsNullOrEmpty(name) Then
            If Not suppress_error Then
                raise_error(error_type.warning, "input method name is empty")
            End If
            Return
        End If

        Me.t = t
        Me.name = name
        Try
            mi = t.GetMethod(name, binding_flags Or BindingFlags.InvokeMethod)
        Catch ex As Exception
            If Not suppress_error Then
                raise_error(error_type.warning,
                            "cannot find method ",
                            name,
                            " in type ",
                            t,
                            ", ex ",
                            ex.details())
            End If
            Return
        End Try

        If mi Is Nothing Then
            If Not suppress_error Then
                raise_error(error_type.warning,
                            "cannot find method ",
                            name,
                            " in type ",
                            t)
            End If
            Return
        End If

        s = mi.IsStatic()
        postb = False
        If t.IsValueType() Then
            postb = Not [static]()
        Else
            postb = Not [static]() AndAlso obj Is Nothing
        End If
        If postb Then
            If Not is_not_resolved_type_delegate Then
                postb = delegate_info(Of delegate_t).match(mi)
            End If
        End If
        If Not postb Then
            preb = create_delegate(obj, m, suppress_error)
        End If
    End Sub

    Private Function create_delegate(ByVal obj As Object,
                                     ByRef m As delegate_t,
                                     ByVal suppress_error As Boolean) As Boolean
        assert(Not t Is Nothing)
        assert(Not String.IsNullOrEmpty(name))
        assert(Not mi Is Nothing)
        If is_not_resolved_type_delegate Then
            Return True
        End If
        Dim d As [Delegate] = Nothing
        Try
            If [static]() Then
                d = [Delegate].CreateDelegate(dt, mi)
            Else
                If obj Is Nothing Then
                    Return False
                End If
                d = [Delegate].CreateDelegate(dt, obj, mi)
            End If
        Catch ex As Exception
            If Not suppress_error Then
                raise_error(error_type.warning,
                            "cannot create delegate for method ",
                            name,
                            " of type ",
                            t,
                            ", ex ",
                            ex.details())
            End If
            Return False
        End Try

        If Not direct_cast(Of delegate_t)(d, m) Then
            If Not suppress_error Then
                raise_error(error_type.warning,
                            "failed to convert method ",
                            name,
                            " in type ",
                            t,
                            " to delegate ",
                            dt)
            End If
            Return False
        End If

        Return True
    End Function

    Private Function valid() As Boolean
        Return Not mi Is Nothing
    End Function

    Public Function [static]() As Boolean
        Return s
    End Function

    Public Function [get]() As delegate_t
        Return +Me
    End Function

    Public Function resolved_type_delegate() As Boolean
        Return Not is_not_resolved_type_delegate
    End Function

    Public Function not_resolved_type_delegate() As Boolean
        Return is_not_resolved_type_delegate
    End Function

    Public Function pre_binding() As Boolean
        Return preb AndAlso assert(valid())
    End Function

    Public Function post_binding() As Boolean
        Return postb AndAlso assert(valid())
    End Function

    Public Function pre_bind(ByRef d As delegate_t) As Boolean
        If pre_binding() AndAlso resolved_type_delegate() Then
            assert(Not m Is Nothing)
            d = m
            Return True
        End If
        Return False
    End Function

    Public Function pre_bind() As delegate_t
        Dim o As delegate_t = Nothing
        assert(pre_bind(o))
        Return o
    End Function

    Public Shared Operator +(ByVal this As invoker(Of delegate_t)) As delegate_t
        assert(Not this Is Nothing)
        Return this.pre_bind()
    End Operator

    Public Function post_bind(ByVal obj As Object, ByRef d As delegate_t, ByVal suppress_error As Boolean) As Boolean
        Return Not obj Is Nothing AndAlso
               valid() AndAlso
               Not [static]() AndAlso
               post_binding() AndAlso
               resolved_type_delegate() AndAlso
               create_delegate(obj, d, suppress_error)
    End Function

    Public Function post_bind(ByVal obj As Object, ByRef d As delegate_t) As Boolean
        Return post_bind(obj, d, suppress.invoker_error)
    End Function

    Public Function post_bind(ByVal obj As Object) As delegate_t
        Dim r As delegate_t = Nothing
        assert(post_bind(obj, r))
        Return r
    End Function

    Public Shared Operator +(ByVal this As invoker(Of delegate_t), ByVal obj As Object) As delegate_t
        assert(Not this Is Nothing)
        Return this(obj)
    End Operator

    Default Public ReadOnly Property bind(ByVal obj As Object) As delegate_t
        Get
            Return post_bind(obj)
        End Get
    End Property

    Public Function invoke(ByVal obj As Object, ByVal ParamArray params() As Object) As Object
        assert(valid())
        Return mi.Invoke(obj, params)
    End Function

    ' TODO: remove
    Private Function method_info(ByRef o As MethodInfo) As Boolean
        If valid() Then
            o = mi
            Return True
        End If
        Return False
    End Function

    Public Function method_info() As MethodInfo
        Dim r As MethodInfo = Nothing
        assert(method_info(r))
        Return r
    End Function

    ' TODO: remove
    Private Function target_type(ByRef o As Type) As Boolean
        If valid() Then
            o = t
            Return True
        End If
        Return False
    End Function

    Public Function target_type() As Type
        Dim r As Type = Nothing
        assert(target_type(r))
        Return r
    End Function

    ' TODO: remove
    Private Function identity(ByRef o As String) As Boolean
        Dim d As delegate_t = Nothing
        If pre_bind(d) Then
            assert(Not d Is Nothing)
            o = direct_cast(Of [Delegate])(d).method_identity()
            Return True
        End If

        Dim r As MethodInfo = Nothing
        If method_info(r) Then
            assert(Not r Is Nothing)
            o = r.full_name()
            Return True
        End If

        Return False
    End Function

    Public Function identity() As String
        Dim r As String = Nothing
        assert(identity(r))
        Return r
    End Function
End Class
