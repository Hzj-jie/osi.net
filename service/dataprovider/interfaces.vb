
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.procedure

Public Interface idataprovider
    Function last_refered_ticks() As Int64
    Function last_updated_ticks() As Int64
    Function get_object() As Object
    Function valid() As Boolean
    Sub expire()
    Function expired() As Boolean
    Sub register_update_event(Of T)(ByVal v As T, ByVal a As Action(Of T))
End Interface

Public Interface idatawatcher
    ' Continually work until error (event_comb.end_result() == false) or change (event_comb.end_result() == true).
    ' If expiration_controller shows expired, this procedure should stop as soon as possible.
    Function watch(ByVal exp As expiration_controller) As event_comb
End Interface

Public Interface idatafetcher
    Function fetch(ByVal localfile As String) As event_comb
End Interface

Public Interface idataloader(Of T)
    Function load(ByVal localfile As String, ByVal result As ref(Of T)) As event_comb
End Interface

Public Interface istreamdataloader(Of T)
    Function load(ByVal s As Stream, ByVal result As ref(Of T)) As event_comb
End Interface
