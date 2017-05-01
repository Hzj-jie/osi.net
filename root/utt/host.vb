
Imports System.IO
Imports System.Reflection
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.formation
Imports osi.root.utils
Imports osi.root.constants.filesystem
Imports osi.root.delegates

Partial Friend NotInheritable Class host
    Public NotInheritable Class case_type_restriction
        Public Shared Function accept(ByVal j As Type) As Boolean
            assert(Not j Is Nothing)
            Return Not j.IsAbstract() AndAlso
                   Not j.IsGenericType() AndAlso
                   (j.IsPublic() OrElse j.IsNestedPublic()) AndAlso
                   j.inherit(Of [case])() AndAlso
                   j.has_parameterless_public_constructor()
        End Function

        Private Sub New()
        End Sub
    End Class

    Private NotInheritable Class loader
        ' This function cannot be placed in host class. It depends on functions of host, which will trigger a deadlock.
        Public Shared Sub load(ByVal cases As vector(Of case_info))
            ' Cannot use event_comb, allocators of some cases may use async_sync.
            assert(Not cases Is Nothing)
            concurrency_runner.execute(Sub(i As Assembly)
                                           Try
                                               For Each j In i.GetTypes()
                                                   If case_type_restriction.accept(j) Then
                                                       Dim n As case_info = Nothing
                                                       n = New case_info(j.FullName(), alloc(Of [case])(j))
                                                       SyncLock cases
                                                           cases.emplace_back(n)
                                                       End SyncLock
                                                       raise_error("loaded case ", j.FullName())
                                                   End If
                                               Next
                                           Catch ex As Exception
                                               raise_error(error_type.warning,
                                                           "failed to load type from assembly ",
                                                           i.FullName())
                                           End Try
                                       End Sub,
                                       AppDomain.CurrentDomain().GetAssemblies())
        End Sub

        Private Sub New()
        End Sub
    End Class

    Public Shared ReadOnly cases As vector(Of case_info) = Nothing

    Shared Sub New()
        assert((envs.utt_concurrency >= 0 AndAlso envs.utt_concurrency <= Environment.ProcessorCount()) OrElse
               envs.utt_concurrency = npos)

        assert(Not strstartwith(extensions.dynamic_link_library, extension_prefix, False))
        cases = New vector(Of case_info)()
        AppDomain.CurrentDomain().load_all(Environment.CurrentDirectory(), utt_file_pattern)
        If Not Environment.CurrentDirectory().path_same(application_directory) Then
            AppDomain.CurrentDomain().load_all(application_directory, utt_file_pattern)
        End If
        ' for newly loaded types
        global_init.execute()
        loader.load(cases)
        assert(cases.size() > 0)
    End Sub

    Private Shared Function [select](ByVal selector As vector(Of String), ByVal c As case_info) As Boolean
        If selector Is Nothing OrElse selector.empty() Then
            Return True
        Else
            assert(Not c Is Nothing)
            Dim has_fit_true As Boolean = False
            Dim r As Byte = 0
            r = match_all(c.name(), selector)
            If r = pattern_match.fit_true Then
                has_fit_true = True
            ElseIf r = pattern_match.fit_false Then
                Return False
            End If

            r = match_all(c.full_name(), selector)
            If r = pattern_match.fit_true Then
                has_fit_true = True
            ElseIf r = pattern_match.fit_false Then
                Return False
            End If

            r = match_all(c.assembly_qualified_name(), selector)
            If r = pattern_match.fit_true Then
                has_fit_true = True
            ElseIf r = pattern_match.fit_false Then
                Return False
            End If

            Return has_fit_true
        End If
    End Function

    Public Shared Function foreach(ByVal d As _do(Of case_info, Boolean, Boolean)) As Boolean
        If d Is Nothing Then
            Return False
        Else
            Return cases.foreach(d)
        End If
    End Function

    Public Shared Function foreach(ByVal d As _do(Of case_info, Boolean)) As Boolean
        Return utils.foreach(AddressOf foreach, d)
    End Function

    Public Shared Function foreach(ByVal d As void(Of case_info)) As Boolean
        Return utils.foreach(AddressOf foreach, d)
    End Function

    Public Shared Sub clear_selection()
        foreach(Sub(ByRef x) x.finished = False)
    End Sub

    Public Shared Function run(ByVal selector As vector(Of String)) As Int32
        Dim r As Int32 = 0
        clear_selection()
        foreach(Sub(ByRef x)
                    If [select](selector, x) Then
                        r += 1
                    Else
                        x.finished = True
                    End If
                End Sub)

        host.run()
        Return r
    End Function

    Private Sub New()
    End Sub
End Class
