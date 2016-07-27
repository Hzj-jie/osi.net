
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.interpreter.fullstack
Imports osi.service.interpreter.fullstack.executor

Namespace fullstack.executor
    Public Class variable_test
        Inherits repeat_case_wrapper

        Public Sub New()
            MyBase.New(New variable_case(), 1024 * 128)
        End Sub

        Private Class variable_case
            Inherits [case]

            Private Shared Function run_case(Of T)(ByVal type As type,
                                                   ByVal i As T,
                                                   ByVal c1 As _do_val_ref(Of variable, T, Boolean),
                                                   ByVal c2 As Func(Of variable, T)) As Boolean
                assert(Not c1 Is Nothing)
                assert(Not c2 Is Nothing)
                Dim v As variable = Nothing
                v = New variable(type, i)
                assert_equal(v, New variable(type, i))
                assert_not_equal(v, Nothing)
                Dim j As T = Nothing
                If cast(Of T)(v.value, j) Then
                    assert_equal(j, i)
                End If
                If Not v.value Is Nothing Then
                    If assert_true(TypeOf v.value Is T) Then
                        assert_equal(cast(Of T)(v.value), i)
                    End If
                End If
                If assert_true(c1(v, j)) Then
                    assert_equal(j, i)
                    assert_equal(c2(v), i)
                End If
                assert_equal(v.true(), Not v.false())
                assert_equal(v.false(), Not v.true())
                Return True
            End Function

            Private Shared Function bool_case() As Boolean
                Return run_case(Type.bool,
                                rnd_bool(),
                                AddressOf bool,
                                AddressOf bool)
            End Function

            Private Shared Function int_case() As Boolean
                Return run_case(Type.int,
                                rnd_int(),
                                AddressOf int,
                                AddressOf int)
            End Function

            Private Shared Function float_case() As Boolean
                Return run_case(Type.float,
                                rnd_double(),
                                AddressOf float,
                                AddressOf float)
            End Function

            Private Shared Function char_case() As Boolean
                Return run_case(Type.char,
                                rndchar(),
                                AddressOf _variable.[char],
                                AddressOf _variable.[char])
            End Function

            Private Shared Function string_case() As Boolean
                Return run_case(Type.string,
                                rndenchars(rnd_int(10, 100)),
                                AddressOf [String],
                                AddressOf [String])
            End Function

            Private Shared Function var_case() As Boolean
                Return run_case(Type.var,
                                rnd_var(),
                                AddressOf var,
                                AddressOf var)
            End Function

            Public Overrides Function run() As Boolean
                Return bool_case() AndAlso
                       int_case() AndAlso
                       float_case() AndAlso
                       char_case() AndAlso
                       string_case() AndAlso
                       var_case()
            End Function
        End Class
    End Class
End Namespace
