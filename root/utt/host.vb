
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.constants.filesystem
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
                Using defer.to(Sub()
                                   raise_error("loaded case ", j.FullName())
                               End Sub)
                    Return vector.of(New case_info(j.FullName(), j.allocate(Of [case])()))
                End Using
            End If
            If case_type_restriction.accepted_case2_type(j) Then
                Return case2.create(j).
                             stream().
                             map(Function(ByVal i As [case]) As case_info
                                     Using defer.to(Sub()
                                                        raise_error("loaded case ", i.full_name)
                                                    End Sub)
                                         Return New case_info(i.full_name, i)
                                     End Using
                                 End Function).
                             collect_to(Of vector(Of case_info))()
            End If
            Return New vector(Of case_info)()
        End Function

        ' This function cannot be placed in host class. It depends on functions of host, which will trigger a deadlock.
        Public Shared Sub load(ByVal cases As vector(Of case_info))
            ' Cannot use event_comb, allocators of some cases may use async_sync.
            assert(Not cases Is Nothing)
            concurrency_runner.execute(Sub(ByVal i As Assembly)
                                           Try
                                               For Each j As Type In i.GetTypes()
                                                   Dim c As vector(Of case_info) = load(j)
                                                   If Not c.null_or_empty() Then
                                                       SyncLock cases
                                                           cases.emplace_back(c)
                                                       End SyncLock
                                                   End If
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

    Public Shared ReadOnly cases As vector(Of case_info)

    ' Ensure "cases" is always initialized after main. Debug mode uses eager static variable initialization.
    Shared Sub New()
        utt_raise_error("start loading tests")
        assert((envs.utt.concurrency >= 0 AndAlso envs.utt.concurrency <= Environment.ProcessorCount()) OrElse
               envs.utt.concurrency = npos)
        assert(Not extensions.dynamic_link_library.StartsWith(extension_prefix))
        cases = New vector(Of case_info)()
        ' The test files should be loaded by global_init already.
        loader.load(cases)
        assert(cases.size() > 0)
        utt_raise_error("finished loading ", cases.size(), " tests")
    End Sub

    Private Shared Function [select](ByVal selector As vector(Of String), ByVal c As case_info) As Boolean
        If selector.null_or_empty() Then
            Return True
        End If
        assert(Not c Is Nothing)
        For Each n As String In {c.name(), c.full_name(), c.assembly_qualified_name()}
            Dim r As Byte = fit_patterns(n, selector)
            If r = pattern_match.fit_true Then
                Return True
            ElseIf r = pattern_match.fit_false Then
                Return False
            End If
        Next
        Return False
    End Function

    Public Shared Function run(ByVal selector As vector(Of String)) As Int32
        Dim r As Int32 = 0
        cases.stream().foreach(Sub(ByVal x As case_info)
                                   x.finished = Not [select](selector, x)
                                   If Not x.finished Then
                                       r += 1
                                   End If
                               End Sub)
        host.run()
        Return r
    End Function

    Private Sub New()
    End Sub
End Class
