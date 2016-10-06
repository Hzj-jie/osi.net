
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.interpreter.turing
Imports osi.service.interpreter.turing.executor

Namespace turing.executor
    'TODO: merge two types / variables
#If 0 Then
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
                If cast(Of T)(v.get_value(), j) Then
                    assert_equal(j, i)
                End If
                If Not v.get_value() Is Nothing Then
                    If assert_true(TypeOf v.get_value() Is T) Then
                        assert_equal(cast(Of T)(v.get_value()), i)
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
                Return run_case(type.bool,
                                rnd_bool(),
                                AddressOf bool,
                                AddressOf bool)
            End Function

            Private Shared Function int_case() As Boolean
                Return run_case(type.int,
                                rnd_int(),
                                AddressOf int,
                                AddressOf int)
            End Function

            Private Shared Function float_case() As Boolean
                Return run_case(type.float,
                                rnd_double(),
                                AddressOf float,
                                AddressOf float)
            End Function

            Private Shared Function char_case() As Boolean
                Return run_case(type.char,
                                rnd_char(),
                                AddressOf _variable.[char],
                                AddressOf _variable.[char])
            End Function

            Private Shared Function string_case() As Boolean
                Return run_case(type.string,
                                rnd_en_chars(rnd_int(10, 100)),
                                AddressOf [string],
                                AddressOf [string])
            End Function

            Public Overrides Function run() As Boolean
                Return bool_case() AndAlso
                       int_case() AndAlso
                       float_case() AndAlso
                       char_case() AndAlso
                       string_case()
            End Function
        End Class
    End Class
#End If
End Namespace
