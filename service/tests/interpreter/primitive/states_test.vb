
Option Strict On

Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt
Imports osi.service.interpreter.primitive
Imports osi.service.resource

Namespace primitive
    Public Class states_test
        Inherits chained_case_wrapper

        Public Sub New()
            MyBase.New(New throw_exception(), New normal())
        End Sub

        Private Class throw_exception
            Inherits [case]

            Public Overrides Function run() As Boolean
                Dim s As simulator = Nothing
                s = New simulator()
                assert_true(s.import(sim6.as_text()))
                s.execute()
                assert_true(s.halt())
                If assert_equal(s.errors().size(), uint32_1) Then
                    assert_equal(s.errors()(uint32_0), executor.error_type.stack_access_out_of_boundary)
                End If
                Return True
            End Function
        End Class

        Private Class normal
            Inherits [case]

            Public Overrides Function run() As Boolean
                Dim s As simulator = Nothing
                s = New simulator()
                assert_true(s.import(sim7.as_text()))
                s.execute()
                assert_equal(s.stack_size(), uint32_1)
                assert_equal(s.states_size(), uint32_0)
                assert_array_equal(+s.access_stack(data_ref.abs(0)),
                                   str_bytes(strcat("hello world", character.newline)))
                Return True
            End Function
        End Class
    End Class
End Namespace
