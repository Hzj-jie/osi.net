
Option Strict On

Imports osi.root.connector
Imports osi.root.utt

Public Class out_generic_behavior_test
    Inherits [case]

    Private Interface I
    End Interface

    Private Interface I2
    End Interface

    Private Class C
        Implements I
    End Class

    Private Class C2
        Implements I2
    End Class

    Private Class C3
        Implements I, I2
    End Class

    Private Interface TI(Of Out K)
        Function X() As K
        'Function Y(ByVal i As K) As Boolean
    End Interface

    Private Class T(Of K)
        Implements TI(Of K)

        Public Function X() As K Implements TI(Of K).X
        End Function
    End Class

    Private Delegate Function F(Of Out K)() As K
    Private Delegate Function G(Of K)(ByRef o As K) As Boolean

    Private Shared Function case1() As Boolean
        Dim x As TI(Of I) = Nothing
        x = New T(Of C)()
        assertion.is_true(TypeOf x Is TI(Of C))
        assertion.is_true(TypeOf x Is TI(Of I))
        assertion.is_true(TypeOf x Is T(Of C))
        assertion.is_false(TypeOf x Is T(Of I))
        x = New T(Of C3)()
        assertion.is_true(TypeOf x Is TI(Of C3))
        assertion.is_true(TypeOf x Is TI(Of I))
        assertion.is_true(TypeOf x Is T(Of C3))
        assertion.is_false(TypeOf x Is T(Of I))

        assertion.is_true(cast(New T(Of C)(), x))
        assertion.is_false(cast(New T(Of C2)(), x))
        assertion.is_true(cast(New T(Of C3)(), x))

        Return True
    End Function

    Private Shared Function case2() As Boolean
        Dim y As TI(Of I2) = Nothing
        y = New T(Of C2)()
        y = New T(Of C3)()

        Return True
    End Function

    Private Shared Function case3() As Boolean
        Dim f As F(Of I) = Nothing
        f = Function() As C
                Return New C()
            End Function
        assertion.is_true(TypeOf f Is F(Of I))
        assertion.is_false(TypeOf f Is F(Of C))
        assertion.is_true(TypeOf f() Is I)
        assertion.is_true(TypeOf f() Is C)

        f = Function() As C3
                Return New C3()
            End Function
        assertion.is_true(TypeOf f Is F(Of I))
        assertion.is_false(TypeOf f Is F(Of C3))
        assertion.is_true(TypeOf f() Is I)
        assertion.is_true(TypeOf f() Is C3)

        assertion.is_false(cast(Function() As C
                              Return New C()
                          End Function, f))
        assertion.is_false(cast(Function() As C2
                              Return New C2()
                          End Function, f))
        assertion.is_false(cast(Function() As C3
                              Return New C3()
                          End Function, f))

        Return True
    End Function

    Private Shared Function case4() As Boolean
        Dim f As F(Of TI(Of I)) = Nothing
        f = Function() As T(Of C)
                Return New T(Of C)()
            End Function
        assertion.is_true(TypeOf f Is F(Of TI(Of I)))
        assertion.is_false(TypeOf f Is F(Of TI(Of C)))
        assertion.is_false(TypeOf f Is F(Of T(Of I)))
        assertion.is_false(TypeOf f Is F(Of T(Of C)))
        assertion.is_true(TypeOf f() Is TI(Of I))
        assertion.is_true(TypeOf f() Is TI(Of C))
        assertion.is_false(TypeOf f() Is T(Of I))
        assertion.is_true(TypeOf f() Is T(Of C))

        f = Function() As T(Of C3)
                Return New T(Of C3)()
            End Function
        assertion.is_true(TypeOf f Is F(Of TI(Of I)))
        assertion.is_false(TypeOf f Is F(Of TI(Of C3)))
        assertion.is_false(TypeOf f Is F(Of T(Of I)))
        assertion.is_false(TypeOf f Is F(Of T(Of C3)))
        assertion.is_true(TypeOf f() Is TI(Of I))
        assertion.is_true(TypeOf f() Is TI(Of C3))
        assertion.is_false(TypeOf f() Is T(Of I))
        assertion.is_true(TypeOf f() Is T(Of C3))

        Return True
    End Function

    Private Shared Function case5() As Boolean
        Dim f As G(Of TI(Of I)) = Nothing
        f = Function(ByRef r As TI(Of I)) As Boolean
                r = New T(Of C)()
                Return True
            End Function
        Dim o As TI(Of I) = Nothing
        assertion.is_true(f(o))
        assertion.is_true(TypeOf o Is T(Of C))

        Return True
    End Function

    Private Shared Function case6() As Boolean
        Dim x As TI(Of TI(Of I)) = Nothing
        x = New T(Of TI(Of I))()
        x = New T(Of T(Of I))()
        assertion.is_true(TypeOf x Is TI(Of TI(Of I)))
        assertion.is_false(TypeOf x Is T(Of TI(Of I)))
        assertion.is_true(TypeOf x Is T(Of T(Of I)))
        assertion.is_true(TypeOf x Is TI(Of T(Of I)))

        x = New T(Of T(Of C))()
        assertion.is_true(TypeOf x Is TI(Of TI(Of I)))
        assertion.is_false(TypeOf x Is T(Of TI(Of I)))
        assertion.is_false(TypeOf x Is T(Of T(Of I)))
        assertion.is_true(TypeOf x Is TI(Of TI(Of C)))
        assertion.is_false(TypeOf x Is T(Of TI(Of C)))
        assertion.is_true(TypeOf x Is T(Of T(Of C)))
        assertion.is_false(TypeOf x Is TI(Of T(Of I)))
        assertion.is_true(TypeOf x Is TI(Of T(Of C)))

        Return True
    End Function

    Private Shared Function case7() As Boolean
        Dim x As T(Of TI(Of I)) = Nothing
        x = New T(Of TI(Of I))()
        assertion.is_true(TypeOf x Is TI(Of TI(Of I)))
        assertion.is_true(TypeOf x Is T(Of TI(Of I)))
        assertion.is_false(TypeOf x Is TI(Of T(Of I)))

        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return case1() AndAlso case2() AndAlso case3() AndAlso case4() AndAlso case5() AndAlso case6() AndAlso case7()
    End Function
End Class
