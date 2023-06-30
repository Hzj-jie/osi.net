
Imports System.Runtime.CompilerServices
Imports osi.root.delegates
Imports osi.root.connector

Public Interface attachable_event
    ' Attach one action in the event, this action may be execued immediately. Return false if i is nothing.
    Function attach(ByVal i As iaction) As Boolean
    ' Return true if current event is in ON state, i.e. actions send to attach calls will be executed immediately.
    Function marked() As Boolean
End Interface

Public Module _attachable_event
    <Extension()> Public Function attach(ByVal this As attachable_event, ByVal i As Action) As Boolean
        If this Is Nothing Then
            Return False
        Else
            Return this.attach(i.as_iaction())
        End If
    End Function
End Module