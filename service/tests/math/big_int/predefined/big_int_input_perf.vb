
Imports osi.root.utt
Imports osi.root.constants
Imports osi.root.connector
Imports osi.service.math

Public Class big_int_input_perf
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(New big_int_input_case())
    End Sub

    Private Class big_int_input_case
        Inherits [case]

        Private Sub invalid_input()
            Console.WriteLine("invalid input")
        End Sub

        Private Sub run_case(ByVal r As String)
            If String.IsNullOrEmpty(r) Then
                invalid_input()
            Else
                Dim v As big_int = Nothing
                If Not big_int.parse(r, v) Then
                    invalid_input()
                End If
            End If
        End Sub

        Public Overrides Function run() As Boolean
            Using New boost()
                Dim start As Date = Nothing
                start = Date.Now()
                Console.WriteLine(start)
                Dim r As String = Nothing
                r = Console.ReadLine()
                While Not r Is Nothing
                    Try
                        run_case(r)
                    Catch ex As Exception
                        Console.WriteLine("Exception: " + ex.Message() + " @ " + ex.StackTrace() + " case: " + r)
                    End Try
                    r = Console.ReadLine()
                End While
                Dim [end] As Date = Nothing
                [end] = Date.Now()
                Console.WriteLine([end])
                Console.WriteLine("total ticks " + Convert.ToString([end].Ticks() - start.Ticks()))
                Return True
            End Using
        End Function
    End Class
End Class
