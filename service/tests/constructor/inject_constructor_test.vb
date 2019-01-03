
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.argument
Imports osi.service.constructor

<test>
Public NotInheritable Class inject_constructor_test
    Private NotInheritable Class test_class
        <inject_constructor>
        Public Sub New(ByVal i As Int32, ByVal s As String, ByVal b As Boolean)
            assertion.equal(Convert.ToString(i), s)
            assertion.equal(b, int_bool(i))
        End Sub

        Public Shared Function int_bool(ByVal i As Int32) As Boolean
            Return (i And 1) = 1
        End Function
    End Class

    <test>
    Private Shared Sub invoke_object_case()
        For x As Int32 = 0 To 100
            Dim o As test_class = Nothing
            Dim i As Int32 = 0
            i = rnd_int()
            assertion.is_true(inject_constructor.invoke(o, i, Convert.ToString(i), test_class.int_bool(i)))
        Next
    End Sub

    <test>
    Private Shared Sub invoke_string_case()
        For x As Int32 = 0 To 100
            Dim o As test_class = Nothing
            Dim i As Int32 = 0
            i = rnd_int()
            assertion.is_true(inject_constructor.invoke(o,
                                                  Convert.ToString(i),
                                                  string_serializer.to_str(i),
                                                  string_serializer.to_str(test_class.int_bool(i))))
        Next
    End Sub

    <test>
    Private Shared Sub invoke_var_case()
        For x As Int32 = 0 To 100
            Dim o As test_class = Nothing
            Dim i As Int32 = 0
            i = rnd_int()
            assertion.is_true(inject_constructor.invoke(New var({"--i=" + Convert.ToString(i),
                                                           "--s=" + string_serializer.to_str(i),
                                                           "--b=" + string_serializer.to_str(test_class.int_bool(i))}),
                                                  o))
        Next
    End Sub

    Private Sub New()
    End Sub
End Class
