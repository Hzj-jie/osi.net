
Imports osi.root.formation

Public Interface isynckeyvalue2(Of SEEK_RESULT)
    Function read_existing(ByVal r As SEEK_RESULT, ByRef value() As Byte) As Boolean
    Function append_existing(ByVal r As SEEK_RESULT, ByVal value() As Byte, ByRef result As Boolean) As Boolean
    Function delete_existing(ByVal r As SEEK_RESULT, ByRef result As Boolean) As Boolean
    Function seek(ByVal key() As Byte, ByRef r As SEEK_RESULT, ByRef result As Boolean) As Boolean
    Function list(ByRef result As vector(Of Byte())) As Boolean
    Function write_new(ByVal key() As Byte, ByVal value() As Byte, ByRef result As Boolean) As Boolean
    Function sizeof_existing(ByVal r As SEEK_RESULT, ByRef result As Int64) As Boolean
    Function full(ByRef result As Boolean) As Boolean
    Function empty(ByRef result As Boolean) As Boolean
    Function retire() As Boolean
    Function capacity(ByRef result As Int64) As Boolean
    Function valuesize(ByRef result As Int64) As Boolean
    Function keycount(ByRef result As Int64) As Boolean
    Function heartbeat() As Boolean
    Function [stop]() As Boolean
End Interface
