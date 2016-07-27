
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.interpreter.fullstack.executor

Namespace fullstack.executor
    Public Class domain_test
        Inherits [case]

        Private ReadOnly root_size As Int32
        Private ReadOnly sub_size As Int32

        Public Sub New()
            root_size = rnd_int(1, 16)
            sub_size = rnd_int(1, 16)
        End Sub

        Public Overrides Function run() As Boolean
            Dim vars() As variable = Nothing
            ReDim vars(root_size + sub_size - 1)
            assert(array_size(vars), root_size + sub_size)
            For i As Int32 = 0 To array_size(vars) - 1
                vars(i) = rnd_variable()
            Next
            Dim vs As variables = Nothing
            vs = New variables()
            Dim root As domain = Nothing
            root = New domain(vs)
            For i As Int32 = 0 To root_size - 1
                root.define(vars(i))
            Next
            assert_equal(root.increment(), root_size)
            Dim domain As domain = Nothing
            Using root.create_disposer(domain)
                For i As Int32 = 0 To sub_size - 1
                    domain.define(vars(i + root_size))
                Next
                assert_equal(domain.increment(), sub_size)
                assert_equal(root.increment() + domain.increment(), root_size + sub_size)
                assert_equal(CInt(vs.size()), root_size + sub_size)
                assert_reference_equal(domain.parent(), root)
                For i As Int32 = 0 To sub_size - 1
                    assert_reference_equal(domain.variable(0, i), vars(i + root_size))
                Next
                For i As Int32 = 0 To root_size - 1
                    assert_reference_equal(domain.variable(1, i), vars(i))
                Next
            End Using
            assert_equal(domain.increment(), 0)
            assert_equal(root.increment(), root_size)
            assert_equal(CInt(vs.size()), root_size)
            For i As Int32 = 0 To root_size - 1
                assert_reference_equal(root.variable(0, i), vars(i))
            Next

            Return True
        End Function
    End Class
End Namespace
