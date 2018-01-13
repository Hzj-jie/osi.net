
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.convertor

Public Module _list
    <Extension()> Public Function push_back(ByVal this As istrkeyvt,
                                            ByVal key As String,
                                            ByVal value() As Byte,
                                            ByVal ts As Int64,
                                            ByVal result As pointer(Of Boolean)) As event_comb
        If this Is Nothing Then
            Return Nothing
        Else
            Return this.append(key, value.to_chunk(), ts, result)
        End If
    End Function

    <Extension()> Public Function push_back(ByVal this As istrkeyvt,
                                            ByVal key As String,
                                            ByVal value As String,
                                            ByVal ts As Int64,
                                            ByVal result As pointer(Of Boolean)) As event_comb
        Return push_back(this, key, str_bytes(value), ts, result)
    End Function

    <Extension()> Public Function push_back(ByVal this As istrkeyvt,
                                            ByVal key As String,
                                            ByVal value() As Byte,
                                            ByVal result As pointer(Of Boolean)) As event_comb
        Return push_back(this, key, value, now_as_timestamp(), result)
    End Function

    <Extension()> Public Function push_back(ByVal this As istrkeyvt,
                                            ByVal key As String,
                                            ByVal value As String,
                                            ByVal result As pointer(Of Boolean)) As event_comb
        Return push_back(this, key, str_bytes(value), result)
    End Function

    Private Function read(ByVal d As Func(Of pointer(Of Byte()), event_comb),
                          ByVal result As pointer(Of vector(Of Byte()))) As event_comb
        assert(Not d Is Nothing)
        Dim ec As event_comb = Nothing
        Dim p As pointer(Of Byte()) = Nothing
        Return New event_comb(Function() As Boolean
                                  p = New pointer(Of Byte())()
                                  ec = d(p)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         Not isemptyarray(+p) AndAlso
                                         eva(result, (+p).to_vector_bytes()) AndAlso
                                         goto_end()
                              End Function)
    End Function

    <Extension()> Public Function read(ByVal this As istrkeyvt,
                                       ByVal key As String,
                                       ByVal result As pointer(Of vector(Of Byte())),
                                       ByVal ts As pointer(Of Int64)) As event_comb
        If this Is Nothing Then
            Return Nothing
        Else
            Return read(Function(p As pointer(Of Byte())) As event_comb
                            Return this.read(key, p, ts)
                        End Function,
                        result)
        End If
    End Function

    <Extension()> Public Function read(ByVal this As istrkeyvt,
                                       ByVal key As String,
                                       ByVal result As pointer(Of vector(Of Byte()))) As event_comb
        If this Is Nothing Then
            Return Nothing
        Else
            Return read(Function(p As pointer(Of Byte())) As event_comb
                            Return this.read(key, p)
                        End Function,
                        result)
        End If
    End Function

    Private Function read(ByVal d As Func(Of pointer(Of vector(Of Byte())), event_comb),
                          ByVal result As pointer(Of vector(Of String))) As event_comb
        assert(Not d Is Nothing)
        Dim ec As event_comb = Nothing
        Dim r As pointer(Of vector(Of Byte())) = Nothing
        Return New event_comb(Function() As Boolean
                                  r = New pointer(Of vector(Of Byte()))()
                                  ec = d(r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Dim vs As vector(Of String) = Nothing
                                  Return ec.end_result() AndAlso
                                         bytes_serializer(Of vector(Of Byte())).default.
                                                 to_container(Of vector(Of String), String)(+r, vs) AndAlso
                                         eva(result, vs) AndAlso
                                         goto_end()
                              End Function)
    End Function

    <Extension()> Public Function read(ByVal this As istrkeyvt,
                                       ByVal key As String,
                                       ByVal result As pointer(Of vector(Of String)),
                                       ByVal ts As pointer(Of Int64)) As event_comb
        Return read(Function(r As pointer(Of vector(Of Byte()))) As event_comb
                        Return read(this, key, r, ts)
                    End Function,
                    result)
    End Function

    <Extension()> Public Function read(ByVal this As istrkeyvt,
                                       ByVal key As String,
                                       ByVal result As pointer(Of vector(Of String))) As event_comb
        Return read(Function(r As pointer(Of vector(Of Byte()))) As event_comb
                        Return read(this, key, r)
                    End Function,
                    result)
    End Function

    <Extension()> Public Function contains(ByVal i As istrkeyvt,
                                           ByVal key As String,
                                           ByVal v() As Byte,
                                           ByVal r As pointer(Of Boolean)) As event_comb
        Dim ec As event_comb = Nothing
        Dim p As pointer(Of Byte()) = Nothing
        Return New event_comb(Function() As Boolean
                                  If i Is Nothing Then
                                      Return False
                                  Else
                                      p = New pointer(Of Byte())()
                                      ec = i.read(key, p)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         Not isemptyarray(+p) AndAlso
                                         eva(r, (+p).contains_chunk(v)) AndAlso
                                         goto_end()
                              End Function)
    End Function

    <Extension()> Public Function contains(ByVal i As istrkeyvt,
                                           ByVal key As String,
                                           ByVal v As String,
                                           ByVal r As pointer(Of Boolean)) As event_comb
        Return contains(i, key, str_bytes(v), r)
    End Function

    <Extension()> Public Function unique_push_back(ByVal i As istrkeyvt,
                                                   ByVal key As String,
                                                   ByVal value() As Byte,
                                                   ByVal r As pointer(Of Boolean)) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  r.renew()
                                  ec = contains(i, key, value, r)
                                  Return ec.end_result() AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() AndAlso
                                     Not (+r) Then
                                      ec = push_back(i, key, value, r)
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

    <Extension()> Public Function unique_push_back(ByVal i As istrkeyvt,
                                                   ByVal key As String,
                                                   ByVal value As String,
                                                   ByVal r As pointer(Of Boolean)) As event_comb
        Return unique_push_back(i, key, str_bytes(value), r)
    End Function
End Module
