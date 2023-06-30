
Imports System.Runtime.CompilerServices
Imports osi.root.connector

Friend Module _signal_case
    Private Function valid_signal_action(ByVal a As action) As Boolean
        Return a = action.exit
    End Function

    Private Function valid_signal_meta(ByVal a As action, ByVal m() As Byte) As Boolean
        Return isemptyarray(m)
    End Function

    <Extension()> Public Function valid_signal_case(ByVal c As [case]) As Boolean
        Return Not c Is Nothing AndAlso
               c.mode = mode.signal AndAlso
               valid_signal_action(c.action) AndAlso
               valid_signal_meta(c.action, c.meta)
    End Function
End Module
