
Option Explicit On
Option Infer Off
Option Strict On

Public NotInheritable Class counter_event
    Public Shared Event write_counter(ByVal name As String, ByVal count As Int64)
    Public Shared Event increase_counter(ByVal id As Int64, ByVal c As Int64)
    Public Shared Event decrease_counter(ByVal id As Int64, ByVal c As Int64)
    Public Shared Event change_counter(ByVal id As Int64, ByVal c As Int64)

    Public Shared Sub raise_write_counter(ByVal name As String, ByVal count As Int64)
        RaiseEvent write_counter(name, count)
    End Sub

    Public Shared Sub raise_increase_counter(ByVal id As Int64, ByVal c As Int64)
        RaiseEvent increase_counter(id, c)
        RaiseEvent change_counter(id, c)
    End Sub

    Public Shared Sub raise_decrease_counter(ByVal id As Int64, ByVal c As Int64)
        RaiseEvent decrease_counter(id, c)
        RaiseEvent change_counter(id, -c)
    End Sub

    Private Sub New()
    End Sub
End Class