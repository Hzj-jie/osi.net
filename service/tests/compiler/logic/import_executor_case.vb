
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.interpreter.primitive
Imports osi.service.compiler.logic
Imports exportable = osi.service.compiler.logic.exportable

Namespace logic
    Public Class import_executor_case
        Inherits executor_case

        Private Shared Function build_case(ByVal str As String,
                                           Optional ByVal functions As interrupts = Nothing) As exportable()
            Dim o As writer = Nothing
            o = New writer()
            o.append(str)
            Dim es As vector(Of exportable) = Nothing
            es = New vector(Of exportable)()
            If functions Is Nothing Then
                assertion.is_true(o.dump(es))
            Else
                assertion.is_true(o.dump(functions, es))
            End If
            Return +es
        End Function

        Protected Sub New(ByVal str As String, ByVal functions As interrupts)
            MyBase.New(build_case(str, functions))
        End Sub

        Protected Sub New(ByVal str As String)
            MyBase.New(build_case(str))
        End Sub
    End Class
End Namespace
