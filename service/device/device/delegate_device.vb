
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.template

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

    Public Shared Function [New](Of DT As T)(ByVal i As DT,
                                             Optional ByVal validator As Func(Of DT, Boolean) = Nothing,
                                             Optional ByVal closer As Action(Of DT) = Nothing,
                                             Optional ByVal identifier As Func(Of DT, String) = Nothing,
                                             Optional ByVal checker As Action(Of DT) = Nothing) As delegate_device(Of T)
        Dim tvalidator As Func(Of T, Boolean) = Nothing
        Dim tcloser As Action(Of T) = Nothing
        Dim tidentifier As Func(Of T, String) = Nothing
        Dim tchecker As Action(Of T) = Nothing
        If Not validator Is Nothing Then
            tvalidator = Function(ByVal x As T) As Boolean
                             assert(object_compare(i, x) = 0)
                             Return validator(i)
                         End Function
        End If
        If Not closer Is Nothing Then
            tcloser = Sub(ByVal x As T)
                          assert(object_compare(i, x) = 0)
                          closer(i)
                      End Sub
        End If
        If Not identifier Is Nothing Then
            tidentifier = Function(ByVal x As T) As String
                              assert(object_compare(i, x) = 0)
                              Return identifier(i)
                          End Function
        End If
        If Not checker Is Nothing Then
            tchecker = Sub(ByVal x As T)
                           assert(object_compare(i, x) = 0)
                           checker(i)
                       End Sub
        End If
        Return New delegate_device(Of T)(i, tvalidator, tcloser, tidentifier, tchecker)
    End Function

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
