
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection

Public NotInheritable Class binding_flags
    Public Const instance_public As BindingFlags = BindingFlags.Public Or BindingFlags.Instance
    Public Const instance_private As BindingFlags = BindingFlags.NonPublic Or BindingFlags.Instance
    Public Const instance_all As BindingFlags = BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance

    Public Const static_public As BindingFlags = BindingFlags.Public Or BindingFlags.Static
    Public Const static_private As BindingFlags = BindingFlags.NonPublic Or BindingFlags.Static
    Public Const static_all As BindingFlags = BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Static

    Public Const [public] As BindingFlags = BindingFlags.Public Or BindingFlags.Instance Or BindingFlags.Static
    Public Const [private] As BindingFlags = BindingFlags.NonPublic Or BindingFlags.Instance Or BindingFlags.Static
    Public Const [all] As BindingFlags = BindingFlags.Public Or
                                         BindingFlags.NonPublic Or
                                         BindingFlags.Instance Or
                                         BindingFlags.Static

    Public Const instance_public_method As BindingFlags = instance_public Or BindingFlags.InvokeMethod
    Public Const instance_private_method As BindingFlags = instance_private Or BindingFlags.InvokeMethod
    Public Const instance_all_method As BindingFlags = instance_all Or BindingFlags.InvokeMethod

    Public Const static_public_method As BindingFlags = static_public Or BindingFlags.InvokeMethod
    Public Const static_private_method As BindingFlags = static_private Or BindingFlags.InvokeMethod
    Public Const static_all_method As BindingFlags = static_all Or BindingFlags.InvokeMethod

    Public Const public_method As BindingFlags = [public] Or BindingFlags.InvokeMethod
    Public Const private_method As BindingFlags = [private] Or BindingFlags.InvokeMethod
    Public Const all_method As BindingFlags = [all] Or BindingFlags.InvokeMethod

    Private Sub New()
    End Sub
End Class
