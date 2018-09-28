
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports osi.root.connector
Imports osi.root.constants

' TODO: Use invoker.undefined_delegate_type, and hide the implementation detail by using invoker class.
Public Delegate Sub not_resolved_type_delegate()

Partial Public NotInheritable Class invoker(Of delegate_t)
    Private Shared ReadOnly dt As Type
    Private Shared ReadOnly is_not_resolved_type_delegate As Boolean
    Private ReadOnly mi As MethodInfo
    Private ReadOnly m As delegate_t
    Private ReadOnly preb As Boolean
    Private ReadOnly postb As Boolean

    ' Conditions:
    ' preb && postb: impossible
    ' preb: The bind can be resolved during constructing, e.g. static or the object has been provided.
    ' postb: The bind can not be resolved during constructing, e.g. not static and the object has not been provided.
    ' !preb && !postb: The delegate_t is undefined_delegate_type, only invoke() can be used.

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
        o = Nothing
        If t Is Nothing Then
            If Not suppress_error Then
                raise_error(error_type.warning, "input type is nothing")
            End If
            Return False
        End If
        If String.IsNullOrEmpty(name) Then
            If Not suppress_error Then
                raise_error(error_type.warning, "input method name is empty")
            End If
            Return False
        End If

        Dim mi As MethodInfo = Nothing
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
            Return False
        End Try

        If mi Is Nothing Then
            If Not suppress_error Then
                raise_error(error_type.warning,
                            "cannot find method ",
                            name,
                            " in type ",
                            t)
            End If
            Return False
        End If

        Dim postb As Boolean = False
        Dim preb As Boolean = False
        Dim m As delegate_t = Nothing
        If Not is_not_resolved_type_delegate Then
            If t.IsValueType() Then
                postb = Not mi.IsStatic()
            Else
                postb = Not mi.IsStatic() AndAlso obj Is Nothing
            End If
            If postb Then
                postb = delegate_info(Of delegate_t).match(mi)
            End If
            If Not postb Then
                preb = create_delegate(obj, mi, m, suppress_error)
            End If
        End If

        o = New invoker(Of delegate_t)(mi, m, preb, postb)
        Return True
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

    Private Sub New(ByVal mi As MethodInfo,
                    ByVal m As delegate_t,
                    ByVal preb As Boolean,
                    ByVal postb As Boolean)
        assert(Not mi Is Nothing)
        assert(Not preb OrElse Not postb)
        If preb Then
            assert(Not m Is Nothing)
        End If
        If preb OrElse postb Then
            assert(Not is_not_resolved_type_delegate)
        End If
        If is_not_resolved_type_delegate Then
            assert(m Is Nothing)
        End If

        Me.mi = mi
        Me.m = m
        Me.preb = preb
        Me.postb = postb
    End Sub

    Private Shared Function create_delegate(ByVal obj As Object,
                                            ByVal mi As MethodInfo,
                                            ByRef m As delegate_t,
                                            ByVal suppress_error As Boolean) As Boolean
        assert(Not mi Is Nothing)
        If is_not_resolved_type_delegate Then
            Return False
        End If
        Dim d As [Delegate] = Nothing
        Try
            If mi.IsStatic() Then
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
                            mi.Name(),
                            " of type ",
                            mi.DeclaringType(),
                            ", ex ",
                            ex.details())
            End If
            Return False
        End Try

        If Not direct_cast(Of delegate_t)(d, m) Then
            If Not suppress_error Then
                raise_error(error_type.warning,
                            "failed to convert method ",
                            mi.Name(),
                            " in type ",
                            mi.DeclaringType(),
                            " to delegate ",
                            dt)
            End If
            Return False
        End If

        Return True
    End Function

    Private Function create_delegate(ByVal obj As Object,
                                     ByRef m As delegate_t,
                                     ByVal suppress_error As Boolean) As Boolean
        Return create_delegate(obj, mi, m, suppress_error)
    End Function

    Public Function [static]() As Boolean
        Return mi.IsStatic()
    End Function

    Public Function [get]() As delegate_t
        Return +Me
    End Function

    Public Function pre_binding() As Boolean
        Return preb
    End Function

    Public Function post_binding() As Boolean
        Return postb
    End Function

    Public Function invoke_only() As Boolean
        Return is_not_resolved_type_delegate
    End Function

    Public Function instance_invokeable() As Boolean
        Return Not [static]() AndAlso Not pre_binding()
    End Function

    Public Function static_invokeable() As Boolean
        Return [static]() AndAlso Not pre_binding()
    End Function

    Public Function pre_bind(ByRef d As delegate_t) As Boolean
        If pre_binding() Then
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
               Not [static]() AndAlso
               post_binding() AndAlso
               Not is_not_resolved_type_delegate AndAlso
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
        ' Allow sending null for instance invoke.
        Return mi.Invoke(obj, params)
    End Function

    Public Function static_invoke(ByVal ParamArray params() As Object) As Object
        assert([static]())
        Return invoke(Nothing, params)
    End Function

    Public Function method_info() As MethodInfo
        Return mi
    End Function

    Public Function target_type() As Type
        Return mi.DeclaringType()
    End Function

    Public Function identity() As String
        Dim d As delegate_t = Nothing
        If pre_bind(d) Then
            assert(Not d Is Nothing)
            Return direct_cast(Of [Delegate])(d).method_identity()
        End If

        Return method_info().full_name()
    End Function
End Class
