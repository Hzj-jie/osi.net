
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

        Private ReadOnly functions As interrupts

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

        Private Sub New(ByVal es() As exportable, ByVal functions As interrupts)
            MyBase.New(es)
            Me.functions = functions
        End Sub

        Protected Sub New(ByVal str As String, ByVal functions As interrupts)
            Me.New(build_case(str, functions), functions)
        End Sub

        Protected Sub New(ByVal str As String)
            Me.New(build_case(str), Nothing)
        End Sub

        Protected NotOverridable Overrides Function interrupts() As interrupts
            Return functions
        End Function
    End Class
End Namespace
