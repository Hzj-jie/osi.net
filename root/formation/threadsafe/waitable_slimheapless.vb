
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with waitable_slimheapless.vbp ----------
'so change waitable_slimheapless.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with waitable_slimqless2.vbp ----------
'so change waitable_slimqless2.vbp instead of this file



Imports System.Threading
Imports osi.root.connector
' count_reset_event is in osi.root.event, which is not included in osi.root.formation.

Public Class waitable_slimheapless(Of T)
    Implements IDisposable

    Private ReadOnly q As slimheapless(Of T)
    Private ReadOnly are As AutoResetEvent

    Public Sub New()
        q = New slimheapless(Of T)()
        are = New AutoResetEvent(False)
    End Sub

    Public Sub push(ByVal v As T)
        emplace(copy_no_error(v))
    End Sub

    Public Sub emplace(ByVal v As T)
        q.emplace(v)
        assert(are.force_set())
    End Sub

    Public Function pop(ByRef o As T) As Boolean
        Return q.pop(o)
    End Function

    Public Function pop() As T
        Return q.pop()
    End Function

    Public Function pick(ByRef o As T) As Boolean
        Return q.pick(o)
    End Function

    Public Function pick() As T
        Return q.pick()
    End Function

    Public Function empty() As Boolean
        Return q.empty()
    End Function

    Public Sub wait()
        assert(are.wait())
    End Sub

    Public Function wait(ByVal ms As Int64) As Boolean
        If ms < 0 Then
            wait()
            Return True
        Else
            Return are.wait(ms)
        End If
    End Function

    Public Sub clear()
        ' If one push happens concurrently, are may be released one more time.
        assert(are.force_reset())
        q.clear()
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        are.Close()
        GC.SuppressFinalize(Me)
    End Sub

    Protected Overrides Sub Finalize()
        are.Close()
        MyBase.Finalize()
    End Sub
End Class

'finish waitable_slimqless2.vbp --------

'finish waitable_slimheapless.vbp --------
