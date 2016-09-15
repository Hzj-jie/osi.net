
Imports osi.root.constants
Imports osi.root.connector

Public Class repeat_case_wrapper
    Inherits case_wrapper

    <ThreadStatic()> Public Shared this As repeat_case_wrapper
    Private ReadOnly size As Int64
    Private i As Int64

    Public Sub New(ByVal c As [case], Optional ByVal test_size As Int64 = npos)
        MyBase.New(c)
        Me.size = test_size
    End Sub

    Public Function current_round() As Int64
        Return i
    End Function

    Public Function last_round() As Boolean
        Return i = test_size() - 1
    End Function

    Protected Overridable Function test_size() As Int64
        Return size
    End Function

    Public NotOverridable Overrides Function run() As Boolean
        assert(test_size() > 0)
        this = Me
        For Me.i = 0 To test_size() - 1
            If Not MyBase.run() Then
                this = Nothing
                Return False
            End If
        Next
        this = Nothing
        Return True
    End Function
End Class

Public Class rinne_case_wrapper
    Inherits case_wrapper

    Private ReadOnly size As Int64 = 0

    Public Sub New(ByVal c As [case], Optional ByVal test_size As Int64 = npos)
        MyBase.New(c)
        Me.size = test_size
    End Sub

    Protected Overridable Function test_size() As Int64
        Return size
    End Function

    Public NotOverridable Overrides Function prepare() As Boolean
        Return MyBase.case_prepare_proxy()
    End Function

    Public NotOverridable Overrides Function run() As Boolean
        assert(test_size() > 0)
        For i As Int64 = 0 To test_size() - 1
            If Not (MyBase.prepare() AndAlso
                    MyBase.run() AndAlso
                    MyBase.finish()) Then
                Return False
            End If
        Next
        Return True
    End Function

    Public NotOverridable Overrides Function finish() As Boolean
        Return MyBase.case_finish_proxy()
    End Function
End Class