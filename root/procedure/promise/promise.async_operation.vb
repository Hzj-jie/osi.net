
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public NotInheritable Class promise
    Public Shared Function [New](Of T)(ByVal begin As Func(Of AsyncCallback, IAsyncResult),
                                       ByVal [end] As Func(Of IAsyncResult, T)) As promise
        assert(Not begin Is Nothing)
        assert(Not [end] Is Nothing)
        Return New promise(Sub(ByVal resolve As Action(Of Object), ByVal reject As Action(Of Object))
                               begin(Sub(ByVal ar As IAsyncResult)
                                         Try
                                             resolve([end](ar))
                                         Catch ex As Exception
                                             log_unhandled_exception(ex)
                                             reject(Nothing)
                                         End Try
                                     End Sub)
                           End Sub)
    End Function

    Public Shared Function [New](ByVal begin As Func(Of AsyncCallback, IAsyncResult),
                                 ByVal [end] As Action(Of IAsyncResult)) As promise
        assert(Not begin Is Nothing)
        assert(Not [end] Is Nothing)
        Return New promise(Sub(ByVal resolve As Action(Of Object), ByVal reject As Action(Of Object))
                               begin(Sub(ByVal ar As IAsyncResult)
                                         Try
                                             [end](ar)
                                             resolve(Nothing)
                                         Catch ex As Exception
                                             log_unhandled_exception(ex)
                                             reject(Nothing)
                                         End Try
                                     End Sub)
                           End Sub)
    End Function
End Class
