
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class var
    Private Sub parse_one(ByVal s As String)
        If s.null_or_empty() Then
            Return
        End If
        Dim a As String = Nothing
        Dim b As String = Nothing
        If c.is_switcher(s, a) Then
            For j As Int32 = 0 To strlen_i(a) - 1
                If raw.find(a(j)) = raw.end() Then
                    raw.insert(a(j), Nothing)
                End If
            Next
        ElseIf c.is_full_switcher(s, a) Then
            If raw.find(a) = raw.end() Then
                raw.insert(a, Nothing)
            End If
        ElseIf c.is_arg(s, a, b) Then
            'priority of switcher is higher than normal value
            If Not raw(a) Is Nothing Then
                raw(a).push_back(b)
            End If
        Else
            others.push_back(s)
        End If
    End Sub

    'param array for test only
    Public Sub parse(ByVal ParamArray args() As String)
        raw.clear()
        others.clear()
        For i As Int32 = 0 To array_size_i(args) - 1
            parse_one(args(i))
        Next
    End Sub

    Public Sub parse(ByVal args As vector(Of pair(Of String, String)))
        raw.clear()
        others.clear()
        If Not args Is Nothing Then
            For i As UInt32 = uint32_0 To args.size() - uint32_1
                assert(Not args(i) Is Nothing)
                assert(Not (args(i).first.null_or_empty() AndAlso
                            args(i).second.null_or_empty()))
                If args(i).second.null_or_empty() Then
                    parse_one(args(i).first)
                ElseIf args(i).first.null_or_empty() Then
                    parse_one(args(i).second)
                Else
                    parse_one(c.create_arg(args(i).first, args(i).second))
                End If
            Next
        End If
    End Sub

    Public Sub parse(ByVal i As String)
        If Not i.null_or_empty() Then
            Dim ss() As String = Nothing
            ss = i.Split({c.argument_separator}, StringSplitOptions.RemoveEmptyEntries)
            parse(ss)
        End If
    End Sub
End Class
