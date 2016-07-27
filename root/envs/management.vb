
Imports Microsoft.VisualBasic.Devices

Public Module _management
    Public ReadOnly computer As Computer
    Public ReadOnly machine_name As String
    Public ReadOnly computer_name As String
    Public ReadOnly domain_name As String
    Public ReadOnly user_name As String

    Sub New()
        computer = New Computer()
        machine_name = Environment.MachineName()
        computer_name = computer.Name()
        domain_name = Environment.UserDomainName()
        user_name = Environment.UserName()
    End Sub
End Module
