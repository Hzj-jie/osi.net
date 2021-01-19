
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

    Private Shared ReadOnly predefined_column_counts As const_array(Of UInt32)
    Private Shared ReadOnly unique As Boolean
    Private Shared ReadOnly hasher As _HASHER
    Private Shared ReadOnly equaler As _equaler(Of T)

    Shared Sub New()
        predefined_column_counts = New const_array(Of UInt32)(doubled_prime_sequence_int32())
        unique = +(alloc(Of _UNIQUE)())
        hasher = alloc(Of _HASHER)()
        equaler = alloc(Of _EQUALER)()
    End Sub

    Private v As array(Of vector(Of hasher_node(Of T)))
    Private c As UInt32
    Private s As UInt32

    Private Sub New(ByVal c As UInt32)
#If DEBUG Then
        assert(c < predefined_column_counts.size())
#End If
        Me.c = c
        reset_array()
    End Sub

    <copy_constructor()>
    Protected Sub New(ByVal v As array(Of vector(Of hasher_node(Of T))),
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

    Public Sub New()
        Me.New(0)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function size() As UInt32
        Return s
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function empty() As Boolean
        Return size() = uint32_0
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function begin() As iterator
        If empty() Then
            Return [end]()
        End If
        Dim it As iterator = Nothing
        it = iterator_at(first_non_empty_column(), 0)
        If it.ref().empty() Then
            it += 1
        End If
        Return it
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function [end]() As iterator
        Return iterator.end
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function rbegin() As iterator
        If empty() Then
            Return rend()
        End If
        Dim it As iterator = Nothing
        Dim c As UInt32 = 0
        c = last_non_empty_column()
        it = iterator_at(c, last_row(c))
        If it.ref().empty() Then
            it -= 1
        End If
        Return it
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function rend() As iterator
        Return iterator.end
    End Function
End Class
