
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

' Use attributes instead of inheritance to execute test case.
Partial Public NotInheritable Class case2
    Private Shared Function create(ByVal c As [case], ByVal info As info) As utt.case
        assert(Not c Is Nothing)
        assert(Not info Is Nothing)
        Dim r As utt.[case] = c
        If info.repeat_times > 1 Then
            r = repeat(r, CLng(info.repeat_times))
        End If
        If info.thread_count > 1 Then
            assert(info.thread_count <= max_int32)
            r = multithreading(r, info.thread_count)
        End If
        If info.command_line_specified Then
            r = commandline_specified(r, c.full_name, c.assembly_qualified_name, c.name)
        End If
        If info.flaky Then
            r = flaky(r)
        End If
        If object_compare(r, c) <> 0 Then
            Return New case_wrapper(r, c.full_name, c.assembly_qualified_name, c.name)
        End If
        Return c
    End Function

    Private Shared Function create(ByVal t As Type,
                                   ByVal class_info As info,
                                   ByVal prepare As Func(Of Object, Boolean),
                                   ByVal finish As Func(Of Object, Boolean),
                                   ByVal function_info As function_info) As utt.[case]
        assert(Not t Is Nothing)
        assert(Not class_info Is Nothing)
        assert(Not function_info Is Nothing)

        Dim n As info = info.merge(class_info, function_info)
        Return create(New [case](t,
                                 function_info.name,
                                 prepare,
                                 function_info.f,
                                 finish,
                                 n.reserved_processors),
                      n)
    End Function

    Private Shared Function create(ByVal t As Type,
                                   ByVal class_info As info,
                                   ByVal prepare As Func(Of Object, Boolean),
                                   ByVal finish As Func(Of Object, Boolean),
                                   ByVal randoms As vector(Of random_function_info)) As utt.[case]
        assert(Not t Is Nothing)
        assert(Not class_info Is Nothing)
        assert(Not randoms Is Nothing)

        Return create(New random_run_case(t,
                                          prepare,
                                          finish,
                                          class_info.reserved_processors,
                                          randoms),
                      class_info)
    End Function

    Public Shared Function create(ByVal t As Type) As vector(Of utt.[case])
        assert(Not t Is Nothing)
        Dim r As New vector(Of utt.[case])()
        If Not t.has_custom_attribute(Of attributes.test)() OrElse
           t.IsAbstract() OrElse
           t.IsGenericType() Then
            Return r
        End If

        Dim class_info As info = info.from(t)
        Dim prepare As Func(Of Object, Boolean) = parse_prepare(t)
        Dim finish As Func(Of Object, Boolean) = parse_finish(t)
        Dim tests As vector(Of function_info) = parse_tests(t)
        Dim i As UInt32 = 0
        While i < tests.size()
            r.emplace_back(create(t, class_info, prepare, finish, tests(i)))
            i += uint32_1
        End While
        Dim randoms As vector(Of random_function_info) = parse_randoms(t)
        If Not randoms.null_or_empty() Then
            r.emplace_back(create(t, class_info, prepare, finish, randoms))
        End If
        Return r
    End Function

    Public NotInheritable Class run_result
        Public ReadOnly cases As UInt32
        Public ReadOnly succeeded As UInt32

        Public Sub New(ByVal cases As UInt32,
                       ByVal succeeded As UInt32)
            Me.cases = cases
            Me.succeeded = succeeded
        End Sub

        Public Sub assert_succeeded(ByVal cases As UInt32)
            assertion.equal(Me.cases, cases)
            assertion.equal(Me.succeeded, cases)
        End Sub

        Public Sub assert_succeeded()
            assert_succeeded(uint32_1)
        End Sub
    End Class

    ' Runs a type as a case2
    Public Shared Function run(Of T)() As run_result
        Return run(GetType(T))
    End Function

    Public Shared Function run(ByVal t As Type) As run_result
        Dim v As vector(Of utt.case) = create(t)
        Dim i As UInt32 = 0
        Dim succeeded As UInt32 = 0
        While i < v.size()
            If host.execute_case(v(i)) Then
                succeeded += uint32_1
            End If
            i += uint32_1
        End While
        Return New run_result(v.size(), succeeded)
    End Function

    Private Sub New()
    End Sub
End Class
