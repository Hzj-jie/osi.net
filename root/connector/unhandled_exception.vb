
Imports osi.root.constants

Public Module _unhandled_exception
    Public Sub log_unhandled_exception(ByVal prefix As String, ByVal ex As Exception)
        If Not ex Is Nothing Then
            log_unhandled_exception(prefix, ex.InnerException())
            raise_error(error_type.critical,
                        prefix,
                        ", type ",
                        ex.GetType().FullName(),
                        ", source ",
                        ex.Source(),
                        ", message ",
                        ex.Message(),
                        ", stacktrace ",
                        ex.StackTrace())
        End If
    End Sub

    Public Sub log_unhandled_exception(ByVal ex As Exception)
        log_unhandled_exception("caught unhandled exception", ex)
    End Sub
End Module
