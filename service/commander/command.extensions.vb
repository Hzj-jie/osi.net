
Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils
Imports osi.service.convertor

Public Module _command_extensions
    <Extension()> Public Function attach(ByVal i As command,
                                         ByVal action() As Byte) As command
        If Not i Is Nothing Then
            i.set_action(action)
        End If
        Return i
    End Function

    <Extension()> Public Function attach(Of T)(ByVal i As command,
                                               ByVal action As T,
                                               Optional ByVal T_bytes As  _
                                                   binder(Of Func(Of T, Byte()), 
                                                             bytes_conversion_binder_protector) = Nothing) _
                                              As command
        If Not i Is Nothing Then
            i.set_action(action, T_bytes)
        End If
        Return i
    End Function

    <Extension()> Public Function attach(ByVal i As command,
                                         ByVal k() As Byte,
                                         ByVal v() As Byte) As command
        If Not i Is Nothing Then
            i.set_parameter(k, v)
        End If
        Return i
    End Function

    <Extension()> Public Function attach(Of KT)(ByVal i As command,
                                                ByVal k As KT,
                                                ByVal v() As Byte,
                                                Optional ByVal KT_bytes As  _
                                                    binder(Of Func(Of KT, Byte()), 
                                                              bytes_conversion_binder_protector) = Nothing) _
                                               As command
        If Not i Is Nothing Then
            i.set_parameter(k, v, KT_bytes)
        End If
        Return i
    End Function

    <Extension()> Public Function attach(Of VT)(ByVal i As command,
                                                ByVal k() As Byte,
                                                ByVal v As VT,
                                                Optional ByVal VT_bytes As  _
                                                    binder(Of Func(Of VT, Byte()), 
                                                              bytes_conversion_binder_protector) = Nothing) _
                                               As command
        If Not i Is Nothing Then
            i.set_parameter(k, v, VT_bytes)
        End If
        Return i
    End Function

    <Extension()> Public Function attach(Of KT, VT)(ByVal i As command,
                                                    ByVal k As KT,
                                                    ByVal v As VT,
                                                    Optional ByVal KT_bytes As  _
                                                        binder(Of Func(Of KT, Byte()), 
                                                                  bytes_conversion_binder_protector) = Nothing,
                                                    Optional ByVal VT_bytes As  _
                                                        binder(Of Func(Of VT, Byte()), 
                                                                  bytes_conversion_binder_protector) = Nothing) _
                                                   As command
        If Not i Is Nothing Then
            i.set_parameter(k, v, KT_bytes, VT_bytes)
        End If
        Return i
    End Function

    <Extension()> Public Sub renew(ByRef i As command)
        If i Is Nothing Then
            i = New command()
        Else
            i.clear()
        End If
    End Sub
End Module
