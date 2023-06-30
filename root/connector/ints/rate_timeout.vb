
Imports osi.root.constants

Public Class rate_timeout
    Implements ICloneable

    Private ReadOnly rate_ms As Double
    Private ReadOnly overhead As UInt32
    Private ms As Int64
    Private size As UInt32

    Public Sub New(ByVal rate_sec As UInt32, Optional ByVal overhead As UInt32 = uint32_0)
        Me.rate_ms = rate_sec
        Me.rate_ms /= constants.second_milli
    End Sub

    Public Sub update(ByVal size As UInt32)
        Me.ms = nowadays.milliseconds()
        Me.size = size + overhead
    End Sub

    Public Function timeout() As Boolean
        Return rate_ms > 0 AndAlso
               nowadays.milliseconds() > ms AndAlso
               rate_ms * (nowadays.milliseconds() - ms) > size
    End Function

    Public Function timeout_ms(ByVal size As UInt32) As Int64
        Return CLng((size + overhead) / rate_ms)
    End Function

    Public Function clone() As rate_timeout
        Return New rate_timeout(rate_ms * constants.second_milli, overhead)
    End Function

    Public Function IClonable_Clone() As Object Implements ICloneable.Clone
        Return clone()
    End Function
End Class
