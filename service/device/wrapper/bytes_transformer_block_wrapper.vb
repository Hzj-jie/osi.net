
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.convertor

Public Class bytes_transformer_block_wrapper
    Implements block

    Public ReadOnly block_dev As block
    Private ReadOnly st As bytes_transformer_collection
    Private ReadOnly rt As bytes_transformer_collection

    Public Sub New(ByVal impl As block)
        assert(Not impl Is Nothing)
        Me.block_dev = impl
        Me.st = New bytes_transformer_collection()
        Me.rt = New bytes_transformer_collection()
    End Sub

    Public Shared Function create(ByVal i As block,
                                  ByVal f As bytes_transformer,
                                  ByVal s As bytes_transformer,
                                  ByRef o As bytes_transformer_block_wrapper) As Boolean
        If i Is Nothing Then
            Return False
        Else
            If Not cast(Of bytes_transformer_block_wrapper)(i, o) Then
                o = New bytes_transformer_block_wrapper(i)
            End If
            assert(Not o Is Nothing)
            Return o.chain(f, s)
        End If
    End Function

    Public Sub clear()
        st.clear()
        rt.clear()
    End Sub

    Public Function empty() As Boolean
        If st.empty() Then
            Return assert(rt.empty())
        Else
            Return assert(Not rt.empty())
        End If
    End Function

    Public Function size() As UInt32
        assert(st.size() = rt.size())
        Return st.size()
    End Function

    Public Function chain(ByVal f As bytes_transformer, ByVal s As bytes_transformer) As Boolean
        Return st.chain(f) AndAlso
               rt.chain(s)
    End Function

    Public Function chain(ByVal v As pair(Of bytes_transformer, bytes_transformer)) As Boolean
        Return Not v Is Nothing AndAlso
               chain(v.first, v.second)
    End Function

    Public Function chain(ByVal ParamArray v() As pair(Of bytes_transformer, bytes_transformer)) As Boolean
        For i As Int32 = 0 To array_size(v) - 1
            If Not chain(v(i)) Then
                Return False
            End If
        Next
        Return True
    End Function

    'test only
    Public Function send_transformer() As bytes_transformer_collection
        Return st
    End Function

    'test only
    Public Function receive_transformer() As bytes_transformer_collection
        Return rt
    End Function

    Public Function send(ByVal buff() As Byte,
                         ByVal offset As UInt32,
                         ByVal count As UInt32) As event_comb Implements block.send
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If Not st.transform_forward(buff, offset, count) Then
                                      Return False
                                  Else
                                      ec = block_dev.send(buff, offset, count)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function receive(ByVal result As pointer(Of Byte())) As event_comb Implements block.receive
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  ec = block_dev.receive(result)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      If result Is Nothing Then
                                          Return goto_end()
                                      Else
                                          Dim o() As Byte = Nothing
                                          Return rt.transform_backward(+result, o) AndAlso
                                                 eva(result, o) AndAlso
                                                 goto_end()
                                      End If
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function

    Public Function sense(ByVal pending As pointer(Of Boolean),
                          ByVal timeout_ms As Int64) As event_comb Implements block.sense
        Return block_dev.sense(pending, timeout_ms)
    End Function
End Class
