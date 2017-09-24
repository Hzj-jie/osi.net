
Imports osi.root.connector
Imports osi.root.utt

Public Class trie_test
    Inherits repeat_case_wrapper

    Public Sub New()
        MyBase.New(New trie_case(True), 1000000 << (If(isreleasebuild(), 2, 0)))
    End Sub
End Class

Public Class trie_specified_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(repeat(New trie_case(True), 1000000000 << (If(isreleasebuild(), 2, 0))))
    End Sub
End Class