
Imports osi.root.formation
Imports osi.root.connector
Imports osi.root.constants

Public Class isynckeyvalue2_isynckeyvalue(Of SEEK_RESULT)
    Implements isynckeyvalue

    Private ReadOnly impl As isynckeyvalue2(Of SEEK_RESULT)

    Public Sub New(ByVal impl As isynckeyvalue2(Of SEEK_RESULT))
        assert(impl IsNot Nothing)
        Me.impl = impl
    End Sub

    Public Function append(ByVal key() As Byte,
                           ByVal value() As Byte,
                           ByRef result As Boolean) As Boolean Implements isynckeyvalue.append
        Dim r As Boolean = False
        Dim sr As SEEK_RESULT = Nothing
        If impl.seek(key, sr, r) Then
            Return If(r, impl.append_existing(sr, value, result), impl.write_new(key, value, result))
        Else
            Return False
        End If
    End Function

    Public Function capacity(ByRef result As Int64) As Boolean Implements isynckeyvalue.capacity
        Return impl.capacity(result)
    End Function

    Public Function delete(ByVal key() As Byte, ByRef result As Boolean) As Boolean Implements isynckeyvalue.delete
        Dim r As Boolean = False
        Dim sr As SEEK_RESULT = Nothing
        If impl.seek(key, sr, r) Then
            If r Then
                Return impl.delete_existing(sr, result)
            Else
                result = False
                Return True
            End If
        Else
            Return False
        End If
    End Function

    Public Function empty(ByRef result As Boolean) As Boolean Implements isynckeyvalue.empty
        Return impl.empty(result)
    End Function

    Public Function full(ByRef result As Boolean) As Boolean Implements isynckeyvalue.full
        Return impl.full(result)
    End Function

    Public Function heartbeat() As Boolean Implements isynckeyvalue.heartbeat
        Return impl.heartbeat()
    End Function

    Public Function keycount(ByRef result As Int64) As Boolean Implements isynckeyvalue.keycount
        Return impl.keycount(result)
    End Function

    Public Function list(ByRef result As vector(Of Byte())) As Boolean Implements isynckeyvalue.list
        Return impl.list(result)
    End Function

    Public Function modify(ByVal key() As Byte,
                           ByVal value() As Byte,
                           ByRef result As Boolean) As Boolean Implements isynckeyvalue.modify
        Dim r As Boolean = False
        Dim sr As SEEK_RESULT = Nothing
        If impl.seek(key, sr, r) Then
            If r Then
                If impl.delete_existing(sr, r) AndAlso r Then
                    Return impl.write_new(key, value, result)
                Else
                    Return False
                End If
            Else
                Return impl.write_new(key, value, result)
            End If
        Else
            Return False
        End If
    End Function

    Public Function read(ByVal key() As Byte, ByRef value() As Byte) As Boolean Implements isynckeyvalue.read
        Dim r As Boolean = False
        Dim sr As SEEK_RESULT = Nothing
        If impl.seek(key, sr, r) Then
            If r Then
                Return impl.read_existing(sr, value)
            Else
                value = Nothing
                Return True
            End If
        Else
            Return False
        End If
    End Function

    Public Function retire() As Boolean Implements isynckeyvalue.retire
        Return impl.retire()
    End Function

    Public Function seek(ByVal key() As Byte, ByRef result As Boolean) As Boolean Implements isynckeyvalue.seek
        Return impl.seek(key, Nothing, result)
    End Function

    Public Function sizeof(ByVal key() As Byte, ByRef result As Int64) As Boolean Implements isynckeyvalue.sizeof
        Dim r As Boolean = False
        Dim sr As SEEK_RESULT = Nothing
        If impl.seek(key, sr, r) Then
            If r Then
                Return impl.sizeof_existing(sr, result)
            Else
                result = npos
                Return True
            End If
        Else
            Return False
        End If
    End Function

    Public Function [stop]() As Boolean Implements isynckeyvalue.stop
        Return impl.stop()
    End Function

    Public Function valuesize(ByRef result As Int64) As Boolean Implements isynckeyvalue.valuesize
        Return impl.valuesize(result)
    End Function
End Class
