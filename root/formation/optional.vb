
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
            Return empty(Of T)()
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
    Public Shared Function optionally(Of T)(ByVal condition As Boolean, ByVal v As Func(Of T)) As [optional](Of T)
        If condition Then
            Return New [optional](Of T)(v())
        End If
        Return empty(Of T)()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function empty(Of T)() As [optional](Of T)
        Return empty_cache(Of T).v
    End Function

    Private Sub New()
    End Sub

    Private NotInheritable Class empty_cache(Of T)
        Public Shared ReadOnly v As [optional](Of T) = New [optional](Of T)()

        Private Sub New()
        End Sub
    End Class
End Class

Public Structure [optional](Of T)
    Private ReadOnly b As Boolean
    Private ReadOnly v As T

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Sub New(ByVal b As Boolean, ByVal v As T)
        assert(Not (New [optional](Of T)()).b)
        If Not type_info(Of T).is_valuetype Then
            assert((New [optional](Of T)()).v Is Nothing)
        End If

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
        If b Then
            Return [optional].of(f(v))
        End If
        Return [optional].empty(Of R)()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function or_else(ByVal r As T) As T
        assert(Not r Is Nothing)
        If b Then
            Return v
        End If
        Return r
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function or_else(ByVal f As Func(Of T)) As T
        assert(Not f Is Nothing)
        If b Then
            Return v
        End If
        Return f()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function or_assert() As T
        assert(b)
        Return v
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function empty() As Boolean
        Return Not b
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub if_present(ByVal a As Action(Of T))
        assert(Not a Is Nothing)
        If b Then
            a(v)
        End If
    End Sub

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
        Return this.or_assert()
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator -(ByVal this As [optional](Of T)) As T
        If this Then
            Return this.v
        End If
        Return Nothing
    End Operator

    Public Overrides Function ToString() As String
        If b Then
            Return strcat("optional(", v, ")")
        End If
        Return "optional.empty()"
    End Function
End Structure
