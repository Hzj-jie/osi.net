
Option Explicit On
Option Infer Off
Option Strict On

Public Interface iclient_pool(Of T)
    Function [get](ByRef o As T) As Boolean
    Function release(ByVal i As T) As Boolean
End Interface

Public Interface iserver_pool(Of T)
    Event export(ByVal i As T)
End Interface