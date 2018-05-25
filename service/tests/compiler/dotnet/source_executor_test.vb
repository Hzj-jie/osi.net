
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.compiler.dotnet
Imports osi.service.resource

Namespace dotnet
    <test>
    Public NotInheritable Class source_executor_test
        <test>
        Private Shared Sub execute_from_source()
            Dim se As source_executor = Nothing
            assert_true(source_executor.
                            [New]().
                            add_source(from_source1.as_text()).
                            with_language(source_executor.language.cs).
                            build(se))
            Dim r As Object = Nothing
            assert_true(se.execute("from_source1", "execute", r, 1))
            assert_equal(r, 2)
            assert_true(se.execute("from_source1", "execute2", r, 2))
            assert_equal(r, 1)
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub execute_from_source_perf()
            Dim se As source_executor = Nothing
            assert_true(source_executor.
                            [New]().
                            add_source(from_source1.as_text()).
                            with_language(source_executor.language.cs).
                            build(se))
            For i As Int32 = 0 To 1000000
                Dim r As Object = Nothing
                assert_true(se.execute("from_source1", "execute", r, 1))
                assert_equal(r, 2)
                assert_true(se.execute("from_source1", "execute2", r, 2))
                assert_equal(r, 1)
            Next
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
