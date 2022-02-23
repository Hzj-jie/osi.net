
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class promise
    Partial Private Class thenable
        Private status As status
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
            assert(result IsNot Nothing)
            result.then(AddressOf resolve, AddressOf reject)
        End Sub

        Public Sub resolve(ByVal result As Object)
            Dim p As promise = Nothing
            If result IsNot Nothing AndAlso direct_cast(result, p) Then
                p.then(AddressOf resolve, AddressOf reject)
                Return
            End If
            SyncLock Me
                If status <> status.pending Then
                    Return
                End If
                status = status.fulfilled
                Me.result = result
                execute_next()
                trace_stop()
            End SyncLock
        End Sub

        Public Sub reject(ByVal reason As Object)
            SyncLock Me
                If status <> status.pending Then
                    Return
                End If
                Me.reason = reason
                status = status.rejected
                execute_next()
                trace_stop()
            End SyncLock
        End Sub

        Public Sub [then](ByVal on_resolve As Action(Of Object), ByVal on_reject As Action(Of Object))
            SyncLock Me
                assert(on_resolve IsNot Nothing)
                assert(on_reject IsNot Nothing)
                assert(Me.next Is Nothing)
                [next] = const_pair.emplace_of(on_resolve, on_reject)
                execute_next()
            End SyncLock
        End Sub

        Private Sub execute_next()
            If [next] Is Nothing Then
                Return
            End If
            If status = status.fulfilled Then
                [next].first(result)
            ElseIf status = status.rejected Then
                [next].second(reason)
            End If
        End Sub
    End Class
End Class
