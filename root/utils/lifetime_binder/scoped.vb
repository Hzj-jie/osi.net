
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.event
Imports osi.root.lock

Public NotInheritable Class scoped
    Public Shared Function atomic_bool(ByVal i As atomic_bool) As IDisposable
        assert(i IsNot Nothing)
        i.inc()
        Return defer.to(AddressOf i.dec)
    End Function

    Public Shared Function count_event(ByVal i As count_event) As IDisposable
        assert(i IsNot Nothing)
        i.increment()
        Return defer.to(AddressOf i.decrement)
    End Function

    Private Sub New()
    End Sub
End Class
