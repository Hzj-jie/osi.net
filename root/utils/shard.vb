
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Public NotInheritable Class shard(Of T)
    Public Shared ReadOnly all As New shard(Of T)(0, 1)

    Private ReadOnly index As UInt32
    Private ReadOnly count As UInt32

    Public Sub New(ByVal index As UInt32, ByVal count As UInt32)
        assert(count > 0)
        assert(index < count)
        Me.index = index
        Me.count = count
    End Sub

    Default Public ReadOnly Property _R(ByVal i As T) As Boolean
        Get
            Return fast_to_uint32(Of T).on(i) Mod count = index
        End Get
    End Property
End Class
