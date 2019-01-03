
Imports osi.root.connector
Imports osi.root.utt
Imports osi.service.interpreter.fullstack
Imports osi.service.interpreter.fullstack.executor

Namespace fullstack.executor
    Public Class definition_test
        Inherits [case]

        Private Shared Function run_case(ByVal t As type,
                                         ByVal d As domain) As Boolean
            assert(Not t Is Nothing)
            assert(Not d Is Nothing)
            Dim def As definition = Nothing
            def = New definition(t)
            Dim l As Int32 = 0
            l = d.increment()
            def.execute(d)
            assertion.equal(l + 1, d.increment())
            assertion.is_true(d.last().is_type(t))
            Return True
        End Function

        Public Overrides Function run() As Boolean
            Dim d As domain = Nothing
            d = New domain(New variables())
            Return run_case(type.bool, d) AndAlso
                   run_case(type.char, d) AndAlso
                   run_case(type.float, d) AndAlso
                   run_case(type.int, d) AndAlso
                   run_case(type.string, d) AndAlso
                   run_case(type.var, d) AndAlso
                   run_case(a_struct(), d)
        End Function
    End Class
End Namespace
