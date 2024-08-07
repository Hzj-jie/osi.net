
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.template

Partial Public Class check(Of IS_TRUE_FUNC As __void(Of Boolean, Object()))
    Public Shared Function array_equal(Of T)(ByVal i() As T,
                                             ByVal j() As T,
                                             ByVal ParamArray msg() As Object) As Boolean
        If Not equal(connector.array_size(i), connector.array_size(j), msg) Then
            Return False
        End If
        For k As Int32 = 0 To array_size_i(i) - 1
            If Not equal(i(k), j(k), msg) Then
                Return False
            End If
        Next
        Return True
    End Function

    Public Shared Function array_not_equal(Of T)(ByVal i() As T,
                                                 ByVal j() As T,
                                                 ByVal ParamArray msg() As Object) As Boolean
        If connector.array_size(i) <> connector.array_size(j) Then
            Return True
        End If
        For k As Int32 = 0 To array_size_i(i) - 1
            If compare(i(k), j(k)) <> 0 Then
                Return True
            End If
        Next
        Return is_true(False, msg)
    End Function

    Public Shared Function array_size(Of T)(ByVal i() As T,
                                            ByVal size As UInt32,
                                            ByVal ParamArray msg() As Object) As Boolean
        Return equal(connector.array_size(i), size, msg)
    End Function

    Public Shared Function array_empty(Of T)(ByVal i() As T, ByVal ParamArray msg() As Object) As Boolean
        Return array_size(i, uint32_0, msg)
    End Function

    Public Shared Function array_not_empty(Of T)(ByVal i() As T, ByVal ParamArray msg() As Object) As Boolean
        Return not_equal(connector.array_size(i), uint32_0, msg)
    End Function
End Class
