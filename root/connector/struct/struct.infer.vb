
Option Explicit On
Option Infer Off
Option Strict On

Partial Public NotInheritable Class struct
    Public Shared Function disassemble(Of T)(ByVal i As T, ByRef o() As variable) As Boolean
        Return struct(Of T).default().disassemble(i, o)
    End Function

    Public Shared Function disassemble(Of T)(ByVal i As T) As variable()
        Return struct(Of T).default().disassemble(i)
    End Function

    Public Shared Function assemble(Of T)(ByVal vs() As variable, ByRef o As T) As Boolean
        Return struct(Of T).default().assemble(vs, o)
    End Function

    Public Shared Function assemble(Of T)(ByVal vs() As Object, ByRef o As T) As Boolean
        Return struct(Of T).default().assemble(vs, o)
    End Function

    Public Shared Function assemble(Of T)(ByVal ParamArray vs() As variable) As T
        Return struct(Of T).default().assemble(vs)
    End Function

    Public Shared Function assemble(Of T)(ByVal ParamArray vs() As Object) As T
        Return struct(Of T).default().assemble(vs)
    End Function

    Public Shared Function to_str(Of T)(ByVal i As T, ByRef o As String) As Boolean
        Return struct(Of T).default().to_str(i, o)
    End Function

    Private Sub New()
    End Sub
End Class
