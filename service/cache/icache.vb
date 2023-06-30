
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.procedure

'no thread-safe expected
Public Interface islimcache(Of KEY_T As IComparable(Of KEY_T), VALUE_T)
    Sub [set](ByVal key As KEY_T, ByVal value As VALUE_T)
    Function [get](ByVal key As KEY_T, ByRef value As VALUE_T) As Boolean
    Function size() As Int64
    Sub clear()
    Function [erase](ByVal key As KEY_T) As Boolean
End Interface

Public Interface islimcache2(Of KEY_T As IComparable(Of KEY_T), VALUE_T)
    Inherits islimcache(Of KEY_T, VALUE_T)
    Function have(ByVal key As KEY_T) As Boolean
    Function empty() As Boolean
End Interface

'thread-safe
Public Interface icache(Of KEY_T As IComparable(Of KEY_T), VALUE_T)
    Inherits islimcache2(Of KEY_T, VALUE_T)
    Overloads Function [get](ByVal key As KEY_T) As VALUE_T
End Interface

'thread-safe
Public Interface icache2(Of KEY_T As IComparable(Of KEY_T), VALUE_T)
    'the event_comb will never fail
    Function [set](ByVal key As KEY_T, ByVal value As VALUE_T) As event_comb
    'the event_comb will fail when value is nothing or the key is not existing, and the value will be an empty ref
    Function [get](ByVal key As KEY_T, ByVal value As ref(Of VALUE_T)) As event_comb
    'the event_comb will fail when value is nothing
    Function size(ByVal value As ref(Of Int64)) As event_comb
    'the event_comb will never fail
    Function clear() As event_comb
    'the event_comb will fail when the key is not existing
    Function [erase](ByVal key As KEY_T) As event_comb
    'the event_comb will fail when the key is not existing
    Function have(ByVal key As KEY_T) As event_comb
    'the event_comb will fail when the cache is not empty
    Function empty() As event_comb
End Interface
