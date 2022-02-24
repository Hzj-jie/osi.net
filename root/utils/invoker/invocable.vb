
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports osi.root.connector

Public MustInherit Class invocable(Of delegate_t)
    Public MustOverride Function [static]() As Boolean
    Public MustOverride Function pre_binding() As Boolean
    Public MustOverride Function post_binding() As Boolean
    Public MustOverride Function pre_bind(ByRef d As delegate_t) As Boolean
    Public MustOverride Function post_bind(ByVal obj As Object, ByRef d As delegate_t) As Boolean
    Public MustOverride Function invoke(ByVal obj As Object, ByVal ParamArray params() As Object) As Object
    Public MustOverride Function method_info() As MethodInfo

    Public Function instance() As Boolean
        Return Not [static]()
    End Function

    Public Function pre_bind() As delegate_t
        Dim o As delegate_t = Nothing
        assert(pre_bind(o))
        Return o
    End Function

    Public Shared Operator +(ByVal this As invocable(Of delegate_t)) As delegate_t
        assert(Not this Is Nothing)
        Return this.pre_bind()
    End Operator

    Public Function post_bind(ByVal obj As Object) As delegate_t
        Dim o As delegate_t = Nothing
        assert(post_bind(obj, o))
        Return o
    End Function

    Public Shared Operator +(ByVal this As invocable(Of delegate_t), ByVal obj As Object) As delegate_t
        assert(Not this Is Nothing)
        Return this(obj)
    End Operator

    Default Public ReadOnly Property bind(ByVal obj As Object) As delegate_t
        Get
            Return post_bind(obj)
        End Get
    End Property

    Public Function target_type() As Type
        Return method_info().DeclaringType()
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
