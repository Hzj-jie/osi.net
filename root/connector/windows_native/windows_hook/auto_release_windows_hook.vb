
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants

Public NotInheritable Class auto_release_windows_hook
    Implements IDisposable

    Private ReadOnly hook As IntPtr
    ' Keep a reference to the original callback to ensure it won't be GCed.
    Private ReadOnly h As windows_hook.HOOKPROC
    Private r As Boolean

    Public Sub New(ByVal hook As IntPtr, ByVal h As windows_hook.HOOKPROC)
        Me.hook = hook
        If Me.hook = IntPtr.Zero Then
            raise_error(error_type.warning, "Invalid hook received, last error ", windows_native_error.get())
        End If
        Me.h = h
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        r = windows_hook.remove(hook)
    End Sub

    Public Function installed() As Boolean
        Return hook <> IntPtr.Zero
    End Function

    Public Function removed() As Boolean
        Return r
    End Function
End Class
