
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
            Dim es As vector(Of exportable) = Nothing
            es = New vector(Of exportable)()
            If functions Is Nothing Then
                assertion.is_true(importer.[New]().import(str, es))
            Else
                assertion.is_true(importer.[New](functions).import(str, es))
            End If
            If Not assertion.is_false(es.empty()) Then
                es.emplace_back(New [stop]())
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
