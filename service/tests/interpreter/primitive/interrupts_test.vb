
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
            assertion.is_true(f.of("stdout", p))
            assertion.equal(p, uint32_0)
            assertion.is_true(f.of("stderr", p))
            assertion.equal(p, uint32_1)
            assertion.is_true(f.of("stdin", p))
            assertion.equal(p, uint32_2)
            assertion.is_true(f.of("current_ms", p))
            assertion.equal(p, uint32_3)
            Return True
        End Function
    End Class
End Namespace
