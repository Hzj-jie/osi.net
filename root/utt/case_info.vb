
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Friend NotInheritable Class case_info
    Public ReadOnly [case] As [case]
    Public finished As Boolean

    Public Sub New(ByVal name As String, ByVal [case] As [case])
        assert(Not String.IsNullOrEmpty(name))
        assert([case] IsNot Nothing, name)
        Me.case = [case]
    End Sub

    Public Function full_name() As String
        Return [case].full_name
    End Function

    Public Function assembly_qualified_name() As String
        Return [case].assembly_qualified_name
    End Function

    Public Function name() As String
        Return [case].name
    End Function
End Class