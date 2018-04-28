﻿
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

    Private Shared Sub run_case(ByVal type_name As String)
        Dim i As invoker(Of Func(Of Int32, Int32)) = Nothing
        assert_true(typeless_invoker.[New](type_name,
                                           binding_flags.static_private_method,
                                           "f",
                                           i))
        assert_not_nothing(i)
        For j As Int32 = -100 To 100
            assert_equal(direct_cast(Of Int32)(i.invoke(Nothing, j)), j + 1)
        Next
    End Sub

    Private Shared Sub run_case(ByVal type_name As String, ByVal assembly_name As String)
        Dim i As invoker(Of Func(Of Int32, Int32)) = Nothing
        assert_true(typeless_invoker.[New](type_name, assembly_name,
                                           binding_flags.static_private_method,
                                           "f",
                                           i))
        assert_not_nothing(i)
        For j As Int32 = -100 To 100
            assert_equal(direct_cast(Of Int32)(i.invoke(Nothing, j)), j + 1)
        Next
    End Sub

    <test>
    Private Shared Sub run()
        run_case("osi.tests.root.utils.typeless_invoker_test, osi.tests.root.utils")
        run_case("osi.tests.root.utils.typeless_invoker_test, osi.tests.root.utils", default_str)
        run_case("osi.tests.root.utils.typeless_invoker_test, osi.tests.root.utils", default_string)
        run_case("osi.tests.root.utils.typeless_invoker_test")
        run_case("osi.tests.root.utils.typeless_invoker_test", default_str)
        run_case("osi.tests.root.utils.typeless_invoker_test", default_string)
        run_case(GetType(typeless_invoker_test).AssemblyQualifiedName())
        run_case(GetType(typeless_invoker_test).AssemblyQualifiedName(), default_str)
        run_case(GetType(typeless_invoker_test).AssemblyQualifiedName(), default_string)
        run_case(".typeless_invoker_test", "osi.tests.root.utils")
        run_case(".typeless_invoker_test", GetType(typeless_invoker_test).Assembly().FullName())

        Dim i As invoker(Of Func(Of Int32, Int32)) = Nothing
        assert_false(typeless_invoker.[New]("osi.tests.root.utils.typeless_invoker_test, osi.tests.root.utils",
                                            binding_flags.static_public_method,
                                            "f",
                                            i))
        assert_false(typeless_invoker.[New]("osi.tests.root.utils.typeless_invoker_test, osi.tests.root.utils",
                                            binding_flags.instance_private_method,
                                            "f",
                                            i))
        assert_false(typeless_invoker.[New]("osi.tests.root.utils.typeless_invoker_test,
                                            osi.tests.root.utils",
                                            "fg",
                                            i))
        assert_false(typeless_invoker.[New]("osi.tests.root.utils.typeless_invoker_test2, osi.tests.root.utils",
                                            "fg",
                                            i))
    End Sub
End Class
