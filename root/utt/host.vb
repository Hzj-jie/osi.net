
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.constants.filesystem
Imports osi.root.delegates
Imports osi.root.envs
Imports osi.root.formation
Imports osi.root.utils

Partial Friend NotInheritable Class host
    ' Use a standalone class to avoid executing host.cctor().
    Public NotInheritable Class case_type_restriction
        Public Shared Function accepted_case_type(ByVal j As Type) As Boolean
            assert(Not j Is Nothing)
            Return Not j.IsAbstract() AndAlso
                   Not j.IsGenericType() AndAlso
                   (j.IsPublic() OrElse j.IsNestedPublic()) AndAlso
                   j.inherit(Of [case])() AndAlso
                   j.has_parameterless_public_constructor()
        End Function

        Public Shared Function accepted_case2_type(ByVal j As Type) As Boolean
            assert(Not j Is Nothing)
            ' Other requirements are in the case2 implementation.
            Return j.IsPublic() OrElse j.IsNestedPublic()
        End Function

        Private Sub New()
        End Sub
    End Class

    Private NotInheritable Class loader
        Private Shared Function load(ByVal j As Type) As vector(Of case_info)
            If case_type_restriction.accepted_case_type(j) Then
                Using defer(Sub()
                                raise_error("loaded case ", j.FullName())
                            End Sub)
                    Return vector.of(New case_info(j.FullName(), j.allocate(Of [case])()))
                End Using
            End If
            If case_type_restriction.accepted_case2_type(j) Then
                Return case2.create(j).map(Function(ByVal i As [case]) As case_info
                                               Using defer(Sub()
                                                               raise_error("loaded case ", i.full_name)
                                                           End Sub)
                                                   Return New case_info(i.full_name, i)
                                               End Using
                                           End Function)
            End If
            Return New vector(Of case_info)()
        End Function

        ' This function cannot be placed in host class. It depends on functions of host, which will trigger a deadlock.
        Public Shared Sub load(ByVal cases As vector(Of case_info))
            ' Cannot use event_comb, allocators of some cases may use async_sync.
            assert(Not cases Is Nothing)
            concurrency_runner.execute(Sub(i As Assembly)
                                           Try
                                               For Each j As Type In i.GetTypes()
                                                   SyncLock cases
                                                       cases.emplace_back(load(j))
                                                   End SyncLock
                                               Next
                                           Catch ex As Exception
                                               raise_error(error_type.warning,
                                                           "Failed to load type from assembly ",
                                                           i,
                                                           ", ex",
                                                           ex)
                                           End Try
                                       End Sub,
                                       AppDomain.CurrentDomain().GetAssemblies())
        End Sub

        Private Sub New()
        End Sub
    End Class

    Public Shared ReadOnly cases As vector(Of case_info) = Nothing

    Shared Sub New()
        assert((envs.utt.concurrency >= 0 AndAlso envs.utt.concurrency <= Environment.ProcessorCount()) OrElse
               envs.utt.concurrency = npos)

        assert(Not strstartwith(extensions.dynamic_link_library, extension_prefix, False))
        cases = New vector(Of case_info)()
        AppDomain.CurrentDomain().load_all(Environment.CurrentDirectory(), envs.utt.file_pattern)
        If Not Environment.CurrentDirectory().path_same(application_directory) Then
            AppDomain.CurrentDomain().load_all(application_directory, envs.utt.file_pattern)
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
            r = fit_patterns(c.name(), selector)
            If r = pattern_match.fit_true Then
                has_fit_true = True
            ElseIf r = pattern_match.fit_false Then
                Return False
            End If

            r = fit_patterns(c.full_name(), selector)
            If r = pattern_match.fit_true Then
                has_fit_true = True
            ElseIf r = pattern_match.fit_false Then
                Return False
            End If

            r = fit_patterns(c.assembly_qualified_name(), selector)
            If r = pattern_match.fit_true Then
                has_fit_true = True
            ElseIf r = pattern_match.fit_false Then
                Return False
            End If

            Return has_fit_true
        End If
    End Function

    Public Shared Sub clear_selection()
        cases.stream().foreach(Sub(ByVal x As case_info)
                                   x.finished = False
                               End Sub)
    End Sub

    Public Shared Function run(ByVal selector As vector(Of String)) As Int32
        Dim r As Int32 = 0
        clear_selection()
        cases.stream().foreach(Sub(ByVal x)
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
