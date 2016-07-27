
Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.service.convertor

Public Module _command_T_conversion
    <Extension()> Public Function command_to_T(Of T)(ByVal c As command_T_conversion(Of T),
                                                     ByVal i As command,
                                                     ByRef o As T) As Boolean
        Dim b As binder(Of _do_val_ref(Of command, T, Boolean), command_T_conversion(Of T)) = Nothing
        If Not c Is Nothing Then
            b = c.command_T
        End If
        If b.has_value() Then
            Return (+b)(i, o)
        Else
            Dim b1 As binder(Of _do_val_ref(Of command, Byte(), Boolean), bytes_conversion_binder_protector) = Nothing
            Dim b2 As binder(Of _do_val_ref(Of Byte(), T, Boolean), bytes_conversion_binder_protector) = Nothing
            Dim buff() As Byte = Nothing
            Return b1.has_value() AndAlso
                   b2.has_value() AndAlso
                   (+b1)(i, buff) AndAlso
                   (+b2)(buff, o)
        End If
    End Function

    <Extension()> Public Function T_to_command(Of T)(ByVal c As command_T_conversion(Of T),
                                                     ByVal i As T,
                                                     ByRef o As command) As Boolean
        Dim b As binder(Of _do_val_ref(Of T, command, Boolean), command_T_conversion(Of T)) = Nothing
        If Not c Is Nothing Then
            b = c.T_command
        End If
        If b.has_value() Then
            Return (+b)(i, o)
        Else
            Dim b1 As binder(Of _do_val_ref(Of T, Byte(), Boolean), bytes_conversion_binder_protector) = Nothing
            Dim b2 As binder(Of _do_val_ref(Of Byte(), command, Boolean), bytes_conversion_binder_protector) = Nothing
            Dim buff() As Byte = Nothing
            Return b1.has_value() AndAlso
                   b2.has_value() AndAlso
                   (+b1)(i, buff) AndAlso
                   (+b2)(buff, o)
        End If
    End Function
End Module

Public Class command_T_conversion(Of T)
    Public ReadOnly T_command As binder(Of _do_val_ref(Of T, command, Boolean), command_T_conversion(Of T))
    Public ReadOnly command_T As binder(Of _do_val_ref(Of command, T, Boolean), command_T_conversion(Of T))

    Public Sub New(Optional ByVal T_command As binder(Of _do_val_ref(Of T, command, Boolean), 
                                                         command_T_conversion(Of T)) = Nothing,
                   Optional ByVal command_T As binder(Of _do_val_ref(Of command, T, Boolean), 
                                                         command_T_conversion(Of T)) = Nothing)
        Me.T_command = T_command
        Me.command_T = command_T
    End Sub
End Class
