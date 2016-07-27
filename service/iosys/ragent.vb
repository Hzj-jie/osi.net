
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.service.commander
Imports osi.service.commander.constants

'the design cannot work with one connection
Public Class ragent(Of CASE_T)
    Private ReadOnly dispatcher As target_dispatcher(Of command_ragent(Of CASE_T))

    Public Sub New(ByVal dispatcher As target_dispatcher(Of command_ragent(Of CASE_T)))
        assert(Not dispatcher Is Nothing)
        Me.dispatcher = dispatcher
        assert(listen(Me.dispatcher))
    End Sub

    Public Sub New(ByVal dispatcher As dispatcher)
        Me.New(New target_dispatcher(Of command_ragent(Of CASE_T))(dispatcher))
    End Sub

    Public Sub New()
        Me.New(New target_dispatcher(Of command_ragent(Of CASE_T))())
    End Sub

    Public Shared Function listen(ByVal dispatcher As target_dispatcher(Of command_ragent(Of CASE_T))) As Boolean
        Return Not dispatcher Is Nothing AndAlso
               dispatcher.register(constants.remote.action.push, AddressOf push)
    End Function

    Public Shared Function ignore(ByVal dispatcher As target_dispatcher(Of command_ragent(Of CASE_T))) As Boolean
        Return Not dispatcher Is Nothing AndAlso
               dispatcher.erase(constants.remote.action.push)
    End Function

    Public Function ignore() As Boolean
        Return ignore(dispatcher)
    End Function

    Public Shared Function push(ByVal d As command_ragent(Of CASE_T),
                                ByVal i As command,
                                ByVal o As command) As event_comb
        Return sync_async(Function() As Boolean
                              assert(Not d Is Nothing)
                              o.attach(If(d.push(i), response.success, response.failure))
                              Return True
                          End Function)
    End Function

    Public Shared Operator +(ByVal this As ragent(Of CASE_T)) As target_dispatcher(Of command_ragent(Of CASE_T))
        If this Is Nothing Then
            Return Nothing
        Else
            Return this.dispatcher
        End If
    End Operator
End Class
