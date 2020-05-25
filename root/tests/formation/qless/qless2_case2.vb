
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with qless2_case2.vbp ----------
'so change qless2_case2.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with qless_case2.vbp ----------
'so change qless_case2.vbp instead of this file



Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.utt

Friend Class qless2_case2
    Inherits [case]

    Private Const max As Int64 = 1024 * 1024 * 32
    Private ReadOnly q As qless2(Of Int64)

    Public Sub New()
        q = New qless2(Of Int64)()
    End Sub

    Private Sub push_thread(ByVal finished As pointer(Of singleentry))
        assert(Not finished Is Nothing)
        For i As Int64 = -max To max
#If False Then
            If Not q.push(i) Then
                i -= 1
            End If
#Else
            q.push(i)
#End If
            If Environment.ProcessorCount() > 1 Then
                sleep(rnd_int(-1, 1))
            End If
        Next
        finished.mark_in_use()
    End Sub

    Private Sub pop_thread(ByVal finished As pointer(Of singleentry))
        assert(Not finished Is Nothing)
        While Not finished.in_use()
            If Not q.pop(Nothing) Then
                sleep(rnd_int(-1, 2))
            End If
        End While
    End Sub

    Public Overrides Function reserved_processors() As Int16
        Return 2
    End Function

    Public Overrides Function run() As Boolean
        Dim finished As pointer(Of singleentry) = Nothing
        finished = New pointer(Of singleentry)()
        queue_in_managed_threadpool(Sub() push_thread(finished))
        queue_in_managed_threadpool(Sub() pop_thread(finished))
        While Not finished.in_use()
            Const max_checked As Int32 = 2000
            Dim s As Int64 = 0
            s = q.size()
            s = If(s > max_checked, max_checked, s)
            Dim m As [set](Of Int64) = Nothing
            m = New [set](Of Int64)()
            While s > 0
                Dim v As Int64 = 0
                If q.pop(v) Then
                    assertion.equal(m.find(v), m.end(), v)
                    m.emplace(v)
                End If
                s -= 1
            End While
            If q.size() >= max_checked Then
                q.clear()
            End If
        End While
        Return True
    End Function
End Class
'finish qless_case2.vbp --------
'finish qless2_case2.vbp --------
