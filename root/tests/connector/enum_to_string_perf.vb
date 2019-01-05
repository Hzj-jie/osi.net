
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.utt

Public NotInheritable Class enum_to_string_perf
    Inherits performance_comparison_case_wrapper

    Private Shared Function R(ByVal i As [case]) As [case]
        Return repeat(i, 1000000)
    End Function

    Public Sub New()
        MyBase.New(R(New enum_getnames_case()), R(New enum_def_case()))
    End Sub

    Protected Overrides Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        Return loosen_bound({152, 244}, i, j)
    End Function

    Private Enum test_enum
        a
        b
        c
        d
        e
        f
        g
        h
        i
        j
        k
    End Enum

    Private Class enum_getnames_case
        Inherits [case]

        Public Overrides Function run() As Boolean
            [Enum].GetNames(GetType(test_enum))
            Return True
        End Function
    End Class

    Private Class enum_def_case
        Inherits [case]

        Public Overrides Function run() As Boolean
            enum_def(Of test_enum).strings()
            Return True
        End Function
    End Class
End Class
