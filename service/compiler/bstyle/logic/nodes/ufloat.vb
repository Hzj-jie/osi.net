
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.service.automata
Imports osi.service.interpreter.primitive
Imports osi.service.math

Partial Public NotInheritable Class bstyle
    Private NotInheritable Class ufloat
        Inherits raw_value

        Public Const type_name As String = "BigUnsignedFloat"

        Public Sub New()
            MyBase.New(type_name)
        End Sub

        Protected Overrides Function parse(ByVal n As typed_node, ByRef o As data_block) As Boolean
            Dim i As big_udec = Nothing
            If Not big_udec.parse(n.word().str(), i) Then
                Return False
            End If
            o = New data_block(i.as_bytes())
            Return True
        End Function
    End Class
End Class
