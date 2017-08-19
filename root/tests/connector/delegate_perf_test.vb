
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.utils
Imports osi.root.utt

Public Class delegate_perf_test
    Inherits [case]

    Private Const size As Int64 = 1024 * 1024 * 1024

    Private Sub increment(ByRef i As Int64)
        i += 1
    End Sub

    Private Sub increment()
        Dim i As Int32 = 0
        i += 1
    End Sub

    Private Sub void2(ByVal d As Action)
        Try
            d()
        Catch ex As Exception
            log_unhandled_exception(ex)
        End Try
    End Sub

    Private Sub run_case(ByVal name As String, ByVal v As action)
        assert(Not v Is Nothing)
        Dim start_ms As Int64 = 0
        raise_error("start ", name)
        start_ms = nowadays.high_res_milliseconds()
        v()
        raise_error("finished ",
                      name,
                      ", used ",
                      nowadays.high_res_milliseconds() - start_ms,
                      " milliseconds")
    End Sub

    Public Overrides Function run() As Boolean
        run_case("function call",
                 Sub()
                     For i As Int64 = 0 To size - 1
                         increment()
                     Next
                 End Sub)
        run_case("delegate <Action>",
                 Sub()
                     Dim v As Action = Nothing
                     v = AddressOf increment
                     For i As Int64 = 0 To size - 1
                         v()
                     Next
                 End Sub)
        run_case("void2 delegate <Action>",
                 Sub()
                     Dim v As Action = Nothing
                     v = AddressOf increment
                     For i As Int64 = 0 To size - 1
                         void2(v)
                     Next
                 End Sub)
        run_case("void_ delegate <Action>",
                 Sub()
                     Dim v As Action = Nothing
                     v = AddressOf increment
                     For i As Int64 = 0 To size - 1
                         void_(v)
                     Next
                 End Sub)
        run_case("function call with parameter",
                 Sub()
                     Dim j As Int64 = 0
                     For i As Int64 = 0 To size - 1
                         increment(j)
                     Next
                 End Sub)
        run_case("delegate with parameter",
                 Sub()
                     Dim v As void(Of Int64) = Nothing
                     v = AddressOf increment
                     Dim j As Int64 = 0
                     For i As Int64 = 0 To size - 1
                         v(j)
                     Next
                 End Sub)
        run_case("void_ delegate with parameter",
                 Sub()
                     Dim v As void(Of Int64) = Nothing
                     v = AddressOf increment
                     Dim j As Int64 = 0
                     For i As Int64 = 0 To size - 1
                         void_(v, j)
                     Next
                 End Sub)
        Return True
    End Function

    Public Overrides Function reserved_processors() As Int16
        Return Environment.ProcessorCount()
    End Function
End Class
