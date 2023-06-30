
Imports System.Runtime.CompilerServices
Imports osi.root.delegates
Imports osi.root.connector

Public Module _device_creator
    <Extension()> Public Function create_valid_device(Of T)(ByVal this As idevice_creator(Of T),
                                                            ByRef o As idevice(Of T)) As Boolean
        assert(Not this Is Nothing)
        If this.create(o) Then
            If o Is Nothing Then
                Return False
            ElseIf o.is_valid() Then
                Return True
            Else
                o.close()
                Return False
            End If
        Else
            Return False
        End If
    End Function
End Module

Public Class delegate_device_creator
    Public Shared Function [New](Of T)(ByVal c As _do(Of idevice(Of T), Boolean)) As delegate_device_creator(Of T)
        Return New delegate_device_creator(Of T)(c)
    End Function

    Public Shared Function [New](Of T)(ByVal c As Func(Of idevice(Of T))) As delegate_device_creator(Of T)
        Return New delegate_device_creator(Of T)(c)
    End Function

    Public Shared Function [New](Of T)(ByVal c As _do(Of T, Boolean),
                                       Optional ByVal validator As Func(Of T, Boolean) = Nothing,
                                       Optional ByVal closer As Action(Of T) = Nothing,
                                       Optional ByVal identifier As Func(Of T, String) = Nothing,
                                       Optional ByVal checker As Action(Of T) = Nothing) _
                                      As delegate_device_creator(Of T)
        Return New delegate_device_creator(Of T)(c, validator, closer, identifier, checker)
    End Function

    Public Shared Function [New](Of T)(ByVal c As Func(Of T),
                                       Optional ByVal validator As Func(Of T, Boolean) = Nothing,
                                       Optional ByVal closer As Action(Of T) = Nothing,
                                       Optional ByVal identifier As Func(Of T, String) = Nothing,
                                       Optional ByVal checker As Action(Of T) = Nothing) _
                                      As delegate_device_creator(Of T)
        Return New delegate_device_creator(Of T)(c, validator, closer, identifier, checker)
    End Function

    Private Sub New()
    End Sub
End Class

Public Class delegate_device_creator(Of T)
    Implements idevice_creator(Of T)

    Private ReadOnly c As _do(Of idevice(Of T), Boolean)

    Public Sub New(ByVal c As _do(Of idevice(Of T), Boolean))
        assert(Not c Is Nothing)
        Me.c = c
    End Sub

    Public Sub New(ByVal c As Func(Of idevice(Of T)))
        assert(Not c Is Nothing)
        Me.c = Function(ByRef o As idevice(Of T)) As Boolean
                   o = c()
                   Return True
               End Function
    End Sub

    Private Shared Function creator(ByVal c As _do(Of T, Boolean),
                                    ByVal validator As Func(Of T, Boolean),
                                    ByVal closer As Action(Of T),
                                    ByVal identifier As Func(Of T, String),
                                    ByVal checker As Action(Of T)) As _do(Of idevice(Of T), Boolean)
        Return Function(ByRef o As idevice(Of T)) As Boolean
                   Dim d As T = Nothing
                   If c(d) Then
                       o = d.make_device(validator, closer, identifier, checker)
                       Return True
                   Else
                       Return False
                   End If
               End Function
    End Function

    Public Sub New(ByVal c As _do(Of T, Boolean),
                   Optional ByVal validator As Func(Of T, Boolean) = Nothing,
                   Optional ByVal closer As Action(Of T) = Nothing,
                   Optional ByVal identifier As Func(Of T, String) = Nothing,
                   Optional ByVal checker As Action(Of T) = Nothing)
        assert(Not c Is Nothing)
        Me.c = creator(c, validator, closer, identifier, checker)
    End Sub

    Public Sub New(ByVal c As Func(Of T),
                   Optional ByVal validator As Func(Of T, Boolean) = Nothing,
                   Optional ByVal closer As Action(Of T) = Nothing,
                   Optional ByVal identifier As Func(Of T, String) = Nothing,
                   Optional ByVal checker As Action(Of T) = Nothing)
        assert(Not c Is Nothing)
        Me.c = creator(Function(ByRef o As T) As Boolean
                           o = c()
                           Return True
                       End Function,
                       validator,
                       closer,
                       identifier,
                       checker)
    End Sub

    Public Function create(ByRef o As idevice(Of T)) As Boolean Implements idevice_creator(Of T).create
        Return c(o)
    End Function
End Class