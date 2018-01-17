
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Net
Imports osi.root.constants

' This class should not be involved by compare. Other classes should be able to register its comparer correctly with
' global_init_level.foundamental.
<global_init(global_init_level.foundamental)>
Friend Module _comparer_registry
    Sub New()
        comparer.register(Function(ByVal i As IPAddress, ByVal j As IPAddress) As Int32
                              assert(Not i Is Nothing)
                              assert(Not j Is Nothing)
                              If i.AddressFamily() = j.AddressFamily() Then
                                  Return memcmp(i.GetAddressBytes(), j.GetAddressBytes())
                              Else
                                  Return i.AddressFamily() - j.AddressFamily()
                              End If
                          End Function)
        comparer.register(Function(ByVal i As String, ByVal j As String) As Int32
                              Return strcmp(i, j)
                          End Function)

        comparer.register(Function(ByVal i() As Byte, ByVal j() As Byte) As Int32
                              Return memcmp(i, j)
                          End Function)

        comparer.register(Function(ByVal i As SByte, ByVal j As Byte) As Int32
                              Return CShort(i).CompareTo(j)
                          End Function)
        comparer.register(Function(ByVal i As SByte, ByVal j As Int16) As Int32
                              Return -j.CompareTo(i)
                          End Function)
        comparer.register(Function(ByVal i As SByte, ByVal j As UInt16) As Int32
                              Return CInt(i).CompareTo(j)
                          End Function)
        comparer.register(Function(ByVal i As SByte, ByVal j As Int32) As Int32
                              Return -j.CompareTo(i)
                          End Function)
        comparer.register(Function(ByVal i As SByte, ByVal j As UInt32) As Int32
                              Return CInt(i).CompareTo(j)
                          End Function)
        comparer.register(Function(ByVal i As SByte, ByVal j As Int64) As Int32
                              Return -j.CompareTo(i)
                          End Function)
        comparer.register(Function(ByVal i As SByte, ByVal j As UInt64) As Int32
                              Return CDec(i).CompareTo(j)
                          End Function)
        comparer.register(Function(ByVal i As SByte, ByVal j As Single) As Int32
                              Return -j.CompareTo(i)
                          End Function)
        comparer.register(Function(ByVal i As SByte, ByVal j As Double) As Int32
                              Return -j.CompareTo(i)
                          End Function)
        comparer.register(Function(ByVal i As SByte, ByVal j As Decimal) As Int32
                              Return -j.CompareTo(i)
                          End Function)

        comparer.register(Function(ByVal i As Byte, ByVal j As Int16) As Int32
                              Return -j.CompareTo(i)
                          End Function)
        comparer.register(Function(ByVal i As Byte, ByVal j As UInt16) As Int32
                              Return -j.CompareTo(i)
                          End Function)
        comparer.register(Function(ByVal i As Byte, ByVal j As Int32) As Int32
                              Return -j.CompareTo(i)
                          End Function)
        comparer.register(Function(ByVal i As Byte, ByVal j As UInt32) As Int32
                              Return -j.CompareTo(i)
                          End Function)
        comparer.register(Function(ByVal i As Byte, ByVal j As Int64) As Int32
                              Return -j.CompareTo(i)
                          End Function)
        comparer.register(Function(ByVal i As Byte, ByVal j As UInt64) As Int32
                              Return -j.CompareTo(i)
                          End Function)
        comparer.register(Function(ByVal i As Byte, ByVal j As Single) As Int32
                              Return -j.CompareTo(i)
                          End Function)
        comparer.register(Function(ByVal i As Byte, ByVal j As Double) As Int32
                              Return -j.CompareTo(i)
                          End Function)
        comparer.register(Function(ByVal i As Byte, ByVal j As Decimal) As Int32
                              Return -j.CompareTo(i)
                          End Function)

        comparer.register(Function(ByVal i As Int16, ByVal j As UInt16) As Int32
                              Return CInt(i).CompareTo(j)
                          End Function)
        comparer.register(Function(ByVal i As Int16, ByVal j As Int32) As Int32
                              Return -j.CompareTo(i)
                          End Function)
        comparer.register(Function(ByVal i As Int16, ByVal j As UInt32) As Int32
                              Return CLng(i).CompareTo(j)
                          End Function)
        comparer.register(Function(ByVal i As Int16, ByVal j As Int64) As Int32
                              Return -j.CompareTo(i)
                          End Function)
        comparer.register(Function(ByVal i As Int16, ByVal j As UInt64) As Int32
                              Return CDec(i).CompareTo(j)
                          End Function)
        comparer.register(Function(ByVal i As Int16, ByVal j As Single) As Int32
                              Return -j.CompareTo(i)
                          End Function)
        comparer.register(Function(ByVal i As Int16, ByVal j As Double) As Int32
                              Return -j.CompareTo(i)
                          End Function)
        comparer.register(Function(ByVal i As Int16, ByVal j As Decimal) As Int32
                              Return -j.CompareTo(i)
                          End Function)

        comparer.register(Function(ByVal i As UInt16, ByVal j As Int32) As Int32
                              Return -j.CompareTo(i)
                          End Function)
        comparer.register(Function(ByVal i As UInt16, ByVal j As UInt32) As Int32
                              Return -j.CompareTo(i)
                          End Function)
        comparer.register(Function(ByVal i As UInt16, ByVal j As Int64) As Int32
                              Return -j.CompareTo(i)
                          End Function)
        comparer.register(Function(ByVal i As UInt16, ByVal j As UInt64) As Int32
                              Return -j.CompareTo(i)
                          End Function)
        comparer.register(Function(ByVal i As UInt16, ByVal j As Single) As Int32
                              Return -j.CompareTo(i)
                          End Function)
        comparer.register(Function(ByVal i As UInt16, ByVal j As Double) As Int32
                              Return -j.CompareTo(i)
                          End Function)
        comparer.register(Function(ByVal i As UInt16, ByVal j As Decimal) As Int32
                              Return -j.CompareTo(i)
                          End Function)

        comparer.register(Function(ByVal i As Int32, ByVal j As UInt32) As Int32
                              Return CLng(i).CompareTo(j)
                          End Function)
        comparer.register(Function(ByVal i As Int32, ByVal j As Int64) As Int32
                              Return -j.CompareTo(i)
                          End Function)
        comparer.register(Function(ByVal i As Int32, ByVal j As UInt64) As Int32
                              Return CDec(i).CompareTo(j)
                          End Function)
        comparer.register(Function(ByVal i As Int32, ByVal j As Single) As Int32
                              Return -j.CompareTo(i)
                          End Function)
        comparer.register(Function(ByVal i As Int32, ByVal j As Double) As Int32
                              Return -j.CompareTo(i)
                          End Function)
        comparer.register(Function(ByVal i As Int32, ByVal j As Decimal) As Int32
                              Return -j.CompareTo(i)
                          End Function)

        comparer.register(Function(ByVal i As UInt32, ByVal j As Int64) As Int32
                              Return -j.CompareTo(i)
                          End Function)
        comparer.register(Function(ByVal i As UInt32, ByVal j As UInt64) As Int32
                              Return -j.CompareTo(i)
                          End Function)
        comparer.register(Function(ByVal i As UInt32, ByVal j As Single) As Int32
                              Return -j.CompareTo(i)
                          End Function)
        comparer.register(Function(ByVal i As UInt32, ByVal j As Double) As Int32
                              Return -j.CompareTo(i)
                          End Function)
        comparer.register(Function(ByVal i As UInt32, ByVal j As Decimal) As Int32
                              Return -j.CompareTo(i)
                          End Function)

        comparer.register(Function(ByVal i As Int64, ByVal j As UInt64) As Int32
                              Return CDec(i).CompareTo(j)
                          End Function)
        comparer.register(Function(ByVal i As Int64, ByVal j As Single) As Int32
                              Return -j.CompareTo(i)
                          End Function)
        comparer.register(Function(ByVal i As Int64, ByVal j As Double) As Int32
                              Return -j.CompareTo(i)
                          End Function)
        comparer.register(Function(ByVal i As Int64, ByVal j As Decimal) As Int32
                              Return -j.CompareTo(i)
                          End Function)

        comparer.register(Function(ByVal i As UInt64, ByVal j As Single) As Int32
                              Return -j.CompareTo(i)
                          End Function)
        comparer.register(Function(ByVal i As UInt64, ByVal j As Double) As Int32
                              Return -j.CompareTo(i)
                          End Function)
        comparer.register(Function(ByVal i As UInt64, ByVal j As Decimal) As Int32
                              Return -j.CompareTo(i)
                          End Function)

        comparer.register(Function(ByVal i As Single, ByVal j As Double) As Int32
                              Return -j.CompareTo(i)
                          End Function)
        comparer.register(Function(ByVal i As Single, ByVal j As Decimal) As Int32
                              Return -j.CompareTo(i)
                          End Function)

        comparer.register(Function(ByVal i As Double, ByVal j As Decimal) As Int32
                              Return -j.CompareTo(i)
                          End Function)
    End Sub

    Private Sub init()
    End Sub
End Module