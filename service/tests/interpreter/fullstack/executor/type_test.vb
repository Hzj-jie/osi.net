
Imports osi.root.connector
Imports osi.root.utt
Imports osi.service.interpreter.fullstack
Imports osi.service.interpreter.fullstack.executor

Namespace fullstack.executor
    Public Class type_test
        Inherits [case]

        Private Shared Function run_case(ByVal t As Type,
                                         Optional ByVal should_be_bool As Boolean = False,
                                         Optional ByVal should_be_int As Boolean = False,
                                         Optional ByVal should_be_float As Boolean = False,
                                         Optional ByVal should_be_char As Boolean = False,
                                         Optional ByVal should_be_string As Boolean = False,
                                         Optional ByVal should_be_var As Boolean = False,
                                         Optional ByVal should_be_struct As Boolean = False) As Boolean
            assert(Not t Is Nothing)
            assertion.equal(t.is_bool(), should_be_bool)
            assertion.equal(t.is_int(), should_be_int)
            assertion.equal(t.is_float(), should_be_float)
            assertion.equal(t.is_char(), should_be_char)
            assertion.equal(t.is_string(), should_be_string)
            assertion.equal(t.is_var(), should_be_var)
            assertion.equal(t.is_struct(), should_be_struct)
            assertion.equal(t.is_type(type.bool), should_be_bool)
            assertion.equal(t.is_type(type.int), should_be_int)
            assertion.equal(t.is_type(type.float), should_be_float)
            assertion.equal(t.is_type(type.char), should_be_char)
            assertion.equal(t.is_type(type.string), should_be_string)
            assertion.equal(t.is_type(type.var), should_be_var)
            'two types with exactly same definition should be considered as same type
            assertion.equal(t.is_type(a_struct()), should_be_struct)
            Return True
        End Function

        Public Overrides Function run() As Boolean
            Return run_case(type.bool, should_be_bool:=True) AndAlso
                   run_case(type.int, should_be_int:=True) AndAlso
                   run_case(type.float, should_be_float:=True) AndAlso
                   run_case(type.char, should_be_char:=True) AndAlso
                   run_case(type.string, should_be_string:=True) AndAlso
                   run_case(type.var, should_be_var:=True) AndAlso
                   run_case(a_struct(), should_be_struct:=True)
        End Function
    End Class
End Namespace
