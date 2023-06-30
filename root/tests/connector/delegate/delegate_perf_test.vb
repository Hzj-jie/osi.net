
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.utt

Public Class delegate_perf_test
    Inherits performance_comparison_case_wrapper

    Private Const size As Int64 = 1024 * 1024 * 1024

    Public Sub New()
        MyBase.New(New delegate_case(Sub()
                                         run_case("function call",
                                                  Sub()
                                                      For i As Int64 = 0 To size - 1
                                                          increment()
                                                      Next
                                                  End Sub)
                                     End Sub),
                   New delegate_case(Sub()
                                         run_case("delegate <Action>",
                                                  Sub()
                                                      Dim v As Action = Nothing
                                                      v = AddressOf increment
                                                      For i As Int64 = 0 To size - 1
                                                          v()
                                                      Next
                                                  End Sub)
                                     End Sub),
                   New delegate_case(Sub()
                                         run_case("void2 delegate <Action>",
                                                  Sub()
                                                      Dim v As Action = Nothing
                                                      v = AddressOf increment
                                                      For i As Int64 = 0 To size - 1
                                                          void2(v)
                                                      Next
                                                  End Sub)
                                     End Sub),
                   New delegate_case(Sub()
                                         run_case("void_ delegate <Action>",
                                                  Sub()
                                                      Dim v As Action = Nothing
                                                      v = AddressOf increment
                                                      For i As Int64 = 0 To size - 1
                                                          void_(v)
                                                      Next
                                                  End Sub)
                                     End Sub),
                   New delegate_case(Sub()
                                         run_case("function call with parameter",
                                                  Sub()
                                                      Dim j As Int64 = 0
                                                      For i As Int64 = 0 To size - 1
                                                          increment(j)
                                                      Next
                                                  End Sub)
                                     End Sub),
                   New delegate_case(Sub()
                                         run_case("delegate with parameter",
                                                  Sub()
                                                      Dim v As void(Of Int64) = Nothing
                                                      v = AddressOf increment
                                                      Dim j As Int64 = 0
                                                      For i As Int64 = 0 To size - 1
                                                          v(j)
                                                      Next
                                                  End Sub)
                                     End Sub))
    End Sub

    Protected Overrides Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        If isdebugbuild() Then
            Return loosen_bound({4439, 5702, 21404, 22504, 4826, 5562}, i, j)
        Else
            Return loosen_bound({874, 7663, 10563, 8461, 1705, 7665}, i, j)
        End If
    End Function

    Private Shared Sub increment(ByRef i As Int64)
        i += 1
    End Sub

    Private Shared Sub increment()
        Dim i As Int32 = 0
        i += 1
    End Sub

    Private Shared Sub void2(ByVal d As Action)
        Try
            d()
        Catch ex As Exception
            log_unhandled_exception(ex)
        End Try
    End Sub

    Private Shared Sub run_case(ByVal name As String, ByVal v As Action)
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
End Class
