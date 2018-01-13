
#If RETIRED
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.service.commander
Imports parameters = osi.service.iosys.constants.remote.parameters

'serialize the case to the command, preserve the action as empty
'deserialize the command to the case
Public Class case_codec(Of CASE_T)
    Private Sub New()
    End Sub

    Private Shared Function case_bytes_command(ByVal d As _do_val_ref(Of CASE_T, Byte(), Boolean),
                                               ByVal c As CASE_T,
                                               ByRef o As command) As Boolean
        assert(Not d Is Nothing)
        assert(Not c Is Nothing)
        Dim b() As Byte = Nothing
        If d(c, b) AndAlso Not isemptyarray(b) Then
            o = New command()
            o.attach(parameters.bytes, b)
            Return True
        Else
            Return False
        End If
    End Function

    Private Shared Function output_valid(ByVal o As command) As Boolean
        Return Not o Is Nothing AndAlso Not o.has_action()
    End Function

    Private Shared Function command_bytes_case(ByVal d As _do_val_ref(Of Byte(), CASE_T, Boolean),
                                               ByVal c As command,
                                               ByRef o As CASE_T) As Boolean
        assert(Not d Is Nothing)
        assert(Not c Is Nothing)
        Dim b() As Byte = Nothing
        Return c.parameter(parameters.bytes, b) AndAlso
               Not isemptyarray(b) AndAlso
               d(b, o)
    End Function

    Private Shared Function output_valid(ByVal o As CASE_T) As Boolean
        Return Not o Is Nothing
    End Function

    Public Shared Function serialize(ByVal c As CASE_T,
                                     ByRef o As command,
                                     Optional ByVal case_command As binder(Of _do_val_ref(Of CASE_T, command, Boolean), 
                                                                              case_codec(Of CASE_T)) = Nothing,
                                     Optional ByVal case_bytes As  _
                                         binder(Of _do_val_ref(Of CASE_T, Byte(), Boolean), 
                                                   bytes_conversion_binder_protector) = Nothing) _
                                    As Boolean
        If c Is Nothing Then
            Return False
        ElseIf Not case_command Is Nothing AndAlso case_command.has_local_value() Then
            Return case_command.local()(c, o) AndAlso
                   output_valid(o)
        ElseIf Not case_bytes Is Nothing AndAlso case_bytes.has_local_value() Then
            Return case_bytes_command(case_bytes.local(), c, o) AndAlso
                   assert(output_valid(o))
        ElseIf case_command.has_value() Then
            Return (+case_command)(c, o) AndAlso
                   output_valid(o)
        ElseIf case_bytes.has_value() Then
            Return case_bytes_command(+case_bytes, c, o) AndAlso
                   assert(output_valid(o))
        Else
            Return False
        End If
    End Function

    Public Shared Function deserialize(
                               ByVal c As command,
                               ByRef o As CASE_T,
                               Optional ByVal command_case As binder(Of _do_val_ref(Of command, CASE_T, Boolean), 
                                                                        case_codec(Of CASE_T)) = Nothing,
                               Optional ByVal bytes_case As  _
                                   binder(Of _do_val_ref(Of Byte(), CASE_T, Boolean), 
                                             bytes_conversion_binder_protector) = Nothing) _
                                      As Boolean
        If c Is Nothing Then
            Return False
        ElseIf Not command_case Is Nothing AndAlso command_case.has_local_value() Then
            Return command_case.local()(c, o) AndAlso
                   output_valid(o)
        ElseIf Not bytes_case Is Nothing AndAlso bytes_case.has_local_value() Then
            Return command_bytes_case(bytes_case.local(), c, o) AndAlso
                   output_valid(o)
        ElseIf command_case.has_value() Then
            Return (+command_case)(c, o) AndAlso
                   output_valid(o)
        ElseIf bytes_case.has_value() Then
            Return command_bytes_case(+bytes_case, c, o) AndAlso
                   output_valid(o)
        Else
            Return False
        End If
    End Function
End Class
#End If
