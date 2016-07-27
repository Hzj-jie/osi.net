
Imports System.DateTime
Imports System.Runtime.CompilerServices
Imports osi.root.connector

'public for test cases
Public Module _timestamp
    Public Function now_as_timestamp() As Int64
        Return Now().to_timestamp()
    End Function

    <Extension()> Public Function to_timestamp(ByVal this As Date) As Int64
        Return this.Ticks()
    End Function

    <Extension()> Public Function to_date(ByVal this As Int64) As Date
        Return New Date(this)
    End Function

    <Extension()> Public Function to_bytes(ByVal i As Int64) As Byte()
        Return int64_bytes(i)
    End Function

    <Extension()> Public Function to_timestamp(ByVal i() As Byte, ByRef o As Int64) As Boolean
        Return bytes_int64(i, o)
    End Function
End Module
