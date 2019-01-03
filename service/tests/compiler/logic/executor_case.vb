
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.compiler.logic
Imports osi.service.interpreter.primitive
Imports exportable = osi.service.compiler.logic.exportable

Namespace logic
    Public Class executor_case
        Inherits [case]

        Private ReadOnly es() As exportable

        Protected Sub New(ByVal ParamArray es() As exportable)
            assert(Not isemptyarray(es))
            Me.es = es
        End Sub

        Protected Overridable Sub check_result(ByVal e As not_null(Of simulator))
        End Sub

        Protected Overridable Function interrupts() As interrupts
            Return Nothing
        End Function

        Public Overrides Function run() As Boolean
            Dim e As simulator = Nothing
            Dim ext As interrupts = Nothing
            ext = interrupts()
            If ext Is Nothing Then
                e = New simulator()
            Else
                e = New simulator(ext)
            End If
            If assertion.is_true(e.import(es)) Then
                e.execute()
                assertion.is_false(e.halt())
                assertion.is_true(e.errors().empty())
                Try
                    check_result(not_null.[New](e))
                Catch ex As executor_stop_error
                    assertion.is_true(False, ex)
                End Try
                Return True
            Else
                Return False
            End If
        End Function
    End Class
End Namespace
