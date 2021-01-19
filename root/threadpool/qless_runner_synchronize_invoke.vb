
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with qless_runner_synchronize_invoke.vbp ----------
'so change qless_runner_synchronize_invoke.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with slimqless2_runner_synchronize_invoke.vbp ----------
'so change slimqless2_runner_synchronize_invoke.vbp instead of this file


Imports osi.root.connector

Public NotInheritable Class qless_runner_synchronize_invoke
    Inherits synchronize_invoke

    Private ReadOnly r As qless_runner

    Public Sub New(ByVal r As qless_runner)
        assert(Not r Is Nothing)
        Me.r = r
    End Sub

    Protected Overrides Sub push(ByVal v As Action)
        assert(r.push(v))
    End Sub

    Protected Overrides Function synchronously() As Boolean
        Return qless_runner.current_thread_is_managed()
    End Function
End Class
'finish slimqless2_runner_synchronize_invoke.vbp --------
'finish qless_runner_synchronize_invoke.vbp --------
