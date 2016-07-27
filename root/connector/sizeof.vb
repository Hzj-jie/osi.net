
Imports System.Runtime.InteropServices
Imports osi.root.constants

Public Module _sizeof
    Public ReadOnly sizeof_bool As UInt32 = sizeof(Of Boolean)()
    Public ReadOnly sizeof_sbyte As UInt32 = sizeof(Of SByte)()
    Public ReadOnly sizeof_byte As UInt32 = sizeof(Of Byte)()
    Public ReadOnly sizeof_int8 As UInt32 = sizeof(Of SByte)()
    Public ReadOnly sizeof_uint8 As UInt32 = sizeof(Of Byte)()
    Public ReadOnly sizeof_int16 As UInt32 = sizeof(Of Int16)()
    Public ReadOnly sizeof_uint16 As UInt32 = sizeof(Of UInt16)()
    Public ReadOnly sizeof_int32 As UInt32 = sizeof(Of Int32)()
    Public ReadOnly sizeof_uint32 As UInt32 = sizeof(Of UInt32)()
    Public ReadOnly sizeof_int64 As UInt32 = sizeof(Of Int64)()
    Public ReadOnly sizeof_uint64 As UInt32 = sizeof(Of UInt64)()
    Public ReadOnly sizeof_single As UInt32 = sizeof(Of Single)()
    Public ReadOnly sizeof_double As UInt32 = sizeof(Of Double)()

    Private Class sizeof_cache(Of T)
        Public Shared ReadOnly size As Int32

        Shared Sub New()
            size = sizeof(GetType(T))
        End Sub
    End Class

    Public Function sizeof(Of T)() As Int32
        Return sizeof_cache(Of T).size
    End Function

    Public Function sizeof(Of T)(ByVal obj As T) As Int32
        Try
            Return Marshal.SizeOf(obj)
        Catch
            Return npos
        End Try
    End Function

    Public Function sizeof(ByVal t As Type) As Int32
        Try
            Return Marshal.SizeOf(t)
        Catch
            Return npos
        End Try
    End Function

    Public Function sizeof(Of T)(ByVal i() As T) As Int32
        Return array_size(i)
    End Function
End Module
