
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.automata
Imports osi.service.automata.syntaxer

Namespace syntaxer
    Public MustInherit Class matching_test
        Inherits [case]

        Protected Class matching_case
            Public ReadOnly m As matching
            Public ReadOnly exp As Boolean
            Public ReadOnly start As UInt32
            Public ReadOnly [end] As UInt32
            Public ReadOnly v As vector(Of typed_word)

            Public Sub New(ByVal m As matching,
                           ByVal exp As Boolean,
                           ByVal start As UInt32,
                           ByVal [end] As UInt32,
                           ByVal v() As UInt32)
                assert(Not m Is Nothing)
                assert(exp OrElse [end] >= start)
                Me.m = m
                Me.exp = exp
                Me.start = start
                Me.end = [end]
                Me.v = fake_typed_word.create(v)
                assert(Not Me.v Is Nothing)
            End Sub

            Public Sub New(ByVal m As matching,
                           ByVal exp As Boolean,
                           ByVal [end] As UInt32,
                           ByVal v() As UInt32)
                Me.New(m, exp, uint32_0, [end], v)
            End Sub
        End Class

        Protected ReadOnly c As syntax_collection

        Protected Sub New()
            c = New syntax_collection()
        End Sub

        Protected MustOverride Function create() As matching_case()

        Public Overrides Function run() As Boolean
            Dim ms() As matching_case = Nothing
            ms = create()
            assert(Not isemptyarray(ms))
            For i As Int32 = 0 To array_size_i(ms) - 1
                Dim m As matching_case = Nothing
                m = ms(i)
                assert(Not m Is Nothing)
                Dim pos As UInt32 = 0
                pos = m.start
                assertion.equal(m.m.match(m.v, pos), m.exp)
                assertion.equal(pos, m.end)
            Next
            Return True
        End Function
    End Class
End Namespace
