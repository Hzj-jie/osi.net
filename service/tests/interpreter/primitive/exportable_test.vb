
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.interpreter.primitive

Namespace primitive
    Public MustInherit Class exportable_test(Of T As {exportable, IComparable(Of T)})
        Inherits [case]

        Protected MustOverride Function create() As T

        Public NotOverridable Overrides Function run() As Boolean
            For i As UInt32 = 0 To 65535
                Dim x As T = Nothing
                Dim y As T = Nothing
                x = create()
                assert(Not x Is Nothing)
                Dim b() As Byte = Nothing
                assertion.is_true(x.export(b))
                y = create()
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
