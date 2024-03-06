
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class promise
    Partial Private Class thenable
        Private Enum status_t
            pending
            fulfilled
            rejected
        End Enum

        Private status As status_t
        Private [next] As const_pair(Of Action(Of Object), Action(Of Object))
        Private result As Object
        Private reason As Object

        Public Sub New()
            trace_start()
        End Sub

        Public Function pending() As Boolean
            Return status = status.pending
        End Function

        Public Function fulfilled() As Boolean
            Return status = status.fulfilled
        End Function

        Public Function rejected() As Boolean
            Return status = status.rejected
        End Function

        Public Sub resolve(ByVal result As thenable)
            assert(Not result Is Nothing)
            result.then(AddressOf resolve, AddressOf reject)
        End Sub

        Public Sub resolve(ByVal result As Object)
            Dim p As promise = Nothing
            If Not result Is Nothing AndAlso direct_cast(result, p) Then
                p.then(AddressOf resolve, AddressOf reject)
                Return
            End If
            SyncLock Me
                If status <> status_t.pending Then
                    Return
                End If
                status = status_t.fulfilled
                Me.result = result
                execute_next()
            End SyncLock
            trace_stop()
        End Sub

        Public Sub reject(ByVal reason As Object)
            SyncLock Me
                If status <> status_t.pending Then
                    Return
                End If
                status = status_t.rejected
                Me.reason = reason
                execute_next()
            End SyncLock
            trace_stop()
        End Sub

        Public Sub [then](ByVal on_resolve As Action(Of Object), ByVal on_reject As Action(Of Object))
            assert(Not on_resolve Is Nothing)
            assert(Not on_reject Is Nothing)
            SyncLock Me
                assert([next] Is Nothing)
                [next] = const_pair.emplace_of(on_resolve, on_reject)
                execute_next()
            End SyncLock
        End Sub

        Private Sub execute_next()
            If [next] Is Nothing OrElse status = status_t.pending Then
                Return
            End If
            Select Case status
                Case status_t.fulfilled
                    [next].first(result)
                Case status_t.rejected
                    [next].second(reason)
                Case Else
                    assert(False)
            End Select
        End Sub
    End Class
End Class
