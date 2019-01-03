
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.interpreter.fullstack.executor

Namespace fullstack.executor
    Public Class rnd_function_test
        Inherits [case]

        Private Shared Function assert_return(ByVal d As domain,
                                              ByVal exp_inc As Int32,
                                              ByVal min As Int32,
                                              ByVal max As Int32) As Boolean
            assert(Not d Is Nothing)
            If assertion.equal(d.increment(), exp_inc) Then
                assertion.is_true(d.last().is_int())
                assertion.more_and_less(d.last().int(), min, max)
            End If
            Return True
        End Function

        Private Shared Function try_exec(ByVal f As rnd_int_function,
                                         ByVal d As domain,
                                         ByVal exp_inc As Int32,
                                         ByVal min As Int32,
                                         ByVal max As Int32) As Boolean
            Try
                f.execute(d)
            Catch ex As invalid_runtime_casting_exception
                assertion.is_true(False)
            End Try
            If assert_return(d, exp_inc, min, max) Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Overrides Function run() As Boolean
            Dim f As rnd_int_function = Nothing
            f = New rnd_int_function(0)
            Dim d As domain = Nothing
            d = New domain(New variables())
            If Not try_exec(f, d, 1, min_int32, max_int32) Then
                Return False
            End If

            d.over()
            f = New rnd_int_function(1)
            Dim min As Int32 = 0
            min = rnd_int()
            d.define(New variable(min))
            If Not try_exec(f, d, 2, min, max_int32) Then
                Return False
            End If

            d.over()
            f = New rnd_int_function(2)
            Dim max As Int32 = 0
            min = rnd_int(min_int32, max_int32 - 2)
            max = rnd_int(min + 1, max_int32)
            d.define(New variable(min))
            d.define(New variable(max))
            If Not try_exec(f, d, 3, min, max) Then
                Return False
            End If

            Return True
        End Function
    End Class
End Namespace
