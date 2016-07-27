
Imports System.Runtime.CompilerServices
Imports System.Text

Public Module _stringbuilder
    <Extension()> Public Sub clear(ByVal this As StringBuilder)
        If Not this Is Nothing Then
            this.Length() = 0
        End If
    End Sub

    <Extension()> Public Sub shrink_to_fit(ByVal this As StringBuilder)
        If Not this Is Nothing Then
            this.Capacity() = this.Length()
        End If
    End Sub

    <Extension()> Public Function empty(ByVal this As StringBuilder) As Boolean
        Return this Is Nothing OrElse this.Length() = 0
    End Function
End Module
