
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utils
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class weak_ref_delegate_test
    <test>
    Private Shared Sub action_bind()
        Dim r As Boolean = False
        Dim o As Object = Nothing
        o = New Object()
        weak_ref_delegate.bind(o, Sub(ByVal i As Object)
                                      assert_reference_equal(i, o)
                                      r = True
                                  End Sub)()
        assert_true(r)
    End Sub

    <test>
    Private Shared Sub func_bind()
        Dim r As Boolean = False
        Dim o As Object = Nothing
        o = New Object()
        Dim j As Object = Nothing
        j = weak_ref_delegate.bind(o, Function(ByVal i As Object) As Object
                                          assert_reference_equal(i, o)
                                          r = True
                                          Return i
                                      End Function)()
        assert_true(r)
        assert_reference_equal(o, j)
    End Sub

    Private Sub New()
    End Sub
End Class
