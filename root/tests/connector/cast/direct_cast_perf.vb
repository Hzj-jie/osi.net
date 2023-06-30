
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt

' Performance of direct_cast(Of T)() is on-par with DirectCast().
Public Class direct_cast_perf
    Inherits performance_comparison_case_wrapper

    Private Interface i1
    End Interface

    Private Interface i2
        Inherits i1
    End Interface

    Private Class c1
    End Class

    Private Class c2
        Inherits c1
    End Class

    Private Class c3
        Implements i1
    End Class

    Private Class c4
        Inherits c3
        Implements i2
    End Class

    Private Shared ReadOnly i1obj As i1
    Private Shared ReadOnly i2obj As i2
    Private Shared ReadOnly c1obj As c1
    Private Shared ReadOnly c2obj As c2
    Private Shared ReadOnly c3obj As c3
    Private Shared ReadOnly c4obj As c4

    Private Shared i1obj2 As i1
    Private Shared i2obj2 As i2
    Private Shared c1obj2 As c1
    Private Shared c2obj2 As c2
    Private Shared c3obj2 As c3
    Private Shared c4obj2 As c4

    Shared Sub New()
        i1obj = New c3()
        i2obj = New c4()
        c1obj = New c1()
        c2obj = New c2()
        c3obj = New c3()
        c4obj = New c4()
    End Sub

    Private Shared Function R(ByVal c As [case]) As [case]
        Return repeat(c, 1024 * 128)
    End Function

    Public Sub New()
        MyBase.New(R(New direct_cast_case()), R(New DirectCast_case()))
    End Sub

    Protected Overrides Function min_rate_table() As Double(,)
        Return {{0, 1.5},
                {1.5, 0}}
    End Function

    Private Class direct_cast_case
        Inherits [case]

        Public Overrides Function run() As Boolean
            assertion.is_false(direct_cast(i1obj, i2obj2))
            assertion.is_true(direct_cast(i2obj, i1obj2))
            assertion.is_false(direct_cast(c1obj, c2obj2))
            assertion.is_true(direct_cast(c2obj, c1obj2))
            assertion.is_true(direct_cast(c3obj, i1obj2))
            assertion.is_true(direct_cast(i1obj, c3obj2))
            assertion.is_false(direct_cast(c3obj, c4obj2))
            assertion.is_true(direct_cast(c4obj, c3obj2))
            assertion.is_true(direct_cast(c4obj, i2obj2))
            assertion.is_true(direct_cast(i2obj, c4obj2))

            Return True
        End Function
    End Class

    Private Class DirectCast_case
        Inherits [case]

        Public Overrides Function run() As Boolean
            Try
                i2obj2 = DirectCast(i1obj, i2)
                assertion.is_true(False)
            Catch ex As Exception
            End Try

            Try
                i1obj2 = DirectCast(i2obj, i1)
            Catch ex As Exception
                assertion.is_true(False)
            End Try

            Try
                c2obj2 = DirectCast(c1obj, c2)
                assertion.is_true(False)
            Catch ex As Exception
            End Try

            Try
                c1obj2 = DirectCast(c2obj, c1)
            Catch ex As Exception
                assertion.is_true(False)
            End Try

            Try
                i1obj2 = DirectCast(c3obj, i1)
            Catch ex As Exception
                assertion.is_true(False)
            End Try

            Try
                c3obj2 = DirectCast(i1obj, c3)
            Catch ex As Exception
                assertion.is_true(False)
            End Try

            Try
                c4obj2 = DirectCast(DirectCast(c3obj, Object), c4)
                assertion.is_true(False)
            Catch ex As Exception
            End Try

            Try
                c3obj2 = DirectCast(c4obj, c3)
            Catch ex As Exception
                assertion.is_true(False)
            End Try

            Try
                i2obj2 = DirectCast(c4obj, i2)
            Catch ex As Exception
                assertion.is_true(False)
            End Try

            Try
                c4obj2 = DirectCast(i2obj, c4)
            Catch ex As Exception
                assertion.is_true(False)
            End Try

            Return True
        End Function
    End Class
End Class
