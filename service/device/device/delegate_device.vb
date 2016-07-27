
Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.template
Imports osi.root.utils

Public Module _delegate_device
    <Extension()> Public Function make_device(Of T)(ByVal i As T,
                                                    Optional ByVal validator As Func(Of T, Boolean) = Nothing,
                                                    Optional ByVal closer As Action(Of T) = Nothing,
                                                    Optional ByVal identifier As Func(Of T, String) = Nothing,
                                                    Optional ByVal checker As Action(Of T) = Nothing) _
                                                   As idevice(Of T)
        Return New delegate_device(Of T)(i, validator, closer, identifier, checker)
    End Function

    <Extension()> Public Function make_device(Of T,
                                                 VALIDATOR As __do(Of T, Boolean),
                                                 CLOSER As __void(Of T),
                                                 IDENTIFIER As __do(Of T, String),
                                                 CHECKER As __void(Of T))(ByVal i As T) As idevice(Of T)
        Return New delegate_device(Of T, VALIDATOR, CLOSER, IDENTIFIER, CHECKER)(i)
    End Function
End Module

Public Class delegate_device(Of T)
    Inherits device(Of T)

    Private ReadOnly sr As delegate_device_status_retriever(Of T)

    Protected Sub New(ByVal c As T, ByVal sr As delegate_device_status_retriever(Of T))
        MyBase.New(c)
        assert(Not sr Is Nothing)
        Me.sr = sr
    End Sub

    Public Sub New(ByVal c As T,
                   Optional ByVal validator As Func(Of T, Boolean) = Nothing,
                   Optional ByVal closer As Action(Of T) = Nothing,
                   Optional ByVal identifier As Func(Of T, String) = Nothing,
                   Optional ByVal checker As Action(Of T) = Nothing)
        Me.New(c, New delegate_device_status_retriever(Of T)(validator, closer, identifier, checker))
    End Sub

    Protected NotOverridable Overrides Sub close(ByVal c As T)
        sr.close(c)
    End Sub

    Protected NotOverridable Overrides Function validate(ByVal c As T) As Boolean
        Return sr.validate(c)
    End Function

    Protected NotOverridable Overrides Function identity(ByVal c As T) As String
        Return sr.identity(c)
    End Function

    Protected NotOverridable Overrides Sub check(ByVal c As T)
        sr.check(c)
    End Sub
End Class

Public NotInheritable Class delegate_device(Of T,
                                               _VALIDATOR As __do(Of T, Boolean),
                                               _CLOSER As __void(Of T),
                                               _IDENTIFIER As __do(Of T, String),
                                               _CHECKER As __void(Of T))
    Inherits delegate_device(Of T)

    Public Sub New(ByVal c As T)
        MyBase.New(c, delegate_device_status_retriever(Of T, _VALIDATOR, _CLOSER, _IDENTIFIER, _CHECKER).instance)
    End Sub
End Class
