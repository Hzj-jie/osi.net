
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.procedure
Imports action_map = osi.root.formation.map(Of _
                            osi.root.formation.array_pointer(Of Byte), _
                            System.Func(Of osi.service.commander.command, _
                                           osi.service.commander.command, _
                                           osi.root.procedure.event_comb))

Public Class dispatcher
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
                                   Else
                                       Return m.erase(make_array_pointer(action))
                                   End If
                               End Function)
    End Function

    Public Function [erase](Of T)(ByVal action As T,
                                  Optional ByVal T_bytes As bytes_serializer(Of T) = Nothing) As Boolean
        Return [erase]((+T_bytes).to_bytes(action))
    End Function

    Public Function register(ByVal action() As Byte,
                             ByVal act As Func(Of command, command, event_comb),
                             Optional ByVal replace As Boolean = False) As Boolean
        Return Not isemptyarray(action) AndAlso
               Not act Is Nothing AndAlso
               l.writer_locked(Function() As Boolean
                                   Dim ap As array_pointer(Of Byte) = Nothing
                                   ap = make_array_pointer(action)
                                   Dim it As action_map.iterator = Nothing
                                   it = m.find(ap)
                                   If it = m.end() OrElse replace Then
                                       m(ap) = act
                                       Return True
                                   Else
                                       Return False
                                   End If
                               End Function)
    End Function

    Public Function register(Of T)(ByVal action As T,
                                   ByVal act As Func(Of command, command, event_comb),
                                   Optional ByVal replace As Boolean = False,
                                   Optional ByVal T_bytes As bytes_serializer(Of T) = Nothing) As Boolean
        Return register((+T_bytes).to_bytes(action), act, replace)
    End Function

    Public Function execute(ByVal i As command, ByVal o As command) As event_comb Implements executor.execute
        Dim a As Func(Of command, command, event_comb) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If i Is Nothing OrElse
                                     o Is Nothing OrElse
                                     Not l.reader_locked(Function() As Boolean
                                                             Dim action As array_pointer(Of Byte) = Nothing
                                                             action = make_array_pointer(i.action())
                                                             Dim it As action_map.iterator = Nothing
                                                             it = m.find(action)
                                                             If it = m.end() Then
                                                                 Return False
                                                             Else
                                                                 a = (+it).second
                                                                 Return True
                                                             End If
                                                         End Function) Then
                                      Return False
                                  Else
                                      assert(Not a Is Nothing)
                                      ec = a(i, o)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)

    End Function
End Class
