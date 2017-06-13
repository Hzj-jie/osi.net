
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.template

Partial Public Class hashtable(Of T,
                                  _UNIQUE As _boolean,
                                  _COLUMN_SIZE As _int64,
                                  _HASHER As _to_uint32(Of T),
                                  _COMPARER As _comparer(Of T))
    Public Function find(ByVal value As T) As iterator
        Dim index As UInt32 = 0
        index = hash(value)
        For i As UInt32 = 0 To last_row()
            If Not cell(i, index) Is Nothing AndAlso compare(+cell(i, index), value) = 0 Then
                Return iterator_at(i, index)
            End If
        Next
        Return iterator.[end]
    End Function

    Public Function emplace(ByVal value As T) As Boolean
        Dim index As UInt32 = 0
        index = hash(value)
        If unique Then
            For i As UInt32 = 0 To last_row()
                If Not cell(i, index) Is Nothing AndAlso compare(+cell(i, index), value) = 0 Then
                    Return False
                End If
            Next
        End If
        For i As UInt32 = 0 To last_row()
            If cell(i, index) Is Nothing Then
                set_cell(i, index, value)
                Return True
            End If
        Next
        new_row()
        set_cell(last_row(), index, value)
        Return True
    End Function

    Public Function insert(ByVal value As T) As Boolean
        Return emplace(copy_no_error(value))
    End Function

    Public Function [erase](ByVal value As T) As UInt32
        Dim r As UInt32 = 0
        Dim index As UInt32 = 0
        index = hash(value)
        For i As UInt32 = 0 To last_row()
            If Not cell(i, index) Is Nothing AndAlso compare(+cell(i, index), value) = 0 Then
                clear_cell(i, index)
                r += uint32_1
                If unique Then
                    Exit For
                End If
            End If
        Next
        Return r
    End Function

    Public Sub clear()
        v.clear()
        new_row()
        s = 0
    End Sub
End Class
