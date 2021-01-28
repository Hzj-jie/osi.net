
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.template

Partial Public NotInheritable Class hashset(Of T, _HASHER As _to_uint32(Of T), _COMPARER As _comparer(Of T))
    Private Shared ReadOnly predefined_column_counts As const_array(Of UInt32) =
        New const_array(Of UInt32)(doubled_prime_sequence_int32())

    Private v As array(Of [set](Of hasher_node))
    Private c As UInt32
    Private s As UInt32

    Private Sub New(ByVal c As UInt32)
        assert(c < predefined_column_counts.size())
        Me.c = c
        reset_array()
    End Sub

    <copy_constructor()>
    Protected Sub New(ByVal v As array(Of [set](Of hasher_node)),
                      ByVal s As UInt32,
                      ByVal c As UInt32)
        assert(Not v.null_or_empty())
        assert(v.size() = predefined_column_counts(c))
        assert(c < predefined_column_counts.size())
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
End Class
