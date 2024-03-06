
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils

Partial Public NotInheritable Class promise
    Partial Private Class thenable
        Private status As status
        Private [next] As const_pair(Of Action(Of Object), Action(Of Object))
        Private result As Object
        Private reason As Object

        Private Shared Sub trace_start()
            counter.instance_count_counter(Of promise).alloc()
        End Sub

        Private Shared Sub trace_stop()
            counter.instance_count_counter(Of promise).dealloc()
        End Sub

        Public Sub New()
            trace_start()
        End Sub

        Public Sub resolve(ByVal result As Object)
            Dim p As promise = Nothing
            If Not result Is Nothing AndAlso direct_cast(result, p) Then
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
                assert(Not on_resolve Is Nothing)
                assert(Not on_reject Is Nothing)
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

    Private NotInheritable Class auto
        Inherits thenable

        Public Sub New(ByVal executor As Action(Of Action(Of Object), Action(Of Object)))
            MyBase.New()
            assert(Not executor Is Nothing)
            Try
                executor(AddressOf resolve, AddressOf reject)
            Catch ex As Exception
                log_unhandled_exception(ex)
                reject(ex)
            End Try
        End Sub
    End Class
End Class
