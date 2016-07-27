
Imports osi.root.formation
Imports osi.root.constants

Public Module _default
    Public ReadOnly [default] As var

    Sub New()
        [default] = New var()
    End Sub

    Public Function value(ByVal n As String, ByRef o As vector(Of String)) As Boolean
        Return [default].value(n, o)
    End Function

    Public Function value(ByVal i As String,
                          ByRef o As String,
                          Optional ByVal select_first As Boolean = False,
                          Optional ByVal separator As String = character.blank) As Boolean
        Return [default].value(i, o, select_first, separator)
    End Function

    Public Function value(ByVal i As String) As String
        Return [default].value(i)
    End Function

    Public Function switch(ByVal i As String, ByRef o As Boolean) As Boolean
        Return [default].switch(i, o)
    End Function

    Public Function switch(ByVal i As String) As Boolean
        Return [default].switch(i)
    End Function

    Public Function other_values() As vector(Of String)
        Return [default].other_values()
    End Function

    Public Sub parse(ByVal ParamArray args() As String)
        [default].parse(args)
    End Sub

    Public Sub parse(ByVal i As String)
        [default].parse(i)
    End Sub

    Public Function bind(ByVal ParamArray s() As String) As Boolean
        Return [default].bind(s)
    End Function
End Module
