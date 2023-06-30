
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports System.Net
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.transmitter
Imports Text = System.Text

Public Module _rr
    Public Sub set_chunked_transfer(ByVal x As HttpWebRequest)
        assert(Not x Is Nothing)
        x.SendChunked() = True
        ' x.TransferEncoding() = constants.headers.values.transfer_encoding.chunked
    End Sub

    Public Sub set_content_length(ByVal x As HttpWebRequest, ByVal l As Int64)
        assert(Not x Is Nothing)
        x.ContentLength() = l
    End Sub

    Public Function fetch_stream(ByVal x As HttpWebRequest, ByVal os As ref(Of Stream)) As event_comb
        assert(Not x Is Nothing)
        Return x.get_request_stream(os)
    End Function

    Public Function fetch_headers(ByVal x As HttpWebResponse) As WebHeaderCollection
        assert(Not x Is Nothing)
        Return x.Headers()
    End Function

    Public Function fetch_stream(ByVal x As HttpWebResponse) As Stream
        assert(Not x Is Nothing)
        Return x.GetResponseStream()
    End Function

    Public Function get_content_length(ByVal x As HttpWebResponse) As Int64
        assert(Not x Is Nothing)
        Return x.ContentLength()
    End Function


    Public Sub set_chunked_transfer(ByVal x As HttpListenerResponse)
        assert(Not x Is Nothing)
        x.SendChunked() = True
    End Sub

    Public Sub set_content_length(ByVal x As HttpListenerResponse, ByVal l As Int64)
        assert(Not x Is Nothing)
        x.ContentLength64() = l
    End Sub

    Public Function fetch_stream(ByVal x As HttpListenerResponse, ByVal os As ref(Of Stream)) As event_comb
        assert(Not x Is Nothing)
        Return sync_async(Sub()
                              eva(os, x.OutputStream())
                          End Sub)
    End Function

    Public Function fetch_headers(ByVal x As HttpListenerRequest) As WebHeaderCollection
        assert(Not x Is Nothing)
        Return cast(Of WebHeaderCollection)(x.Headers())
    End Function

    Public Function fetch_stream(ByVal x As HttpListenerRequest) As Stream
        assert(Not x Is Nothing)
        Return x.InputStream()
    End Function

    Public Function get_content_length(ByVal x As HttpListenerRequest) As Int64
        assert(Not x Is Nothing)
        Return x.ContentLength64()
    End Function


    Public Function write_to_string(Of T)(ByVal i As T,
                                          ByVal p As ref(Of String),
                                          ByVal enc As Text.Encoding,
                                          ByVal buff_size As UInt32,
                                          ByVal receive_rate_sec As UInt32,
                                          ByVal max_content_length As UInt64,
                                          ByVal fetch_headers As Func(Of T, WebHeaderCollection),
                                          ByVal fetch_stream As Func(Of T, Stream),
                                          ByVal get_content_length As Func(Of T, Int64)) As event_comb
        Dim r As ref(Of Byte()) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  r = New ref(Of Byte())()
                                  ec = write_to_bytes(i,
                                                      r,
                                                      buff_size,
                                                      receive_rate_sec,
                                                      max_content_length,
                                                      fetch_headers,
                                                      fetch_stream,
                                                      get_content_length)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         eva(p, If(enc Is Nothing, default_encoding, enc).GetString(+r)) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function write_to_bytes(Of T)(ByVal i As T,
                                         ByVal p As ref(Of Byte()),
                                         ByVal buff_size As UInt32,
                                         ByVal receive_rate_sec As UInt32,
                                         ByVal max_content_length As UInt64,
                                         ByVal fetch_headers As Func(Of T, WebHeaderCollection),
                                         ByVal fetch_stream As Func(Of T, Stream),
                                         ByVal get_content_length As Func(Of T, Int64)) As event_comb
        Dim o As MemoryStream = Nothing
        Dim r As ref(Of UInt64) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  o = New MemoryStream()
                                  r = New ref(Of UInt64)()
                                  ec = write_to_stream(i,
                                                       o,
                                                       buff_size,
                                                       receive_rate_sec,
                                                       uint32_0,
                                                       max_content_length,
                                                       r,
                                                       fetch_headers,
                                                       fetch_stream,
                                                       get_content_length)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         assert((+r) = o.Length()) AndAlso
                                         eva(p, o.fit_buffer()) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function write_to_stream(Of T)(ByVal i As T,
                                          ByVal o As Stream,
                                          ByVal buff_size As UInt32,
                                          ByVal receive_rate_sec As UInt32,
                                          ByVal send_rate_sec As UInt32,
                                          ByVal max_content_length As UInt64,
                                          ByVal result As ref(Of UInt64),
                                          ByVal fetch_headers As Func(Of T, WebHeaderCollection),
                                          ByVal fetch_stream As Func(Of T, Stream),
                                          ByVal get_content_length As Func(Of T, Int64)) As event_comb
        assert(Not fetch_headers Is Nothing)
        assert(Not fetch_stream Is Nothing)
        assert(Not get_content_length Is Nothing)
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If i Is Nothing OrElse o Is Nothing Then
                                      Return False
                                  Else
                                      Dim chunked_transfer As Boolean = False
                                      chunked_transfer = fetch_headers(i).chunked_transfer()
                                      If chunked_transfer Then
                                          ec = fetch_stream(i).copy_to(o,
                                                                       buff_size,
                                                                       receive_rate_sec,
                                                                       send_rate_sec,
                                                                       result:=result)
                                      Else
                                          Dim content_length As Int64 = 0
                                          content_length = get_content_length(i)
                                          If content_length < 0 Then
                                              Return False
                                          ElseIf content_length = 0 Then
                                              Return goto_end()
                                          Else
                                              If max_content_length = 0 OrElse
                                                 content_length <= max_content_length Then
                                                  ec = fetch_stream(i).copy_to(o,
                                                                               CULng(content_length),
                                                                               buff_size,
                                                                               receive_rate_sec,
                                                                               send_rate_sec)
                                                  eva(result, CULng(content_length))
                                              End If
                                          End If
                                      End If
                                      assert(Not ec Is Nothing)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function send(Of T)(ByVal o As T,
                               ByVal count As UInt64,
                               ByVal set_content_length As Action(Of T, Int64),
                               ByVal fetch_stream As Func(Of T, ref(Of Stream), event_comb),
                               ByVal send_to As Func(Of Stream, UInt64, event_comb)) As event_comb
        assert(Not set_content_length Is Nothing)
        assert(Not fetch_stream Is Nothing)
        assert(Not send_to Is Nothing)
        Dim ec As event_comb = Nothing
        Dim os As ref(Of Stream) = Nothing
        Return New event_comb(Function() As Boolean
                                  If o Is Nothing Then
                                      Return False
                                  Else
                                      set_content_length(o, CLng(count))
                                      If count = 0 Then
                                          Return goto_end()
                                      Else
                                          os = New ref(Of Stream)()
                                          ec = fetch_stream(o, os)
                                          Return waitfor(ec) AndAlso
                                                 goto_next()
                                      End If
                                  End If
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() AndAlso
                                     Not (+os) Is Nothing Then
                                      ec = send_to(+os, count)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function read_from_string(Of T)(ByVal i As String,
                                           ByVal offset As UInt32,
                                           ByVal count As UInt32,
                                           ByVal enc As Text.Encoding,
                                           ByVal o As T,
                                           ByVal send_rate_sec As UInt32,
                                           ByVal set_content_length As Action(Of T, Int64),
                                           ByVal fetch_stream As Func(Of T, ref(Of Stream), event_comb)) _
                                          As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  Dim b() As Byte = Nothing
                                  If Not i Is Nothing Then
                                      If enc Is Nothing Then
                                          enc = default_encoding
                                      End If
                                      b = enc.GetBytes(i, CInt(offset), CInt(count))
                                  End If
                                  ec = read_from_bytes(b,
                                                       uint32_0,
                                                       array_size(b),
                                                       o,
                                                       send_rate_sec,
                                                       set_content_length,
                                                       fetch_stream)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function read_from_bytes(Of T)(ByVal i() As Byte,
                                          ByVal offset As UInt32,
                                          ByVal count As UInt64,
                                          ByVal o As T,
                                          ByVal send_rate_sec As UInt32,
                                          ByVal set_content_length As Action(Of T, Int64),
                                          ByVal fetch_stream As Func(Of T, ref(Of Stream), event_comb)) _
                                         As event_comb
        Return send(o,
                    count,
                    set_content_length,
                    fetch_stream,
                    Function(ByVal x As Stream, ByVal l As UInt64) As event_comb
                        Return x.send(i,
                                      offset,
                                      CUInt(l),
                                      send_rate_sec,
                                      True)
                    End Function)
    End Function

    Public Function read_from_stream(Of T)(ByVal i As Stream,
                                           ByVal o As T,
                                           ByVal count As UInt64,
                                           ByVal buff_size As UInt32,
                                           ByVal receive_rate_sec As UInt32,
                                           ByVal send_rate_sec As UInt32,
                                           ByVal set_content_length As Action(Of T, Int64),
                                           ByVal fetch_stream As Func(Of T, ref(Of Stream), event_comb),
                                           ByVal close_input_stream As Boolean) As event_comb
        Return send(o,
                    count,
                    set_content_length,
                    fetch_stream,
                    Function(ByVal x As Stream, ByVal l As UInt64) As event_comb
                        Return i.copy_to(x,
                                         l,
                                         buff_size,
                                         receive_rate_sec,
                                         send_rate_sec,
                                         close_input_stream,
                                         True)
                    End Function)
    End Function

    Public Function read_from_stream(Of T)(ByVal i As Stream,
                                           ByVal o As T,
                                           ByVal buff_size As UInt32,
                                           ByVal receive_rate_sec As UInt32,
                                           ByVal send_rate_sec As UInt32,
                                           ByVal set_chunked_transfer As Action(Of T),
                                           ByVal fetch_stream As Func(Of T, ref(Of Stream), event_comb),
                                           ByVal result As ref(Of UInt64),
                                           ByVal close_input_stream As Boolean) As event_comb
        assert(Not set_chunked_transfer Is Nothing)
        assert(Not fetch_stream Is Nothing)
        Dim ec As event_comb = Nothing
        Dim os As ref(Of Stream) = Nothing
        Return New event_comb(Function() As Boolean
                                  If i Is Nothing OrElse o Is Nothing Then
                                      Return False
                                  Else
                                      set_chunked_transfer(o)
                                      os = New ref(Of Stream)()
                                      ec = fetch_stream(o, os)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() AndAlso
                                     Not (+os) Is Nothing Then
                                      ec = i.copy_to(+os,
                                                     buff_size,
                                                     receive_rate_sec,
                                                     send_rate_sec,
                                                     close_input_stream,
                                                     True,
                                                     result)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function
End Module

