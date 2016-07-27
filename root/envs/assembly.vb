
Imports System.Reflection
Imports osi.root.connector

Public Module _assembly
    Public ReadOnly current_assembly As Assembly = Nothing

    Sub New()
        current_assembly = Assembly.GetEntryAssembly()
        If current_assembly Is Nothing Then
            current_assembly = Assembly.GetCallingAssembly()
            If current_assembly Is Nothing Then
                current_assembly = Assembly.GetExecutingAssembly()
            End If
        End If
        assert(Not current_assembly Is Nothing)
    End Sub
End Module
