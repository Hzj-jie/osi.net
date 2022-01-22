
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.interpreter.primitive
Imports osi.service.compiler.logic

Namespace logic
    Public Class import_executor_case
        Inherits executor_case

        Private ReadOnly functions As interrupts

        Private Shared Function build_case(ByVal str As String,
                                           Optional ByVal functions As interrupts = Nothing) As instruction_gen()
            Dim es As New vector(Of instruction_gen)()
            If functions Is Nothing Then
                assertion.is_true(New importer().import(str, es))
            Else
                assertion.is_true(New importer(functions).import(str, es))
            End If
            Return +es
        End Function

        Private Sub New(ByVal es As Func(Of instruction_gen()), ByVal functions As interrupts)
            MyBase.New(es)
            Me.functions = functions
        End Sub

        Protected Sub New(ByVal str As String, ByVal functions As interrupts)
            Me.New(Function() As instruction_gen()
                       Return build_case(str, functions)
                   End Function,
                   functions)
        End Sub

        Protected Sub New(ByVal str As String)
            Me.New(Function() As instruction_gen()
                       Return build_case(str)
                   End Function,
                   Nothing)
        End Sub

        Protected NotOverridable Overrides Function interrupts() As interrupts
            Return functions
        End Function
    End Class
End Namespace
