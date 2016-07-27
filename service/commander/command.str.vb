
Imports System.Runtime.CompilerServices
Imports System.Text
Imports osi.root.constants
Imports osi.root.connector

Public Module _command_str
    Private Const separator As Char = character.newline
    Private ReadOnly separators() As Char = {separator}

    Private Function encode(ByVal i() As Byte) As String
        If isemptyarray(i) Then
            Return Nothing
        Else
            Return Convert.ToBase64String(i)
        End If
    End Function

    Private Function decode(ByVal i As String) As Byte()
        If String.IsNullOrEmpty(i) Then
            Return Nothing
        Else
            Return Convert.FromBase64String(i)
        End If
    End Function

    <Extension()> Public Function to_str(ByVal this As command) As String
        If this Is Nothing Then
            Return Nothing
        Else
            Dim o As StringBuilder = Nothing
            o = New StringBuilder()
            o.Append(encode(this.action()))
            this.foreach(Sub(x, y)
                             o.Append(separator) _
                              .Append(encode(x)) _
                              .Append(separator) _
                              .Append(encode(y))
                         End Sub)
            Return Convert.ToString(o)
        End If
    End Function

    <Extension()> Public Function from_str(ByVal this As command, ByVal s As String) As Boolean
        If this Is Nothing OrElse
           String.IsNullOrEmpty(s) Then
            Return False
        Else
            Dim ss() As String = Nothing
            ss = s.Split(separators)
            If (array_size(ss) And 1) <> 1 Then
                Return False
            Else
                this.clear()
                this.set_action_no_copy(decode(ss(0)))
                For i As Int32 = 1 To array_size(ss) - 1 Step 2
                    this.set_parameter_no_copy(decode(ss(i)), decode(ss(i + 1)))
                Next
                Return True
            End If
        End If
    End Function
End Module
