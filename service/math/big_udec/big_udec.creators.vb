
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public NotInheritable Class big_udec
    Public Shared Function fraction(ByVal numerator As big_uint,
                                    ByVal denominator As big_uint,
                                    ByRef o As big_udec) As Boolean
        Dim r As big_udec = Nothing
        r = big_udec.zero()
        If r.replace_by(numerator, denominator) Then
            o = r
            Return True
        End If
        Return False
    End Function

    Public Shared Function fraction(ByVal numerator As big_uint, ByVal denominator As big_uint) As big_udec
        Dim r As big_udec = Nothing
        assert(fraction(numerator, denominator, r))
        Return r
    End Function

    Public Shared Function fraction(ByVal numerator As Int32,
                                    ByVal denominator As Int32,
                                    ByRef o As big_udec) As Boolean
        If numerator < 0 AndAlso denominator < 0 Then
            Return fraction(New big_uint(CUInt(-numerator)), New big_uint(CUInt(-denominator)), o)
        End If
        If numerator >= 0 AndAlso denominator >= 0 Then
            Return fraction(New big_uint(CUInt(numerator)), New big_uint(CUInt(denominator)), o)
        End If
        Return False
    End Function

    Public Shared Function fraction(ByVal numerator As Int32, ByVal denominator As Int32) As big_udec
        Dim r As big_udec = Nothing
        assert(fraction(numerator, denominator, r))
        Return r
    End Function

    Public Shared Function fraction(ByVal numerator As Int64,
                                    ByVal denominator As Int64,
                                    ByRef o As big_udec) As Boolean
        If numerator < 0 AndAlso denominator < 0 Then
            Return fraction(New big_uint(CULng(-numerator)), New big_uint(CULng(-denominator)), o)
        End If
        If numerator >= 0 AndAlso denominator >= 0 Then
            Return fraction(New big_uint(CULng(numerator)), New big_uint(CULng(denominator)), o)
        End If
        Return False
    End Function

    Public Shared Function fraction(ByVal numerator As Int64, ByVal denominator As Int64) As big_udec
        Dim r As big_udec = Nothing
        assert(fraction(numerator, denominator, r))
        Return r
    End Function

    Public Shared Function fraction(ByVal numerator As UInt32,
                                    ByVal denominator As UInt32,
                                    ByRef o As big_udec) As Boolean
        Return fraction(New big_uint(numerator), New big_uint(denominator), o)
    End Function

    Public Shared Function fraction(ByVal numerator As UInt32, ByVal denominator As UInt32) As big_udec
        Dim r As big_udec = Nothing
        assert(fraction(numerator, denominator, r))
        Return r
    End Function

    Public Shared Function fraction(ByVal numerator As UInt64,
                                    ByVal denominator As UInt64,
                                    ByRef o As big_udec) As Boolean
        Return fraction(New big_uint(numerator), New big_uint(denominator), o)
    End Function

    Public Shared Function fraction(ByVal numerator As UInt64, ByVal denominator As UInt64) As big_udec
        Dim r As big_udec = Nothing
        assert(fraction(numerator, denominator, r))
        Return r
    End Function
End Class
