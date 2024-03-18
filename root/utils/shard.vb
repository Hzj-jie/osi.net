
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Public NotInheritable Class shard(Of T)
    Private ReadOnly index As UInt32
    Private ReadOnly count As UInt32

    Public Sub New(ByVal index As UInt32, ByVal count As UInt32)
        assert(count > 0)
        assert(index < count)
        Me.index = index
        Me.count = count
    End Sub
End Class
