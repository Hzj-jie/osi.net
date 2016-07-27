
Imports osi.root.delegates
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.template

Friend Class event_queue(Of PARA_T, _ONCE As _boolean)
    Private Shared ReadOnly attach_after As Boolean

    Shared Sub New()
        attach_after = Not (+(alloc(Of _ONCE)()))
    End Sub

    Private ReadOnly q As qless2(Of iparameter_action(Of PARA_T))

    Public Sub New()
        q = New qless2(Of iparameter_action(Of PARA_T))()
    End Sub

    Public Function attach(ByVal v As iparameter_action(Of PARA_T)) As Boolean
        If Not v Is Nothing AndAlso v.valid() Then
            q.push(v)
            Return True
        Else
            Return False
        End If
    End Function

    Public Function attached_count() As UInt32
        Return q.size()
    End Function

    Public Function attached() As Boolean
        Return Not q.empty()
    End Function

    Public Sub raise(ByVal p As PARA_T)
        Dim s As Int64 = 0
        s = q.size()
        While s > 0
            assert(raise_one(p))
            s -= 1
        End While
    End Sub

    Public Function raise_one(ByVal p As PARA_T) As Boolean
        Dim w As iparameter_action(Of PARA_T) = Nothing
        If q.pop(w) Then
            assert(Not w Is Nothing)
            If w.valid() Then
                w.run(p)
                If attach_after Then
                    attach(w)
                End If
            End If
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub clear()
        q.clear()
    End Sub
End Class
