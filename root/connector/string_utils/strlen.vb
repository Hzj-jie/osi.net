
Imports System.Runtime.CompilerServices
Imports System.Text
Imports osi.root.constants

Public Module _strlen
    <Extension()> Public Function strlen(ByVal input As String) As UInt32
        Return input.len()
    End Function

    <Extension()> Public Function strlen(ByVal input As StringBuilder) As UInt32
        Return input.len()
    End Function

    <Extension()> Public Function len(ByVal input As String) As UInt32
        Return If(input Is Nothing, uint32_0, CUInt(input.Length()))
    End Function

    <Extension()> Public Function len(ByVal input As StringBuilder) As UInt32
        Return If(input Is Nothing, uint32_0, CUInt(input.Length()))
    End Function
End Module
