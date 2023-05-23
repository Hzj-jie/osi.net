
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.interpreter.primitive
Imports osi.service.math

Partial Public NotInheritable Class bstyle
    Private NotInheritable Class biguint
        Inherits raw_value

        Public Const type_name As String = "BigUnsignedInteger"

        Public Sub New()
            MyBase.New(type_name)
        End Sub

        Protected Overrides Function parse(ByVal n As typed_node, ByRef o As data_block) As Boolean
            Dim s As String = n.word().str()
            s = s.Substring(0, s.Length() - 1)
            Dim i As big_uint = Nothing
            If Not big_uint.parse(s, i) Then
                Return False
            End If
            assert(Not i Is Nothing)
            o = New data_block(i.as_bytes())
            Return True
        End Function
    End Class
End Class
