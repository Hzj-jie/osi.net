
Option Explicit On
Option Infer Off
Option Strict On

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

    <Extension()> Public Function strlen_i(ByVal input As String) As Int32
        Return input.len_i()
    End Function

    <Extension()> Public Function strlen_i(ByVal input As StringBuilder) As Int32
        Return input.len_i()
    End Function

    <Extension()> Public Function len(ByVal input As String) As UInt32
        Return CUInt(len_i(input))
    End Function

    <Extension()> Public Function len(ByVal input As StringBuilder) As UInt32
        Return CUInt(len_i(input))
    End Function

    <Extension()> Public Function len_i(ByVal input As String) As Int32
        Return If(input Is Nothing, 0, input.Length())
    End Function

    <Extension()> Public Function len_i(ByVal input As StringBuilder) As Int32
        Return If(input Is Nothing, 0, input.Length())
    End Function

    <Extension()> Public Function last_index(ByVal input As String) As Int32
        Return len_i(input) - 1
    End Function

    <Extension()> Public Function last_index(ByVal input As StringBuilder) As Int32
        Return len_i(input) - 1
    End Function

    <Extension()> Public Sub last_index(ByVal input As StringBuilder, ByVal v As Int32)
        throws.not_null(input)
        v += 1
        assert(v >= 0)
        If input.Length() <> v Then
            input.Length() = v
        End If
    End Sub
End Module
