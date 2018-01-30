
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.template

Partial Public Class hashset(Of T,
                                _UNIQUE As _boolean,
                                _HASHER As _to_uint32(Of T),
                                _EQUALER As _equaler(Of T))

    Private Shared ReadOnly predefined_row_counts As const_array(Of UInt32)
    Private Shared ReadOnly row_count_upper_bound As UInt32
    Private Shared ReadOnly hasher As _HASHER

    Shared Sub New()
        assert(+alloc(Of _UNIQUE)(), "hashset supports only unique storage.")
        assert(type_info(Of _EQUALER, type_info_operators.equal, default_equaler(Of T)).v,
               "hashset supports only default equaler.")
        predefined_row_counts = New const_array(Of UInt32)(doubled_prime_sequence_int32())
        row_count_upper_bound = 16
        hasher = alloc(Of _HASHER)()
    End Sub

    Private v As array(Of [set](Of T))
    Private c As UInt32
    Private s As UInt32

    Private Sub New(ByVal c As UInt32)
        assert(c < predefined_row_counts.size())
        Me.c = c
        reset_array()
    End Sub

    <copy_constructor()>
    Protected Sub New(ByVal v As array(Of [set](Of T)), ByVal s As UInt32, ByVal c As UInt32)
        assert(Not v.null_or_empty())
        assert(c < predefined_row_counts.size())
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
        it = iterator_at(0, v(0).begin())
        If it.ref().empty() Then
            it += 1
        End If
        Return it
    End Function

    Public Function [end]() As iterator
        Return iterator.end
    End Function

    Public Function rbegin() As iterator
        Dim it As iterator = Nothing
        it = iterator_at(last_row(), v(last_row()).rbegin())
        If it.ref().empty() Then
            it -= 1
        End If
        Return it
    End Function

    Public Function rend() As iterator
        Return iterator.end
    End Function
End Class
