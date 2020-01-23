
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    ' do { do() } while (var)
    Public NotInheritable Class do_while
        Implements exportable

        Private ReadOnly v As String
        Private ReadOnly p As paragraph

        Public Sub New(ByVal v As String, ByVal p As unique_ptr(Of paragraph))
            assert(Not String.IsNullOrEmpty(v))
            assert(p)
            Me.v = v
            Me.p = p.release()
        End Sub

        Public Function export(ByVal scope As scope,
                               ByVal o As vector(Of String)) As Boolean Implements exportable.export
            assert(Not o Is Nothing)
            Dim start As UInt32 = 0
            start = o.size()
            If Not p.export(scope, o) Then
                Return False
            End If

            Dim var As variable = Nothing
            If Not variable.[New](scope, v, var) Then
                Return False
            End If

            o.emplace_back(instruction_builder.str(command.jumpif, data_ref.rel(CLng(start) - o.size()), var))

            Return True
        End Function
    End Class
End Namespace
