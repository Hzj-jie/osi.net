
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with bit_array_thread_safe.vbp ----------
'so change bit_array_thread_safe.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with bit_array.vbp ----------
'so change bit_array.vbp instead of this file


#If True Then
Imports System.Threading
#End If
Imports osi.root.constants
Imports osi.root.connector

#If True Then
Public NotInheritable Class bit_array_thread_safe
#Else
Public NotInheritable Class bit_array
#End If
    Private Shared ReadOnly bit_count_in_uint32 As Byte = CByte(bit_count_in_byte * sizeof_uint32)
#If True Then
    Private b() As Int32
#Else
    Private b() As UInt32
#End If
    Private _size As UInt32

    Public Sub New()
        Me.New(0)
    End Sub

    Public Sub New(ByVal size As UInt32)
        resize(size)
    End Sub

    Public Sub resize(ByVal size As UInt32)
        _size = size
        If size Mod bit_count_in_uint32 = 0 Then
            ReDim b(CInt(size \ bit_count_in_uint32) - 1)
        Else
            ReDim b(CInt(size \ bit_count_in_uint32))
        End If
    End Sub

    Public Function size() As UInt32
        Return _size
    End Function

    Public Sub clear()
        arrays.clear(b)
    End Sub

    Private Sub devide(ByVal i As UInt32, ByRef p As Byte, ByRef index As Int32)
        assert(i < size())
        p = CByte(i Mod bit_count_in_uint32)
        index = CInt(i \ bit_count_in_uint32)
        assert(index < array_size_i(b))
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
#If True Then
            While True
                Dim d As Int32 = 0
                d = b(index)
                Dim n As Int32 = 0
                n = d
                setbit(n, p, value)
                If Interlocked.CompareExchange(b(index), n, d) = d Then
                    Exit While
                End If
            End While
#Else
            setbit(b(index), p, value)
#End If
        End Set
    End Property
End Class
'finish bit_array.vbp --------
'finish bit_array_thread_safe.vbp --------
