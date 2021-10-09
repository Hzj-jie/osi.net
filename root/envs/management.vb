
Option Explicit On
Option Infer Off
Option Strict On

Imports Microsoft.VisualBasic.Devices

Public Module _management
    Public ReadOnly computer As Computer = New Computer()
    Public ReadOnly machine_name As String = Environment.MachineName()
    Public ReadOnly computer_name As String = computer.Name()
    Public ReadOnly domain_name As String = Environment.UserDomainName()
    Public ReadOnly user_name As String = Environment.UserName()
End Module
