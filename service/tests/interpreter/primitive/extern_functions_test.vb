
Imports osi.root.constants
Imports osi.root.utt
Imports osi.service.interpreter.primitive

Namespace primitive
    Public Class extern_functions_test
        Inherits [case]

        Public Overrides Function run() As Boolean
            Dim f As extern_functions = Nothing
            f = New extern_functions()
            Dim p As UInt32 = 0
            assert_true(f.find_extern_function("stdout", p))
            assert_equal(p, uint32_0)
            assert_true(f.find_extern_function("stderr", p))
            assert_equal(p, uint32_1)
            assert_true(f.find_extern_function("stdin", p))
            assert_equal(p, uint32_2)
            Return True
        End Function
    End Class
End Namespace
