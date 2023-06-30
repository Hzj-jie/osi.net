
Imports osi.root.formation

Public Interface isynckeyvalue
    Function read(ByVal key() As Byte, ByRef value() As Byte) As Boolean
    Function append(ByVal key() As Byte, ByVal value() As Byte, ByRef result As Boolean) As Boolean
    Function delete(ByVal key() As Byte, ByRef result As Boolean) As Boolean
    Function seek(ByVal key() As Byte, ByRef result As Boolean) As Boolean
    Function list(ByRef result As vector(Of Byte())) As Boolean
    Function modify(ByVal key() As Byte, ByVal value() As Byte, ByRef result As Boolean) As Boolean
    Function sizeof(ByVal key() As Byte, ByRef result As Int64) As Boolean
    Function full(ByRef result As Boolean) As Boolean
    Function empty(ByRef result As Boolean) As Boolean
    Function retire() As Boolean
    Function capacity(ByRef result As Int64) As Boolean
    Function valuesize(ByRef result As Int64) As Boolean
    Function keycount(ByRef result As Int64) As Boolean
    Function heartbeat() As Boolean
    Function [stop]() As Boolean
End Interface
