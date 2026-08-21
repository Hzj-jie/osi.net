
Option Explicit On
Option Infer Off
Option Strict On

#If Not NET8_0_OR_GREATER Then
Imports Microsoft.VisualBasic.Devices
#End If

Public Module _management
#If Not NET8_0_OR_GREATER Then
    Public ReadOnly computer As Computer = New Computer()
    Public ReadOnly computer_name As String = computer.Name()
#Else
    Public ReadOnly computer_name As String = Environment.MachineName()
#End If
    Public ReadOnly machine_name As String = Environment.MachineName()
    Public ReadOnly domain_name As String = Environment.UserDomainName()
    Public ReadOnly user_name As String = Environment.UserName()
End Module
