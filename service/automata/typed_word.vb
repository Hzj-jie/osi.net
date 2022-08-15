
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Public NotInheritable Class typed_word
    Public Const unknown_type As UInt32 = max_uint32
    Public Const unknown_type_name As String = "UNKNOWN_TYPE"
    Public ReadOnly ref As String
    Public ReadOnly start As UInt32
    Public ReadOnly [end] As UInt32
    Public ReadOnly len As UInt32
    ' Used by syntax to match the node type without using string comparison.
    Public ReadOnly type As UInt32
    Public ReadOnly type_name As String

    Public Sub New(ByVal ref As String,
                   ByVal start As UInt32,
                   ByVal [end] As UInt32,
                   ByVal type As UInt32,
                   ByVal type_name As String)
        assert(Not ref.null_or_empty())
        assert(start < [end])
        assert(strlen(ref) >= [end])
        assert(Not type_name.null_or_whitespace())
        Me.ref = ref
        Me.start = start
        Me.end = [end]
        Me.len = ([end] - start)
        Me.type = type
        Me.type_name = type_name
    End Sub

    ' TODO: Remove
    Public Sub New(ByVal ref As String,
                   ByVal start As UInt32,
                   ByVal [end] As UInt32,
                   ByVal type As UInt32)
        Me.New(ref, start, [end], type, unknown_type_name)
    End Sub

    ' VisibleForTesting
    Public Sub New(ByVal ref As String,
                   ByVal start As UInt32,
                   ByVal [end] As UInt32)
        Me.New(ref, start, [end], unknown_type, unknown_type_name)
    End Sub

    Public Shared Function fake(ByVal type As UInt32) As typed_word
        Return New typed_word("fake-typed-word", uint32_0, uint32_1, type, unknown_type_name)
    End Function

    Public Shared Function fakes(ByVal ParamArray types() As UInt32) As vector(Of typed_word)
        Dim r As New vector(Of typed_word)()
        For i As Int32 = 0 To array_size_i(types) - 1
            r.emplace_back(fake(types(i)))
        Next
        Return r
    End Function

    Public Function unknown() As Boolean
        Return type = unknown_type
    End Function

    Public Function str() As String
        Return ref.Substring(CInt(start), CInt(len))
    End Function

    Public Function debug_str() As String
        Return String.Concat("[", type_name, "]: ", str(), "@", start, "-", [end])
    End Function

    Public Overrides Function ToString() As String
        Return debug_str()
    End Function
End Class
