
Imports System.Runtime.CompilerServices
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.connector
Imports osi.root.utils
Imports osi.service.selector
Imports osi.service.transmitter

' Non-generic interface for device_manager
Public Interface idevice_pool
    Event closing()
    Function max_count() As UInt32
    Function identity() As String
    Function total_count() As UInt32
    Function free_count() As UInt32
    Function expired() As Boolean
    Sub close()
End Interface

Public Interface idevice_pool(Of T)
    Inherits idevice_pool
    Function [get](ByRef c As idevice(Of T)) As Boolean
    Function release(ByVal c As idevice(Of T)) As Boolean
    'device<T> has closed state, which helps to make sure each instance would be exactly closed once
    'Function [get](ByRef c As T) As Boolean
    'Function release(ByVal c As T) As Boolean
    Event new_device_inserted(ByVal d As idevice(Of T))
    'Event new_device_inserted(ByVal d As T)
    Event new_device_created(ByVal d As idevice(Of T))
    'Event new_device_created(ByVal d As T)
    Event device_removed(ByVal d As idevice(Of T))
    'Event device_removed(ByVal d As T)
End Interface

Public Module _device_pool
    <Extension()> Public Function limited_max_count(ByVal d As idevice_pool) As Boolean
        assert(d IsNot Nothing)
        Return d.max_count() > 0
    End Function

    <Extension()> Public Function empty(ByVal d As idevice_pool) As Boolean
        assert(d IsNot Nothing)
        Return d.free_count() = 0
    End Function

    <Extension()> Public Function [get](Of T)(ByVal d As idevice_pool(Of T),
                                              ByVal r As ref(Of idevice(Of T)),
                                              ByVal timeout_ms As Int64) As event_comb
        Dim p As pending_io_punishment = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If d Is Nothing Then
                                      Return False
                                  Else
                                      p = New pending_io_punishment(timeout_ms)
                                      Return goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  If ec.end_result_or_null() Then
                                      Dim x As idevice(Of T) = Nothing
                                      If d.[get](x) Then
                                          Return eva(r, x) AndAlso
                                                 goto_end()
                                      Else
                                          ec = p(False)
                                          Return waitfor(ec)
                                      End If
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function
End Module
