
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.root.formation
Imports osi.root.utils

Public Class executor_wrapper
    Implements executor

    Private ReadOnly e As Func(Of command, command, event_comb)

    Public Sub New()
        e = Nothing
    End Sub

    Public Sub New(ByVal a As Func(Of command, command, event_comb))
        e = a
    End Sub

    Public Sub New(ByVal a As Func(Of Byte(), ref(Of Byte()), event_comb))
        If a Is Nothing Then
            e = Nothing
        Else
            e = Function(i As command, o As command) As event_comb
                    Dim ec As event_comb = Nothing
                    Dim p As ref(Of Byte()) = Nothing
                    Return New event_comb(Function() As Boolean
                                              assert(Not i Is Nothing)
                                              assert(Not o Is Nothing)
                                              p = New ref(Of Byte())()
                                              ec = a(i.action(), p)
                                              Return waitfor(ec) AndAlso
                                                     goto_next()
                                          End Function,
                                          Function() As Boolean
                                              If ec.end_result() Then
                                                  o.attach(+p)
                                                  Return goto_end()
                                              Else
                                                  Return False
                                              End If
                                          End Function)
                End Function
        End If
    End Sub

    Public Sub New(ByVal a As Func(Of String, ref(Of String), event_comb))
        If a Is Nothing Then
            e = Nothing
        Else
            e = Function(i As command, o As command) As event_comb
                    Dim ec As event_comb = Nothing
                    Dim p As ref(Of String) = Nothing
                    Return New event_comb(Function() As Boolean
                                              assert(Not i Is Nothing)
                                              assert(Not o Is Nothing)
                                              p = New ref(Of String)()
                                              ec = a(bytes_str(i.action()), p)
                                              Return waitfor(ec) AndAlso
                                                     goto_next()
                                          End Function,
                                          Function() As Boolean
                                              If ec.end_result() Then
                                                  o.attach(str_bytes(+p))
                                                  Return goto_end()
                                              Else
                                                  Return False
                                              End If
                                          End Function)
                End Function
        End If
    End Sub

    Public Shared Widening Operator CType(ByVal this As Func(Of command, command, event_comb)) As executor_wrapper
        Return New executor_wrapper(this)
    End Operator

    Public Shared Widening Operator CType(ByVal this As Func(Of Byte(),
                                                                ref(Of Byte()),
                                                                event_comb)) As executor_wrapper
        Return New executor_wrapper(this)
    End Operator

    Public Shared Widening Operator CType(ByVal this As Func(Of String,
                                                                ref(Of String),
                                                                event_comb)) As executor_wrapper
        Return New executor_wrapper(this)
    End Operator

    Public Function execute(ByVal i As command,
                            ByVal o As command) As event_comb Implements executor.execute
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If i Is Nothing OrElse
                                     o Is Nothing OrElse
                                     e Is Nothing Then
                                      Return False
                                  Else
                                      ec = e(i, o)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function
End Class
