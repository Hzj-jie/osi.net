
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

Public Class typed_word
    Public Const unknown_type As UInt32 = max_uint32
    Public ReadOnly ref As String
    Public ReadOnly start As UInt32
    Public ReadOnly [end] As UInt32
    Public ReadOnly len As UInt32
    Public ReadOnly type As UInt32

    Public Sub New(ByVal ref As String,
                   ByVal start As UInt32,
                   ByVal [end] As UInt32,
                   ByVal type As UInt32)
        assert(Not String.IsNullOrEmpty(ref))
        assert(start < [end])
        assert(strlen(ref) >= [end])
        Me.ref = ref
        Me.start = start
        Me.end = [end]
        Me.len = ([end] - start)
        Me.type = type
    End Sub

    Public Sub New(ByVal ref As String,
                   ByVal start As UInt32,
                   ByVal [end] As UInt32)
        Me.New(ref, start, [end], unknown_type)
    End Sub

    Public Function unknown() As Boolean
        Return type = unknown_type
    End Function

    Public Function str() As String
        Return strmid(ref, start, len)
    End Function

    Public Function debug_str() As String
        Return strcat("[", type, "]: ", str(), "@", start, "-", [end])
    End Function

    Public NotOverridable Overrides Function ToString() As String
        Return str()
    End Function
End Class
