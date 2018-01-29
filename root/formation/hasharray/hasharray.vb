
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.template

Partial Public Class hasharray(Of T,
                                  _UNIQUE As _boolean,
                                  _HASHER As _to_uint32(Of T),
                                  _EQUALER As _equaler(Of T))

    Private Shared ReadOnly predefined_column_counts As const_array(Of UInt32)
    Private Shared ReadOnly hasher As _HASHER
    Private Shared ReadOnly equaler As _equaler(Of T)

    Shared Sub New()
        assert(+alloc(Of _UNIQUE)() = True, "_UNIQUE of hasharray should be true.")
        predefined_column_counts = New const_array(Of UInt32)(doubled_prime_sequence_int32())
        hasher = alloc(Of _HASHER)()
        equaler = alloc(Of _EQUALER)()
    End Sub

    Private v As array(Of constant(Of T))
    Private c As UInt32
    Private s As UInt32

    Private Sub New(ByVal c As UInt32)
        assert(c < predefined_column_counts.size())
        Me.c = c
        reset_array()
    End Sub

    <copy_constructor()>
    Protected Sub New(ByVal v As array(Of constant(Of T)), ByVal s As UInt32, ByVal c As UInt32)
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

    Public Function size() As UInt32
        Return s
    End Function

    Public Function empty() As Boolean
        Return size() = uint32_0
    End Function

    Public Function begin() As iterator
        Dim it As iterator = Nothing
        it = iterator_at(0)
        If v(0) Is Nothing Then
            it += 1
        End If
        Return it
    End Function

    Public Function [end]() As iterator
        Return iterator.end
    End Function

    Public Function rbegin() As iterator
        Dim it As iterator = Nothing
        it = iterator_at(last_column())
        If v(last_column()) Is Nothing Then
            it -= 1
        End If
        Return it
    End Function

    Public Function rend() As iterator
        Return iterator.end
    End Function
End Class
