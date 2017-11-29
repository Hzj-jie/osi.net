
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utils
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class typeless_invoker_test
    Private Shared Function f(ByVal i As Int32) As Int32
        Return i + 1
    End Function

    <test>
    Private Shared Sub run()
        Dim i As invoker(Of Func(Of Int32, Int32)) = Nothing
        assert_true(typeless_invoker.[New]("osi.tests.root.utils.typeless_invoker_test, osi.tests.root.utils",
                                           binding_flags.static_private_method,
                                           "f",
                                           i))
        assert_not_nothing(i)
        For j As Int32 = -100 To 100
            assert_equal(direct_cast(Of Int32)(i.invoke(Nothing, j)), j + 1)
        Next

        assert_true(typeless_invoker.[New](GetType(typeless_invoker_test).AssemblyQualifiedName(),
                                           binding_flags.static_private_method,
                                           "f",
                                           i))
        assert_not_nothing(i)
        For j As Int32 = -100 To 100
            assert_equal(direct_cast(Of Int32)(i.invoke(Nothing, j)), j + 1)
        Next

        ' TODO: Make typeless_invoker.[New] safe when handling unexisting functions.
        ' assert_false(typeless_invoker.[New]("osi.tests.root.utils.typeless_invoker_test, osi.tests.root.utils",
        '                                     binding_flags.static_public_method,
        '                                     "f",
        '                                     i))
        ' assert_false(typeless_invoker.[New]("osi.tests.root.utils.typeless_invoker_test, osi.tests.root.utils",
        '                                     binding_flags.private_method,
        '                                     "f",
        '                                     i))
        ' assert_false(typeless_invoker.[New]("osi.tests.root.utils.typeless_invoker_test,
        '                                     osi.tests.root.utils",
        '                                     "fg",
        '                                     i))
        assert_false(typeless_invoker.[New](
                "osi.tests.root.utils.typeless_invoker_test2, osi.tests.root.utils", "fg", i))
    End Sub
End Class
