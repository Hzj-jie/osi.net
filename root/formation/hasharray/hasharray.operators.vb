
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
    Public Function find(ByVal value As T) As iterator
        Dim column As UInt32 = 0
        column = hash(value)
        If [is](column, value) Then
            Return iterator_at(column)
        End If
        Return iterator.end
    End Function

    Public Function emplace(ByVal value As T) As pair(Of iterator, Boolean)
        Dim column As UInt32 = 0
        Dim r As Boolean = False
        r = emplace(value, column)
        Return emplace_make_pair(iterator_at(column), r)
    End Function

    Public Function insert(ByVal value As T) As pair(Of iterator, Boolean)
        Return emplace(copy_no_error(value))
    End Function

    Public Function [erase](ByVal value As T) As UInt32
        Dim column As UInt32 = 0
        column = hash(value)
        If [is](column, value) Then
            clear(column)
            Return uint32_1
        End If
        Return uint32_0
    End Function

    Public Function [erase](ByVal it As iterator) As Boolean
        If it = [end]() OrElse object_compare(it.ref().owner, Me) <> 0 OrElse it.ref().empty() Then
            Return False
        End If

        clear(it.ref().column)
        Return True
    End Function

    Public Sub clear()
        reset_array()
        s = 0
    End Sub
End Class
