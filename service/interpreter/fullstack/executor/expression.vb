
Imports osi.root.connector
Imports osi.root.utils

Namespace fullstack.executor
    Public Class expression
        Private ReadOnly f As [function]
        Private ReadOnly inputs() As expression
        Private ReadOnly location As location

        Public Sub New(ByVal f As [function],
                       ByVal inputs() As expression)
            assert(Not f Is Nothing)
            assert(f.has_return())
            assert(array_size(inputs) = f.input_type_count())
            Me.f = f
            Me.inputs = inputs
        End Sub

        Public Sub New(ByVal location As location)
            assert(Not location Is Nothing)
            Me.location = location
        End Sub

        Public Function execute(ByVal domain As domain) As variable
            If Not f Is Nothing Then
                Dim vs() As variable = Nothing
                ReDim vs(array_size(inputs) - 1)
                For i As Int32 = 0 To array_size(inputs) - 1
                    vs(i) = inputs(i).execute(domain)
                Next
                Dim d As domain = Nothing
                Using domain.create_disposer(d)
                    For i As Int32 = 0 To array_size(inputs) - 1
                        d.define(vs(i))
                    Next
                    f.execute(d)
                    Return d.variable(0, array_size(inputs))
                End Using
            ElseIf Not location Is Nothing Then
                Return location(domain)
            Else
                assert(False)
                Return Nothing
            End If
        End Function
    End Class
End Namespace
