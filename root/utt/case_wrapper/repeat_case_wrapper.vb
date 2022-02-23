
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utils

Public Class repeat_case_wrapper
    Inherits case_wrapper

    <ThreadStatic> Private Shared this As repeat_case_wrapper
    <ThreadStatic> Private Shared this_round As Func(Of UInt64)
    Private ReadOnly size As UInt64

    Public Sub New(ByVal c As [case], Optional ByVal test_size As UInt64 = 1)
        MyBase.New(c)
        Me.size = test_size
    End Sub

    Public Sub New(ByVal c As [case], ByVal test_size As Int64)
        Me.New(c, assert_which.of(test_size).can_cast_to_uint64())
    End Sub

    Public Shared Function current() As repeat_case_wrapper
        assert(this IsNot Nothing)
        Return this
    End Function

    Public Shared Function current_round() As UInt64
        assert(this_round IsNot Nothing)
        Return this_round()
    End Function

    Protected Overridable Function test_size() As UInt64
        Return size
    End Function

    Public NotOverridable Overrides Function run() As Boolean
        Dim i As UInt64 = 0
        this = Me
        this_round = Function() As UInt64
                         Return i
                     End Function
        Using defer.to(Sub()
                           this = Nothing
                           this_round = Nothing
                       End Sub)
            assert(test_size() > 0)
            For i = 0 To test_size() - uint64_1
                If Not MyBase.run() Then
                    Return False
                End If
            Next
            Return True
        End Using
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
        Return MyBase.mybase_prepare()
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
        Return MyBase.mybase_finish()
    End Function
End Class