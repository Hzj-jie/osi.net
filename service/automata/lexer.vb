
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.template
Imports osi.root.formation
Imports osi.root.utils

Public Class lexer
    Public Const separator_type As UInt32 = 0
    Public Const unknown_type As UInt32 = separator_type + 1
    Public Const end_type As UInt32 = unknown_type + 1
    Public Const first_user_type As UInt32 = end_type + 1
    Private Shared ReadOnly default_separators() As String

    Shared Sub New()
        ReDim default_separators(strlen(space_chars) - 1)
        For i As Int32 = 0 To strlen(space_chars) - 1
            default_separators(i) = space_chars(i)
        Next
    End Sub

    Private ReadOnly adfa As adfa(Of Byte, 
                                     _max_uint8, 
                                     _byte_to_uint32, 
                                     vector(Of word), 
                                     stringtrie(Of Func(Of Byte(), UInt32, vector(Of word), Boolean)))

    Public Class MAX_TYPE(Of _MAX_TYPE As _int64)
        Inherits _int64

        Protected Overrides Function at() As Int64
            Return (+(alloc(Of _MAX_TYPE)())) + first_user_type
        End Function
    End Class

    Public Class typed_word_to_uint32
        Inherits _to_uint32(Of typed_word)

        Public Overrides Function at(ByRef k As typed_word) As UInt32
            Return k.type
        End Function

        Public Overrides Function reverse(ByVal i As UInt32) As typed_word
            Return New typed_word(i)
        End Function
    End Class

    Public Class typed_word
        Public Shared ReadOnly unknown_typed_word As typed_word
        Public Shared ReadOnly separator_typed_word As typed_word
        Public ReadOnly type As UInt32

        Shared Sub New()
            unknown_typed_word = New typed_word(unknown_type)
            separator_typed_word = New typed_word(separator_type)
        End Sub

        Public Sub New(ByVal type As UInt32)
            Me.type = type
        End Sub

        Public Function unknown() As Boolean
            Return type = unknown_type
        End Function

        Public Function [end]() As Boolean
            Return type = end_type
        End Function

        Public Function separator() As Boolean
            Return type = separator_type
        End Function

        Public Function text() As String
            Return cast(Of word)(Me).text
        End Function
    End Class

    Public Class word
        Inherits typed_word
        Public Shared ReadOnly end_word As word
        Public Shadows ReadOnly text As String

        Shared Sub New()
            end_word = New word("", end_type)
        End Sub

        Public Sub New(ByVal text As String, ByVal type As UInt32)
            MyBase.New(type)
            Me.text = text
        End Sub

        Public Shared Function unknown_word(ByVal text As String) As word
            Return New word(text, unknown_type)
        End Function
    End Class

    Private Class unknown_word
        Private ReadOnly v As vector(Of Byte)

        Public Sub New()
            v = New vector(Of Byte)()
        End Sub

        Public Sub append(ByVal b As Byte)
            v.emplace_back(b)
        End Sub

        Public Function output() As word
            Return word.unknown_word(bytes_str(+v))
        End Function
    End Class

    Public Sub New(ByVal use_default_separators As Boolean, ByVal ignore_separators As Boolean)
        adfa = New adfa(Of Byte, 
                           _max_uint8, 
                           _byte_to_uint32, 
                           vector(Of word), 
                           stringtrie(Of Func(Of Byte(), UInt32, vector(Of word), Boolean)))()
        If use_default_separators Then
            Me.use_default_separators(ignore_separators)
        End If
    End Sub

    Public Sub New(ByVal ignore_separators As Boolean)
        Me.New(True, ignore_separators)
    End Sub

    Public Sub New()
        Me.New(True, True)
    End Sub

    Public Sub use_default_separators(ByVal ignore As Boolean)
        assert(define(default_separators, separator_type, ignore))
    End Sub

    Public Sub use_default_separators()
        use_default_separators(False)
    End Sub

    Public Function define(ByVal st() As pair(Of String, UInt32), ByVal ignore As Boolean) As Boolean
        If isemptyarray(st) Then
            Return False
        Else
            For i As Int32 = 0 To array_size(st) - 1
                If st(i) Is Nothing OrElse Not define(st(i).first, st(i).second, ignore) Then
                    Return False
                End If
            Next
            Return True
        End If
    End Function

    Public Function define(ByVal st() As pair(Of String, UInt32)) As Boolean
        Return define(st, False)
    End Function

    Public Function define(ByVal s() As String, ByVal type As UInt32, ByVal ignore As Boolean) As Boolean
        If isemptyarray(s) Then
            Return False
        Else
            For i As Int32 = 0 To array_size(s) - 1
                If Not define(s(i), type, ignore) Then
                    Return False
                End If
            Next
            Return True
        End If
    End Function

    Public Function define(ByVal s() As String, ByVal type As UInt32) As Boolean
        Return define(s, type, False)
    End Function

    Public Function define_separators(ByVal s() As String, ByVal ignore As Boolean) As Boolean
        Return define(s, separator_type, ignore)
    End Function

    Public Function define_separators(ByVal s() As String) As Boolean
        Return define_separators(s, False)
    End Function

    Public Function define(ByVal s As String, ByVal type As UInt32, ByVal ignore As Boolean) As Boolean
        If type < first_user_type AndAlso type <> separator_type Then
            Return False
        Else
            Return adfa.insert(str_bytes(s),
                               Function(b() As Byte, pos As UInt32, result As vector(Of word)) As Boolean
                                   If Not ignore Then
                                       result.emplace_back(New word(s, type))
                                   End If
                                   Return True
                               End Function)
        End If
    End Function

    Public Function define(ByVal s As String, ByVal type As UInt32) As Boolean
        Return define(s, type, False)
    End Function

    Public Function define_separator(ByVal s As String, ByVal ignore As Boolean) As Boolean
        Return define(s, separator_type, ignore)
    End Function

    Public Function define_separator(ByVal s As String) As Boolean
        Return define_separator(s, False)
    End Function

    Public Function parse(ByVal s As String, ByRef o As vector(Of word)) As Boolean
        o.renew()
        Dim b() As Byte = Nothing
        b = str_bytes(s)
        Dim uw As unknown_word = Nothing
        Dim result As vector(Of word) = Nothing
        result = o
        If adfa.parse(b,
                      result,
                      Function(NIU() As Byte, pos As UInt32) As Boolean
                          If Not uw Is Nothing Then
                              result.emplace_back(uw.output())
                              uw = Nothing
                          End If
                          Return True
                      End Function,
                      Function(NIU() As Byte, pos As UInt32) As Boolean
                          If uw Is Nothing Then
                              uw = New unknown_word()
                          End If
                          uw.append(b(pos))
                          Return True
                      End Function) Then
            If Not uw Is Nothing Then
                o.emplace_back(uw.output)
            End If
            o.emplace_back(word.end_word)
            Return True
        Else
            Return False
        End If
    End Function
End Class
