
Imports osi.root.connector
Imports osi.root.utils

Namespace turing.executor
    Public Class add
        Inherits interrupter

        Public Shared ReadOnly instance As add

        Shared Sub New()
            instance = New add()
        End Sub

        Private Sub New()
        End Sub

        Protected Overrides Function execute(ByVal inputs() As variable) As variable
            assert(array_size(inputs) >= 2)
            Dim r As Double = 0
            For i As UInt32 = 0 To CUInt(array_size(inputs) - 1)
                r += inputs(i).number()
            Next
            If r.is_int() Then
                Return New variable(CInt(r))
            ElseIf r.is_not_int() Then
                Return New variable(r)
            Else
                assert(False)
                Return Nothing
            End If
        End Function
    End Class

    Public Class [sub]
        Inherits interrupter

        Public Shared ReadOnly instance As [sub]

        Shared Sub New()
            instance = New [sub]()
        End Sub

        Private Sub New()
        End Sub

        Protected Overrides Function execute(ByVal inputs() As variable) As variable
            assert(array_size(inputs) >= 2)
            Dim r As Double = 0
            r = inputs(0).number()
            For i As UInt32 = 1 To CUInt(array_size(inputs) - 1)
                r -= inputs(i).number()
            Next
            If r.is_int() Then
                Return New variable(CInt(r))
            ElseIf r.is_not_int() Then
                Return New variable(r)
            Else
                assert(False)
                Return Nothing
            End If
        End Function
    End Class
End Namespace
