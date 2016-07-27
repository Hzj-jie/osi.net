
' Provide a
' Using code_block.N
' End
' block to reduce the scope of variables, etc
Public Module _code_block
    Private Structure N
        Implements IDisposable

        Public Sub Dispose() Implements IDisposable.Dispose
        End Sub
    End Structure

    Public ReadOnly code_block As IDisposable

    Sub New()
        code_block = New N()
    End Sub
End Module
