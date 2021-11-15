
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utt
Imports osi.service.interpreter.primitive
Imports osi.service.resource

Namespace primitive
    Public NotInheritable Class states_test
        Inherits chained_case_wrapper

        Public Sub New()
            MyBase.New(New throw_exception(), New normal())
        End Sub

        Private NotInheritable Class throw_exception
            Inherits [case]

            Public Overrides Function run() As Boolean
                Dim s As simulator = Nothing
                s = New simulator()
                assertion.is_true(s.import(sim6.as_text()))
                s.execute()
                assertion.is_true(s.halt())
                If assertion.equal(s.errors().size(), uint32_1) Then
                    assertion.equal(s.errors()(uint32_0), executor.error_type.stack_access_out_of_boundary)
                End If
                Return True
            End Function
        End Class

        Private NotInheritable Class normal
            Inherits [case]

            Public Overrides Function run() As Boolean
                Dim s As simulator = Nothing
                s = New simulator()
                assertion.is_true(s.import(sim7.as_text()))
                s.execute()
                assertion.equal(s.stack_size(), uint32_1)
                assertion.equal(s.states_size(), uint32_0)
                assertion.array_equal(+s.access(data_ref.abs(0)),
                                      str_bytes(strcat("hello world", character.newline)))
                Return True
            End Function
        End Class
    End Class
End Namespace
