
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class lambda_behavior_test
    Private Sub New()
    End Sub

    <test>
    Private Shared Sub should_capture_latest_value()
        Dim c As Object = Nothing
        Dim f As Action = Nothing
        f = Sub()
                c = New Object()
            End Sub
        Dim g As Action = Nothing
        g = Sub()
                assertion.is_not_null(c)
            End Sub

        f()
        g()
    End Sub

    <test>
    Private Shared Sub should_modify_value_type()
        Dim a As Int32 = 0
        Dim f As Action = Sub()
                              a = 100
                          End Sub
        Dim g As Action = Sub()
                              assertion.equal(a, 100)
                          End Sub
        f()
        g()
    End Sub

    <test>
    Private Shared Sub same_lambda_expression_should_create_different_objects_with_different_captured_parameters()
        Dim a As Int32 = 0
        Dim f1 As Func(Of Boolean) = Function() As Boolean
                                         Return a = 0
                                     End Function
        a = 100
        Dim f2 As Func(Of Boolean) = Function() As Boolean
                                         Return a = 100
                                     End Function
        assertion.is_false(f1())
        assertion.is_true(f2())
        assertion.not_reference_equal(f1, f2)
    End Sub

    <test>
    Private Shared Sub same_lambda_expression_should_create_different_objects_without_capturing()
        assertion.not_reference_equal(Function(ByVal x As Int32) As Boolean
                                       Return x = 0
                                   End Function,
                                   Function(ByVal x As Int32) As Boolean
                                       Return x = 0
                                   End Function)
    End Sub
End Class
