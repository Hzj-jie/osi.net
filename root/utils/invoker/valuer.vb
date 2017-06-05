
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports osi.root.constants
Imports osi.root.connector

Partial Public NotInheritable Class valuer
    Private Shared Function get_type(Of T)(ByVal obj As T) As Type
        ' get or set from an object is usually senseless.
        If type_info(Of T).is_object AndAlso Not obj Is Nothing Then
            Return obj.GetType()
        Else
            Return GetType(T)
        End If
    End Function

    Public Shared Function try_get(Of T, VT)(ByVal obj As T,
                                             ByVal bindingflags As BindingFlags,
                                             ByVal name As String,
                                             ByRef o As VT) As Boolean
        Dim v As valuer(Of VT) = Nothing
        v = New valuer(Of VT)(get_type(obj), bindingflags, obj, name)
        Return v.try_get(o)
    End Function

    Public Shared Function try_get(Of T, VT)(ByVal obj As T,
                                             ByVal name As String,
                                             ByRef o As VT) As Boolean
        Return try_get(obj, binding_flags.public, name, o)
    End Function

    Public Shared Function try_get(Of T, VT)(ByVal name As String,
                                             ByVal bindingflags As BindingFlags,
                                             ByRef o As VT) As Boolean
        Return try_get(Of T, VT)(Nothing, bindingflags, name, o)
    End Function

    Public Shared Function try_get(Of T, VT)(ByVal name As String,
                                             ByRef o As VT) As Boolean
        Return try_get(Of T, VT)(Nothing, name, o)
    End Function

    Public Shared Function [get](Of T, VT)(ByVal obj As T,
                                           ByVal bindingflags As BindingFlags,
                                           ByVal name As String) As VT
        Dim o As VT = Nothing
        assert(try_get(obj, bindingflags, name, o))
        Return o
    End Function

    Public Shared Function [get](Of T, VT)(ByVal obj As T, ByVal name As String) As VT
        Return [get](Of T, VT)(obj, binding_flags.public, name)
    End Function

    Public Shared Function [get](Of T, VT)(ByVal name As String,
                                           ByVal bindingflags As BindingFlags) As VT
        Dim o As VT = Nothing
        assert(try_get(Of T, VT)(name, bindingflags, o))
        Return o
    End Function

    Public Shared Function [get](Of T, VT)(ByVal name As String) As VT
        Dim o As VT = Nothing
        assert(try_get(Of T, VT)(name, o))
        Return o
    End Function

    Public Shared Function [get](Of VT)(ByVal obj As Object,
                                        ByVal bindingflags As BindingFlags,
                                        ByVal name As String) As VT
        Dim v As valuer(Of VT) = Nothing
        v = New valuer(Of VT)(obj, bindingflags, name)
        Return v.get()
    End Function

    Public Shared Function [get](Of VT)(ByVal obj As Object, ByVal name As String) As VT
        Return [get](Of VT)(obj, binding_flags.public, name)
    End Function

    Public Shared Function try_set(Of T, VT)(ByVal obj As T,
                                             ByVal bindingflags As BindingFlags,
                                             ByVal name As String,
                                             ByVal i As VT) As Boolean
        Dim v As valuer(Of VT) = Nothing
        v = New valuer(Of VT)(get_type(obj), bindingflags, obj, name)
        Return v.try_set(i)
    End Function

    Public Shared Function try_set(Of T, VT)(ByVal obj As T,
                                             ByVal name As String,
                                             ByVal i As VT) As Boolean
        Return try_set(obj, binding_flags.public, name, i)
    End Function

    Public Shared Sub [set](Of T, VT)(ByVal obj As T,
                                      ByVal bindingflags As BindingFlags,
                                      ByVal name As String,
                                      ByVal i As VT)
        assert(try_set(obj, bindingflags, name, i))
    End Sub

    Public Shared Sub [set](Of T, VT)(ByVal obj As T,
                                      ByVal name As String,
                                      ByVal i As VT)
        [set](obj, binding_flags.public, name, i)
    End Sub

    Private Sub New()
    End Sub
End Class

