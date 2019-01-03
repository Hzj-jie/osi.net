
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt

Public Class c_escape_test
    Inherits multithreading_case_wrapper

    Public Sub New()
        MyBase.New(chained(New c_escape_predefined_case(),
                           repeat(New c_escape_case(), 16 * 1024)),
                   2)
    End Sub

    Private Class c_escape_predefined_case
        Inherits [case]

        Private Shared ReadOnly failed() As String = {"\",
                                                      "\c",
                                                      "\uxxxx",
                                                      "\u123",
                                                      "\u123x",
                                                      "\Uxxxxxxxx",
                                                      "\U1234567",
                                                      "\U123",
                                                      "\U1234567X"}

        Private Shared ReadOnly mapping(,) As String = {
            {"abc", "abc"},
            {"\x2C", ","},
            {"\x5D", "]"},
            {"\""\'", """'"},
            {"", ""},
            {"\ufeff", Convert.ToChar(65279)},
            {"\uFEFF", Convert.ToChar(65279)},
            {"\ufffe", Convert.ToChar(65534)},
            {"\uFFFE", Convert.ToChar(65534)},
            {"that\'s a sentence. usually we are using \\ as an escape character.",
             "that's a sentence. usually we are using \ as an escape character."}}

        Public Overrides Function run() As Boolean
            For i As UInt32 = 0 To array_size(failed) - uint32_1
                Dim s As String = Nothing
                assertion.is_false(failed(i).c_unescape(s), failed(i))
            Next

            For i As Int32 = 0 To array_size(mapping) - 1
                Dim escaped As String = Nothing
                Dim unescaped As String = Nothing
                escaped = mapping(i, 0)
                assertion.is_true(escaped.c_unescape(unescaped))
                assertion.is_not_null(unescaped)
                assertion.equal(unescaped, mapping(i, 1))

                unescaped = mapping(i, 1)
                assertion.is_true(unescaped.c_escape(escaped))
                assertion.is_not_null(escaped)
                If escaped <> mapping(i, 0) Then
                    assertion.equal(escaped, mapping(i, 1))
                End If
                If escaped <> mapping(i, 1) Then
                    assertion.equal(escaped, mapping(i, 0))
                End If
            Next
            Return True
        End Function
    End Class

    Private Class c_escape_case
        Inherits [case]

        Private Shared ReadOnly should_not_contains() As Char = {character.alert,
                                                                 character.feed,
                                                                 character.return,
                                                                 character.vtab,
                                                                 character.tab,
                                                                 character.newline,
                                                                 character.backspace,
                                                                 character.null}

        Public Overrides Function run() As Boolean
            Dim s As String = Nothing
            s = rnd_chars(rnd_int(max_uint8, max_uint16))
            Dim escaped As String = Nothing
            assertion.is_true(s.c_escape(escaped))
            assertion.is_not_null(escaped)
            For i As Int32 = 0 To array_size(should_not_contains) - 1
                assertion.is_false(escaped.Contains(should_not_contains(i)))
            Next
            Dim unescaped As String = Nothing
            assertion.is_true(escaped.c_unescape(unescaped))
            assertion.is_not_null(unescaped)
            If Not assertion.equal(unescaped, s) Then
                For i As Int32 = 0 To min(strlen(unescaped), strlen(s)) - 1
                    'debug
                    If Not assertion.equal(unescaped(i), s(i), unescaped(i), " <> ", s(i)) Then
                        Exit For
                    End If
                Next
            End If
            Return True
        End Function
    End Class
End Class
