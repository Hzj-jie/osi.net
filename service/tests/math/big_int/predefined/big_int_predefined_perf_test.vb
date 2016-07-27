
Imports osi.root.utt
Imports osi.root.constants
Imports osi.root.connector
Imports osi.service.math

Public Class big_int_predefined_perf_test
    Inherits commandline_specific_case_wrapper

    Public Sub New()
        MyBase.New(performance(New big_int_predefined_calculate()))
    End Sub

    Private Class big_int_predefined_calculate
        Inherits [case]

        Private Const divide_by_zero As String = "divide_by_zero"

        Private Sub invalid_input()
            Console.WriteLine("invalid input")
        End Sub

        Private Sub run_case(ByVal r As String)
            Dim s() As String = Nothing
            s = r.Split(character.blank)
            If array_size(s) <> 3 Then
                invalid_input()
            Else
                Dim left As big_int = Nothing
                Dim right As big_int = Nothing
                If Not big_int.parse(s(0), left) Then
                    invalid_input()
                    Return
                End If
                If Not big_int.parse(s(2), right) Then
                    invalid_input()
                    Return
                End If

                Select Case s(1)
                    Case "+"
                        Console.WriteLine((left + right).str())
                    Case "-"
                        Console.WriteLine((left - right).str())
                    Case "*"
                        Console.WriteLine((left * right).str())
                    Case "/"
                        Try
                            Console.WriteLine((left \ right).str())
                        Catch ex As DivideByZeroException
                            Console.WriteLine(divide_by_zero)
                        End Try
                    Case "%"
                        Try
                            Console.WriteLine((left Mod right).str())
                        Catch ex As DivideByZeroException
                            Console.WriteLine(divide_by_zero)
                        End Try
                    Case "^"
                        Console.WriteLine((left ^ right).str())
                    Case Else
                        invalid_input()
                End Select
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
