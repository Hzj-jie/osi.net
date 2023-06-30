
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.procedure
Imports osi.root.threadpool
Imports osi.service.configuration
Imports osi.service.http

Public Module http_proxy
    Sub New()
        enable_domain_unhandled_exception_handler()
        register_slimqless2_threadpool()
    End Sub

    Private Sub load_configuration(ByVal args() As String)
        Dim f As String = Nothing
        If isemptyarray(args) Then
            f = "http_proxy.ini"
        Else
            f = args(0)
        End If
        assert_load(f)
    End Sub

    Public Sub main(ByVal args() As String)

    End Sub
End Module
