
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text

Partial Public Class struct(Of T)
    Public Shared Function [default]() As struct(Of T)
        Return reflector.instance
    End Function

    Public Overridable Function definitions() As struct.definition()
        Return [default]().definitions()
    End Function

    Public Overridable Function disassemble(ByVal i As T, ByRef o As struct.variable()) As Boolean
        Return [default]().disassemble(i, o)
    End Function

    Public Overridable Function to_str(ByVal i As T, ByRef o As String) As Boolean
        Dim v() As struct.variable = Nothing
        If Not disassemble(i, v) Then
            Return False
        End If
        o = strcat("{", v.strjoin(","), "}")
        Return True
    End Function

    Public Function disassemble(ByVal i As T) As struct.variable()
        Dim o() As struct.variable = Nothing
        assert(disassemble(i, o))
        Return o
    End Function

    Public Overridable Function assemble(ByVal vs() As Object, ByRef o As T) As Boolean
        Return [default]().assemble(vs, o)
    End Function

    Public Function assemble(ByVal vs() As struct.variable, ByRef o As T) As Boolean
        Dim objs() As Object = Nothing
        ReDim objs(array_size_i(vs) - 1)
        For i As Int32 = 0 To array_size_i(vs) - 1
            If vs(i) Is Nothing Then
                Return False
            End If
            objs(i) = vs(i).value
        Next
        Return assemble(objs, o)
    End Function

    Public Function assemble(ByVal ParamArray vs() As struct.variable) As T
        Dim o As T = Nothing
        assert(assemble(vs, o))
        Return o
    End Function

    Public Function assemble(ByVal ParamArray vs() As Object) As T
        Dim o As T = Nothing
        assert(assemble(vs, o))
        Return o
    End Function

    Public Shared Operator +(ByVal this As struct(Of T)) As struct(Of T)
        If this Is Nothing Then
            Return [default]()
        End If
        Return this
    End Operator

    Protected Sub New()
    End Sub
End Class
