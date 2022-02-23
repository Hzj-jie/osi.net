
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils

Public Module _list
    <Extension()> Public Function push_back(ByVal this As istrkeyvt,
                                            ByVal key As String,
                                            ByVal value() As Byte,
                                            ByVal ts As Int64,
                                            ByVal result As ref(Of Boolean)) As event_comb
        If this Is Nothing Then
            Return Nothing
        End If
        Return this.append(key, chunk.from_bytes(value), ts, result)
    End Function

    <Extension()> Public Function push_back(ByVal this As istrkeyvt,
                                            ByVal key As String,
                                            ByVal value As String,
                                            ByVal ts As Int64,
                                            ByVal result As ref(Of Boolean)) As event_comb
        Return push_back(this, key, str_bytes(value), ts, result)
    End Function

    <Extension()> Public Function push_back(ByVal this As istrkeyvt,
                                            ByVal key As String,
                                            ByVal value() As Byte,
                                            ByVal result As ref(Of Boolean)) As event_comb
        Return push_back(this, key, value, now_as_timestamp(), result)
    End Function

    <Extension()> Public Function push_back(ByVal this As istrkeyvt,
                                            ByVal key As String,
                                            ByVal value As String,
                                            ByVal result As ref(Of Boolean)) As event_comb
        Return push_back(this, key, str_bytes(value), result)
    End Function

    Private Function read(ByVal d As Func(Of ref(Of Byte()), event_comb),
                          ByVal result As ref(Of vector(Of Byte()))) As event_comb
        assert(d IsNot Nothing)
        Dim ec As event_comb = Nothing
        Dim p As ref(Of Byte()) = Nothing
        Return New event_comb(Function() As Boolean
                                  p = New ref(Of Byte())()
                                  ec = d(p)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         eva(result, chunks.parse_or_null(+p)) AndAlso
                                         goto_end()
                              End Function)
    End Function

    <Extension()> Public Function read(ByVal this As istrkeyvt,
                                       ByVal key As String,
                                       ByVal result As ref(Of vector(Of Byte())),
                                       ByVal ts As ref(Of Int64)) As event_comb
        If this Is Nothing Then
            Return Nothing
        End If
        Return read(Function(p As ref(Of Byte())) As event_comb
                        Return this.read(key, p, ts)
                    End Function,
                    result)
    End Function

    <Extension()> Public Function read(ByVal this As istrkeyvt,
                                       ByVal key As String,
                                       ByVal result As ref(Of vector(Of Byte()))) As event_comb
        If this Is Nothing Then
            Return Nothing
        End If
        Return read(Function(p As ref(Of Byte())) As event_comb
                        Return this.read(key, p)
                    End Function,
                    result)
    End Function

    Private Function read(ByVal d As Func(Of ref(Of vector(Of Byte())), event_comb),
                          ByVal result As ref(Of vector(Of String))) As event_comb
        assert(d IsNot Nothing)
        Dim ec As event_comb = Nothing
        Dim r As ref(Of vector(Of Byte())) = Nothing
        Return New event_comb(Function() As Boolean
                                  r = New ref(Of vector(Of Byte()))()
                                  ec = d(r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Dim vs As vector(Of String) = Nothing
                                  Return ec.end_result() AndAlso
                                         bytes_serializer(Of vector(Of Byte())).r.
                                                 to_container(Of vector(Of String), String)(+r, vs) AndAlso
                                         eva(result, vs) AndAlso
                                         goto_end()
                              End Function)
    End Function

    <Extension()> Public Function read(ByVal this As istrkeyvt,
                                       ByVal key As String,
                                       ByVal result As ref(Of vector(Of String)),
                                       ByVal ts As ref(Of Int64)) As event_comb
        Return read(Function(r As ref(Of vector(Of Byte()))) As event_comb
                        Return read(this, key, r, ts)
                    End Function,
                    result)
    End Function

    <Extension()> Public Function read(ByVal this As istrkeyvt,
                                       ByVal key As String,
                                       ByVal result As ref(Of vector(Of String))) As event_comb
        Return read(Function(r As ref(Of vector(Of Byte()))) As event_comb
                        Return read(this, key, r)
                    End Function,
                    result)
    End Function

    <Extension()> Public Function contains(ByVal i As istrkeyvt,
                                           ByVal key As String,
                                           ByVal v() As Byte,
                                           ByVal r As ref(Of Boolean)) As event_comb
        Dim ec As event_comb = Nothing
        Dim p As ref(Of Byte()) = Nothing
        Return New event_comb(Function() As Boolean
                                  If i Is Nothing Then
                                      Return False
                                  End If
                                  p = New ref(Of Byte())()
                                  ec = i.read(key, p)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         eva(r, chunks.contains(+p, v)) AndAlso
                                         goto_end()
                              End Function)
    End Function

    <Extension()> Public Function contains(ByVal i As istrkeyvt,
                                           ByVal key As String,
                                           ByVal v As String,
                                           ByVal r As ref(Of Boolean)) As event_comb
        Return contains(i, key, str_bytes(v), r)
    End Function

    <Extension()> Public Function unique_push_back(ByVal i As istrkeyvt,
                                                   ByVal key As String,
                                                   ByVal value() As Byte,
                                                   ByVal r As ref(Of Boolean)) As event_comb
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
                                  End If
                                  Return False
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    <Extension()> Public Function unique_push_back(ByVal i As istrkeyvt,
                                                   ByVal key As String,
                                                   ByVal value As String,
                                                   ByVal r As ref(Of Boolean)) As event_comb
        Return unique_push_back(i, key, str_bytes(value), r)
    End Function
End Module
