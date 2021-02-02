
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.argument

<test>
Public NotInheritable Class argument_setter_test
    Private Shared argument_setter_test_bool_arg As argument(Of Boolean)
    Private Shared argument_setter_test_int_arg As argument(Of Int32)
    Private Shared argument_setter_test_str_arg As argument(Of String)
#If TODO Then
    Private Shared argument_setter_test_map_arg As argument(Of map(Of String, Int32))
#End If

    <test>
    Private Shared Sub set_different_types()
        argument_setter_test_bool_arg = Nothing
        argument_setter_test_int_arg = Nothing
        argument_setter_test_str_arg = Nothing
#If TODO Then
        argument_setter_test_map_arg = Nothing
#End If

        argument_setter.process_type(GetType(argument_setter_test), New var({
            "~argument_setter_test_bool_arg",
            "--argument_setter_test_int_arg=100",
            "--argument_setter_test_str_arg=abc",
            "--argument_setter_test_map_arg={a:1, b:2, c:3}"
        }))
        If assertion.is_not_null(argument_setter_test_bool_arg) Then
            assertion.is_true(+argument_setter_test_bool_arg)
        End If
        If assertion.is_not_null(argument_setter_test_int_arg) Then
            assertion.equal(+argument_setter_test_int_arg, 100)
        End If
        If assertion.is_not_null(argument_setter_test_str_arg) Then
            assertion.equal(+argument_setter_test_str_arg, "abc")
        End If
#If TODO Then
        If assertion.is_not_null(argument_setter_test_map_arg) Then
            assertion.equal(+argument_setter_test_map_arg, map.of(pair.of("a", 1), pair.of("b", 2), pair.of("c", 3)))
        End If
#End If
    End Sub

    Private NotInheritable Class argument_holder
        Public Shared argument_setter_test_argument_holder_arg As argument(Of Int32)

        Private Sub New()
        End Sub
    End Class

    <test>
    Private Shared Sub should_not_reset_values()
        argument_holder.argument_setter_test_argument_holder_arg = Nothing
        argument_setter.process_type(GetType(argument_holder), New var("--argument_setter_test_argument_holder_arg=1"))
        If assertion.is_not_null(argument_holder.argument_setter_test_argument_holder_arg) Then
            assertion.equal(+(argument_holder.argument_setter_test_argument_holder_arg), 1)
        End If
        Dim last As argument(Of Int32) = argument_holder.argument_setter_test_argument_holder_arg
        argument_setter.process_type(GetType(argument_holder), New var("--argument_setter_test_argument_holder_arg=2"))
        If assertion.is_not_null(argument_holder.argument_setter_test_argument_holder_arg) Then
            assertion.equal(+(argument_holder.argument_setter_test_argument_holder_arg), 1)
        End If
        assertion.reference_equal(last, argument_holder.argument_setter_test_argument_holder_arg)
    End Sub

    Private NotInheritable Class argument_holder2
        Public Shared argument_setter_test_argument_holder2_arg As argument(Of Int32)

        Private Sub New()
        End Sub
    End Class

    <test>
    Private Shared Sub global_init_should_cover_all_arguments()
        If assertion.is_not_null(argument_holder2.argument_setter_test_argument_holder2_arg) Then
            assertion.equal(+(argument_holder2.argument_setter_test_argument_holder2_arg), 0)
        End If
    End Sub

    Private Sub New()
    End Sub
End Class
