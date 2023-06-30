
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.template

Public Interface ikeyvt(Of THREADSAFE As _boolean)
    'refer to io validation for the meaning of each parameter

    'read the value of the key with timestamp
    Function read(ByVal key() As Byte,
                  ByVal result As ref(Of Byte()),
                  ByVal ts As ref(Of Int64)) As event_comb

    'append the value to the key and update the timestamp, if the key does not exist, it will be created
    Function append(ByVal key() As Byte,
                    ByVal value() As Byte,
                    ByVal ts As Int64,
                    ByVal result As ref(Of Boolean)) As event_comb

    'delete the key / value and timestamp of the key  
    Function delete(ByVal key() As Byte, ByVal result As ref(Of Boolean)) As event_comb

    'if the key & value & timestamp exist
    Function seek(ByVal key() As Byte, ByVal result As ref(Of Boolean)) As event_comb

    'output all the keys into result parameter
    Function list(ByVal result As ref(Of vector(Of Byte()))) As event_comb

    'modify the value of the key and update the timestamp, if the key does not exist, it will be created
    Function modify(ByVal key() As Byte,
                    ByVal value() As Byte,
                    ByVal ts As Int64,
                    ByVal result As ref(Of Boolean)) As event_comb

    'get the size of value of the key, and set the result into result parameter
    Function sizeof(ByVal key() As Byte, ByVal result As ref(Of Int64)) As event_comb

    'whether the storage is full, set the result into result parameter
    Function full(ByVal result As ref(Of Boolean)) As event_comb

    'whether the storage is empty, set the result into result parameter
    Function empty(ByVal result As ref(Of Boolean)) As event_comb

    'clear all the keys / values / timestamps
    Function retire() As event_comb

    'get the capacity of the storage, and set the result into result parameter
    Function capacity(ByVal result As ref(Of Int64)) As event_comb

    'get the total size of all the values and timestamps, and set the result into result parameter
    Function valuesize(ByVal result As ref(Of Int64)) As event_comb

    'get the count of all keys, and set the result into result parameter
    Function keycount(ByVal result As ref(Of Int64)) As event_comb

    'try to heartbeat when other commands return an internal error
    Function heartbeat() As event_comb

    'stop the storage system
    Function [stop]() As event_comb

    'if the key does not exist, same as append / modify, otherwise, return false in result parameter
    Function unique_write(ByVal key() As Byte,
                          ByVal value() As Byte,
                          ByVal ts As Int64,
                          ByVal result As ref(Of Boolean)) As event_comb
End Interface
