
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.interpreter.primitive

Partial Public NotInheritable Class bstyle
    Private NotInheritable Class bool
        Inherits raw_value

        Public Const type_name As String = "Boolean"

        Public Sub New()
            MyBase.New(type_name)
        End Sub

        Protected Overrides Function parse(ByVal n As typed_node, ByRef o As data_block) As Boolean
            Dim i As Boolean = False
            If Not str_bool(n.word().str(), i) Then
                Return False
            End If
            o = New data_block(i)
            Return True
        End Function
    End Class
End Class
