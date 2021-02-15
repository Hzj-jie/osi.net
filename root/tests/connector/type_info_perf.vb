
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt

' type_info(Of T1, equal, T2) is 100% faster than GetType(T1).Equals(GetType(T2)).
Public NotInheritable Class type_info_equal_perf
    Inherits performance_comparison_case_wrapper

    Private Const size As Int64 = 1024 * 1024 * 64

    Public Sub New()
        MyBase.New(repeat(New type_info_equal(), size), repeat(New equal(), size))
    End Sub

    Protected Overrides Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        Return loosen_bound({34, 1286}, i, j)
    End Function

    Private NotInheritable Class type_info_equal
        Inherits [case]

        Public Overrides Function run() As Boolean
            Dim b As Boolean = False
            b = type_info(Of String, type_info_operators.equal, String).v
            b = type_info(Of Int32, type_info_operators.equal, Int32).v
            b = type_info(Of Int32, type_info_operators.equal, String).v
            b = type_info(Of type_info_equal_perf, type_info_operators.equal, String).v
            b = type_info(Of type_info_equal_perf, type_info_operators.equal, type_info_equal_perf).v
            b = type_info(Of Int32, type_info_operators.equal, type_info_equal_perf).v
            b = type_info(Of SByte, type_info_operators.equal, SByte).v
            b = type_info(Of SByte, type_info_operators.equal, Byte).v
            Return True
        End Function
    End Class

    Private NotInheritable Class equal
        Inherits [case]

        Public Overrides Function run() As Boolean
            Dim b As Boolean = False
            b = GetType(String).Equals(GetType(String))
            b = GetType(Int32).Equals(GetType(Int32))
            b = GetType(Int32).Equals(GetType(String))
            b = GetType(type_info_equal_perf).Equals(GetType(String))
            b = GetType(type_info_equal_perf).Equals(GetType(type_info_equal_perf))
            b = GetType(Int32).Equals(GetType(type_info_equal_perf))
            b = GetType(SByte).Equals(GetType(SByte))
            b = GetType(SByte).Equals(GetType(Byte))
            Return True
        End Function
    End Class
End Class
