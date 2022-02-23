
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.utils
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class utt_diff
    Private Shared inputs As argument(Of vector(Of String))

    <command_line_specified>
    <test>
    Private Shared Sub run()
        If (+inputs).size() < 2 Then
            raise_error(error_type.exclamation, "input two log files to compare")
            Return
        End If
        If (+inputs).size() = 2 Then
            Dim m1 As map(Of String, case_info) = Nothing
            Dim m2 As map(Of String, case_info) = Nothing
            Dim f1 As Int64 = 0
            Dim f2 As Int64 = 0
            If parse((+inputs)(0), m1, f1) AndAlso
               parse((+inputs)(1), m2, f2) Then
                compare(m1, m2, False, True)
                output("failure_count", f1, f2)
                compare(m1, m2, True, True)
                compare(m2, m1, True, False)
            End If
            Return
        End If
        Dim m(CInt((+inputs).size() - 1)) As map(Of String, case_info)
        Dim t(CInt((+inputs).size() - 1)) As Int64
        Dim f(CInt((+inputs).size() - 1)) As Int64
        Dim a As New [set](Of String)()
        Using code_block
            For i As Int32 = 0 To CInt((+inputs).size() - uint32_1)
                If parse((+inputs)(CUInt(i)), m(i), f(i)) Then
                    Dim it As map(Of String, case_info).iterator = m(i).begin()
                    While it <> m(i).end()
                        assert(a.insert((+it).first) <> a.end())
                        t(i) += (+it).second.time
                        it += 1
                    End While
                Else
                    Return
                End If
            Next
        End Using

        Using code_block
            Dim it As [set](Of String).iterator = a.begin()
            While it <> a.end()
                Console.Write(strcat(+it, character.tab))
                Dim i As Int32 = 0
                For i = i To m.array_size_i() - 1
                    Dim add_tab As Boolean = (i < array_size(m) - 1)
                    If m(i).find(+it) <> m(i).end() Then
                        output(m(i)(+it).time, m(i)(+it).time, add_tab)
                        Exit For
                    End If
                    output_no_found(add_tab)
                Next
                For j As Int32 = i + 1 To m.array_size_i() - 1
                    Dim add_tab As Boolean = (j < array_size(m) - 1)
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
        End Using
    End Sub

    Private Shared Sub output(ByVal n As String, ByVal f() As Int64)
        Console.Write(strcat(n, character.tab))
        output(f(0), f(0), True)
        For i As Int32 = 1 To f.array_size_i() - 1
            output(f(i), f(0), i < array_size(f) - 1)
        Next
        Console.WriteLine()
    End Sub

    Private Shared Sub output_no_found(ByVal add_tab As Boolean)
        Console.Write(strcat("---(---)", If(add_tab, character.tab, "")))
    End Sub

    Private Shared Sub output(ByVal nv As Int64, ByVal base As Int64, ByVal add_tab As Boolean)
        Dim orc As ConsoleColor = Console.ForegroundColor()
        Dim perc As Double = 0
        Console.ForegroundColor() = choose_color(nv, base, perc)
        Console.Write(strcat(nv,
                             "(",
                             strleft(Convert.ToString(perc), 6),
                             "%)",
                             If(add_tab, character.tab, "")))
        Console.ForegroundColor() = orc
    End Sub

    Private Shared Function choose_color(ByVal nv As Int64, ByVal base As Int64, ByRef perc As Double) As ConsoleColor
        perc = CDbl(nv - base) * 100 / base
        If perc > 20 Then
            Return ConsoleColor.Red
        End If
        If perc < -20 Then
            Return ConsoleColor.Green
        End If
        If perc <= 1 AndAlso perc >= -1 Then
            Return ConsoleColor.DarkGray
        End If
        Return ConsoleColor.White
    End Function

    Private Shared Sub output(ByVal name As String,
                              ByVal base As Int64,
                              ByVal nv As Int64,
                              Optional ByVal base_usage? As Double = Nothing,
                              Optional ByVal nv_usage? As Double = Nothing)
        Dim orc As ConsoleColor = Console.ForegroundColor()
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
                                 strleft(Convert.ToString(perc), 6),
                                 character.percent_mark,
                                 If(base_usage.HasValue() AndAlso nv_usage.HasValue(),
                                    strcat(character.tab,
                                           base_usage,
                                           character.tab,
                                           nv_usage,
                                           character.tab,
                                           strleft(Convert.ToString((nv_usage - base_usage) * 100 / base_usage), 6),
                                           character.percent_mark),
                                    Nothing)))
        Console.ForegroundColor() = orc
    End Sub

    Private Shared Sub compare(ByVal left As map(Of String, case_info),
                               ByVal right As map(Of String, case_info),
                               ByVal miss_only As Boolean,
                               ByVal left_first As Boolean)
        assert(left IsNot Nothing)
        assert(right IsNot Nothing)
        Dim it As map(Of String, case_info).iterator = left.begin()
        Dim lt As Int64 = 0
        Dim rt As Int64 = 0
        Dim lu As Double = 0
        Dim ru As Double = 0
        While it <> left.end()
            If Not miss_only Then
                lt += (+it).second.time
                lu += (+it).second.usage
            End If
            Dim it2 As map(Of String, case_info).iterator = right.find((+it).first)
            If it2 = right.end() OrElse Not miss_only Then
                If Not miss_only AndAlso it2 <> right.end() Then
                    rt += (+it2).second.time
                    ru += (+it2).second.usage
                End If
                Dim base As Int64 = (+it).second.time
                Dim nv As Int64 = If(it2 = right.end(), 0, (+it2).second.time)
                Dim base_usage As Double = (+it).second.usage
                Dim nv_usage As Double = If(it2 = right.end(), 0, (+it2).second.usage)
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

    Private Shared Function parse_running_time(ByVal l As String,
                                               ByRef c As String,
                                               ByRef t As Int64,
                                               ByRef u As Double) As Boolean
        Const m1 As String = "finish running "
        Const m2 As String = ", total time in milliseconds "
        Const m3 As String = ", processor usage milliseconds "
        Const m4 As String = ", processor usage percentage "
        Const m5 As String = " at "
        Dim v As vector(Of String) = Nothing
        If strindexof(l, m1) = npos OrElse
           strindexof(l, m2) = npos OrElse
           Not strsplit(l, {m1, m2, m3, m4, m5}, New String() {}, v) Then
            Return False
        End If
        assert(v IsNot Nothing)
        If v.size() <= 1 Then
            Return False
        End If
        c = v(1)
        If v.size() <= 2 OrElse Not Int64.TryParse(v(2), t) Then
            Return False
        End If
        If v.size() > 4 Then
            strsep(v(4), v(4), Nothing, character.comma)
            If Not Double.TryParse(v(4), u) OrElse u = Double.NaN Then
                u = 0
            End If
        End If
        Return True
    End Function

    Private Shared Function parse_assert_failure(ByVal l As String) As Boolean
        Const m As String = " assert failed, "
        Return strindexof(l, m) <> npos
    End Function

    Private NotInheritable Class case_info
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

    Private Shared Function parse(ByVal f As String,
                                  ByRef o As vector(Of case_info),
                                  ByRef failure As Int64) As Boolean
        o.renew()
        Try
            Using s As Stream = New FileStream(f, FileMode.Open, FileAccess.Read, FileShare.ReadWrite),
                  r As StreamReader = New StreamReader(s)
                Dim l As String = r.ReadLine()
                While l IsNot Nothing
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
            Return True
        Catch ex As Exception
            raise_error(error_type.exclamation, "unable to parse file ", f, ", ex ", ex.Message())
            Return False
        End Try
    End Function

    Private Shared Function parse(ByVal f As String, ByRef o As map(Of String, case_info), ByRef failure As Int64) As Boolean
        Dim v As vector(Of case_info) = Nothing
        If Not parse(f, v, failure) Then
            Return False
        End If
        o.renew()
        assert(v IsNot Nothing)
        For i As Int32 = 0 To CInt(v.size()) - 1
            o(v(CUInt(i)).name) = v(CUInt(i))
        Next
        Return True
    End Function

    Private Sub New()
    End Sub
End Class
