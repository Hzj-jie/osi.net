
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.selector

Public Class async_thread_safe_lazier_test
    Inherits rinne_case_wrapper

    Private Const procedure_count As Int16 = 32

    Public Sub New()
        MyBase.New(multi_procedure(New async_thread_safe_lazier_case(), procedure_count), 65536)
    End Sub

    Public Overrides Function reserved_processors() As Int16
        Return procedure_count
    End Function

    Private Class async_thread_safe_lazier_case
        Inherits event_comb_case

        Private l As async_thread_safe_lazier(Of String)
        Private lock As slimlock.monitorlock
        Private f As String

        Public Overrides Function prepare() As Boolean
            l = New async_thread_safe_lazier(Of String)(Function(r As pointer(Of String)) As event_comb
                                                            Return sync_async(Sub()
                                                                                  eva(r, guid_str())
                                                                              End Sub)
                                                        End Function)
            f = Nothing
            Return MyBase.prepare()
        End Function

        Public Overrides Function create() As event_comb
            assert(Not l Is Nothing)
            Dim s As pointer(Of String) = Nothing
            Dim ec As event_comb = Nothing
            Return New event_comb(Function() As Boolean
                                      s = New pointer(Of String)()
                                      ec = l.get(s)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End Function,
                                  Function() As Boolean
                                      assertion.is_true(ec.end_result())
                                      assertion.is_not_null(+s)
                                      lock.locked(Sub()
                                                      If f Is Nothing Then
                                                          f = +s
                                                      End If
                                                  End Sub)
                                      assertion.equal(+s, f)
                                      Return goto_end()
                                  End Function)
        End Function
    End Class
End Class
