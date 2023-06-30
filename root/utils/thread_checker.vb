
Option Explicit On
Option Infer Off
Option Strict Off

Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.lock

Public NotInheritable Class thread_checker
    Private ReadOnly id As atomic_int

    Public Sub New()
        id = New atomic_int(INVALID_THREAD_ID)
    End Sub

    Public Function check() As Boolean
        id.compare_exchange(current_thread_id(), INVALID_THREAD_ID)
        Return (+id) = current_thread_id()
    End Function

    Public Sub assert()
        connector.assert(check())
    End Sub
End Class

Public NotInheritable Class debug_thread_checker
    Private ReadOnly tc As thread_checker

    Public Sub New()
#If DEBUG Then
        tc = New thread_checker()
#Else
        tc = Nothing
#End If
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function check() As Boolean
        If tc Is Nothing Then
            Return True
        End If
        Return tc.check()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub assert()
        If tc Is Nothing Then
            Return
        End If
        tc.assert()
    End Sub
End Class
