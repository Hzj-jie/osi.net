
Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Text
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils

Public Module _as
    <Extension()> Public Function as_stream(ByVal b() As Byte) As Stream
        If isemptyarray(b) Then
            Return Nothing
        Else
            Return New MemoryStream(b)
        End If
    End Function

    <Extension()> Public Function as_text_reader(ByVal b() As Byte,
                                                 ByVal e As Encoding,
                                                 ByVal detect_bom As ternary) As TextReader
        Dim s As Stream = Nothing
        s = as_stream(b)
        If s Is Nothing Then
            Return Nothing
        Else
            Return If(e Is Nothing,
                      If(detect_bom.unknown_(),
                         New StreamReader(s),
                         New StreamReader(s, detect_bom.true_())),
                      If(detect_bom.unknown_(),
                         New StreamReader(s, e),
                         New StreamReader(s, e, detect_bom.true_())))
        End If
    End Function

    <Extension()> Public Function as_text_reader(ByVal b() As Byte,
                                                 ByVal e As Encoding,
                                                 ByVal detect_bom As Boolean) As TextReader
        Return as_text_reader(b, e, If(detect_bom, ternary.true, ternary.false))
    End Function

    <Extension()> Public Function as_text_reader(ByVal b() As Byte,
                                                 ByVal e As Encoding) As TextReader
        Return as_text_reader(b, e, ternary.unknown)
    End Function

    <Extension()> Public Function as_text_reader(ByVal b() As Byte,
                                                 ByVal detect_bom As ternary) As TextReader
        Return as_text_reader(b, Nothing, detect_bom)
    End Function

    <Extension()> Public Function as_text_reader(ByVal b() As Byte,
                                                 ByVal detect_bom As Boolean) As TextReader
        Return as_text_reader(b, Nothing, detect_bom)
    End Function

    <Extension()> Public Function as_text_reader(ByVal b() As Byte) As TextReader
        Return as_text_reader(b, ternary.unknown)
    End Function

    <Extension()> Public Function as_text(ByVal b() As Byte,
                                          ByVal e As Encoding,
                                          ByVal detect_bom As ternary) As String
        Using r As TextReader = as_text_reader(b, e, detect_bom)
            If r Is Nothing Then
                Return Nothing
            Else
                Return r.ReadToEnd()
            End If
        End Using
    End Function

    <Extension()> Public Function as_text(ByVal b() As Byte,
                                          ByVal e As Encoding,
                                          ByVal detect_bom As Boolean) As String
        Return as_text(b, e, If(detect_bom, ternary.true, ternary.false))
    End Function

    <Extension()> Public Function as_text(ByVal b() As Byte,
                                          ByVal e As Encoding) As String
        Return as_text(b, e, ternary.unknown)
    End Function

    <Extension()> Public Function as_text(ByVal b() As Byte,
                                          ByVal detect_bom As ternary) As String
        Return as_text(b, Nothing, detect_bom)
    End Function

    <Extension()> Public Function as_text(ByVal b() As Byte,
                                          ByVal detect_bom As Boolean) As String
        Return as_text(b, Nothing, detect_bom)
    End Function

    <Extension()> Public Function as_text(ByVal b() As Byte) As String
        Return as_text(b, ternary.unknown)
    End Function

    <Extension()> Public Function as_lines(ByVal b() As Byte,
                                           ByVal e As Encoding,
                                           ByVal detect_bom As ternary) As vector(Of String)
        Using r As TextReader = as_text_reader(b, e, detect_bom)
            Return r.read_lines()
        End Using
    End Function

    <Extension()> Public Function as_lines(ByVal b() As Byte,
                                           ByVal e As Encoding,
                                           ByVal detect_bom As Boolean) As vector(Of String)
        Return as_lines(b, e, If(detect_bom, ternary.true, ternary.false))
    End Function

    <Extension()> Public Function as_lines(ByVal b() As Byte,
                                           ByVal e As Encoding) As vector(Of String)
        Return as_lines(b, e, ternary.unknown)
    End Function

    <Extension()> Public Function as_lines(ByVal b() As Byte,
                                           ByVal detect_bom As ternary) As vector(Of String)
        Return as_lines(b, Nothing, detect_bom)
    End Function

    <Extension()> Public Function as_lines(ByVal b() As Byte,
                                           ByVal detect_bom As Boolean) As vector(Of String)
        Return as_lines(b, Nothing, detect_bom)
    End Function

    <Extension()> Public Function as_lines(ByVal b() As Byte) As vector(Of String)
        Return as_lines(b, ternary.unknown)
    End Function
End Module
