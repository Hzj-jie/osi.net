
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils
Imports osi.root.utt

Public Class comparable_type_test
    Inherits [case]

    Private Class c1
    End Class

    Private Class c2
    End Class

    Private Class c3
    End Class

    Private Shared Function run_case(ByVal t1 As Type, ByVal t2 As Type, ByVal exp As Int32) As Boolean
        Dim f As Func(Of Int32, Int32, Boolean) = Nothing
        If exp > 0 Then
            f = AddressOf assertion.more
        ElseIf exp = 0 Then
            f = AddressOf assertion.equal
        Else
            f = AddressOf assertion.less
        End If
        assert(Not f Is Nothing)
        f(compare(New comparable_type(t1), New comparable_type(t2)), 0)
        f(compare(New comparable_type(t1), t2), 0)
        f(compare(New comparable_type(t1), DirectCast(t2, Object)), 0)
        f((New comparable_type(t1)).CompareTo(New comparable_type(t2)), 0)
        f((New comparable_type(t1)).CompareTo(t2), 0)
        f((New comparable_type(t1)).CompareTo(DirectCast(t2, Object)), 0)
        Return True
    End Function

    Private Shared Function run_case(Of T1, T2)(ByVal exp As Int32) As Boolean
        Return run_case(GetType(T1), GetType(T2), exp)
    End Function

    Private Shared Function run_case(Of T)() As Boolean
        Return run_case(Of T, T)(0)
    End Function

    Private Shared Function run_case2() As Boolean
        assertion.not_equal(comparable_type.compare_type(GetType(c1), GetType(c2)), 0)
        assertion.not_equal(comparable_type.compare_type(GetType(c1), GetType(c3)), 0)
        assertion.not_equal(comparable_type.compare_type(GetType(c2), GetType(c3)), 0)
        Dim t1 As Type = Nothing
        Dim t2 As Type = Nothing
        Dim t3 As Type = Nothing
        If comparable_type.compare_type(GetType(c1), GetType(c2)) > 0 Then
            t1 = GetType(c1)
            t2 = GetType(c2)
        Else
            t1 = GetType(c2)
            t2 = GetType(c1)
        End If
        If comparable_type.compare_type(t2, GetType(c3)) > 0 Then
            t3 = GetType(c3)
        ElseIf comparable_type.compare_type(GetType(c3), t1) > 0 Then
            t3 = t2
            t2 = t1
            t1 = GetType(c3)
        Else
            assertion.is_true(comparable_type.compare_type(GetType(c3), t1) < 0 AndAlso
                        comparable_type.compare_type(GetType(c3), t2) > 0)
            t3 = t2
            t2 = GetType(c3)
        End If
        Return run_case(t1, t2, 1) AndAlso
               run_case(t2, t3, 1) AndAlso
               run_case(t1, t3, 1) AndAlso
               run_case(t1, t1, 0) AndAlso
               run_case(t2, t2, 0) AndAlso
               run_case(t3, t3, 0) AndAlso
               run_case(t2, t1, -1) AndAlso
               run_case(t3, t2, -1) AndAlso
               run_case(t3, t1, -1)
        Return True
    End Function

    Private Shared Function not_equal_case(ByVal t1 As Type, ByVal t2 As Type) As Boolean
        assertion.not_equal(comparable_type.compare_type(t1, t2), 0)
        Return True
    End Function

    Private Shared Function run_case3() As Boolean
        Return run_case(GetType(vector(Of Int32)), GetType(vector(Of Integer)), 0) AndAlso
               not_equal_case(GetType(vector(Of Int32)), GetType(vector(Of UInt32))) AndAlso
               not_equal_case(GetType(map(Of Int32, Int32)), GetType(map(Of UInt32, Int32))) AndAlso
               not_equal_case(GetType(map(Of Int32, vector(Of Int32))), GetType(map(Of Int32, vector(Of UInt32))))
    End Function

    Public Overrides Function run() As Boolean
        Return run_case(Of c1)() AndAlso
               run_case(Of c2)() AndAlso
               run_case(Of c3)() AndAlso
               run_case2() AndAlso
               run_case3()
    End Function
End Class
