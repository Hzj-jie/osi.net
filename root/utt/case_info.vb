﻿
Imports osi.root.connector

Friend Class case_info
    Public ReadOnly [case] As [case]
    Public finished As Boolean

    Public Sub New(ByVal name As String, ByVal [case] As [case])
        assert(Not String.IsNullOrEmpty(name))
        assert(Not [case] Is Nothing, name)
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