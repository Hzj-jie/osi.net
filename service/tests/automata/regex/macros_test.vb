
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt
Imports osi.service.automata
Imports macros = osi.service.automata.rlexer.macros

Namespace rlexer
    Public Class macros_test
        Inherits [case]

        Private Shared ReadOnly pairs(,) As String =
            {{"\D", "[0,1,2,3,4,5,6,7,8,9]!"},
             {"\\", "\x5C"},
             {"\d", "0,1,2,3,4,5,6,7,8,9"},
             {"\d,d", "0,1,2,3,4,5,6,7,8,9,d"},
             {"", ""},
             {"abc", "abc"},
             {"\\d", "\x5Cd"},
             {"\d\D\\", "0,1,2,3,4,5,6,7,8,9[0,1,2,3,4,5,6,7,8,9]!\x5C"}}

        Public Overrides Function run() As Boolean
            For i As UInt32 = 0 To array_size(pairs) - uint32_1
                assert_equal(macros.default.expand(pairs(i, 0)), pairs(i, 1))
            Next
            Return True
        End Function
    End Class
End Namespace
