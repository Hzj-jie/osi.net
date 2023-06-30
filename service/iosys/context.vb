
Imports osi.root.connector

Public Class context(Of INPUT_CASE As Class,
                        OUTPUT_CASE As Class,
                        INPUT_T As iosys(Of INPUT_CASE),
                        OUTPUT_T As iosys(Of OUTPUT_CASE))
    Public ReadOnly input As INPUT_T
    Public ReadOnly output As OUTPUT_T

    Public Sub New(ByVal input As INPUT_T, ByVal output As OUTPUT_T)
        assert(Not input Is Nothing)
        assert(Not output Is Nothing)
        Me.input = input
        Me.output = output
    End Sub
End Class
