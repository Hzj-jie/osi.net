
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
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

    Public Shared Function vector_equal(Of T)(ByVal i As vector(Of T),
                                              ByVal j As vector(Of T),
                                              ByVal ParamArray msg() As Object) As Boolean
        Return array_equal(+i, +j, msg)
    End Function

    Public Shared Function vector_not_equal(Of T)(ByVal i As vector(Of T),
                                                  ByVal j As vector(Of T),
                                                  ByVal ParamArray msg() As Object) As Boolean
        Return array_not_equal(+i, +j, msg)
    End Function
End Class
