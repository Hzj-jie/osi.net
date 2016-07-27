
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
            assert_equal(t.is_bool(), should_be_bool)
            assert_equal(t.is_int(), should_be_int)
            assert_equal(t.is_float(), should_be_float)
            assert_equal(t.is_char(), should_be_char)
            assert_equal(t.is_string(), should_be_string)
            assert_equal(t.is_var(), should_be_var)
            assert_equal(t.is_struct(), should_be_struct)
            assert_equal(t.is_type(Type.bool), should_be_bool)
            assert_equal(t.is_type(Type.int), should_be_int)
            assert_equal(t.is_type(Type.float), should_be_float)
            assert_equal(t.is_type(Type.char), should_be_char)
            assert_equal(t.is_type(Type.string), should_be_string)
            assert_equal(t.is_type(Type.var), should_be_var)
            'two types with exactly same definition should be considered as same type
            assert_equal(t.is_type(a_struct()), should_be_struct)
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
