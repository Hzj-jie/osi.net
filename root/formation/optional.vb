
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants

Public NotInheritable Class [optional]
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function new_or_empty(Of T)(ByVal v As T) As [optional](Of T)
        If v Is Nothing Then
            Return [empty](Of T)()
        End If
        Return [of](Of T)(v)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function [of](Of T)(ByVal v As T) As [optional](Of T)
        Return New [optional](Of T)(v)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function of_nullable(Of T)(ByVal v As T) As [optional](Of T)
        Return new_or_empty(v)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function [empty](Of T)() As [optional](Of T)
        Return empty_cache(Of T).v
    End Function

    Private Sub New()
    End Sub

    Private NotInheritable Class empty_cache(Of T)
        Public Shared ReadOnly v As [optional](Of T)

        Shared Sub New()
            v = New [optional](Of T)()
        End Sub

        Private Sub New()
        End Sub
    End Class
End Class

Public Structure [optional](Of T)
    Private ReadOnly b As Boolean
    Private ReadOnly v As T

    Shared Sub New()
        assert(Not (New [optional](Of T)()).b)
        If Not type_info(Of T).is_valuetype Then
            assert((New [optional](Of T)()).v Is Nothing)
        End If
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Sub New(ByVal b As Boolean, ByVal v As T)
        Me.b = b
        Me.v = v
        assert(Not b OrElse Not v Is Nothing)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub New(ByVal v As T)
        Me.New(True, v)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function map(Of R)(ByVal f As Func(Of T, R)) As [optional](Of R)
        assert(Not f Is Nothing)
        If Not Me Then
            Return [optional].empty(Of R)()
        End If
        Return [optional].of(f(+Me))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function or_else(ByVal r As T) As T
        assert(Not r Is Nothing)
        If empty() Then
            Return r
        End If
        Return +Me
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function or_else(ByVal f As Func(Of T)) As T
        assert(Not f Is Nothing)
        If empty() Then
            Return f()
        End If
        Return +Me
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function empty() As Boolean
        Return Not Me
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Widening Operator CType(ByVal this As [optional](Of T)) As Boolean
        Return this.b
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator Not(ByVal this As [optional](Of T)) As Boolean
        Return Not this.b
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator +(ByVal this As [optional](Of T)) As T
        assert(this)
        Return this.v
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator -(ByVal this As [optional](Of T)) As T
        If this Then
            Return this.v
        End If
        Return Nothing
    End Operator

    Public Overrides Function ToString() As String
        If empty() Then
            Return "optional.empty()"
        End If
        Return strcat("optional(", v, ")")
    End Function
End Structure