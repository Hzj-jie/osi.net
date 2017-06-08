
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.utt
Imports osi.service.interpreter.primitive

Namespace primitive
    Public Class interrupts_test
        Inherits [case]

        Public Overrides Function run() As Boolean
            Dim f As interrupts = Nothing
            f = New interrupts()
            Dim p As UInt32 = 0
            assert_true(f.find_interrupt("stdout", p))
            assert_equal(p, uint32_0)
            assert_true(f.find_interrupt("stderr", p))
            assert_equal(p, uint32_1)
            assert_true(f.find_interrupt("stdin", p))
            assert_equal(p, uint32_2)
            Return True
        End Function
    End Class
End Namespace
