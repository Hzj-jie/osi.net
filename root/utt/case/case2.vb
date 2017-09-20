
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

' Use attributes instead of inheritance to execute test case.
Partial Public NotInheritable Class case2
    Private Shared Function create(ByVal t As Type,
                                   ByVal class_info As info,
                                   ByVal prepare As Func(Of Object, Boolean),
                                   ByVal finish As Func(Of Object, Boolean),
                                   ByVal function_info As function_info) As utt.[case]
        assert(Not t Is Nothing)
        assert(Not class_info Is Nothing)
        assert(Not function_info Is Nothing)

        Dim r As utt.[case] = Nothing
        r = New [case](t,
                       function_info.name,
                       prepare,
                       function_info.f,
                       finish,
                       If(class_info.has_reserved_processors,
                          class_info.reserved_processors,
                          function_info.reserved_processors))
        If class_info.repeat_times > 1 OrElse function_info.repeat_times > 1 Then
            r = repeat(r, CLng(class_info.repeat_times * function_info.repeat_times))
        End If
        If class_info.command_line_specified OrElse function_info.command_line_specified Then
            r = commandline_specified(r)
        End If
        Return r
    End Function

    Private Shared Function create(ByVal t As Type,
                                   ByVal class_info As info,
                                   ByVal prepare As Func(Of Object, Boolean),
                                   ByVal finish As Func(Of Object, Boolean),
                                   ByVal randoms As vector(Of random_function_info)) As utt.[case]
        assert(Not t Is Nothing)
        assert(Not class_info Is Nothing)
        assert(Not randoms Is Nothing)

        Dim r As utt.[case] = Nothing
        r = New random_run_case(t,
                                prepare,
                                finish,
                                class_info.reserved_processors,
                                randoms)
        If class_info.repeat_times > 1 Then
            r = repeat(r, CLng(class_info.repeat_times))
        End If
        If class_info.command_line_specified Then
            r = commandline_specified(r)
        End If
        Return r
    End Function

    Public Shared Function create(ByVal t As Type) As vector(Of utt.[case])
        assert(Not t Is Nothing)
        If t.has_custom_attribute(Of attributes.test)() AndAlso
           Not t.IsAbstract() AndAlso
           Not t.IsGenericType() Then
            Dim class_info As info = Nothing
            class_info = info.from(t)

            Dim r As vector(Of utt.[case]) = Nothing
            r = New vector(Of utt.[case])()

            Dim prepare As Func(Of Object, Boolean) = Nothing
            Dim finish As Func(Of Object, Boolean) = Nothing
            prepare = parse_prepare(t)
            finish = parse_finish(t)

            Dim tests As vector(Of function_info) = Nothing
            tests = parse_tests(t)
            Dim i As UInt32 = 0
            While i < tests.size()
                r.emplace_back(create(t, class_info, prepare, finish, tests(i)))
                i += uint32_1
            End While

            Dim randoms As vector(Of random_function_info) = Nothing
            randoms = parse_randoms(t)
            If Not randoms.null_or_empty() Then
                r.emplace_back(create(t, class_info, prepare, finish, randoms))
            End If
            Return r
        Else
            Return Nothing
        End If
    End Function

    Private Sub New()
    End Sub
End Class
