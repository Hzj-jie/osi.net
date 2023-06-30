
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt
Imports osi.service.math

Public Class big_int_predefined_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(New big_int_predefined_case())
    End Sub

    Private Class big_int_predefined_case
        Inherits [case]

        Private Const divide_by_zero As String = "divide_by_zero"
        Private failure As UInt32

        Private Sub invalid_input()
            Console.WriteLine("invalid input")
            failure += 1
        End Sub

        Private Sub verify(ByVal left As String, ByVal right As String, ByVal r As String)
            If left <> right Then
                Console.WriteLine("failure @ " + r + ", left " + left + ", right " + right)
                failure += 1
            End If
        End Sub

        Private Sub verify(ByVal left As big_int,
                           ByVal right As big_int,
                           ByVal rs As String,
                           ByVal r As String)
            If left <> right Then
                Console.WriteLine("failure @ " +
                                  r +
                                  ", left " +
                                  left.str() +
                                  ", right " +
                                  right.str())
                failure += 1
            End If
            If left.str() <> rs Then
                Console.WriteLine("failure @ " +
                                  r +
                                  " left as string, left " +
                                  left.str() +
                                  ", right " +
                                  right.str())
                failure += 1
            End If
            If right.str() <> rs Then
                Console.WriteLine("failure @ " +
                                  r +
                                  " right as string, left " +
                                  left.str() +
                                  ", right " +
                                  right.str())
                failure += 1
            End If
        End Sub

        Private Sub run_case(ByVal r As String)
            Dim s() As String = Nothing
            s = r.Split(character.blank)
            If array_size(s) <> 5 OrElse s(3) <> "=" Then
                invalid_input()
            Else
                Dim left As big_int = Nothing
                Dim right As big_int = Nothing
                Dim result As big_int = Nothing
                If Not big_int.parse(s(0), left) Then
                    invalid_input()
                    Return
                End If
                If Not big_int.parse(s(2), right) Then
                    invalid_input()
                    Return
                End If
                If s(4) <> divide_by_zero Then
                    If Not big_int.parse(s(4), result) Then
                        invalid_input()
                        Return
                    End If
                End If

                Select Case s(1)
                    Case "+"
                        verify(left + right, result, s(4), r)
                    Case "-"
                        verify(left - right, result, s(4), r)
                    Case "*"
                        verify(left * right, result, s(4), r)
                    Case "/"
                        If right = 0 Then
                            Try
                                left \= right
                                verify(divide_by_zero + "failed", s(4), r)
                            Catch ex As Exception
                                verify(divide_by_zero, s(4), r)
                            End Try
                        Else
                            verify(left \ right, result, s(4), r)
                        End If
                    Case "%"
                        If right = 0 Then
                            Try
                                left = left Mod right
                                verify(divide_by_zero + "failed", s(4), r)
                            Catch ex As Exception
                                verify(divide_by_zero, s(4), r)
                            End Try
                        Else
                            verify(left Mod right, result, s(4), r)
                        End If
                    Case "^"
                        verify(left ^ right, result, s(4), r)
                    Case Else
                        invalid_input()
                End Select
            End If
        End Sub

        Public Overrides Function run() As Boolean
            Using New boost()
                failure = 0
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
                        failure += 1
                    End Try
                    r = Console.ReadLine()
                End While
                Dim [end] As Date = Nothing
                [end] = Date.Now()
                Console.WriteLine([end])
                Console.WriteLine("failure " + Convert.ToString(failure))
                Console.WriteLine("total ticks " + Convert.ToString([end].Ticks() - start.Ticks()))
                Return True
            End Using
        End Function
    End Class
End Class
