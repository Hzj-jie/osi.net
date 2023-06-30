
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt
Imports osi.service.interpreter.primitive

Namespace primitive
    Public MustInherit Class exportable_test(Of T As {exportable, IComparable(Of T)})
        Inherits [case]

        Protected MustOverride Function create() As T

        Public NotOverridable Overrides Function run() As Boolean
            For i As UInt32 = 0 To 65535
                Dim x As T = create()
                assert(Not x Is Nothing)
                Dim b() As Byte = Nothing
                assertion.is_true(x.export(b))
                Dim y As T = create()
                Dim p As UInt32 = 0
                assertion.is_true(y.import(b, p))
                assertion.equal(p, array_size(b))
                assertion.equal(x, y)
                Dim s As String = Nothing
                assertion.is_true(x.export(s))
                y = create()
                assertion.is_true(DirectCast(y, exportable).import(s))
                assertion.equal(x, y)
            Next
            Return True
        End Function
    End Class
End Namespace
