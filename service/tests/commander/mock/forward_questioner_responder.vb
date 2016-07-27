
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.template
Imports osi.service.commander

Public Class forward_questioner_responder
    Private ReadOnly send_herald() As herald
    Private ReadOnly receive_herald() As herald

    Public Sub New(Optional ByVal count As UInt32 = uint32_1)
        assert(count > uint32_0)
        ReDim send_herald(count - uint32_1)
        ReDim receive_herald(count - uint32_1)
        For i As UInt32 = uint32_0 To count - uint32_1
            Dim mh As mock_herald = Nothing
            mh = New mock_herald()
            send_herald(i) = mh
            receive_herald(i) = mh.the_other_end()
        Next
    End Sub

    Public Sub reset(ByVal id As UInt32)
        assert(id >= uint32_0 AndAlso id < array_size(send_herald))
        DirectCast(send_herald(id), mock_herald).clear()
    End Sub

    Public Function sender(ByVal id As UInt32) As herald
        assert(id >= uint32_0 AndAlso id < array_size(send_herald))
        Return send_herald(id)
    End Function

    Public Function questioner(ByVal id As UInt32) As iquestioner
        Return New herald_questioner(sender(id), npos)
    End Function

    Public Function receiver(ByVal id As UInt32) As herald
        assert(id >= uint32_0 AndAlso id < array_size(receive_herald))
        Return receive_herald(id)
    End Function

    Public Function responder(Of C As _boolean)(ByVal id As UInt32, ByVal e As executor) As iresponder(Of C)
        Return New herald_responder(Of C)(receiver(id), npos, e)
    End Function
End Class
