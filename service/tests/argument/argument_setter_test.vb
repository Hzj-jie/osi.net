
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.argument

<test>
Public NotInheritable Class argument_setter_test
    Private Shared bool_arg As argument(Of Boolean)
    Private Shared int_arg As argument(Of Int32)
    Private Shared str_arg As argument(Of String)
    Private Shared vector_arg As argument(Of vector(Of Int32))
    Private Shared map_arg As argument(Of map(Of String, Int32))
    Private Shared others As argument(Of vector(Of String))

    <test>
    Private Shared Sub set_different_types()
        bool_arg = Nothing
        int_arg = Nothing
        str_arg = Nothing
        vector_arg = Nothing
        map_arg = Nothing
        others = Nothing

        argument_setter.process_type(GetType(argument_setter_test), New var({
            "~argument_setter_test.bool_arg",
            "--argument_setter_test.int_arg=100",
            "--argument_setter_test.str_arg=abc",
            "--argument_setter_test.vector_arg=1,2,3",
            "--argument_setter_test.map_arg=a:1,b:2,c:3",
            "others1",
            "others2",
            "others3"
        }))
        If assertion.is_not_null(bool_arg) Then
            assertion.is_true(+bool_arg)
        End If
        If assertion.is_not_null(int_arg) Then
            assertion.equal(+int_arg, 100)
        End If
        If assertion.is_not_null(str_arg) Then
            assertion.equal(+str_arg, "abc")
        End If
        If assertion.is_not_null(vector_arg) Then
            assertion.equal(+vector_arg, vector.of(1, 2, 3))
        End If
        If assertion.is_not_null(map_arg) Then
            assertion.equal(+map_arg, map.of(pair.of("a", 1), pair.of("b", 2), pair.of("c", 3)))
        End If
        If assertion.is_not_null(others) Then
            assertion.equal(+others, vector.of("others1", "others2", "others3"))
        End If
    End Sub

    Private NotInheritable Class argument_holder7
        Public Shared bool_arg As argument(Of Boolean)

        Private Sub New()
        End Sub
    End Class

    <test>
    Private Shared Sub different_types_of_boolean()
        argument_holder7.bool_arg = Nothing
        argument_setter.process_type(GetType(argument_holder7), New var({
            "~argument_holder7.bool_arg"
        }))
        If assertion.is_not_null(argument_holder7.bool_arg) Then
            assertion.is_true(+argument_holder7.bool_arg)
        End If

        argument_holder7.bool_arg = Nothing
        argument_setter.process_type(GetType(argument_holder7), New var({
            "--argument_holder7.bool_arg"
        }))
        If assertion.is_not_null(argument_holder7.bool_arg) Then
            assertion.is_true(+argument_holder7.bool_arg)
        End If
    End Sub

    Private NotInheritable Class argument_holder
        Public Shared arg As argument(Of Int32)

        Private Sub New()
        End Sub
    End Class

    <test>
    Private Shared Sub should_not_reset_values()
        argument_holder.arg = Nothing
        argument_setter.process_type(GetType(argument_holder), New var("--argument_holder.arg=1"))
        If assertion.is_not_null(argument_holder.arg) Then
            assertion.equal(+argument_holder.arg, 1)
        End If
        Dim last As argument(Of Int32) = argument_holder.arg
        argument_setter.process_type(GetType(argument_holder), New var("--argument_holder.arg=2"))
        If assertion.is_not_null(argument_holder.arg) Then
            assertion.equal(+argument_holder.arg, 1)
        End If
        assertion.reference_equal(last, argument_holder.arg)
    End Sub

    Private NotInheritable Class argument_holder2
        Public Shared arg As argument(Of Int32)
        Public Shared others As argument(Of vector(Of String))

        Private Sub New()
        End Sub
    End Class

    <test>
    Private Shared Sub global_init_should_cover_all_arguments()
        If assertion.is_not_null(argument_holder2.arg) Then
            assertion.equal(+argument_holder2.arg, 0)
            assertion.equal(argument_holder2.arg Or 100, 100)
        End If
        If assertion.is_not_null(argument_holder2.others) Then
            assertion.is_not_null(+argument_holder2.others)
        End If
    End Sub

    Private NotInheritable Class argument_holder3
        Public Shared arg As argument(Of Int32)

        Private Sub New()
        End Sub
    End Class

    <test>
    Private Shared Sub use_dash_instead_of_underscore()
        argument_holder3.arg = Nothing
        argument_setter.process_type(GetType(argument_holder3), New var("--argument-holder3.arg=1"))
        If assertion.is_not_null(argument_holder3.arg) Then
            assertion.equal(+argument_holder3.arg, 1)
        End If
    End Sub

    Private NotInheritable Class argument_holder4
        Public Shared arg As argument(Of Int32)
        Public Shared arg2 As argument(Of Int32)

        Private Sub New()
        End Sub
    End Class

    <test>
    Private Shared Sub prefer_long_name()
        argument_holder4.arg = Nothing
        argument_holder4.arg2 = Nothing
        argument_setter.process_type(GetType(argument_holder4),
                                     New var({"--argument-holder4.arg=1", "--arg=2", "--arg2=3"}))
        If assertion.is_not_null(argument_holder4.arg) Then
            assertion.equal(+argument_holder4.arg, 1)
        End If
        If assertion.is_not_null(argument_holder4.arg2) Then
            assertion.equal(+argument_holder4.arg2, 3)
        End If
    End Sub

    Private NotInheritable Class argument_holder5
        Public Enum e
            a = 1
            b = 3
        End Enum

        Public Shared arg As argument(Of e)
    End Class

    <test>
    Private Shared Sub enum_tests()
        argument_holder5.arg = Nothing
        argument_setter.process_type(GetType(argument_holder5), New var({"--arg=a"}))
        If assertion.is_not_null(argument_holder5.arg) Then
            assertion.equal(+argument_holder5.arg, argument_holder5.e.a)
        End If
        argument_holder5.arg = Nothing
        argument_setter.process_type(GetType(argument_holder5), New var({"--arg=b"}))
        If assertion.is_not_null(argument_holder5.arg) Then
            assertion.equal(+argument_holder5.arg, argument_holder5.e.b)
        End If
    End Sub

    Private NotInheritable Class argument_holder6
        Public Enum e
            a
            b
        End Enum

        Public Shared arg As argument(Of UInt32)
        Public Shared arg2 As argument(Of e)
    End Class

    <test>
    Private Shared Sub crash_if_type_mismatches()
        argument_holder6.arg = Nothing
        assertion.death(Sub()
                            argument_setter.process_type(GetType(argument_holder6),
                                                         New var({"--argument_holder6.arg=x"}))
                        End Sub,
                        Sub(ByVal msg As String)
                            assertion.is_true(msg.contains_all(
                                                  "argument_holder6.arg",
                                                  "x",
                                                  type_info(Of argument_holder6).assembly_qualified_name))
                        End Sub)

        argument_holder6.arg2 = Nothing
        assertion.death(Sub()
                            argument_setter.process_type(GetType(argument_holder6),
                                                         New var({"--argument_holder6.arg2=x"}))
                        End Sub,
                        Sub(ByVal msg As String)
                            assertion.is_true(msg.contains_all(
                                                  "argument_holder6.arg2",
                                                  "x",
                                                  type_info(Of argument_holder6).assembly_qualified_name))
                        End Sub)
    End Sub

    Private NotInheritable Class argument_holder8
        Public Shared arg As argument(Of UInt32)
    End Class

    <test>
    Private Shared Sub use_full_argument_name()
        argument_holder8.arg = Nothing
        argument_setter.process_type(GetType(argument_holder8),
                                     New var({
                                         "--osi.tests.service.argument.argument_setter_test.argument_holder8.arg=100"
                                     }))
        If assertion.is_not_null(argument_holder8.arg) Then
            assertion.equal(+argument_holder8.arg, CUInt(100))
        End If
    End Sub

    Private Sub New()
    End Sub
End Class
