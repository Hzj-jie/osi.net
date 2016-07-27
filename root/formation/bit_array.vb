
Imports osi.root.constants
Imports osi.root.connector

Public Class bit_array
    Private Shared ReadOnly bit_count_in_uint32 As Byte = bit_count_in_byte * sizeof_uint32
    Private b() As UInt32

    Public Sub New()
    End Sub

    Public Sub New(ByVal size As UInt32)
        resize(size)
    End Sub

    Public Sub resize(ByVal size As UInt32)
        If size Mod bit_count_in_uint32 = 0 Then
            ReDim b(size \ bit_count_in_uint32 - 1)
        Else
            ReDim b(size \ bit_count_in_uint32)
        End If
    End Sub

    Public Function size() As UInt32
        Return array_size(b) * bit_count_in_uint32
    End Function

    Public Sub clear()
        memclr(b)
    End Sub

    Private Sub devide(ByVal i As UInt32, ByRef p As Byte, ByRef index As Int32)
        p = i Mod bit_count_in_uint32
        index = CInt(i \ bit_count_in_uint32)
        assert(index < size())
    End Sub

    Default Public Property data(ByVal i As UInt32) As Boolean
        Get
            Dim index As Int32 = 0
            Dim p As Byte = uint8_0
            devide(i, p, index)
            Return getbit(b(index), p)
        End Get
        Set(ByVal value As Boolean)
            Dim index As Int32 = 0
            Dim p As Byte = uint8_0
            devide(i, p, index)
            setbit(b(index), p, value)
        End Set
    End Property
End Class
