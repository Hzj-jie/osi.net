
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.envs

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
                                             If promise_trace Then
                                                 log_unhandled_exception(ex)
                                             End If
                                             reject(ex)
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
                                             If promise_trace Then
                                                 log_unhandled_exception(ex)
                                             End If
                                             reject(ex)
                                         End Try
                                     End Sub)
                           End Sub)
    End Function

    Public Shared Widening Operator CType(ByVal this As event_comb) As promise
        If this Is Nothing Then
            Return Nothing
        End If
        Return New promise(Sub(ByVal resolve As Action(Of Object), ByVal reject As Action(Of Object))
                               assert_begin(New event_comb(Function() As Boolean
                                                               Return waitfor(this) AndAlso
                                                                      goto_next()
                                                           End Function,
                                                           Function() As Boolean
                                                               If this.end_result() Then
                                                                   resolve(Nothing)
                                                               Else
                                                                   reject(Nothing)
                                                               End If
                                                               Return goto_end()
                                                           End Function))
                           End Sub)
    End Operator
End Class
