
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports osi.root.connector
Imports osi.root.constants

Partial Public NotInheritable Class invoker(Of delegate_t)
    Inherits invocable(Of delegate_t)

    Private Shared ReadOnly dt As Type = GetType(delegate_t)
    Private Shared ReadOnly is_delegate_undefined As Boolean = (dt Is GetType(invoker.undefined_delegate_type))
    Private ReadOnly mi As MethodInfo
    Private ReadOnly m As delegate_t
    Private ReadOnly match_d As Boolean
    Private ReadOnly obj As Object

    ' Conditions:
    ' pre_binding && post_binding: impossible
    ' pre_binding: The bind can be resolved during constructing, e.g. static or the object has been provided.
    ' post_binding: The bind can not be resolved during constructing, e.g. not static and the object has not been
    ' provided.
    ' !pre_binding && !post_binding: The delegate_t is undefined_delegate_type or the delegate_t does not match the
    ' signature, only invoke() can be used.

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
        If name.null_or_empty() Then
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

        Dim m As delegate_t = Nothing
        Dim match_d As Boolean = False
        If Not is_delegate_undefined Then
            match_d = delegate_info(Of delegate_t).match(mi)
            create_delegate(obj, mi, m, suppress_error)
        End If

        o = New invoker(Of delegate_t)(mi, m, match_d, obj)
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
                    ByVal match_d As Boolean,
                    ByVal obj As Object)
        assert(GetType(delegate_t).is(GetType([Delegate])))
        assert(Not mi Is Nothing)
        If is_delegate_undefined Then
            assert(m Is Nothing)
            assert(Not match_d)
        End If
        If Not m Is Nothing Then
            assert(match_d)
        End If
        Me.mi = mi
        Me.m = m
        Me.match_d = match_d
        Me.obj = obj

        assert(Not pre_binding() OrElse Not post_binding())
        If pre_binding() OrElse post_binding() Then
            assert(Not is_delegate_undefined)
        End If
    End Sub

    Private Shared Function create_delegate(ByVal obj As Object,
                                            ByVal mi As MethodInfo,
                                            ByRef m As delegate_t,
                                            ByVal suppress_error As Boolean) As Boolean
        assert(Not mi Is Nothing)
        If is_delegate_undefined Then
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

    Public Overrides Function [static]() As Boolean
        Return mi.IsStatic()
    End Function

    Public Function match_signature() As Boolean
        Return match_d
    End Function

    Public Overrides Function pre_binding() As Boolean
        Return Not m Is Nothing
    End Function

    Public Overrides Function post_binding() As Boolean
        Return m Is Nothing AndAlso match_d
    End Function

    Public Function invoke_only() As Boolean
        Return Not match_signature()
    End Function

    Public Function instance_invocable() As Boolean
        Return Not [static]() AndAlso Not pre_binding() AndAlso Not obj Is Nothing
    End Function

    Public Function static_invocable() As Boolean
        Return [static]() AndAlso Not pre_binding()
    End Function

    Public Overrides Function pre_bind(ByRef d As delegate_t) As Boolean
        If pre_binding() Then
            assert(Not m Is Nothing)
            d = m
            Return True
        End If
        Return False
    End Function

    Public Overloads Function post_bind(ByVal obj As Object,
                                        ByRef d As delegate_t,
                                        ByVal suppress_error As Boolean) As Boolean
        Return Not obj Is Nothing AndAlso
               Not [static]() AndAlso
               post_binding() AndAlso
               Not is_delegate_undefined AndAlso
               create_delegate(obj, d, suppress_error)
    End Function

    Public Overrides Function post_bind(ByVal obj As Object, ByRef d As delegate_t) As Boolean
        Return post_bind(obj, d, suppress.invoker_error)
    End Function

    Public Overrides Function invoke(ByVal obj As Object, ByVal ParamArray params() As Object) As Object
        ' Allow sending null for instance invoke.
        Return mi.Invoke(obj, params)
    End Function

    Public Function instance_invoke(ByVal ParamArray params() As Object) As Object
        assert(Not obj Is Nothing)
        Return invoke(obj, params)
    End Function

    Public Function static_invoke(ByVal ParamArray params() As Object) As Object
        assert([static]())
        Return invoke(Nothing, params)
    End Function

    Public Overrides Function method_info() As MethodInfo
        Return mi
    End Function
End Class
