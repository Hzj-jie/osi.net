
Imports System.Runtime.CompilerServices
Imports System.IO
Imports osi.root.formation

Public Module _text_reader
    <Extension()> Public Function read_lines(ByVal r As TextReader) As vector(Of String)
        If r Is Nothing Then
            Return Nothing
        Else
            Dim v As vector(Of String) = Nothing
            v = New vector(Of String)()
            Dim l As String = Nothing
            l = r.ReadLine()
            While Not l Is Nothing
                v.emplace_back(l)
                l = r.ReadLine()
            End While
            Return v
        End If
    End Function
End Module
