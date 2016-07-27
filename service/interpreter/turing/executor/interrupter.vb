
Imports osi.root.formation
Imports osi.root.connector

Namespace turing.executor
    Public Class interrupter
        Private ReadOnly exec As Func(Of variable(), variable)

        Public Sub New()
        End Sub

        Public Sub New(ByVal execute As Func(Of variable(), variable))
            assert(Not execute Is Nothing)
            Me.exec = execute
        End Sub

        Protected Overridable Function execute(ByVal inputs() As variable) As variable
            assert(Not exec Is Nothing)
            Return exec(inputs)
        End Function

        Public Function execute(ByVal space As vector(Of variable)) As Boolean
            assert(Not space Is Nothing)
            Dim var As variable = Nothing
            var = execute(+space)
            space.clear()
            If Not var Is Nothing Then
                space.emplace_back(var)
            End If
            Return True
        End Function
    End Class
End Namespace
