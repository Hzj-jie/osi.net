
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils

Public Class chained_case_wrapper
    Inherits [case]

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
        For i As UInt32 = 0 To array_size(cs) - uint32_1
            assert(Not cs(i) Is Nothing)
            If cs(i).preserved_processors() > pp Then
                pp = cs(i).preserved_processors()
            End If
        Next
    End Sub

    Public Sub New(ByVal ParamArray cs() As [case])
        Me.New(False, cs)
    End Sub

    Public Function cases() As [case]()
        Return cs
    End Function

    Public Overridable Function continue_when_failure() As Boolean
        Return cwf
    End Function

    Public NotOverridable Overrides Function preserved_processors() As Int16
        Return pp
    End Function

    Public NotOverridable Overrides Function run() As Boolean
        Dim r As Boolean = True
        For i As UInt32 = 0 To array_size(cs) - uint32_1
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
            For i As UInt32 = 0 To array_size(cs) - uint32_1
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
            For i As UInt32 = 0 To array_size(cs) - uint32_1
                r = r And cs(i).finish()
            Next
            RaiseEvent finished(r)
            Return r
        Else
            Return False
        End If
    End Function
End Class
