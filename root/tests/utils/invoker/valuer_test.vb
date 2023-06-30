
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utils
Imports osi.root.utt

Public Class valuer_test
    Inherits [case]

    Private Interface test_int
    End Interface

    Private Class test_imp
        Implements test_int
    End Class

    Private Class test_inh
        Inherits test_imp
    End Class

    Private Class test_class
        Public ReadOnly a As Int32
        Public b As Int32
        Public ReadOnly c As test_int
        Public ReadOnly d As test_imp
        Public ReadOnly e As test_inh
        Private ReadOnly f As String
        Private g As String

        Public Shared ReadOnly h As Int32
        Private Shared ReadOnly i As Int32
        Public Shared j As Int32
        Private Shared k As Int32
    End Class

    Public Overrides Function run() As Boolean
        Using scoped.atomic_bool(suppress.valuer_error)
            Return run_case(Of Int32)("a", rnd_int(), binding_flags.instance_public) AndAlso
                   run_case(Of Int32)("b", rnd_int(), binding_flags.instance_public) AndAlso
                   run_case(Of test_int)("c", New test_imp(), binding_flags.instance_public) AndAlso
                   run_case(Of test_imp)("d", New test_imp(), binding_flags.instance_public) AndAlso
                   run_case(Of test_inh)("e", New test_inh(), binding_flags.instance_public) AndAlso
                   run_case(Of String)("f", rnd_chars(rnd_int(10, 100)), binding_flags.instance_private) AndAlso
                   run_case(Of String)("g", rnd_chars(rnd_int(10, 100)), binding_flags.instance_private) AndAlso
                   run_case(Of Int32)("h", rnd_int(), binding_flags.static_public) AndAlso
                   run_case(Of Int32)("i", rnd_int(), binding_flags.static_private) AndAlso
                   run_case(Of Int32)("j", rnd_int(), binding_flags.static_public) AndAlso
                   run_case(Of Int32)("k", rnd_int(), binding_flags.static_private) AndAlso
 _
                   set_only_case(Of test_imp, test_inh)("c", binding_flags.instance_public) AndAlso
                   get_only_case(Of test_imp, test_inh)("e", binding_flags.instance_public) AndAlso
                   fail_case(Of Int32)("f", binding_flags.instance_private) AndAlso
                   fail_case(Of String)("f", binding_flags.instance_public) AndAlso
                   fail_case(Of String)("f", binding_flags.static_all) AndAlso
                   fail_case(Of String)("F", binding_flags.instance_private) AndAlso
                   fail_case(Of String)("z", binding_flags.all)
        End Using
    End Function

    Private Shared Function new_valuer(Of T)(ByVal name As String, ByVal bindingflags As BindingFlags) As valuer(Of T)
        Return new_valuer(Of T)(If((bindingflags And BindingFlags.Instance) <> 0, New test_class(), Nothing),
                                name,
                                bindingflags)
    End Function

    Private Shared Function new_valuer(Of T)(ByVal obj As test_class,
                                             ByVal name As String,
                                             ByVal bindingflags As BindingFlags) As valuer(Of T)
        Return New valuer(Of T)(GetType(test_class),
                                bindingflags,
                                obj,
                                name)
    End Function

    Private Shared Function run_case(Of T)(ByVal name As String,
                                           ByVal value As T,
                                           ByVal bindingflags As BindingFlags) As Boolean
        Dim v As valuer(Of T) = Nothing
        v = new_valuer(Of T)(name, bindingflags)
        If assertion.is_true(v.valid()) Then
            assertion.is_true(v.try_set(value))
            Dim w As T = Nothing
            assertion.is_true(v.try_get(w))
            assertion.equal(value, w)
        End If

        If (bindingflags And BindingFlags.Instance) <> 0 Then
            v = new_valuer(Of T)(Nothing, name, bindingflags)
            If assertion.is_true(v.valid()) Then
                Dim c As test_class = Nothing
                c = New test_class()
                assertion.is_true(v.try_set(c, value))
                Dim w As T = Nothing
                assertion.is_true(v.try_get(c, w))
                assertion.equal(value, w)
            End If
        End If

        Return True
    End Function

    Private Shared Function set_only_case(Of T As New, T2 As {T, New}) _
                                         (ByVal name As String, ByVal bindingflags As BindingFlags) As Boolean
        Dim c As test_class = Nothing
        c = New test_class()
        Dim v As valuer(Of T2) = Nothing
        v = new_valuer(Of T2)(c, name, bindingflags)
        If assertion.is_true(v.valid()) Then
            assertion.is_true(v.try_set(New T2()))
            assertion.is_true(new_valuer(Of T)(c, name, bindingflags).try_set(New T()))
            Dim w As T2 = Nothing
            assertion.is_false(v.try_get(w))
            assertion.is_true(w Is Nothing)
        End If
        Return True
    End Function

    Private Shared Function get_only_case(Of T As New, T2 As {T, New}) _
                                         (ByVal name As String,
                                          ByVal bindingflags As BindingFlags) As Boolean
        Dim v As valuer(Of T2) = Nothing
        v = new_valuer(Of T2)(name, bindingflags)
        If assertion.is_true(v.valid()) Then
            assertion.is_false(new_valuer(Of T)(name, bindingflags).try_set(New T()))
            assertion.is_true(v.try_set(New T2()))
            Dim w As T = Nothing
            w = v.get_or_null()
            assertion.is_not_null(w)
            assertion.is_true(implicit_conversions.valuer_test_get_only_case_try_get(Of T, T2)(v, w))
        End If
        Return True
    End Function

    Private Shared Function fail_case(Of T)(ByVal name As String, ByVal bindingflags As BindingFlags) As Boolean
        Dim v As valuer(Of T) = Nothing
        v = new_valuer(Of T)(name, bindingflags)
        assertion.is_false(v.valid())
        Return True
    End Function
End Class
