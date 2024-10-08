
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.compiler
Imports osi.service.compiler.logic
Imports osi.service.interpreter.primitive

Namespace logic
    Public Class executor_case
        Inherits [case]

        Private ReadOnly es As Func(Of instruction_gen())

        Protected Sub New(ByVal ParamArray es() As instruction_gen)
            assert(Not es.null_or_empty())
            Me.es = Function() As instruction_gen()
                        Return es
                    End Function
        End Sub

        Protected Sub New(ByVal es As Func(Of instruction_gen()))
            assert(Not es Is Nothing)
            Me.es = es
        End Sub

        Protected Overridable Sub check_result(ByVal e As not_null(Of simulator))
        End Sub

        Protected Overridable Function interrupts() As interrupts
            Return Nothing
        End Function

        Public Overrides Function run() As Boolean
            Dim e As simulator = Nothing
            Dim ext As interrupts = interrupts()
            If ext Is Nothing Then
                e = New simulator()
            Else
                e = New simulator(ext)
            End If
            If Not assertion.is_true(e.import(es())) Then
                Return False
            End If
            e.execute()
            assertion.is_false(e.halt(), lazier.of(AddressOf e.halt_error))
            Try
                check_result(not_null.[New](e))
            Catch ex As executor_stop_error
                assertion.is_true(False, ex)
            End Try
            Return True
        End Function
    End Class
End Namespace
