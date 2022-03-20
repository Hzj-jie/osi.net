
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Partial Public NotInheritable Class logic
    ' VisibleForTesting
    Public NotInheritable Class _if
        Implements instruction_gen

        Private ReadOnly v As String
        Private ReadOnly true_path As paragraph
        Private ReadOnly false_path As paragraph

        Public Sub New(ByVal v As String,
                       ByVal true_path As paragraph,
                       ByVal [else] As importer.place_holder,
                       ByVal false_path As paragraph)
            Me.New(v, true_path, false_path)
        End Sub

        Public Sub New(ByVal v As String, ByVal true_path As paragraph)
            Me.New(v, true_path, Nothing, Nothing)
        End Sub

        ' @VisibleForTesting
        Public Sub New(ByVal v As String, ByVal true_path As paragraph, ByVal false_path As paragraph)
            assert(Not String.IsNullOrEmpty(v))
            assert(Not true_path Is Nothing)
            Me.v = v
            Me.true_path = true_path
            Me.false_path = false_path
        End Sub

        Public Function build(ByVal o As vector(Of String)) As Boolean Implements instruction_gen.build
            assert(Not o Is Nothing)
            Dim var As variable = Nothing
            If Not variable.of(v, o, var) Then
                Return False
            End If

            Dim true_o As New vector(Of String)()
            If Not true_path.build(true_o) Then
                Return False
            End If

            Dim false_o As New vector(Of String)()
            If Not false_path Is Nothing AndAlso Not false_path.build(false_o) Then
                Return False
            End If
            false_o.emplace_back(instruction_builder.str(command.jump, data_ref.rel(true_o.size() + uint32_1)))

            o.emplace_back(instruction_builder.str(command.jumpif, data_ref.rel(false_o.size() + uint32_1), var))
            assert(o.emplace_back(false_o))
            assert(o.emplace_back(true_o))

            Return True
        End Function
    End Class
End Class
