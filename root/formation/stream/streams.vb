﻿
Option Explicit On
Option Infer Off
Option Strict On

Partial Public NotInheritable Class streams
    Public Shared Function [of](Of T)(ByVal i() As T) As stream(Of T)
        Return New stream(Of T)(enumerators.from_array(i))
    End Function

    Public Shared Function repeat(Of T)(ByVal e As T, ByVal count As UInt32) As stream(Of T)
        Return New stream(Of T)(enumerators.repeat(e, count))
    End Function

    Public Shared Function range(ByVal start As Int32, ByVal [end] As Int32) As stream(Of Int32)
        Return New stream(Of Int32)(enumerators.range(start, [end]))
    End Function

    Public Shared Function range_closed(ByVal start As Int32, ByVal [end] As Int32) As stream(Of Int32)
        Return range(start, [end] + 1)
    End Function

    Private Sub New()
    End Sub
End Class
