
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports osi.root.connector

Public Module _assembly
    Public ReadOnly current_assembly As Assembly = Function() As Assembly
                                                       Dim current_assembly As Assembly = Assembly.GetEntryAssembly()
                                                       If Not current_assembly Is Nothing Then
                                                           Return current_assembly
                                                       End If
                                                       current_assembly = Assembly.GetCallingAssembly()
                                                       If Not current_assembly Is Nothing Then
                                                           Return current_assembly
                                                       End If
                                                       current_assembly = Assembly.GetExecutingAssembly()
                                                       assert(Not current_assembly Is Nothing)
                                                       Return current_assembly
                                                   End Function()
End Module
