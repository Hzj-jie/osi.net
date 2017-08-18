
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports osi.root.constants

Public Module _custom_attributes
    Public Function custom_attribute(Of T, AT As Attribute) _
                                    (ByRef o As AT,
                                     Optional ByVal inherit As Boolean = False) As Boolean
        Return GetType(T).custom_attribute(o, inherit)
    End Function

    <Extension()> Public Function custom_attribute(Of AT As Attribute) _
                                                  (ByVal this As MemberInfo,
                                                   ByRef o As AT,
                                                   Optional ByVal inherit As Boolean = False) As Boolean
        Dim ats() As AT = Nothing
        If custom_attributes(this, ats) Then
            assert(Not isemptyarray(ats))
            o = ats(0)
            Return True
        Else
            Return False
        End If
    End Function

    <Extension()> Public Function custom_attribute(Of AT As Attribute)(ByVal this As MemberInfo,
                                                                       Optional ByVal inherit As Boolean = False) As AT
        Dim o As AT = Nothing
        assert(this.custom_attribute(o, inherit))
        Return o
    End Function

    Public Function custom_attribute(Of T, AT As Attribute)(Optional ByVal inherit As Boolean = False) As AT
        Dim o As AT = Nothing
        assert(custom_attribute(Of T, AT)(o, inherit))
        Return o
    End Function

    <Extension()> Public Function has_custom_attribute(Of AT As Attribute) _
                                                      (ByVal this As MemberInfo,
                                                       Optional ByVal inherit As Boolean = False) As Boolean
        Dim o As AT = Nothing
        Return this.custom_attribute(o, inherit)
    End Function

    Public Function custom_attributes(Of T, AT As Attribute) _
                                     (ByRef o() As AT,
                                      Optional ByVal inherit As Boolean = False) As Boolean
        Return GetType(T).custom_attributes(o, inherit)
    End Function

    <Extension()> Public Function custom_attributes(Of AT As Attribute) _
                                                   (ByVal this As MemberInfo,
                                                    ByRef o() As AT,
                                                    Optional ByVal inherit As Boolean = False) As Boolean
        If this Is Nothing Then
            Return False
        Else
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
            Else
                ReDim o(array_size_i(objs) - 1)
                For i As Int32 = 0 To array_size_i(objs) - 1
                    assert(cast(objs(i), o(i)))
                Next
                Return True
            End If
        End If
    End Function

    <Extension()> Public Function custom_attributes(Of AT As Attribute) _
                                                   (ByVal this As MemberInfo,
                                                    Optional ByVal inherit As Boolean = False) As AT()
        Dim o() As AT = Nothing
        assert(this.custom_attributes(o, inherit))
        Return o
    End Function

    Public Function custom_attributes(Of T, AT As Attribute)(Optional ByVal inherit As Boolean = False) As AT()
        Dim o() As AT = Nothing
        assert(custom_attributes(Of T, AT)(o, inherit))
        Return o
    End Function
End Module
