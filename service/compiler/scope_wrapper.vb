
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

' TODO: Remove
Public Class scope_wrapper(Of T As scope(Of T))
    Implements IDisposable

    Protected ReadOnly new_scope As T

    Protected Sub New(ByVal scope As T)
        assert(Not scope Is Nothing)
        new_scope = scope.start_scope()
    End Sub

    Public Function scope() As T
        Return new_scope
    End Function

    Protected Overridable Sub when_dispose()
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        when_dispose()
        new_scope.end_scope()
    End Sub
End Class
