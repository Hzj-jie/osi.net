
Imports System.Reflection
Imports osi.root.connector
Imports osi.root.constants

Public Delegate Sub not_resolved_type_delegate()

Public Class invoker(Of delegate_t)
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
                raise_error(error_type.warning, "input method name is empty")
            End If
        Else
            Me.t = t
            Me.name = name
            Try
                mi = t.GetMethod(name, bindingflags)
            Catch ex As Exception
                If Not suppress_error Then
                    raise_error(error_type.warning,
                                "cannot find method ",
                                name,
                                " in type ",
                                t,
                                ", ex ",
                                ex.Message)
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
            If Not [static]() AndAlso obj Is Nothing Then
                postb = True
            Else
                preb = create_delegate(obj, m, suppress_error)
            End If
        End If
    End Sub

    Public Sub New(ByVal t As Type,
                   ByVal bindingflags As BindingFlags,
                   ByVal obj As Object,
                   ByVal name As String)
        Me.New(t, bindingflags, obj, name, suppress.invoker_error)
    End Sub

    Public Sub New(ByVal t As Type,
                   ByVal bindingflags As BindingFlags,
                   ByVal name As String,
                   ByVal suppress_error As Boolean)
        Me.New(t, bindingflags, Nothing, name, suppress_error)
    End Sub

    Public Sub New(ByVal t As Type, ByVal bindingflags As BindingFlags, ByVal name As String)
        Me.New(t, bindingflags, name, suppress.invoker_error)
    End Sub

    Public Sub New(ByVal t As Type, ByVal name As String, ByVal suppress_error As Boolean)
        Me.New(t, BindingFlags.Static Or BindingFlags.Public, name, suppress_error)
    End Sub

    Public Sub New(ByVal t As Type, ByVal name As String)
        Me.New(t, name, suppress.invoker_error)
    End Sub

    Public Sub New(ByVal t As Type, ByVal obj As Object, ByVal name As String, ByVal suppress_error As Boolean)
        Me.New(t, BindingFlags.Static Or BindingFlags.Instance Or BindingFlags.Public, obj, name, suppress_error)
    End Sub

    Public Sub New(ByVal t As Type, ByVal obj As Object, ByVal name As String)
        Me.New(t, obj, name, suppress.invoker_error)
    End Sub

    Public Sub New(ByVal obj As Object, ByVal name As String, ByVal suppress_error As Boolean)
        Me.New(obj.GetType(), obj, name, suppress_error)
    End Sub

    Public Sub New(ByVal obj As Object, ByVal name As String)
        Me.New(obj, name, suppress.invoker_error)
    End Sub

    Public Sub New(ByVal obj As Object,
                   ByVal bindingflags As BindingFlags,
                   ByVal name As String,
                   ByVal suppress_error As Boolean)
        Me.New(obj.GetType(), bindingflags, obj, name, suppress_error)
    End Sub

    Public Sub New(ByVal obj As Object, ByVal bindingflags As BindingFlags, ByVal name As String)
        Me.New(obj, bindingflags, name, suppress.invoker_error)
    End Sub

    Public Sub New(ByVal bindingflags As BindingFlags,
                   ByVal obj As Object,
                   ByVal name As String,
                   ByVal suppress_error As Boolean)
        Me.New(obj, bindingflags, name, suppress_error)
    End Sub

    Public Sub New(ByVal bindingflags As BindingFlags, ByVal obj As Object, ByVal name As String)
        Me.New(bindingflags, obj, name, suppress.invoker_error)
    End Sub

    Private Function create_delegate(ByVal obj As Object,
                                     ByRef m As delegate_t,
                                     ByVal suppress_error As Boolean) As Boolean
        assert(Not t Is Nothing)
        assert(Not String.IsNullOrEmpty(name))
        assert(Not mi Is Nothing)
        If is_not_resolved_type_delegate Then
            Return True
        Else
            Dim d As [Delegate] = Nothing
            Try
                If [static]() Then
                    d = [Delegate].CreateDelegate(dt, mi)
                Else
                    If obj Is Nothing Then
                        Return False
                    Else
                        d = [Delegate].CreateDelegate(dt, obj, mi)
                    End If
                End If
            Catch ex As Exception
                If Not suppress_error Then
                    raise_error(error_type.warning,
                                "cannot create delegate for method ",
                                name,
                                " of type ",
                                t,
                                ", ex ",
                                ex.Message)
                End If
                Return False
            End Try

            If Not cast(Of delegate_t)(d, m) Then
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
        End If
    End Function

    Public Function valid() As Boolean
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
        Else
            Return False
        End If
    End Function

    Public Shared Operator +(ByVal this As invoker(Of delegate_t)) As delegate_t
        assert(Not this Is Nothing)
        Dim o As delegate_t = Nothing
        assert(this.pre_bind(o))
        Return o
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

    Public Shared Operator +(ByVal this As invoker(Of delegate_t), ByVal obj As Object) As delegate_t
        assert(Not this Is Nothing)
        Return this(obj)
    End Operator

    Default Public ReadOnly Property bind(ByVal obj As Object) As delegate_t
        Get
            Dim o As delegate_t = Nothing
            assert(post_bind(obj, o))
            Return o
        End Get
    End Property

    Public Function invoke(ByVal obj As Object, ByVal ParamArray params() As Object) As Object
        assert(valid())
        Return mi.Invoke(obj, params)
    End Function
End Class
