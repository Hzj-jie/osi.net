
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt

' Do I need to cache GetType result?
' Conclusion is, obj.GetType() is 3x times slower than a cached value, GetType(T) is a little bit (0.2x) slower.
Public NotInheritable Class gettype_perf
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
        MyBase.New(New value_type(),
                   New string_type(),
                   New interface_type(),
                   New base_type(),
                   New class_type(),
                   New class_type_get_type_vs_type_info())
    End Sub

    Private NotInheritable Class value_type
        Inherits performance_comparison_case_wrapper

        Protected Overrides Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
            Return loosen_bound({7620, 7877, 7104}, i, j)
        End Function

        Public Sub New()
            MyBase.New(round, build_cases(Of Int32)(rnd_int()))
        End Sub
    End Class

    Private NotInheritable Class string_type
        Inherits performance_comparison_case_wrapper

        Protected Overrides Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
            Return loosen_bound({8715, 20469, 7298}, i, j)
        End Function

        Public Sub New()
            MyBase.New(round, build_cases(Of String)(guid_str()))
        End Sub
    End Class

    Private NotInheritable Class interface_type
        Inherits performance_comparison_case_wrapper

        Protected Overrides Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
            Return loosen_bound({8715, 20791, 7616, 20085}, i, j)
        End Function

        Public Sub New()
            MyBase.New(round, build_cases(Of I)(New B()).append(r(New gettype_run2(Of I)(New B()))))
        End Sub
    End Class

    Private NotInheritable Class base_type
        Inherits performance_comparison_case_wrapper

        Protected Overrides Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
            Return loosen_bound({8649, 20147, 7298, 19824}, i, j)
        End Function

        Public Sub New()
            MyBase.New(round, build_cases(Of B)(New D()).append(r(New gettype_run2(Of B)(New D()))))
        End Sub
    End Class

    Private NotInheritable Class class_type
        Inherits performance_comparison_case_wrapper

        Protected Overrides Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
            Return loosen_bound({8393, 20275, 7554}, i, j)
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

    Private NotInheritable Class class_type_get_type_vs_type_info
        Inherits performance_comparison_case_wrapper

        Protected Overrides Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
            Return loosen_bound({8587, 34033}, i, j)
        End Function

        Public Sub New()
            MyBase.New(round,
                       {r(New gettype_run1(Of D)()),
                        r(New type_info_type_run(Of D)())})
        End Sub
    End Class

    Private NotInheritable Class gettype_run1(Of T)
        Inherits [case]

        Public Overrides Function run() As Boolean
            Dim t As Type = Nothing
            t = GetType(T)
            Return True
        End Function
    End Class

    Private NotInheritable Class gettype_run2(Of T)
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

    Private NotInheritable Class cache_type_run(Of T)
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

    Private NotInheritable Class type_info_type_run(Of T)
        Inherits [case]

        ' Simulate type_info.type
        Private NotInheritable Class cache
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
