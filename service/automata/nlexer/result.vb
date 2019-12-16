
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public NotInheritable Class nlexer
    Public NotInheritable Class result
        Public ReadOnly start As UInt32
        Public ReadOnly [end] As UInt32
        Public ReadOnly rule_index As UInt32
        Public ReadOnly rule As rule

        Public Sub New(ByVal start As UInt32, ByVal [end] As UInt32, ByVal rule_index As UInt32, ByVal rule As rule)
            assert([end] > start)
            Me.start = start
            Me.end = [end]
            Me.rule_index = rule_index
            Me.rule = rule
        End Sub
    End Class
End Class
