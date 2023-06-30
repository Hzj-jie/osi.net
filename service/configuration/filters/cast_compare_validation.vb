
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports c = osi.root.connector

Public MustInherit Class cast_compare_validation(Of T, ST)
    Private ReadOnly v As Boolean
    Private ReadOnly s As ST

    Protected Sub New(ByVal s As String)
        v = parse(s, Me.s)
        If Not validate() Then
            raise_error(error_type.exclamation,
                        "filter value ", s, " is not valid, so this filter will never match")
        End If
    End Sub

    Protected MustOverride Function parse(ByVal s As String, ByRef o As ST) As Boolean

    Protected Function cast(ByVal i As String, ByRef o As T) As Boolean
        Return c.cast(i, o)
    End Function

    Protected Function equal(ByVal x As T, ByVal y As T) As Boolean
        Return compare(x, y) = 0
    End Function

    Protected Function less(ByVal x As T, ByVal y As T) As Boolean
        Return compare(x, y) < 0
    End Function

    Protected Function less_or_equal(ByVal x As T, ByVal y As T) As Boolean
        Return compare(x, y) <= 0
    End Function

    Protected Overridable Function compare(ByVal x As T, ByVal y As T) As Int32
        Return c.compare(x, y)
    End Function

    Protected Function validate() As Boolean
        Return v
    End Function

    Protected Function store() As ST
        Return s
    End Function
End Class
