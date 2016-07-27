
Imports osi.root.connector
Imports osi.root.utt

Public Class trie_perf
    Inherits performance_case_wrapper

    Public Sub New()
        MyBase.New(repeat(New trie_case(False), 1000000 << (If(isreleasebuild(), 2, 0))))
    End Sub

    Protected Overrides Function max_loops() As UInt64
        Return If(isreleasebuild(), 150000000000, 15000000000)
    End Function
End Class