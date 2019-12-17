
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public NotInheritable Class nlexer
    Public NotInheritable Class result
        Public ReadOnly start As UInt32
        Public ReadOnly [end] As UInt32
        Public ReadOnly name As String
        Public ReadOnly rule_index As UInt32

        Shared Sub New()
            struct(Of result).register()
        End Sub

        Public Sub New(ByVal start As UInt32,
                       ByVal [end] As UInt32,
                       ByVal name As String,
                       ByVal rule_index As UInt32)
            assert([end] > start)
            assert(Not name.null_or_whitespace())
            Me.start = start
            Me.end = [end]
            Me.name = name
            Me.rule_index = rule_index
        End Sub
    End Class
End Class
