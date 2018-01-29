
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
    Private Function hash(ByVal v As T) As UInt32
        Return hasher(v) Mod column_count()
    End Function

    Private Function last_column() As UInt32
        Return column_count() - uint32_1
    End Function

    Private Function column_count() As UInt32
        Return predefined_column_counts(c)
    End Function

    Private Sub reset_array()
        v = New array(Of constant(Of T))(column_count())
    End Sub

    Private Sub [set](ByVal column As UInt32, ByVal value As T)
        assert(v(column) Is Nothing)
        v(column) = constant.[New](value)
        s += uint32_1
    End Sub

    Private Sub clear(ByVal column As UInt32)
        assert(Not v(column) Is Nothing)
        v(column) = Nothing
        s -= uint32_1
    End Sub

    Private Function [is](ByVal column As UInt32, ByVal value As T) As Boolean
        Dim c As constant(Of T) = Nothing
        c = v(column)
        Return Not c Is Nothing AndAlso equaler(+c, value)
    End Function

    Private Function iterator_at(ByVal column As UInt32) As iterator
        Return New iterator(Me, column)
    End Function

    Private Function ref_at(ByVal column As UInt32) As ref
        assert(column < column_count())
        Return New ref(Me, column)
    End Function

    Private Function emplace(ByVal value As T, ByRef column As UInt32) As Boolean
        column = hash(value)
        If [is](column, value) Then
            Return False
        End If

        If v(column) Is Nothing Then
            [set](column, value)
            Return True
        End If

        If rehash() Then
            Return emplace(value, column)
        End If

        Return False
    End Function

    Private Function rehash() As Boolean
        If c = predefined_column_counts.size() - uint32_1 Then
            Return False
        End If

        Dim r As hasharray(Of T, _UNIQUE, _HASHER, _EQUALER) = Nothing
        r = New hasharray(Of T, _UNIQUE, _HASHER, _EQUALER)(c + uint32_1)
        For i As UInt32 = 0 To v.size() - uint32_1
            If Not v(i) Is Nothing Then
                assert(r.emplace(+v(i), uint32_0))
            End If
        Next
        swap(r, Me)
        Return True
    End Function
End Class
