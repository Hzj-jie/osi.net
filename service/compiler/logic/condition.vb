
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    Public NotInheritable Class condition
        Implements exportable

        Private ReadOnly v As String
        Private ReadOnly true_path As paragraph
        Private ReadOnly false_path As paragraph

        Public Sub New(ByVal v As String,
                       ByVal true_path As unique_ptr(Of paragraph),
                       Optional ByVal false_path As unique_ptr(Of paragraph) = Nothing)
            assert(Not String.IsNullOrEmpty(v))
            assert(true_path)
            Me.v = v
            Me.true_path = true_path.release()
            Me.false_path = false_path.release_or_null()
        End Sub

        Public Function export(ByVal o As vector(Of String)) As Boolean Implements exportable.export
            assert(Not o Is Nothing)
            Dim var As variable = Nothing
            If Not variable.of_stack(v, var) Then
                Return False
            End If

            Dim true_o As vector(Of String) = Nothing
            true_o = New vector(Of String)()
            If Not true_path.export(true_o) Then
                Return False
            End If

            Dim false_o As vector(Of String) = Nothing
            false_o = New vector(Of String)()
            If Not false_path Is Nothing AndAlso Not false_path.export(false_o) Then
                Return False
            End If
            false_o.emplace_back(instruction_builder.str(command.jump, data_ref.rel(true_o.size() + uint32_1)))

            o.emplace_back(instruction_builder.str(command.jumpif, data_ref.rel(false_o.size() + uint32_1), var))
            assert(o.emplace_back(false_o))
            assert(o.emplace_back(true_o))

            Return True
        End Function
    End Class
End Namespace
