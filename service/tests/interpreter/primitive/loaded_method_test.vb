
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.interpreter.primitive

Namespace primitive
    Public Module _loaded_method_test
        Public Function action(ByVal i() As Byte) As Byte()
            Return i
        End Function
    End Module

    <test>
    Public NotInheritable Class loaded_method_test
        <test>
        Private Shared Sub in_module()
            Dim l As loaded_method = Nothing
            l = New loaded_method()
            l.load(str_bytes(strcat("osi.tests.service.interpreter.primitive._loaded_method_test, ",
                                    "osi.tests.service.interpreter",
                                    ":action")))
            assertion.array_equal(l.execute(str_bytes("abc")), str_bytes("abc"))
        End Sub

        <test>
        Private Shared Sub exception_when_executing_before_load()
            Dim l As loaded_method = Nothing
            l = New loaded_method()
            assertion.thrown(Sub()
                                 l.execute(str_bytes("abc"))
                             End Sub)
        End Sub

        Private Shared Function another_action(ByVal i() As Byte) As Byte()
            Return i
        End Function

        <test>
        Private Shared Sub in_static_class()
            Dim l As loaded_method = Nothing
            l = New loaded_method()
            l.load(str_bytes(strcat("osi.tests.service.interpreter.primitive.loaded_method_test, ",
                                    "osi.tests.service.interpreter",
                                    ":another_action")))
            assertion.array_equal(l.execute(str_bytes("abc")), str_bytes("abc"))
        End Sub

        Private Function instance_action(ByVal i() As Byte) As Byte()
            Return i
        End Function

        <test>
        Private Shared Sub disallow_instance_method()
            Dim l As loaded_method = Nothing
            l = New loaded_method()
            assertion.thrown(Sub()
                                 l.load(str_bytes(strcat("osi.tests.service.interpreter.primitive.loaded_method_test, ",
                                                         "osi.tests.service.interpreter",
                                                         ":instance_action")))
                             End Sub)
        End Sub

        Private Shared Function thrown_action(ByVal i() As Byte) As Byte()
            Throw New Exception()
            Return i
        End Function

        <test>
        Private Shared Sub catch_exception()
            Dim l As loaded_method = Nothing
            l = New loaded_method()
            l.load(str_bytes(strcat("osi.tests.service.interpreter.primitive.loaded_method_test, ",
                                    "osi.tests.service.interpreter",
                                    ":thrown_action")))
            assertion.equal(assertion.catch_thrown(Of executor_stop_error)(Sub()
                                                                               l.execute(str_bytes("abc"))
                                                                           End Sub).error_type,
                            executor.error_type.interrupt_failure)
        End Sub

        Private Shared Sub unsatisfied_signature_1(ByVal i() As Byte)
        End Sub

        Private Shared Function unsatisfied_signature_2(ByVal i As Byte) As Byte()
            Return Nothing
        End Function

        Private Shared Function unsatisfied_signature_3(ByVal i() As Byte) As String
            Return Nothing
        End Function

        <test>
        Private Shared Sub disallow_unsatisfied_signature()
            Dim l As loaded_method = Nothing
            l = New loaded_method()
            assertion.thrown(Sub()
                                 l.load(str_bytes(strcat("osi.tests.service.interpreter.primitive.loaded_method_test, ",
                                                         "osi.tests.service.interpreter",
                                                         ":unsatisfied_signature_1")))
                             End Sub)
            assertion.thrown(Sub()
                                 l.load(str_bytes(strcat("osi.tests.service.interpreter.primitive.loaded_method_test, ",
                                                         "osi.tests.service.interpreter",
                                                         ":unsatisfied_signature_2")))
                             End Sub)
            assertion.thrown(Sub()
                                 l.load(str_bytes(strcat("osi.tests.service.interpreter.primitive.loaded_method_test, ",
                                                         "osi.tests.service.interpreter",
                                                         ":unsatisfied_signature_3")))
                             End Sub)
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
