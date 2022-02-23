
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports osi.root.connector

Public Module _assembly
    Public ReadOnly current_assembly As Assembly = calculate_current_assembly()

    Private Function calculate_current_assembly() As Assembly
        Dim current_assembly As Assembly = Assembly.GetEntryAssembly()
        If current_assembly Is Nothing Then
            current_assembly = Assembly.GetCallingAssembly()
            If current_assembly Is Nothing Then
                current_assembly = Assembly.GetExecutingAssembly()
            End If
        End If
        assert(current_assembly IsNot Nothing)
        Return current_assembly
    End Function
End Module
