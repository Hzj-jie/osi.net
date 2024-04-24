
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class dump_results
    Private Shared inputs As argument(Of vector(Of String))
    Private Shared output As argument(Of String)

    Public Shared Function intersect(Of T)(ByVal models As vector(Of T),
                                            ByVal load As Func(Of T, unordered_map(Of String, Double))) _
                                    As unordered_map(Of String, Double)
        assert(Not models.null_or_empty())
        assert(Not load Is Nothing)
        Dim m As unordered_map(Of String, Double) = Nothing
        For Each model As T In +models
            If m Is Nothing Then
                m = load(model)
            Else
                Dim o As unordered_map(Of String, Double) = m
                m = New unordered_map(Of String, Double)()
                load(model).stream().foreach(Sub(ByVal p As first_const_pair(Of String, Double))
                                                 assert(Not p Is Nothing)
                                                 Dim it As unordered_map(Of String, Double).iterator = o.find(p.first)
                                                 If it <> o.end() Then
                                                     m(p.first) = p.second * (+it).second
                                                 End If
                                             End Sub)
            End If
        Next
        Return m
    End Function

    <command_line_specified>
    <test>
    Private Shared Sub intersect_ssv()
        Dim m As unordered_map(Of String, Double) =
            intersect(+inputs,
                      Function(ByVal f As String) As unordered_map(Of String, Double)
                          ' Any exceptions would trigger failures.
                          Dim lines() As String = File.ReadAllLines(f)
                          Dim r As New unordered_map(Of String, Double)()
                          For Each line As String In lines
                              Dim first As String = Nothing
                              Dim second As String = Nothing
                              assert(line.strsep(first, second, " "))
                              r(first) = Double.Parse(second)
                          Next
                          Return r
                      End Function
)
        Using w As TextWriter = If(-output, New StreamWriter(New FileStream(+output, FileMode.Create)), Console.Out)
            Dim it As unordered_map(Of String, Double).iterator = m.begin()
            While it <> m.end()
                w.WriteLine(String.Concat((+it).first, " ", (+it).second))
                it += 1
            End While
        End Using
    End Sub
End Class
