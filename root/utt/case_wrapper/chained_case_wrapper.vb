
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Public Class chained_case_wrapper
    Inherits [case]

    <Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")>
    Protected Event finished(ByRef finish_result As Boolean)
    Private ReadOnly cs() As [case]
    Private ReadOnly pp As Int16
    Private ReadOnly cwf As Boolean

    Public Sub New(ByVal continue_when_failure As Boolean,
                   ByVal ParamArray cs() As [case])
        Me.cwf = continue_when_failure
        Me.cs = cs
        assert(Not isemptyarray(cs))
        Me.pp = 0
        For i As Int32 = 0 To array_size_i(cs) - 1
            assert(Not cs(i) Is Nothing)
            If cs(i).reserved_processors() > pp Then
                pp = cs(i).reserved_processors()
            End If
        Next
    End Sub

    Public Sub New(ByVal ParamArray cs() As [case])
        Me.New(False, cs)
    End Sub

    Public Function cases() As [case]()
        Return cs
    End Function

    Public Function [case](ByVal i As UInt32) As [case]
        Return cases(CInt(i))
    End Function

    Public Overridable Function continue_when_failure() As Boolean
        Return cwf
    End Function

    Public NotOverridable Overrides Function reserved_processors() As Int16
        Return pp
    End Function

    Public NotOverridable Overrides Function run() As Boolean
        Dim r As Boolean = True
        For i As Int32 = 0 To array_size_i(cs) - 1
            If Not cs(i).run() Then
                r = False
                utt_raise_error("failed to run case ", cs(i).GetType().Name())
            End If
            If Not r AndAlso Not continue_when_failure() Then
                Return False
            End If
        Next
        Return r
    End Function

    Public NotOverridable Overrides Function prepare() As Boolean
        If MyBase.prepare() Then
            For i As Int32 = 0 To array_size_i(cs) - 1
                If Not cs(i).prepare() Then
                    Return False
                End If
            Next
            Return True
        Else
            Return False
        End If
    End Function

    Public NotOverridable Overrides Function finish() As Boolean
        If MyBase.finish() Then
            Dim r As Boolean = False
            r = True
            For i As Int32 = 0 To array_size_i(cs) - 1
                r = r And cs(i).finish()
            Next
            RaiseEvent finished(r)
            Return r
        Else
            Return False
        End If
    End Function
End Class
