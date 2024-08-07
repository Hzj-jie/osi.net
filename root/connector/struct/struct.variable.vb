
Option Explicit On
Option Infer Off
Option Strict On

Partial Public NotInheritable Class struct
    Public NotInheritable Class variable
        Public ReadOnly type As Type
        Public ReadOnly name As String
        Public ReadOnly value As Object

        Public Sub New(ByVal type As Type, ByVal name As String, ByVal value As Object)
            assert(Not type Is Nothing)
            Me.type = type
            assert(Not name.null_or_empty())
            Me.name = name
            Me.value = value
        End Sub

        Public Function [as](Of VT)() As VT
            assert(GetType(VT) Is type)
            Return direct_cast(Of VT)(value)
        End Function

        Public Overrides Function ToString() As String
            Return strcat("[", type, "] ", name, ": ", value)
        End Function
    End Class
End Class
