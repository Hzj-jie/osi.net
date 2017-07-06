
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
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
            assert(Not result Is Nothing)
            result.then(AddressOf resolve, AddressOf reject)
        End Sub

        Public Sub resolve(ByVal result As Object)
            Dim p As promise = Nothing
            If Not result Is Nothing AndAlso direct_cast(result, p) Then
                p.then(AddressOf resolve, AddressOf reject)
            Else
                SyncLock Me
                    If status = status.pending Then
                        status = status.fulfilled
                        Me.result = result
                        execute_next()
                        trace_stop()
                    End If
                End SyncLock
            End If
        End Sub

        <MethodImpl(MethodImplOptions.Synchronized)>
        Public Sub reject(ByVal reason As Object)
            If status = status.pending Then
                Me.reason = reason
                status = status.rejected
                execute_next()
                trace_stop()
            End If
        End Sub

        <MethodImpl(MethodImplOptions.Synchronized)>
        Public Sub [then](ByVal on_resolve As Action(Of Object), ByVal on_reject As Action(Of Object))
            assert(Not on_resolve Is Nothing)
            assert(Not on_reject Is Nothing)
            assert(Me.next Is Nothing)
            Me.next = emplace_make_const_pair(on_resolve, on_reject)
            execute_next()
        End Sub

        Private Sub execute_next()
            If Not [next] Is Nothing Then
                If status = status.fulfilled Then
                    [next].first(result)
                ElseIf status = status.rejected Then
                    [next].second(reason)
                End If
            End If
        End Sub
    End Class
End Class