Public Class valuer(Of VT)
    Private ReadOnly s As Boolean
    Private ReadOnly setter As Action(Of VT)
    Private ReadOnly getter As Func(Of VT)
    Private ReadOnly setter_post As Action(Of Object, VT)
    Private ReadOnly getter_post As Func(Of Object, VT)

    Public Sub New(ByVal t As Type,
                   ByVal bindingflags As BindingFlags,
                   ByVal obj As Object,
                   ByVal name As String,
                   ByVal suppress_error As Boolean)
        If t Is Nothing Then
            If Not suppress_error Then
                raise_error(error_type.warning, "input type is nothing")
            End If
        ElseIf String.IsNullOrEmpty(name) Then
            If Not suppress_error Then
                raise_error(error_type.warning, "input field name is empty")
            End If
        Else
            Dim f As FieldInfo = Nothing
            Try
                f = t.GetField(name, bindingflags)
            Catch ex As Exception
                assert(False, ex.Message())
                Return
            End Try

            If f Is Nothing Then
                If Not suppress_error Then
                    raise_error(error_type.warning, "cannot find field with name ", name)
                End If
                Return
            End If

            ' The first condition is for performance concern, since .[is] function is pretty slow.
            ' It goes through all its super classes and interfaces.
            If f.FieldType() Is GetType(VT) OrElse
               f.FieldType().is(GetType(VT)) OrElse
               GetType(VT).is(f.FieldType()) Then
                If Not suppress_error AndAlso Not f.FieldType() Is GetType(VT) Then
                    If f.FieldType().inherit(GetType(VT)) Then
                        raise_error(error_type.warning,
                                    "field type (",
                                    f.FieldType().FullName(),
                                    ") is an inheritance of input type (",
                                    type_info(Of VT).fullname,
                                    "), so the setter function may not work, ",
                                    "and eventually throw ArgumentException.")
                    ElseIf GetType(VT).inherit(f.FieldType()) Then
                        raise_error(error_type.warning,
                                    "input type (",
                                    type_info(Of VT).fullname,
                                    ") is an inheritance of field type (",
                                    f.FieldType().FullName(),
                                    "), so the getter function may not work, ",
                                    "and eventually throw InvalidCastException.")
                    End If
                End If

                s = f.IsStatic()
                If [static]() Then
                    If Not suppress_error Then
                        assert(obj Is Nothing, "should not bind an object for static fields.")
                    End If
                    setter = Sub(v As VT)
                                 f.SetValue(Nothing, v)
                             End Sub
                    getter = Function() As VT
                                 Return DirectCast(f.GetValue(Nothing), VT)
                             End Function
                ElseIf obj Is Nothing Then
                    setter_post = Sub(o As Object, v As VT)
                                      f.SetValue(o, v)
                                  End Sub
                    getter_post = Function(o As Object) As VT
                                      Return DirectCast(f.GetValue(o), VT)
                                  End Function
                Else
                    setter = Sub(v As VT)
                                 f.SetValue(obj, v)
                             End Sub
                    getter = Function() As VT
                                 Return DirectCast(f.GetValue(obj), VT)
                             End Function
                End If
            Else
                If Not suppress_error Then
                    raise_error(error_type.warning,
                                "field type is inconsistent, expect ",
                                f.FieldType().FullName(),
                                " or its base or inherited types, input type ",
                                type_info(Of VT).fullname)
                End If
            End If
        End If
    End Sub

    Public Sub New(ByVal t As Type,
                   ByVal bindingflags As BindingFlags,
                   ByVal obj As Object,
                   ByVal name As String)
        Me.New(t, bindingflags, obj, name, suppress.valuer_error)
    End Sub

    Public Sub New(ByVal t As Type,
                   ByVal bindingflags As BindingFlags,
                   ByVal name As String,
                   ByVal suppress_error As Boolean)
        Me.New(t, bindingflags, Nothing, name, suppress_error)
    End Sub

    Public Sub New(ByVal t As Type,
                   ByVal bindingflags As BindingFlags,
                   ByVal name As String)
        Me.New(t, bindingflags, name, suppress.valuer_error)
    End Sub

    Public Sub New(ByVal t As Type,
                   ByVal obj As Object,
                   ByVal name As String,
                   ByVal suppress_error As Boolean)
        Me.New(t, binding_flags.public, obj, name, suppress_error)
    End Sub

    Public Sub New(ByVal t As Type,
                   ByVal obj As Object,
                   ByVal name As String)
        Me.New(t, obj, name, suppress.valuer_error)
    End Sub

    Public Sub New(ByVal t As Type, ByVal name As String, ByVal suppress_error As Boolean)
        Me.New(t, Nothing, name, suppress_error)
    End Sub

    Public Sub New(ByVal t As Type, ByVal name As String)
        Me.New(t, name, suppress.valuer_error)
    End Sub

    Public Sub New(ByVal o As Object,
                   ByVal bindingflags As BindingFlags,
                   ByVal name As String,
                   ByVal suppress_error As Boolean)
        Me.New(o.GetType(), bindingflags, o, name, suppress_error)
    End Sub

    Public Sub New(ByVal o As Object,
                   ByVal bindingflags As BindingFlags,
                   ByVal name As String)
        Me.New(o, bindingflags, name, suppress.valuer_error)
    End Sub

    Public Sub New(ByVal o As Object,
                   ByVal name As String,
                   ByVal suppress_error As Boolean)
        Me.New(o.GetType(), o, name, suppress_error)
    End Sub

    Public Sub New(ByVal o As Object, ByVal name As String)
        Me.New(o, name, suppress.valuer_error)
    End Sub

    Public Function [static]() As Boolean
        Return s
    End Function

    Public Function valid() As Boolean
        Return pre_binding() OrElse post_binding()
    End Function

    Public Function pre_binding() As Boolean
        Return Not getter Is Nothing AndAlso
               assert(Not setter Is Nothing) AndAlso
               assert(getter_post Is Nothing) AndAlso
               assert(setter_post Is Nothing)
    End Function

    Public Function post_binding() As Boolean
        Return Not getter_post Is Nothing AndAlso
               assert(Not setter_post Is Nothing) AndAlso
               assert(getter Is Nothing) AndAlso
               assert(setter Is Nothing)
    End Function

    Private Shared Sub [error](ByVal ex As Exception, ByVal s As String)
        If Not suppress.valuer_error.true_() Then
            raise_error(error_type.warning, "failed to ", s, " value, ex ", ex.Message())
        End If
    End Sub

    Private Shared Sub get_error(ByVal ex As Exception)
        [error](ex, "get")
    End Sub

    Private Shared Sub set_error(ByVal ex As Exception)
        [error](ex, "set")
    End Sub

    Public Function [get]() As VT
        assert(pre_binding())
        Return getter()
    End Function

    Public Function try_get(ByRef o As VT) As Boolean
        If Not pre_binding() Then
            Return False
        End If
        Try
            o = [get]()
            Return True
        Catch ex As Exception
            get_error(ex)
            Return False
        End Try
    End Function

    Public Sub [set](ByVal v As VT)
        assert(pre_binding())
        setter(v)
    End Sub

    Public Function try_set(ByVal v As VT) As Boolean
        If Not pre_binding() Then
            Return False
        End If
        Try
            [set](v)
            Return True
        Catch ex As Exception
            set_error(ex)
            Return False
        End Try
    End Function

    Public Function [get](ByVal o As Object) As VT
        assert(post_binding())
        Return getter_post(o)
    End Function

    Public Function try_get(ByVal o As Object, ByRef v As VT) As Boolean
        If Not post_binding() Then
            Return False
        End If
        Try
            v = [get](o)
            Return True
        Catch ex As Exception
            get_error(ex)
            Return False
        End Try
    End Function

    Public Sub [set](ByVal o As Object, ByVal v As VT)
        assert(post_binding())
        setter_post(o, v)
    End Sub

    Public Function try_set(ByVal o As Object, ByVal v As VT) As Boolean
        If Not post_binding() Then
            Return False
        End If
        Try
            [set](o, v)
            Return True
        Catch ex As Exception
            set_error(ex)
            Return False
        End Try
    End Function

    Default Public Property value(ByVal o As Object) As VT
        Get
            Return [get](o)
        End Get
        Set(ByVal value As VT)
            [set](o, value)
        End Set
    End Property

    Public Shared Operator +(ByVal this As valuer(Of VT)) As VT
        assert(Not this Is Nothing)
        Return this.get()
    End Operator
End Class
