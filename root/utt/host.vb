
Imports System.IO
Imports System.Reflection
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.formation
Imports osi.root.utils
Imports osi.root.constants.filesystem
Imports osi.root.delegates

Friend Module host
    Private Class loader
        Private Shared Function accepted_type(ByVal j As Type) As Boolean
            assert(Not j Is Nothing)
            Return Not j.IsAbstract() AndAlso
                   Not j.IsGenericType() AndAlso
                   j.IsPublic() AndAlso
                   j.inherit(Of [case])() AndAlso
                   j.has_parameterless_public_constructor()
        End Function

        ' This function cannot be placed in host module. It depends on function of host, which will trigger a deadlock.
        Public Shared Sub load(ByVal cases As vector(Of case_info))
            ' Cannot use event_comb, allocators of some cases may use async_sync.
            assert(Not cases Is Nothing)
            concurrency_runner.execute(Sub(i As Assembly)
                                           For Each j In i.GetTypes()
                                               If accepted_type(j) Then
                                                   Dim n As case_info = Nothing
                                                   n = New case_info(j.FullName(), alloc(Of [case])(j))
                                                   SyncLock cases
                                                       cases.emplace_back(n)
                                                   End SyncLock
                                                   raise_error("loaded case ", j.FullName())
                                               End If
                                           Next
                                       End Sub,
                                       AppDomain.CurrentDomain().GetAssemblies())
        End Sub

        Private Sub New()
        End Sub
    End Class

    Public ReadOnly cases As vector(Of case_info) = Nothing

    Sub New()
        assert(Not strstartwith(extensions.dynamic_link_library, extension_prefix, False))
        cases = New vector(Of case_info)()
        AppDomain.CurrentDomain().load_all(Environment.CurrentDirectory(), "osi.*.dll")
        If Not Environment.CurrentDirectory().path_same(application_directory) Then
            AppDomain.CurrentDomain().load_all(application_directory, "osi.*.dll")
        End If
        ' for newly loaded types
        global_init.execute()
        loader.load(cases)
        assert(cases.size() > 0)
    End Sub

    Private Function [select](ByVal selector As vector(Of String), ByVal c As case_info) As Boolean
        If selector Is Nothing OrElse selector.empty() Then
            Return True
        Else
            assert(Not c Is Nothing)
            Dim has_fit_true As Boolean = False
            Dim r As Byte = 0
            r = match_all(c.name(), selector)
            If r = stringfilter.fit_true Then
                has_fit_true = True
            ElseIf r = stringfilter.fit_false Then
                Return False
            End If

            r = match_all(c.full_name(), selector)
            If r = stringfilter.fit_true Then
                has_fit_true = True
            ElseIf r = stringfilter.fit_false Then
                Return False
            End If

            r = match_all(c.assembly_qualified_name(), selector)
            If r = stringfilter.fit_true Then
                has_fit_true = True
            ElseIf r = stringfilter.fit_false Then
                Return False
            End If

            Return has_fit_true
        End If
    End Function

    Public Function foreach(ByVal d As _do(Of case_info, Boolean, Boolean)) As Boolean
        If d Is Nothing Then
            Return False
        Else
            Return cases.foreach(d)
        End If
    End Function

    Public Function foreach(ByVal d As _do(Of case_info, Boolean)) As Boolean
        Return utils.foreach(AddressOf foreach, d)
    End Function

    Public Function foreach(ByVal d As void(Of case_info)) As Boolean
        Return utils.foreach(AddressOf foreach, d)
    End Function

    Public Sub clear_selection()
        foreach(Sub(ByRef x) x.finished = False)
    End Sub

    Public Function run(Optional ByVal selector As vector(Of String) = Nothing) As Int32
        Dim r As Int32 = 0
        clear_selection()
        foreach(Sub(ByRef x)
                    If [select](selector, x) Then
                        r += 1
                    Else
                        x.finished = True
                    End If
                End Sub)

        host_run.run()
        Return r
    End Function
End Module
