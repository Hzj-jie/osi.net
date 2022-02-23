
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
                assert(m IsNot Nothing)
                assert(exp OrElse [end] >= start)
                Me.m = m
                Me.exp = exp
                Me.start = start
                Me.end = [end]
                Me.v = typed_word.fakes(v)
                assert(Me.v IsNot Nothing)
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
            Dim ms() As matching_case = create()
            assert(Not isemptyarray(ms))
            For i As Int32 = 0 To array_size_i(ms) - 1
                Dim m As matching_case = ms(i)
                assert(m IsNot Nothing)
                Dim r As matching.result = m.m.match(m.v, m.start)
                assertion.equal(r.succeeded(), m.exp, i)
                If r.succeeded() Then
                    assertion.equal(r.suc.pos, m.end, i)
                End If
            Next
            Return True
        End Function
    End Class
End Namespace
