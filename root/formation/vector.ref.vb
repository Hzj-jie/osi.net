
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants

Partial Public NotInheritable Class vector(Of T)
    Friend Structure ref
        Private ReadOnly p As vector(Of T)
        Private ReadOnly i As UInt32

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Sub New(ByVal p As vector(Of T), ByVal i As UInt32)
            assert(Not p Is Nothing)
            assert(i < p.size())
            Me.p = p
            Me.i = i
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function size() As UInt32
            assert(Not p Is Nothing)
            Return p.size()
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function index() As UInt32
            assert(Not p Is Nothing)
            Return i
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function is_equal_to(ByVal that As ref) As Boolean
            Return object_compare(p, that.p) = 0 AndAlso i = that.i
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function is_end() As Boolean
            If p Is Nothing Then
                Return True
            End If
            assert(i < p.size())
            Return False
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function ref_at(ByVal s As UInt32) As ref
            Return New ref(p, s)
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function value() As T
            assert(Not p Is Nothing)
            Return p(i)
        End Function
    End Structure
End Class
