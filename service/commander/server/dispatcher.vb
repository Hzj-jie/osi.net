
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.procedure
Imports action_map = osi.root.formation.map(Of
                            osi.root.connector.array_ref(Of Byte),
                            System.Func(Of osi.service.commander.command,
                                           osi.service.commander.command,
                                           osi.root.procedure.event_comb))

Public NotInheritable Class dispatcher
    Implements executor

    Public Shared ReadOnly [default] As dispatcher

    Shared Sub New()
        [default] = New dispatcher()
    End Sub

    Private ReadOnly m As action_map
    Private l As duallock

    Public Sub New()
        m = New action_map()
    End Sub

    Public Function [erase](ByVal action() As Byte) As Boolean
        Return l.writer_locked(Function() As Boolean
                                   If isemptyarray(action) Then
                                       Return False
                                   End If
                                   Return m.erase(array_ref.of(action))
                               End Function)
    End Function

    Public Function [erase](Of T)(ByVal action As T) As Boolean
        Return [erase](bytes_serializer.to_bytes(action))
    End Function

    Public Function register(ByVal action() As Byte,
                             ByVal act As Func(Of command, command, event_comb),
                             Optional ByVal replace As Boolean = False) As Boolean
        Return Not isemptyarray(action) AndAlso
               Not act Is Nothing AndAlso
               l.writer_locked(Function() As Boolean
                                   Dim ap As array_ref(Of Byte) = Nothing
                                   ap = array_ref.of(action)
                                   Dim it As action_map.iterator = Nothing
                                   it = m.find(ap)
                                   If it = m.end() OrElse replace Then
                                       m(ap) = act
                                       Return True
                                   End If
                                   Return False
                               End Function)
    End Function

    Public Function register(Of T)(ByVal action As T,
                                   ByVal act As Func(Of command, command, event_comb),
                                   Optional ByVal replace As Boolean = False) As Boolean
        Return register(bytes_serializer.to_bytes(action), act, replace)
    End Function

    Public Function execute(ByVal i As command, ByVal o As command) As event_comb Implements executor.execute
        Dim a As Func(Of command, command, event_comb) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If i Is Nothing OrElse
                                     o Is Nothing OrElse
                                     Not l.reader_locked(Function() As Boolean
                                                             Dim action As array_ref(Of Byte) = Nothing
                                                             action = array_ref.of(i.action())
                                                             Dim it As action_map.iterator = Nothing
                                                             it = m.find(action)
                                                             If it = m.end() Then
                                                                 Return False
                                                             End If
                                                             a = (+it).second
                                                             Return True
                                                         End Function) Then
                                      Return False
                                  End If
                                  assert(Not a Is Nothing)
                                  ec = a(i, o)
                                  Return waitfor(ec) AndAlso
                                             goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)

    End Function
End Class
