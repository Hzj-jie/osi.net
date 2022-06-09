
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports cons = osi.service.configuration.constants.compare_filter

Public Class compare_filter(Of T)
    Inherits cast_compare_validation(Of T, s)
    Implements ifilter

    Public Enum compare_method
        less
        less_or_equal
        larger
        larger_or_equal
        equal
    End Enum

    Public Structure s
        Public ReadOnly cm As compare_method
        Public ReadOnly v As T

        Public Sub New(ByVal cm As compare_method, ByVal v As T)
            Me.cm = cm
            Me.v = v
        End Sub
    End Structure

    Public Sub New(ByVal s As String)
        MyBase.New(s)
    End Sub

    Public Function match(ByVal i As String) As Boolean Implements ifilter.match
        Dim t As T = Nothing
        If cast(i, t) Then
            Select Case store().cm
                Case compare_method.larger
                    Return less(store().v, t)
                Case compare_method.larger_or_equal
                    Return less_or_equal(store().v, t)
                Case compare_method.less
                    Return less(t, store().v)
                Case compare_method.less_or_equal
                    Return less_or_equal(t, store().v)
                Case compare_method.equal
                    Return equal(t, store().v)
                Case Else
                    Return assert(False)
            End Select
        Else
            Return False
        End If
    End Function

    Protected NotOverridable Overrides Function parse(ByVal s As String, ByRef o As s) As Boolean
        If s.null_or_empty() Then
            Return False
        Else
            Dim cm As compare_method
            Dim v As T
            If strstartwith(s, cons.larger, False) Then
                cm = compare_method.larger
                s = strright(s, strlen(s) - strlen(cons.larger))
            ElseIf strstartwith(s, cons.larger_or_equal, False) Then
                cm = compare_method.larger_or_equal
                s = strright(s, strlen(s) - strlen(cons.larger_or_equal))
            ElseIf strstartwith(s, cons.less, False) Then
                cm = compare_method.less
                s = strright(s, strlen(s) - strlen(cons.less))
            ElseIf strstartwith(s, cons.less_or_equal, False) Then
                cm = compare_method.less_or_equal
                s = strright(s, strlen(s) - strlen(cons.less_or_equal))
            ElseIf strstartwith(s, cons.equal, False) Then
                cm = compare_method.equal
                s = strright(s, strlen(s) - strlen(cons.equal))
            Else
                cm = compare_method.equal
            End If

            If cast(s, v) Then
                o = New s(cm, v)
                Return True
            Else
                Return False
            End If
        End If
    End Function
End Class
