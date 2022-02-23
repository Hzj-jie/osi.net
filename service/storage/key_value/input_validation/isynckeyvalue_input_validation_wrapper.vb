
Imports osi.root.formation
Imports osi.root.connector

Friend Class isynckeyvalue_input_validation_wrapper
    Implements isynckeyvalue

    Private ReadOnly impl As isynckeyvalue

    Public Sub New(ByVal impl As isynckeyvalue)
        assert(impl IsNot Nothing)
        Me.impl = impl
    End Sub

    Public Function append(ByVal key() As Byte,
                           ByVal value() As Byte,
                           ByRef result As Boolean) As Boolean Implements isynckeyvalue.append
        Return input_validation.append(key, value, result) AndAlso
               impl.append(key, value, result)
    End Function

    Public Function capacity(ByRef result As Int64) As Boolean Implements isynckeyvalue.capacity
        Return input_validation.capacity(result) AndAlso
               impl.capacity(result)
    End Function

    Public Function delete(ByVal key() As Byte,
                           ByRef result As Boolean) As Boolean Implements isynckeyvalue.delete
        Return input_validation.delete(key, result) AndAlso
               impl.delete(key, result)
    End Function

    Public Function empty(ByRef result As Boolean) As Boolean Implements isynckeyvalue.empty
        Return input_validation.empty(result) AndAlso
               impl.empty(result)
    End Function

    Public Function full(ByRef result As Boolean) As Boolean Implements isynckeyvalue.full
        Return input_validation.full(result) AndAlso
               impl.full(result)
    End Function

    Public Function heartbeat() As Boolean Implements isynckeyvalue.heartbeat
        Return impl.heartbeat()
    End Function

    Public Function keycount(ByRef result As Int64) As Boolean Implements isynckeyvalue.keycount
        Return input_validation.keycount(result) AndAlso
               impl.keycount(result)
    End Function

    Public Function list(ByRef result As vector(Of Byte())) As Boolean Implements isynckeyvalue.list
        Return input_validation.list(result) AndAlso
               impl.list(result)
    End Function

    Public Function modify(ByVal key() As Byte,
                           ByVal value() As Byte,
                           ByRef result As Boolean) As Boolean Implements isynckeyvalue.modify
        Return input_validation.modify(key, value, result) AndAlso
               impl.modify(key, value, result)
    End Function

    Public Function read(ByVal key() As Byte,
                         ByRef value() As Byte) As Boolean Implements isynckeyvalue.read
        Return input_validation.read(key, value) AndAlso
               impl.read(key, value)
    End Function

    Public Function retire() As Boolean Implements isynckeyvalue.retire
        Return impl.retire()
    End Function

    Public Function seek(ByVal key() As Byte,
                         ByRef result As Boolean) As Boolean Implements isynckeyvalue.seek
        Return input_validation.seek(key, result) AndAlso
               impl.seek(key, result)
    End Function

    Public Function sizeof(ByVal key() As Byte,
                           ByRef result As Int64) As Boolean Implements isynckeyvalue.sizeof
        Return input_validation.sizeof(key, result) AndAlso
               impl.sizeof(key, result)
    End Function

    Public Function [stop]() As Boolean Implements isynckeyvalue.stop
        Return impl.stop()
    End Function

    Public Function valuesize(ByRef result As Int64) As Boolean Implements isynckeyvalue.valuesize
        Return input_validation.valuesize(result) AndAlso
               impl.valuesize(result)
    End Function
End Class
