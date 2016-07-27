
Imports osi.root.connector
Imports osi.root.utt

' Do I need to cache GetType result?
' Conclusion is, obj.GetType() is 3x times slower than a cached value, GetType(T) is a little bit (0.2x) slower.
Public Class gettype_perf
    Inherits chained_case_wrapper

    Private Const round As Int64 = 3

    Private Interface I
    End Interface

    Private Class B
        Implements I
    End Class

    Private Class D
        Inherits B
    End Class

    Public Sub New()
        MyBase.New(New case1(), New case2(), New case3(), New case4(), New case5(), New case6())
    End Sub

    Private Class case1
        Inherits performance_comparison_case_wrapper

        Protected Overrides Function min_rate_table() As Double(,)
            Return {{0, 0.8, 2},
                    {4, 0, 7},
                    {0.9, 0.6, 0}}
        End Function

        Public Sub New()
            MyBase.New(round, build_cases(Of Int32)(rnd_int()))
        End Sub
    End Class

    Private Class case2
        Inherits performance_comparison_case_wrapper

        Protected Overrides Function min_rate_table() As Double(,)
            Return {{0, 1.2, 4},
                    {1.5, 0, 4},
                    {0.8, 0.8, 0}}
        End Function

        Public Sub New()
            MyBase.New(round, build_cases(Of String)(guid_str()))
        End Sub
    End Class

    Private Class case3
        Inherits performance_comparison_case_wrapper

        Protected Overrides Function min_rate_table() As Double(,)
            Return {{0, 1.2, 4, 1.2},
                    {1.5, 0, 4, 1.2},
                    {0.8, 0.8, 0, 0.8},
                    {1.5, 1.2, 3.5, 0}}
        End Function

        Public Sub New()
            MyBase.New(round, build_cases(Of I)(New B()).append(r(New gettype_run2(Of I)(New B()))))
        End Sub
    End Class

    Private Class case4
        Inherits performance_comparison_case_wrapper

        Protected Overrides Function min_rate_table() As Double(,)
            Return {{0, 1.2, 4, 1.2},
                    {1.5, 0, 4, 1.2},
                    {0.8, 0.8, 0, 0.8},
                    {1.5, 1.2, 3.5, 0}}
        End Function

        Public Sub New()
            MyBase.New(round, build_cases(Of B)(New D()).append(r(New gettype_run2(Of B)(New D()))))
        End Sub
    End Class

    Private Class case5
        Inherits performance_comparison_case_wrapper

        Protected Overrides Function min_rate_table() As Double(,)
            Return {{0, 1.2, 4},
                    {1.5, 0, 4},
                    {0.8, 0.8, 0}}
        End Function

        Public Sub New()
            MyBase.New(round, build_cases(Of D)(New D()))
        End Sub
    End Class

    Private Shared Function r(ByVal c As [case]) As [case]
        Return repeat(c, 200000000)
    End Function

    Private Shared Function build_cases(Of T)(ByVal i As T) As [case]()
        Return {r(New gettype_run1(Of T)()),
                r(New gettype_run2(Of T)(i)),
                r(New cache_type_run(Of T)(i))}
    End Function

    Private Class case6
        Inherits performance_comparison_case_wrapper

        Protected Overrides Function min_rate_table() As Double(,)
            Return {{0, 0.7},
                    {1.8, 0}}
        End Function

        Public Sub New()
            MyBase.New(round,
                       {r(New gettype_run1(Of D)()),
                        r(New type_info_type_run(Of D)())})
        End Sub
    End Class

    Private Class gettype_run1(Of T)
        Inherits [case]

        Public Overrides Function run() As Boolean
            Dim t As Type = Nothing
            t = GetType(T)
            Return True
        End Function
    End Class

    Private Class gettype_run2(Of T)
        Inherits [case]

        Private ReadOnly v As T

        Public Sub New(ByVal v As T)
            assert(Not v Is Nothing)
            Me.v = v
        End Sub

        Public Overrides Function run() As Boolean
            Dim t As Type = Nothing
            t = v.GetType()
            Return True
        End Function
    End Class

    Private Class cache_type_run(Of T)
        Inherits [case]

        Private ReadOnly i As Type

        Public Sub New(ByVal v As T)
            assert(Not v Is Nothing)
            Me.i = v.GetType()
        End Sub

        Public Overrides Function run() As Boolean
            Dim x As Type = Nothing
            x = i
            Return True
        End Function
    End Class

    Private Class type_info_type_run(Of T)
        Inherits [case]

        ' Simulate type_info.type
        Private Class cache
            Public Shared ReadOnly type As Type

            Shared Sub New()
                type = GetType(T)
            End Sub
        End Class

        Public Overrides Function run() As Boolean
            Dim x As Type = Nothing
            x = cache.type
            Return True
        End Function
    End Class
End Class
