
Imports System.Threading
Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.selector

Public Class sync_thread_safe_lazier_test
    Inherits rinne_case_wrapper

    Public Sub New()
        MyBase.New(multithreading(New sync_thread_safe_lazier_case(), 32), 2048)
    End Sub

    Public Overrides Function reserved_processors() As Int16
        Return 1
    End Function

    Private Class sync_thread_safe_lazier_case
        Inherits [case]

        Private l As sync_thread_safe_lazier(Of String)
        Private lock As slimlock.monitorlock
        Private f As String

        Public Overrides Function prepare() As Boolean
            l = New sync_thread_safe_lazier(Of String)(Function() As String
                                                           Return guid_str()
                                                       End Function)
            f = Nothing
            Return MyBase.prepare()
        End Function

        Public Overrides Function run() As Boolean
            assert(Not l Is Nothing)
            Dim s As String = Nothing
            s = l.get()
            lock.locked(Sub()
                            If f Is Nothing Then
                                f = s
                            End If
                        End Sub)
            assertion.equal(s, f)
            Return True
        End Function
    End Class
End Class
