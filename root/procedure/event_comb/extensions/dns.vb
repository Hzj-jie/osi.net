
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Net
Imports osi.root.connector

Public Module _dns
    Public Function get_host_addresses(ByVal hostname_or_address As String,
                                       ByVal r As pointer(Of IPAddress())) As event_comb
        Return event_comb_async_operation.ctor(Function(ac As AsyncCallback) As IAsyncResult
                                                   Return Dns.BeginGetHostAddresses(hostname_or_address, ac, Nothing)
                                               End Function,
                                               Function(ar As IAsyncResult) As IPAddress()
                                                   Return Dns.EndGetHostAddresses(ar)
                                               End Function,
                                               r)
    End Function

    Public Function get_host_entry(ByVal addr As IPAddress,
                                   ByVal r As pointer(Of IPHostEntry)) As event_comb
        Return event_comb_async_operation.ctor(Function(ac As AsyncCallback) As IAsyncResult
                                                   Return Dns.BeginGetHostEntry(addr, ac, Nothing)
                                               End Function,
                                               Function(ar As IAsyncResult) As IPHostEntry
                                                   Return Dns.EndGetHostEntry(ar)
                                               End Function,
                                               r)
    End Function

    Public Function get_host_entry(ByVal hostname_or_address As String,
                                   ByVal r As pointer(Of IPHostEntry)) As event_comb
        Return event_comb_async_operation.ctor(Function(ac As AsyncCallback) As IAsyncResult
                                                   Return Dns.BeginGetHostEntry(hostname_or_address, ac, Nothing)
                                               End Function,
                                               Function(ar As IAsyncResult) As IPHostEntry
                                                   Return Dns.EndGetHostEntry(ar)
                                               End Function,
                                               r)
    End Function
End Module
