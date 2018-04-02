
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class type_test
    Private Shared Sub run_case(ByVal name As String)
        Dim t As Type = Nothing
        assert_true(t.[New](name))
        assert_not_nothing(t)
        assert_equal(t, GetType(type_test))
    End Sub

    Private Shared Sub run_case(ByVal type_name As String, ByVal assembly_name As String)
        Dim t As Type = Nothing
        assert_true(t.[New](type_name, assembly_name))
        assert_not_nothing(t)
        assert_equal(t, GetType(type_test))
    End Sub

    Private Shared Sub run_failed_case(ByVal name As String)
        Dim t As Type = Nothing
        assert_false(t.[New](name))
        assert_nothing(t)
    End Sub

    Private Shared Sub run_failed_case(ByVal type_name As String, ByVal assembly_name As String)
        Dim t As Type = Nothing
        assert_false(t.[New](type_name, assembly_name))
        assert_nothing(t)
    End Sub

    <test>
    Private Shared Sub new_tests()
        ' osi.tests.root.connector.type_test, osi.tests.root.connector, version ...
        run_case(GetType(type_test).AssemblyQualifiedName())
        run_case(GetType(type_test).AssemblyQualifiedName(), default_str)
        run_case(GetType(type_test).AssemblyQualifiedName(), default_string)

        ' osi.tests.root.connector.type_test, osi.tests.root.connector
        run_case(strcat(GetType(type_test).full_name(),
                        character.comma,
                        character.blank,
                        GetType(type_test).Assembly().GetName().Name()))
        run_case(strcat(GetType(type_test).full_name(),
                        character.comma,
                        character.blank,
                        GetType(type_test).Assembly().GetName().Name()),
                 default_str)
        run_case(strcat(GetType(type_test).full_name(),
                        character.comma,
                        character.blank,
                        GetType(type_test).Assembly().GetName().Name()),
                 default_string)

        ' osi.tests.root.connector.type_test
        run_case(GetType(type_test).full_name())
        run_case(GetType(type_test).full_name(), default_str)
        run_case(GetType(type_test).full_name(), default_string)

        run_case(GetType(type_test).full_name(), GetType(type_test).Assembly().GetName().FullName())
        run_case(GetType(type_test).full_name(), GetType(type_test).Assembly().GetName().Name())
        run_case(".type_test", GetType(type_test).Assembly().GetName().FullName())
        run_case(".type_test", GetType(type_test).Assembly().GetName().Name())

        run_failed_case(strcat(GetType(type_test).AssemblyQualifiedName(), "..."))
        run_failed_case(strcat(GetType(type_test).full_name(),
                               GetType(type_test).Assembly().GetName().Name()))
        run_failed_case(strcat(GetType(type_test).full_name(), "..."))
        run_failed_case(strcat(".", GetType(type_test).full_name()), GetType(type_test).Assembly().GetName().FullName())
        run_failed_case(strcat(".", GetType(type_test).full_name()), GetType(type_test).Assembly().GetName().Name())
        run_failed_case("..type_test", GetType(type_test).Assembly().GetName().FullName())
        run_failed_case("..type_test", GetType(type_test).Assembly().GetName().Name())
        run_failed_case("type_test", GetType(type_test).Assembly().GetName().FullName())
        run_failed_case("type_test", GetType(type_test).Assembly().GetName().Name())
    End Sub

    Private Interface i1(Of T)
    End Interface

    Private Interface i2(Of T, T2)
    End Interface

    Private Class c1(Of T)
        Implements i1(Of T)
    End Class

    Private Class c2(Of T, T2)
        Implements i2(Of T, T2)
    End Class

    Private Class c3(Of T)
        Inherits c1(Of T)
    End Class

    <test>
    Private Shared Sub generic_type_is_tests()
        assert_true(GetType(i1(Of Int32)).generic_type_is(GetType(i1(Of ))))
        assert_true(GetType(c1(Of Int32)).generic_type_is(GetType(c1(Of ))))
        assert_false(GetType(i1(Of Int32)).generic_type_is(GetType(c1(Of ))))
        assert_false(GetType(c1(Of Int32)).generic_type_is(GetType(i1(Of ))))

        assert_false(GetType(i1(Of Int32)).generic_type_is(GetType(i1(Of Int32))))
        assert_false(GetType(c1(Of Int32)).generic_type_is(GetType(c1(Of Int32))))

        assert_true(GetType(i2(Of Int32, UInt32)).generic_type_is(GetType(i2(Of ,))))
        assert_true(GetType(c2(Of Int32, UInt32)).generic_type_is(GetType(c2(Of ,))))
        assert_false(GetType(i2(Of Int32, UInt32)).generic_type_is(GetType(c2(Of ,))))
        assert_false(GetType(c2(Of Int32, UInt32)).generic_type_is(GetType(i2(Of ,))))

        assert_false(GetType(i2(Of Int32, UInt32)).generic_type_is(GetType(i2(Of Int32, UInt32))))
        assert_false(GetType(c2(Of Int32, UInt32)).generic_type_is(GetType(c2(Of Int32, UInt32))))

        assert_true(GetType(c3(Of Int32)).generic_type_is(GetType(c3(Of ))))
        assert_false(GetType(c3(Of Int32)).generic_type_is(GetType(c1(Of ))))
    End Sub

    Private Sub New()
    End Sub
End Class
