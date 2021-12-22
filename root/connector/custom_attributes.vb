
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports System.Runtime.CompilerServices

Public Module _custom_attributes
    Public Function custom_attribute(Of T, AT)(ByRef o As AT,
                                               Optional ByVal inherit As Boolean = False) As Boolean
        Return GetType(T).custom_attribute(o, inherit)
    End Function

    <Extension()> Public Function custom_attribute(Of AT)(ByVal this As MemberInfo,
                                                          ByRef o As AT,
                                                          Optional ByVal inherit As Boolean = False) As Boolean
        Dim ats() As AT = Nothing
        If Not custom_attributes(this, ats) Then
            Return False
        End If
        assert(Not isemptyarray(ats))
        o = ats(0)
        Return True
    End Function

    <Extension()> Public Function custom_attribute(Of AT)(ByVal this As MemberInfo,
                                                          Optional ByVal inherit As Boolean = False) As AT
        Dim o As AT = Nothing
        assert(this.custom_attribute(o, inherit))
        Return o
    End Function

    Public Function custom_attribute(Of T, AT)(Optional ByVal inherit As Boolean = False) As AT
        Dim o As AT = Nothing
        assert(custom_attribute(Of T, AT)(o, inherit))
        Return o
    End Function

    <Extension()> Public Function has_custom_attribute(Of AT)(ByVal this As MemberInfo,
                                                              Optional ByVal inherit As Boolean = False) As Boolean
        Dim o As AT = Nothing
        Return this.custom_attribute(o, inherit)
    End Function

    Public Function custom_attributes(Of T, AT)(ByRef o() As AT,
                                                Optional ByVal inherit As Boolean = False) As Boolean
        Return GetType(T).custom_attributes(o, inherit)
    End Function

    <Extension()> Public Function custom_attributes(Of AT)(ByVal this As MemberInfo,
                                                           ByRef o() As AT,
                                                           Optional ByVal inherit As Boolean = False) As Boolean
        If this Is Nothing Then
            Return False
        End If
        Dim objs() As Object = Nothing
        Try
            objs = this.GetCustomAttributes(GetType(AT), inherit)
        Catch ex As Exception
            raise_error("failed to get attributes of type ",
                            this.full_name(),
                            ", ex ",
                            ex.Message())
            Return False
        End Try

        If isemptyarray(objs) Then
            Return False
        End If
        ReDim o(array_size_i(objs) - 1)
        For i As Int32 = 0 To array_size_i(objs) - 1
            o(i) = direct_cast(Of AT)(objs(i))
        Next
        Return True
    End Function

    <Extension()> Public Function custom_attributes(Of AT)(ByVal this As MemberInfo,
                                                           Optional ByVal inherit As Boolean = False) As AT()
        Dim o() As AT = Nothing
        assert(this.custom_attributes(o, inherit))
        Return o
    End Function

    Public Function custom_attributes(Of T, AT)(Optional ByVal inherit As Boolean = False) As AT()
        Dim o() As AT = Nothing
        assert(custom_attributes(Of T, AT)(o, inherit))
        Return o
    End Function
End Module
