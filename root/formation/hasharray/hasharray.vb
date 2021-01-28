
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.template

Partial Public Class hasharray(Of T,
                                  _UNIQUE As _boolean,
                                  _HASHER As _to_uint32(Of T),
                                  _EQUALER As _equaler(Of T))

    Private Shared ReadOnly predefined_column_counts As const_array(Of UInt32) =
        New const_array(Of UInt32)(doubled_prime_sequence_int32())
    Private Shared ReadOnly unique As Boolean = +(alloc(Of _UNIQUE)())

    Private v As const_array(Of vector(Of hasher_node))
    Private c As UInt32
    Private s As UInt32

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Sub New(ByVal c As UInt32)
#If DEBUG Then
        assert(c < predefined_column_counts.size())
#End If
        Me.c = c
        reset_array()
    End Sub

    <copy_constructor()>
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Protected Sub New(ByVal v As const_array(Of vector(Of hasher_node)),
                      ByVal s As UInt32,
                      ByVal c As UInt32)
#If DEBUG Then
        assert(Not v.null_or_empty())
        assert(v.size() = predefined_column_counts(c))
        assert(c < predefined_column_counts.size())
#End If
        Me.v = v
        Me.s = s
        Me.c = c
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub New()
        Me.New(0)
    End Sub

    <MethodImpl(method_impl_options.no_inlining)>
    Public Function size() As UInt32
        Return s
    End Function

    <MethodImpl(method_impl_options.no_inlining)>
    Public Function empty() As Boolean
        Return size() = uint32_0
    End Function

    <MethodImpl(method_impl_options.no_inlining)>
    Public Function begin() As iterator
        If empty() Then
            Return [end]()
        End If
        Dim it As iterator = iterator_at(first_non_empty_column(), 0)
        If it.ref().empty() Then
            it += 1
        End If
        Return it
    End Function

    <MethodImpl(method_impl_options.no_inlining)>
    Public Function [end]() As iterator
        Return iterator.end
    End Function

    <MethodImpl(method_impl_options.no_inlining)>
    Public Function rbegin() As iterator
        If empty() Then
            Return rend()
        End If
        Dim c As UInt32 = last_non_empty_column()
        Dim it As iterator = iterator_at(c, last_row(c))
        If it.ref().empty() Then
            it -= 1
        End If
        Return it
    End Function

    <MethodImpl(method_impl_options.no_inlining)>
    Public Function rend() As iterator
        Return iterator.end
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function shrink_to_fit() As Boolean
        If c = 0 Then
            Return False
        End If
        For i As UInt32 = 0 To c - uint32_1
            If predefined_column_counts(i) >= size() / row_count_upper_bound() Then
                rehash(i)
                Return True
            End If
        Next
        Return False
    End Function
End Class
