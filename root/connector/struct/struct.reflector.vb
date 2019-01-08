
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection

Partial Public Class struct(Of T)
    Private NotInheritable Class reflector
        Inherits struct(Of T)

        Public NotInheritable Class definition
            Public ReadOnly type As Type
            Public ReadOnly name As String
            Public ReadOnly getter As Func(Of T, Object)
            Public ReadOnly setter As Action(Of T, Object)

            Public Sub New(ByVal fs As FieldInfo)
                assert(Not fs Is Nothing)
                type = fs.FieldType()
                name = fs.Name()
                getter = AddressOf fs.GetValue
                setter = AddressOf fs.SetValue
                assert(Not type Is Nothing)
                assert(Not String.IsNullOrEmpty(name))
                assert(Not getter Is Nothing)
                assert(Not setter Is Nothing)
            End Sub
        End Class

        Public Shared ReadOnly instance As reflector
        Private Shared ReadOnly definitions() As definition

        ' Do not initialize until the default implementation is used.
        Shared Sub New()
            Dim fs() As FieldInfo = Nothing
            fs = GetType(T).GetFields()
            ReDim definitions(array_size_i(fs) - 1)
            For i As Int32 = 0 To array_size_i(fs) - 1
                definitions(i) = New definition(fs(i))
            Next
            instance = New reflector()
        End Sub

        Public Overrides Function disassemble(ByVal i As T) As struct.variable()

        End Function

        Public Overrides Function assemble(ByVal vs() As Object, ByRef o As T) As Boolean

        End Function

        Private Sub New()
        End Sub
    End Class
End Class
