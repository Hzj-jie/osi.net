
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.configuration.constants.interval_filter
Imports cons = osi.service.configuration.constants.interval_filter

Public Class interval_filter(Of T)
    Inherits cast_compare_validation(Of T, s)
    Implements ifilter

    Public Structure s
        Public ReadOnly min As T
        Public ReadOnly max As T
        Public ReadOnly include_min As Boolean
        Public ReadOnly include_max As Boolean

        Public Sub New(ByVal min As T,
                       ByVal max As T,
                       ByVal include_min As Boolean,
                       ByVal include_max As Boolean)
            Me.min = min
            Me.max = max
            Me.include_min = include_min
            Me.include_max = include_max
        End Sub
    End Structure

    Shared Sub New()
        assert(strlen(cons.include_min) = 1)
        assert(strlen(cons.include_max) = 1)
        assert(strlen(cons.exclude_min) = 1)
        assert(strlen(cons.exclude_max) = 1)
    End Sub

    Public Sub New(ByVal s As String)
        MyBase.New(s)
    End Sub

    Public Function match(ByVal i As String) As Boolean Implements ifilter.match
        Dim o As T = Nothing
        Return validate() AndAlso cast(i, o) AndAlso
               If(store().include_min, less_or_equal(store().min, o), less(store().min, o)) AndAlso
               If(store().include_max, less_or_equal(o, store().max), less(o, store().max))
    End Function

    Protected NotOverridable Overrides Function parse(ByVal s As String, ByRef o As s) As Boolean
        If s.null_or_empty() Then
            Return False
        Else
            Dim include_min As Boolean
            Dim include_max As Boolean
            Dim min As T
            Dim max As T

            Dim ss() As String = Nothing
            ss = s.Split(separators, StringSplitOptions.RemoveEmptyEntries)
            If (array_size(ss) <> 2) Then
                Return False
            Else
                If strleft(ss(0), 1) = cons.include_min OrElse
                   strleft(ss(0), 1) = cons.exclude_min Then
                    If strleft(ss(0), 1) = cons.include_min Then
                        include_min = True
                    Else
                        include_min = False
                    End If
                    ss(0) = ss(0).Substring(1)
                End If
                If strright(ss(1), 1) = cons.include_max OrElse
                   strright(ss(1), 1) = cons.exclude_max Then
                    If strright(ss(1), 1) = cons.include_max Then
                        include_max = True
                    Else
                        include_max = False
                    End If
                    ss(1) = strleft(ss(1), strlen(ss(1)) - uint32_1)
                End If
                If cast(ss(0), min) AndAlso cast(ss(1), max) Then
                    o = New s(min, max, include_min, include_max)
                    Return True
                Else
                    Return False
                End If
            End If
        End If
    End Function
End Class
