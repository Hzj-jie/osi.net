
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils

Public Interface iproperty
    Function path() As String
    Function name() As String
    Function [get](ByVal i As pointer(Of Byte())) As event_comb
    Function [set](ByVal v() As Byte) As event_comb
    Function append(ByVal v() As Byte) As event_comb
    Function lock(Optional ByVal wait_ms As Int64 = npos) As event_comb
    Function release() As event_comb
End Interface

Public Module _iproperty
    Private Function [get](Of T)(ByVal i As iproperty,
                                 ByVal r As pointer(Of T),
                                 ByVal convert As _do_val_ref(Of Byte(), T, Boolean)) As event_comb
        assert(Not convert Is Nothing)
        Dim ec As event_comb = Nothing
        Dim p As pointer(Of Byte()) = Nothing
        Return New event_comb(Function() As Boolean
                                  If i Is Nothing Then
                                      Return False
                                  Else
                                      p = New pointer(Of Byte())()
                                      ec = i.[get](p)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  Dim v As T = Nothing
                                  Return ec.end_result() AndAlso
                                         convert(+p, v) AndAlso
                                         eva(r, v) AndAlso
                                         goto_end()
                              End Function)
    End Function

    <Extension()> Public Function [get](ByVal i As iproperty,
                                        ByVal r As pointer(Of String)) As event_comb
        Return [get](i,
                     r,
                     Function(x() As Byte, ByRef y As String) As Boolean
                         y = bytes_str(x)
                         Return True
                     End Function)
    End Function

    <Extension()> Public Function [get](ByVal i As iproperty,
                                        ByVal r As pointer(Of SByte)) As event_comb
        Return [get](i,
                     r,
                     AddressOf bytes_sbyte)
    End Function

    <Extension()> Public Function [get](ByVal i As iproperty,
                                        ByVal r As pointer(Of Byte)) As event_comb
        Return [get](i,
                     r,
                     AddressOf bytes_byte)
    End Function

    <Extension()> Public Function [get](ByVal i As iproperty,
                                        ByVal r As pointer(Of Int16)) As event_comb
        Return [get](i,
                     r,
                     AddressOf bytes_int16)
    End Function

    <Extension()> Public Function [get](ByVal i As iproperty,
                                        ByVal r As pointer(Of UInt16)) As event_comb
        Return [get](i,
                     r,
                     AddressOf bytes_uint16)
    End Function

    <Extension()> Public Function [get](ByVal i As iproperty,
                                        ByVal r As pointer(Of Int32)) As event_comb
        Return [get](i,
                     r,
                     AddressOf bytes_int32)
    End Function

    <Extension()> Public Function [get](ByVal i As iproperty,
                                        ByVal r As pointer(Of UInt32)) As event_comb
        Return [get](i,
                     r,
                     AddressOf bytes_uint32)
    End Function

    <Extension()> Public Function [get](ByVal i As iproperty,
                                        ByVal r As pointer(Of Int64)) As event_comb
        Return [get](i,
                     r,
                     AddressOf bytes_int64)
    End Function

    <Extension()> Public Function [get](ByVal i As iproperty,
                                        ByVal r As pointer(Of UInt64)) As event_comb
        Return [get](i,
                     r,
                     AddressOf bytes_uint64)
    End Function

    <Extension()> Public Function [get](ByVal i As iproperty,
                                        ByVal r As pointer(Of Single)) As event_comb
        Return [get](i,
                     r,
                     AddressOf bytes_single)
    End Function

    <Extension()> Public Function [get](ByVal i As iproperty,
                                        ByVal r As pointer(Of Double)) As event_comb
        Return [get](i,
                     r,
                     AddressOf bytes_double)
    End Function

    Private Function [set](ByVal i As iproperty, ByVal v() As Byte) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If i Is Nothing Then
                                      Return False
                                  Else
                                      ec = i.set(v)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    <Extension()> Public Function [set](ByVal i As iproperty,
                                        ByVal v As String) As event_comb
        Return [set](i, str_bytes(v))
    End Function

    <Extension()> Public Function [set](ByVal i As iproperty,
                                        ByVal v As SByte) As event_comb
        Return [set](i, sbyte_bytes(v))
    End Function

    <Extension()> Public Function [set](ByVal i As iproperty,
                                        ByVal v As Byte) As event_comb
        Return [set](i, byte_bytes(v))
    End Function

    <Extension()> Public Function [set](ByVal i As iproperty,
                                        ByVal v As Int16) As event_comb
        Return [set](i, int16_bytes(v))
    End Function

    <Extension()> Public Function [set](ByVal i As iproperty,
                                        ByVal v As UInt16) As event_comb
        Return [set](i, uint16_bytes(v))
    End Function

    <Extension()> Public Function [set](ByVal i As iproperty,
                                        ByVal v As Int32) As event_comb
        Return [set](i, int32_bytes(v))
    End Function

    <Extension()> Public Function [set](ByVal i As iproperty,
                                        ByVal v As UInt32) As event_comb
        Return [set](i, uint32_bytes(v))
    End Function

    <Extension()> Public Function [set](ByVal i As iproperty,
                                        ByVal v As Int64) As event_comb
        Return [set](i, int64_bytes(v))
    End Function

    <Extension()> Public Function [set](ByVal i As iproperty,
                                        ByVal v As UInt64) As event_comb
        Return [set](i, uint64_bytes(v))
    End Function

    <Extension()> Public Function [set](ByVal i As iproperty,
                                        ByVal v As Single) As event_comb
        Return [set](i, single_bytes(v))
    End Function

    <Extension()> Public Function [set](ByVal i As iproperty,
                                        ByVal v As Double) As event_comb
        Return [set](i, double_bytes(v))
    End Function

    <Extension()> Public Function push_back(ByVal i As iproperty, ByVal value() As Byte) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If i Is Nothing Then
                                      Return False
                                  Else
                                      ec = i.append(chunk.from_bytes(value))
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    <Extension()> Public Function push_back(ByVal i As iproperty, ByVal value As String) As event_comb
        Return push_back(i, str_bytes(value))
    End Function

    <Extension()> Public Function unique_push_back(ByVal i As iproperty, ByVal value() As Byte) As event_comb
        Dim ec As event_comb = Nothing
        Dim r As pointer(Of Boolean) = Nothing
        Return New event_comb(Function() As Boolean
                                  r = New pointer(Of Boolean)()
                                  ec = contains(i, value, r)
                                  Return ec.end_result() AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() AndAlso
                                     Not (+r) Then
                                      ec = push_back(i, value)
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

    <Extension()> Public Function unique_push_back(ByVal i As iproperty, ByVal value As String) As event_comb
        Return unique_push_back(i, str_bytes(value))
    End Function

    <Extension()> Public Function read(ByVal i As iproperty,
                                       ByVal v As pointer(Of vector(Of Byte()))) As event_comb
        Dim ec As event_comb = Nothing
        Dim p As pointer(Of Byte()) = Nothing
        Return New event_comb(Function() As Boolean
                                  If i Is Nothing Then
                                      Return False
                                  Else
                                      p = New pointer(Of Byte())()
                                      ec = i.get(p)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         Not isemptyarray(+p) AndAlso
                                         eva(v, chunks.parse_or_null(+p)) AndAlso
                                         goto_end()
                              End Function)
    End Function

    <Extension()> Public Function contains(ByVal i As iproperty,
                                           ByVal v() As Byte,
                                           ByVal r As pointer(Of Boolean)) As event_comb
        Dim ec As event_comb = Nothing
        Dim p As pointer(Of Byte()) = Nothing
        Return New event_comb(Function() As Boolean
                                  If i Is Nothing Then
                                      Return False
                                  Else
                                      p = New pointer(Of Byte())()
                                      ec = i.get(p)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         eva(r, chunks.contains((+p), v)) AndAlso
                                         goto_end()
                              End Function)
    End Function

    <Extension()> Public Function read(ByVal p As iproperty,
                                       ByVal v As pointer(Of vector(Of String))) As event_comb
        Dim ec As event_comb = Nothing
        Dim r As pointer(Of vector(Of Byte())) = Nothing
        Return New event_comb(Function() As Boolean
                                  r = New pointer(Of vector(Of Byte()))()
                                  ec = read(p, r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Dim vs As vector(Of String) = Nothing
                                  Return ec.end_result() AndAlso
                                         bytes_serializer(Of vector(Of Byte())).default.
                                                 to_container(Of vector(Of String), String)(+r, vs) AndAlso
                                         eva(v, vs) AndAlso
                                         goto_end()
                              End Function)
    End Function

    <Extension()> Public Function contains(ByVal p As iproperty,
                                           ByVal v As String,
                                           ByVal r As pointer(Of Boolean)) As event_comb
        Return contains(p, str_bytes(v), r)
    End Function
End Module
