
Imports System.IO
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.formation

Public Module utt_diff
    Sub New()
        enable_domain_unhandled_exception_handler()
    End Sub

    Public Sub main(ByVal args() As String)
        If array_size(args) < 2 Then
            raise_error(error_type.exclamation, "input two log files to compare")
        ElseIf array_size(args) = 2 Then
            Dim m1 As map(Of String, case_info) = Nothing
            Dim m2 As map(Of String, case_info) = Nothing
            Dim f1 As Int64 = 0
            Dim f2 As Int64 = 0
            If parse(args(0), m1, f1) AndAlso
               parse(args(1), m2, f2) Then
                compare(m1, m2, False, True)
                output("failure_count", f1, f2)
                compare(m1, m2, True, True)
                compare(m2, m1, True, False)
            End If
        Else
            Dim m() As map(Of String, case_info) = Nothing
            Dim t() As Int64 = Nothing
            Dim f() As Int64 = Nothing
            ReDim m(array_size(args) - 1)
            ReDim t(array_size(args) - 1)
            ReDim f(array_size(args) - 1)
            Dim a As [set](Of String) = Nothing
            a = New [set](Of String)()
            For i As Int32 = 0 To array_size(args) - 1
                If parse(args(i), m(i), f(i)) Then
                    Dim it As map(Of String, case_info).iterator = Nothing
                    it = m(i).begin()
                    While it <> m(i).end()
                        assert(a.insert((+it).first) <> a.end())
                        t(i) += (+it).second.time
                        it += 1
                    End While
                Else
                    Return
                End If
            Next

            Do
                Dim it As [set](Of String).iterator = Nothing
                it = a.begin()
                While it <> a.end()
                    Console.Write(strcat(+it, character.tab))
                    Dim i As Int32 = 0
                    For i = i To array_size(m) - 1
                        Dim add_tab As Boolean = False
                        add_tab = (i < array_size(m) - 1)
                        If m(i).find(+it) <> m(i).end() Then
                            output(m(i)(+it).time, m(i)(+it).time, add_tab)
                            Exit For
                        Else
                            output_no_found(add_tab)
                        End If
                    Next
                    For j As Int32 = i + 1 To array_size(m) - 1
                        Dim add_tab As Boolean = False
                        add_tab = (j < array_size(m) - 1)
                        If m(j).find(+it) <> m(j).end() Then
                            output(m(j)(+it).time, m(i)(+it).time, add_tab)
                        Else
                            output_no_found(add_tab)
                        End If
                    Next
                    Console.WriteLine()
                    it += 1
                End While
                output("total_time", t)
                output("failure_count", f)
            Loop While False
        End If
    End Sub

    Private Sub output(ByVal n As String, ByVal f() As Int64)
        Console.Write(strcat(n, character.tab))
        output(f(0), f(0), True)
        For i As Int32 = 1 To array_size(f) - 1
            output(f(i), f(0), i < array_size(f) - 1)
        Next
        Console.WriteLine()
    End Sub

    Private Sub output_no_found(ByVal add_tab As Boolean)
        Console.Write(strcat("---(---)", If(add_tab, character.tab, empty_string)))
    End Sub

    Private Sub output(ByVal nv As Int64, ByVal base As Int64, ByVal add_tab As Boolean)
        Dim orc As ConsoleColor = Nothing
        orc = Console.ForegroundColor()
        Dim perc As Double = 0
        Console.ForegroundColor() = choose_color(nv, base, perc)
        Console.Write(strcat(nv, "(", strleft(perc, 6), "%)", If(add_tab, character.tab, empty_string)))
        Console.ForegroundColor() = orc
    End Sub

    Private Function choose_color(ByVal nv As Int64, ByVal base As Int64, ByRef perc As Double) As ConsoleColor
        perc = CDbl(nv - base) * 100 / base
        If perc > 20 Then
            Return ConsoleColor.Red
        ElseIf perc < -20 Then
            Return ConsoleColor.Green
        ElseIf perc <= 1 AndAlso perc >= -1 Then
            Return ConsoleColor.DarkGray
        Else
            Return ConsoleColor.White
        End If
    End Function

    Private Sub output(ByVal name As String,
                       ByVal base As Int64,
                       ByVal nv As Int64,
                       Optional ByVal base_usage? As Double = Nothing,
                       Optional ByVal nv_usage? As Double = Nothing)
        Dim orc As ConsoleColor = Nothing
        orc = Console.ForegroundColor()
        Dim perc As Double = 0
        Console.ForegroundColor() = choose_color(nv, base, perc)
        Console.WriteLine(strcat(name,
                                 character.tab,
                                 base,
                                 character.tab,
                                 nv,
                                 character.tab,
                                 nv - base,
                                 character.tab,
                                 strleft(perc, 6),
                                 character.percent_mark,
                                 If(base_usage.HasValue() AndAlso nv_usage.HasValue(),
                                    strcat(character.tab,
                                           base_usage,
                                           character.tab,
                                           nv_usage,
                                           character.tab,
                                           strleft((nv_usage - base_usage) * 100 / base_usage, 6),
                                           character.percent_mark),
                                    Nothing)))
        Console.ForegroundColor() = orc
    End Sub

    Private Sub compare(ByVal left As map(Of String, case_info),
                        ByVal right As map(Of String, case_info),
                        ByVal miss_only As Boolean,
                        ByVal left_first As Boolean)
        assert(Not left Is Nothing)
        assert(Not right Is Nothing)
        Dim it As map(Of String, case_info).iterator = Nothing
        it = left.begin()
        Dim lt As Int64 = 0
        Dim rt As Int64 = 0
        Dim lu As Double = 0
        Dim ru As Double = 0
        While it <> left.end()
            If Not miss_only Then
                lt += (+it).second.time
                lu += (+it).second.usage
            End If
            Dim it2 As map(Of String, case_info).iterator = Nothing
            it2 = right.find((+it).first)
            If it2 = right.end() OrElse Not miss_only Then
                If Not miss_only AndAlso it2 <> right.end() Then
                    rt += (+it2).second.time
                    ru += (+it2).second.usage
                End If
                Dim base As Int64 = 0
                Dim nv As Int64 = 0
                Dim base_usage As Double = 0
                Dim nv_usage As Double = 0
                base = (+it).second.time
                base_usage = (+it).second.usage
                nv = If(it2 = right.end(), 0, (+it2).second.time)
                nv_usage = If(it2 = right.end(), 0, (+it2).second.usage)
                If Not left_first Then
                    swap(base, nv)
                End If
                If it2 <> right.end() OrElse miss_only Then
                    output((+it).first, base, nv, base_usage, nv_usage)
                End If
            End If
            it += 1
        End While

        If Not miss_only Then
            If Not left_first Then
                swap(lt, rt)
                swap(lu, ru)
            End If
            output("total_time", lt, rt, lu / left.size(), ru / right.size())
        End If
    End Sub

    Private Function parse_running_time(ByVal l As String,
                                        ByRef c As String,
                                        ByRef t As Int64,
                                        ByRef u As Double) As Boolean
        Const m1 As String = "finish running "
        Const m2 As String = ", total time in milliseconds "
        Const m3 As String = ", processor usage milliseconds "
        Const m4 As String = ", processor usage percentage "
        Const m5 As String = " at "
        Dim v As vector(Of String) = Nothing
        If strindexof(l, m1) <> npos AndAlso
           strindexof(l, m2) <> npos AndAlso
           strsplit(l, {m1, m2, m3, m4, m5}, New String() {}, v) Then
            assert(Not v Is Nothing)
            If v.size() > 1 Then
                c = v(1)
                If v.size() > 2 AndAlso Int64.TryParse(v(2), t) Then
                    If v.size() > 4 Then
                        strsep(v(4), v(4), Nothing, character.comma)
                        If Not Double.TryParse(v(4), u) OrElse
                           u = Double.NaN Then
                            u = 0
                        End If
                    End If
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function

    Private Function parse_assert_failure(ByVal l As String) As Boolean
        Const m As String = " assert failed, "
        Return strindexof(l, m) <> npos
    End Function

    Private Class case_info
        Public ReadOnly name As String
        Public ReadOnly time As Int64
        Public ReadOnly usage As Double

        Public Sub New(ByVal name As String,
                       ByVal time As Int64,
                       ByVal usage As Double)
            Me.name = name
            Me.time = time
            Me.usage = usage
        End Sub
    End Class

    Private Function parse(ByVal f As String,
                           ByRef o As vector(Of case_info),
                           ByRef failure As Int64) As Boolean
        o.renew()
        Try
            Using s As Stream = New FileStream(f, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                Using r As StreamReader = New StreamReader(s)
                    Dim l As String = Nothing
                    l = r.ReadLine()
                    While Not l Is Nothing
                        Dim c As String = Nothing
                        Dim t As Int64 = 0
                        Dim u As Double = 0
                        If parse_running_time(l, c, t, u) Then
                            o.emplace_back(New case_info(c, t, u))
                        ElseIf parse_assert_failure(l) Then
                            failure += 1
                        End If
                        l = r.ReadLine()
                    End While
                End Using
            End Using
            Return True
        Catch ex As Exception
            raise_error(error_type.exclamation, "unable to parse file ", f, ", ex ", ex.Message())
            Return False
        End Try
    End Function

    Private Function parse(ByVal f As String, ByRef o As map(Of String, case_info), ByRef failure As Int64) As Boolean
        Dim v As vector(Of case_info) = Nothing
        If parse(f, v, failure) Then
            o.renew()
            assert(Not v Is Nothing)
            For i As Int32 = 0 To v.size() - 1
                o(v(i).name) = v(i)
            Next
            Return True
        Else
            Return False
        End If
    End Function
End Module
