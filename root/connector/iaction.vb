
Imports System.Runtime.CompilerServices
Imports osi.root.delegates

Public Module _iaction
    <Extension()> Public Function as_action(ByVal i As iaction) As Action
        If i Is Nothing Then
            Return Nothing
        Else
            Return Sub()
                       If i.valid() Then
                           i.run()
                       End If
                   End Sub
        End If
    End Function

    <Extension()> Public Function as_iaction(ByVal i As Action) As iaction
        If i Is Nothing Then
            Return Nothing
        Else
            Return New action_adapter(i)
        End If
    End Function
End Module
