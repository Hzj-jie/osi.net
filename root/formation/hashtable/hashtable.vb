
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.template

Partial Public Class hashtable(Of T,
                                  _UNIQUE As _boolean,
                                  _HASHER As _to_uint32(Of T),
                                  _EQUALER As _equaler(Of T))

    Private Shared ReadOnly predefined_column_counts As const_array(Of UInt32)
    Private Shared ReadOnly unique As Boolean
    Private Shared ReadOnly hasher As _HASHER
    Private Shared ReadOnly equaler As _equaler(Of T)

    Shared Sub New()
        predefined_column_counts = New const_array(Of UInt32)({3, 7, 17, 37, 79, 163, 331, 673, 1361, 2729, 5471, 10949,
                21911, 43853, 87719, 175447, 350899, 701819, 1403641, 2807303, 5614657, 11229331, 22458671, 44917381,
                89834777, 179669557, 359339171, 718678369, 1437356741})
        unique = +(alloc(Of _UNIQUE)())
        hasher = alloc(Of _HASHER)()
        equaler = alloc(Of _EQUALER)()
    End Sub

    Private v As vector(Of array(Of constant(Of T)))
    Private c As UInt32
    Private s As UInt32

    Private Sub New(ByVal c As UInt32)
        assert(c >= 0 AndAlso c < predefined_column_counts.size())
        Me.c = c
        v = _new(v)
        new_row()
    End Sub

    <copy_constructor()>
    Protected Sub New(ByVal v As vector(Of array(Of constant(Of T))), ByVal s As UInt32, ByVal c As UInt32)
        assert(Not v.null_or_empty())
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
        it = iterator_at(0, 0)
        If cell(0, 0) Is Nothing Then
            it += 1
        End If
        Return it
    End Function

    Public Function [end]() As iterator
        Return iterator.end
    End Function

    Public Function rbegin() As iterator
        Dim it As iterator = Nothing
        it = iterator_at(last_row(), last_column())
        If cell(last_row(), last_column()) Is Nothing Then
            it -= 1
        End If
        Return it
    End Function

    Public Function rend() As iterator
        Return iterator.end
    End Function
End Class
