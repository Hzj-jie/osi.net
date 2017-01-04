
Imports osi.root.utt

Public Class ctype_bool_test
    Inherits [case]

    Private Class test_class
        Private ReadOnly v As Int32

        Public Sub New(ByVal v As Int32)
            Me.v = v
        End Sub

        Public Shared Widening Operator CType(ByVal this As test_class) As Boolean
            If this Is Nothing Then
                Return False
            Else
                Return (this.v And 1) = 1
            End If
        End Operator
    End Class

    Public Overrides Function run() As Boolean
        Dim i As test_class = Nothing
        i = New test_class(1)
        assert_true(i)
        i = New test_class(2)
        assert_false(i)
        i = Nothing
        assert_false(i)
        Return True
    End Function
End Class
