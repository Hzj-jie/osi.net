
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata

Namespace syntaxer
    Public NotInheritable Class fake_typed_word
        Inherits typed_word

        Public Sub New(ByVal type As UInt32)
            MyBase.New("fake-typed-word", uint32_0, uint32_1, type)
        End Sub

        Public Shared Function create(ByVal ParamArray types() As UInt32) As vector(Of typed_word)
            Dim r As vector(Of typed_word) = Nothing
            r = New vector(Of typed_word)()
            For i As Int32 = 0 To array_size_i(types) - 1
                r.emplace_back(New fake_typed_word(types(i)))
            Next
            Return r
        End Function
    End Class
End Namespace
