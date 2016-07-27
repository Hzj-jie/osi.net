
Imports osi.root.procedure
Imports osi.root.formation

Public Interface ikeyvalue
    'refer to keyvalue_io for the meaning of each parameter and output

    'read the value of the key, and store in to value parameter
    Function read(ByVal key() As Byte,
                  ByVal value As pointer(Of Byte())) As event_comb

    'append the value to the key
    'if the key does not exist, it will be created
    Function append(ByVal key() As Byte,
                    ByVal value() As Byte,
                    ByVal result As pointer(Of Boolean)) As event_comb

    'delete the value of the key <meanwhile with the key itself>, and set the result into result parameter
    Function delete(ByVal key() As Byte,
                    ByVal result As pointer(Of Boolean)) As event_comb

    'if the key exists, set the result into result parameter
    Function seek(ByVal key() As Byte,
                  ByVal result As pointer(Of Boolean)) As event_comb

    'output all the keys into result parameter
    Function list(ByVal result As pointer(Of vector(Of Byte()))) As event_comb

    'change the value of the key to value
    'if the key does not exist, it will be created
    Function modify(ByVal key() As Byte,
                    ByVal value() As Byte,
                    ByVal result As pointer(Of Boolean)) As event_comb

    'get the size of value of the key, and set the result into result parameter
    Function sizeof(ByVal key() As Byte,
                    ByVal result As pointer(Of Int64)) As event_comb

    'whether the storage is full, set the result into result parameter
    Function full(ByVal result As pointer(Of Boolean)) As event_comb

    'whether the storage is empty, set the result into result parameter
    Function empty(ByVal result As pointer(Of Boolean)) As event_comb

    'clear all the keys and values
    Function retire() As event_comb

    'get the capacity of the storage, and set the result into result parameter
    Function capacity(ByVal result As pointer(Of Int64)) As event_comb

    'get the total size of all the values, and set the result into result parameter
    Function valuesize(ByVal result As pointer(Of Int64)) As event_comb

    'get the count of all keys, and set the result into result parameter
    Function keycount(ByVal result As pointer(Of Int64)) As event_comb

    'try to heartbeat when other command returns an internal error
    Function heartbeat() As event_comb

    'stop the storage system, before the storage system instance to be finalized
    Function [stop]() As event_comb
End Interface
