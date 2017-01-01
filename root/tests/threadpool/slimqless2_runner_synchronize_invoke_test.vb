
Imports osi.root.connector
Imports osi.root.threadpool
Imports osi.root.utt

Public Class slimqless2_runner_synchronize_invoke_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        Dim t As slimqless2_runner = Nothing
        t = New slimqless2_runner()
        Dim s As slimqless2_runner_synchronize_invoke = Nothing
        s = New slimqless2_runner_synchronize_invoke(t)
        For i As Int32 = 0 To 100
            Dim result As Object = Nothing
            Dim args() As Object = Nothing
            result = New Object()
            ReDim args(2)
            For j As Int32 = 0 To array_size(args) - 1
                args(j) = New Object()
            Next
            Dim exec As Func(Of Object, Object, Object, Object) = Nothing
            exec = Function(ByVal a As Object, ByVal b As Object, ByVal c As Object) As Object
                       assert_reference_equal(a, args(0))
                       assert_reference_equal(b, args(1))
                       assert_reference_equal(c, args(2))
                       Return result
                   End Function
            assert_reference_equal(s.Invoke(exec, args), result)
            Dim ar As IAsyncResult = Nothing
            ar = s.BeginInvoke(exec, args)
            If assert_not_nothing(ar) Then
                assert_reference_equal(s.EndInvoke(ar), result)
            End If
        Next
        assert(t.stop())
        Return True
    End Function
End Class
