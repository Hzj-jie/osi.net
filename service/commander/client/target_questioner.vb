
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.template
Imports osi.service.argument
Imports osi.service.device
Imports osi.service.selector
Imports constructor = osi.service.device.constructor

<global_init(global_init_level.server_services)>
Public NotInheritable Class target_questioner
    Inherits iquestioner(Of _false)

    Private ReadOnly name() As Byte
    Private ReadOnly q As questioner(Of _true)

    Shared Sub New()
        assert(global_init_level.services < global_init_level.server_services)
    End Sub

    Protected Sub New(ByVal name() As Byte,
                      ByVal q As questioner(Of _true),
                      ByVal internal As Boolean)
        assert(Not isemptyarray(name))
        assert(Not q Is Nothing)
        Me.name = name
        Me.q = q
    End Sub

    Public Sub New(ByVal name() As Byte, ByVal q As questioner(Of _true))
        Me.New(copy(name), q, True)
    End Sub

    Public Shared Shadows Function ctor(Of KEY_T) _
                                       (ByVal name As KEY_T, ByVal q As questioner(Of _true)) As target_questioner
        Return New target_questioner(bytes_serializer.to_bytes(name), q, True)
    End Function

    Private Shared Function create(ByVal v As var,
                                   ByVal q As questioner(Of _true),
                                   ByRef o As target_questioner) As Boolean
        If v Is Nothing OrElse q Is Nothing Then
            Return False
        End If
        Dim target As String = Nothing
        If v.value("target", target) Then
            o = ctor(target, q)
            Return True
        End If
        Return False
    End Function

    Public Shared Function create(ByVal v As var, ByRef o As target_questioner) As Boolean
        Return secondary_resolve(v,
                                 Function(i As questioner(Of _true), ByRef r As target_questioner) As Boolean
                                     Return create(v, i, r)
                                 End Function,
                                 o)
    End Function

    Public Function [get]() As questioner(Of _true)
        Return q
    End Function

    Public Shared Operator +(ByVal i As target_questioner) As questioner(Of _true)
        If i Is Nothing Then
            Return Nothing
        Else
            Return i.get()
        End If
    End Operator

    Private Function append_parameter_name(ByVal request As command) As Boolean
        If request Is Nothing OrElse
           request.has_parameter(constants.parameter.name) Then
            Return False
        Else
            request.attach(constants.parameter.name, name)
            Return True
        End If
    End Function

    Protected Overrides Function communicate(ByVal request As command,
                                             ByVal response As ref(Of command)) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If request Is Nothing OrElse
                                     request.has_parameter(constants.parameter.name) Then
                                      Return False
                                  Else
                                      request.attach(constants.parameter.name, name)
                                      ec = q(request, response)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Private Shared Sub init()
        assert(constructor.register(Of target_questioner)(AddressOf create))
        assert(constructor.register(parameter_allocator(Function(v As var,
                                                                 q As questioner(Of _true),
                                                                 ByRef o As target_questioner) As Boolean
                                                            Return create(v, q, o)
                                                        End Function)))
    End Sub
End Class
