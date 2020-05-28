
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Public NotInheritable Class streams
    Public Shared Function [of](Of T)(ByVal i() As T) As stream(Of T)
        Return New stream(Of T)(container_operator.enumerators.from_array(i))
    End Function

    Public Shared Function repeat(Of T)(ByVal e As T, ByVal count As UInt32) As stream(Of T)
        Return New stream(Of T)(container_operator.enumerators.repeat(e, count))
    End Function

    Private Sub New()
    End Sub
End Class
