
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

        Public Overrides Function run() As Boolean
            Dim e As simulator = Nothing
            e = New simulator()
            If assert_true(e.import(es)) Then
                e.execute()
                Try
                    check_result(not_null.[New](e))
                Catch ex As executor_stop_error
                    assert_true(False, ex)
                End Try
                Return True
            Else
                Return False
            End If
        End Function
    End Class
End Namespace
