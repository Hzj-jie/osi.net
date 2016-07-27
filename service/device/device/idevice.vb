
Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.service.selector

Public Interface idevice
    Event closing()
    Sub close()
    Function closed() As Boolean
    Function identity() As String
    Function is_valid() As Boolean
    Sub check()
End Interface

Public Interface idevice(Of T)
    Inherits idevice
    Function [get]() As T
End Interface

Public Interface idevice_creator(Of T)
    Function create(ByRef o As idevice(Of T)) As Boolean
End Interface

Public Interface iasync_device_creator(Of T)
    Inherits idevice_creator(Of async_getter(Of T))
End Interface

Public Interface idevice_exporter(Of T)
    Event new_device_exported(ByVal d As idevice(Of T), ByRef export_result As Boolean)
    Function exported() As UInt32
    Event after_start()
    Function start() As Boolean
    Function started() As Boolean
    Event after_stop()
    Function [stop]() As Boolean
    Function stopped() As Boolean
End Interface

' Automatically receives and injects idevice(Of T) for manual_pre_generated_device_pool
Public Interface imanual_device_exporter(Of T)
    Inherits idevice_exporter(Of T)
    Function inject(ByVal d As idevice(Of T)) As Boolean
    Function inject(ByVal d As idevice(Of async_getter(Of T))) As event_comb     ' Do not use
End Interface

' Manually creates and exports idevice(Of T) for auto_pre_generated_device_pool according to require function
Public Interface iauto_device_exporter(Of T)
    Inherits idevice_exporter(Of T)
    Sub require(Optional ByVal c As UInt32 = uint32_1)
    Function starting() As Boolean
    Sub wait_for_start()
    Function stopping() As Boolean
    Sub wait_for_stop()
End Interface

Public Module _idevice
    <Extension()> Public Function create(Of T)(ByVal this As idevice_creator(Of T)) As idevice(Of T)
        assert(Not this Is Nothing)
        Dim o As idevice(Of T) = Nothing
        assert(this.create(o))
        Return o
    End Function
End Module