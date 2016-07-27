
Imports System.Runtime.CompilerServices
Imports osi.root.formation
Imports osi.root.connector
Imports osi.root.utils
Imports osi.service.convertor

Public Module _command_bytes
    <Extension()> Public Function to_bytes(ByVal this As command) As Byte()
        If this Is Nothing Then
            Return Nothing
        Else
            Dim v As vector(Of Byte()) = Nothing
            v = New vector(Of Byte())()
            v.emplace_back(this.action())
            assert(this.foreach(Sub(x, y)
                                    v.emplace_back(x)
                                    v.emplace_back(y)
                                End Sub))
            Return v.to_bytes()
        End If
    End Function

    <Extension()> Public Function from_bytes(ByVal this As command, ByVal b() As Byte) As Boolean
        If this Is Nothing OrElse isemptyarray(b) Then
            Return False
        Else
            Dim v As vector(Of Byte()) = Nothing
            v = b.to_vector_bytes()
            If v Is Nothing OrElse ((v.size() And 1) <> 1) Then
                Return False
            Else
                this.clear()
                this.set_action_no_copy(v(0))
                For i As Int64 = 1 To v.size() - 1 Step 2
                    this.set_parameter_no_copy(v(i), v(i + 1))
                Next
                Return True
            End If
        End If
    End Function
End Module
