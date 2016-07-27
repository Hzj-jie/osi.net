﻿
Imports System.Diagnostics
Imports osi.root.constants
Imports osi.root.connector

<global_init(global_init_level.foundamental)>
Public Module _priority
    Public ReadOnly has_priority_defined As Boolean
    Public ReadOnly priority_class As ProcessPriorityClass
    Private Const priority As String = "priority"
    Private Const process As String = "process"
    Private Const [class] As String = "class"
    Private ReadOnly set1() As String
    Private ReadOnly set2() As String
    Private ReadOnly set3() As String
    Private ReadOnly set4() As String

    Sub New()
        Dim set1() As String = Nothing
        Dim set2() As String = Nothing
        Dim set3() As String = Nothing
        Dim set4() As String = Nothing
        Dim priority_int As Int32 = 0
        Dim priority_string As String = Nothing
        set1 = env_keys(priority)
        set2 = env_keys(priority, [class])
        set3 = env_keys(process, priority)
        set4 = env_keys(process, priority, [class])
        If ((env_value(set1, priority_int) OrElse
             env_value(set2, priority_int) OrElse
             env_value(set3, priority_int) OrElse
             env_value(set4, priority_int)) AndAlso
            enum_cast(priority_int, priority_class)) OrElse
           ((env_value(set1, priority_string) OrElse
             env_value(set2, priority_string) OrElse
             env_value(set3, priority_string) OrElse
             env_value(set4, priority_string)) AndAlso
            enum_cast(priority_string, priority_class)) Then
            has_priority_defined = True
            current_process.PriorityClass() = priority_class
        Else
            has_priority_defined = False
            priority_class = current_process.PriorityClass()
        End If
    End Sub

    Private Sub init()
    End Sub
End Module
