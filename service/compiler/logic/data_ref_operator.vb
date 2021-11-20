
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    Public MustInherit Class data_ref_operator
        Implements exportable

        Private ReadOnly types As types
        Private ReadOnly vs() As String

        Public Sub New(ByVal types As types, ByVal ParamArray vs() As String)
            assert(Not types Is Nothing)
            assert(Not isemptyarray(vs))
            Me.types = types
            Me.vs = vs
        End Sub

        Protected MustOverride Function variable_restrict(ByVal i As UInt32, ByVal v As variable) As Boolean
        Protected MustOverride Function instruction() As command

        Public Function export(ByVal o As vector(Of String)) As Boolean Implements exportable.export
            Dim vars() As variable = Nothing
            ReDim vars(array_size_i(vs) - 1)
            For i As Int32 = 0 To array_size_i(vs) - 1
                If Not variable.of(types, vs(i), vars(i)) OrElse
                   Not variable_restrict(CUInt(i), vars(i)) Then
                    Return False
                End If
            Next

            o.emplace_back(instruction_builder.str(instruction(), vars))
            Return True
        End Function
    End Class
End Namespace
