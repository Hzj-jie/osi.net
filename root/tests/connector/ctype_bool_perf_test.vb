
Imports osi.root.connector
Imports osi.root.utt

Public Class ctype_bool_perf_test
    Inherits performance_comparison_case_wrapper

    Private Shared Function r(ByVal c As [case]) As [case]
        Return repeat(c, 100000000)
    End Function

    Public Sub New()
        MyBase.New(r(New implicit_case()), r(New ctype_case()), r(New inline_case()))
    End Sub

    Protected Overrides Function min_rate_table() As Double(,)
        Return {{0, 1.5, 1.5},
                {1.5, 0, 1.5},
                {1.5, 1.5, 0}}
    End Function

    Private Class test_class
        Public ReadOnly v As Int32

        Public Sub New(ByVal v As Int32)
            Me.v = v
        End Sub

        Public Shared Widening Operator CType(ByVal this As test_class) As Boolean
            If this Is Nothing Then
                Return False
            Else
                Return (this.v And 1) = 1
            End If
        End Operator
    End Class

    Private Class implicit_case
        Inherits [case]

        Public Overrides Function run() As Boolean
            Dim b As Boolean = False
            Dim v As test_class = Nothing
            v = New test_class(rnd_int())
            b = v
            Return True
        End Function
    End Class

    Private Class ctype_case
        Inherits [case]

        Public Overrides Function run() As Boolean
            Dim b As Boolean = False
            Dim v As test_class = Nothing
            v = New test_class(rnd_int())
            b = CType(v, Boolean)
            Return True
        End Function
    End Class

    Private Class inline_case
        Inherits [case]

        Public Overrides Function run() As Boolean
            Dim b As Boolean = False
            Dim v As test_class = Nothing
            v = New test_class(rnd_int())
            If v Is Nothing Then
                b = False
            Else
                b = ((v.v And 1) = 1)
            End If
            Return True
        End Function
    End Class
End Class
