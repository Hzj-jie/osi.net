
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.root.procedure

' Create one instance of T per create() request.
Public Interface icreator(Of T)
    Function create(ByVal o As pointer(Of T)) As event_comb
End Interface

' Automatically create and export instances of T.
Public Interface iexporter(Of T)
    ' The event listener sets |reject| to true to give a hint to iexporter(Of T) instance. E.g. if remote peer overly
    ' created connections.
    Event export(ByVal d As T, ByRef reject As Boolean)
End Interface