
Imports System.Runtime.CompilerServices
Imports osi.service.selector

Public Module _async_getter_delegate_device
    <Extension()> Public Function make_device(Of T)(ByVal c As async_getter(Of T),
                                                    Optional ByVal validator As Func(Of T, Boolean) = Nothing,
                                                    Optional ByVal closer As Action(Of T) = Nothing,
                                                    Optional ByVal identifier As Func(Of T, String) = Nothing,
                                                    Optional ByVal checker As Action(Of T) = Nothing) _
                                                   As async_getter_delegate_device(Of T)
        Return New async_getter_delegate_device(Of T)(c, validator, closer, identifier, checker)
    End Function
End Module

Public Class async_getter_delegate_device(Of T, ASG As {async_getter(Of T), T})
    Inherits async_getter_device(Of T, ASG)

    Private ReadOnly r As delegate_device_status_retriever(Of T)

    Public Sub New(ByVal c As async_getter(Of T),
                   Optional ByVal validator As Func(Of T, Boolean) = Nothing,
                   Optional ByVal closer As Action(Of T) = Nothing,
                   Optional ByVal identifier As Func(Of T, String) = Nothing,
                   Optional ByVal checker As Action(Of T) = Nothing)
        MyBase.New(c)
        r = New delegate_device_status_retriever(Of T)(validator, closer, identifier, checker)
    End Sub

    Protected NotOverridable Overrides Sub check(ByVal c As T)
        r.check(c)
    End Sub

    Protected NotOverridable Overrides Sub close(ByVal c As T)
        r.close(c)
    End Sub

    Protected NotOverridable Overrides Function identity(ByVal c As T) As String
        Return r.identity(c)
    End Function

    Protected NotOverridable Overrides Function validate(ByVal c As T) As Boolean
        Return r.validate(c)
    End Function
End Class

Public Class async_getter_delegate_device(Of T)
    Inherits async_getter_device(Of T)

    Private ReadOnly r As delegate_device_status_retriever(Of T)

    Public Sub New(ByVal c As async_getter(Of T),
                   Optional ByVal validator As Func(Of T, Boolean) = Nothing,
                   Optional ByVal closer As Action(Of T) = Nothing,
                   Optional ByVal identifier As Func(Of T, String) = Nothing,
                   Optional ByVal checker As Action(Of T) = Nothing)
        MyBase.New(c)
        r = delegate_device_status_retriever.[New](validator, closer, identifier, checker)
    End Sub

    Protected NotOverridable Overrides Sub check(ByVal c As T)
        r.check(c)
    End Sub

    Protected NotOverridable Overrides Sub close(ByVal c As T)
        r.close(c)
    End Sub

    Protected NotOverridable Overrides Function identity(ByVal c As T) As String
        Return r.identity(c)
    End Function

    Protected NotOverridable Overrides Function validate(ByVal c As T) As Boolean
        Return r.validate(c)
    End Function
End Class
