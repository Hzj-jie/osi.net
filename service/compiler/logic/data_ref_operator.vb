
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Partial Public NotInheritable Class logic
    Public MustInherit Class data_ref_operator
        Implements instruction_gen

        Private ReadOnly vs() As String

        Public Sub New(ByVal ParamArray vs() As String)
            assert(Not isemptyarray(vs))
            Me.vs = vs
        End Sub

        Protected MustOverride Function variable_restrict(ByVal i As UInt32, ByVal v As variable) As Boolean
        Protected MustOverride Function instruction() As command

        Public Function build(ByVal o As vector(Of String)) As Boolean Implements instruction_gen.build
            Dim vars(array_size_i(vs) - 1) As variable
            For i As Int32 = 0 To array_size_i(vs) - 1
                If Not variable.of(vs(i), o, vars(i)) OrElse
                   Not variable_restrict(CUInt(i), vars(i)) Then
                    Return False
                End If
            Next

            o.emplace_back(instruction_builder.str(instruction(), vars))
            Return True
        End Function
    End Class
End Class
