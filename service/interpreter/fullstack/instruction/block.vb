
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.connector

Namespace fullstack.instruction
    Public Class block
        Public Const end_ip As UInt32 = max_uint32
        Private ReadOnly instructions As vector(Of instruction)

        Public Sub New(ByVal instructions As vector(Of instruction))
            assert(Not instructions.null_or_empty())
            Me.instructions = instructions
#If DEBUG Then
            For i As UInt32 = 0 To CUInt(instructions.size() - 1)
                assert(Not instructions(i) Is Nothing)
            Next
#End If
        End Sub

        Public Function execute(ByRef ip As UInt32) As Boolean
            While ip < instructions.size()
                If Not instructions(ip).execute(ip) Then
                    Return False
                End If
            End While
            Return True
        End Function

        Public Function execute() As Boolean
            Dim ip As UInt32 = 0
            Return execute(ip) AndAlso ip = max_uint32
        End Function
    End Class
End Namespace
