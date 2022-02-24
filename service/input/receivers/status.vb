
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.template
Imports osi.root.utils

Public Class keyboard_status
    Inherits status(Of _0, _max_uint16)

    Shared Sub New()
        assert((+(New _0())) = constants.keyboard.min_meta)
        assert((+(New _max_uint16())) = constants.keyboard.max_meta)
    End Sub

    Public Function caps_lock() As Boolean
        Return down(constants.keyboard.caps_lock)
    End Function

    Public Function num_lock() As Boolean
        Return down(constants.keyboard.num_lock)
    End Function

    Public Function shift() As Boolean
        Return down(constants.keyboard.shift)
    End Function

    Public Function ctrl() As Boolean
        Return down(constants.keyboard.ctrl)
    End Function

    Public Function alt() As Boolean
        Return down(constants.keyboard.alt)
    End Function

    Public Function ctrl_alt() As Boolean
        Return ctrl() AndAlso alt()
    End Function

    Public Function ctrl_or_alt() As Boolean
        Return ctrl() OrElse alt()
    End Function
End Class

Public Class status(Of _MIN As _int64, _MAX As _int64)
    Implements istatus
    Private Shared ReadOnly min As Int64
    Private Shared ReadOnly max As Int64

    Shared Sub New()
        min = +(alloc(Of _MIN)())
        max = +(alloc(Of _MAX)())
        assert(min >= min_int32 AndAlso max <= max_int32)
    End Sub

    Private Shared Function valid_meta(ByVal i As Int32) As Boolean
        Return i >= min AndAlso i <= max
    End Function

    Private ReadOnly d() As Boolean

    Public Sub New()
        ReDim d(max - min + 1)
    End Sub

    Public Function new_status(ByVal c As [case]) As Boolean Implements istatus.new_status
        assert(Not c Is Nothing)
        If c.action = action.down OrElse
           c.action = action.up Then
            Dim i As Int32 = 0
            If bytes_int32(c.meta, i) AndAlso
               valid_meta(i) Then
                d(i - min) = (c.action = action.down)
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function

    Public Function down(ByVal i As Int32) As Boolean Implements istatus.down
        If valid_meta(i) Then
            Return d(i - min)
        Else
            Return False
        End If
    End Function
End Class

Public Interface istatus
    Function new_status(ByVal c As [case]) As Boolean
    Function down(ByVal i As Int32) As Boolean
End Interface
