
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.template

Partial Public Class hashmap(Of KEY_T As IComparable(Of KEY_T),
                                VALUE_T,
                                _KEY_TO_INDEX As _to_uint32(Of KEY_T))
    Private Function valid_index(ByVal index As UInt32) As Boolean
        Return index < hash_size
    End Function

    Private Function invalid_index(ByVal index As UInt32) As Boolean
        Return Not valid_index(index)
    End Function

    Private Function data(ByVal index As UInt32, Optional ByVal auto_insert As Boolean = True) As map(Of KEY_T, VALUE_T)
        If _data(CInt(index)) Is Nothing AndAlso auto_insert Then
            _data(CInt(index)) = New map(Of KEY_T, VALUE_T)()
        End If
        Return _data(CInt(index))
    End Function

    Private Function empty(ByVal i As UInt32) As Boolean
        assert(valid_index(i))
        Return data(i, False) Is Nothing OrElse data(i).empty()
    End Function

    Private Function size(ByVal i As UInt32) As UInt32
        assert(valid_index(i))
        Dim r As map(Of KEY_T, VALUE_T) = Nothing
        r = data(i, False)
        Return If(r Is Nothing, uint32_0, r.size())
    End Function

    Private Function prev_index(ByVal this As UInt32) As UInt32
        Do
            If this = 0 Then
                this = max_uint32
            Else
                this -= uint32_1
            End If
            If invalid_index(this) Then
                Exit Do
            End If
        Loop Until Not empty(this)

        Return this
    End Function

    Private Function first_index() As UInt32
        Const r As UInt32 = 0
        If empty(r) Then
            Return next_index(r)
        End If
        Return r
    End Function

    Private Function next_index(ByVal this As UInt32) As UInt32
        Do
            this += uint32_1
            If invalid_index(this) Then
                Exit Do
            End If
        Loop Until Not empty(this)

        Return this
    End Function

    Private Function key_index(ByVal k As KEY_T) As UInt32
        Return key_to_index(k) Mod hash_size
    End Function
End Class
