
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.argument

Public NotInheritable Class constructor(Of T)
    Private Shared ReadOnly lt As unique_strong_map(Of String, Func(Of var, pointer(Of T), event_comb))
    Private Shared l As Func(Of var, pointer(Of T), event_comb)

    Shared Sub New()
        lt = New unique_strong_map(Of String, Func(Of var, pointer(Of T), event_comb))()
    End Sub

    Public Shared Function empty() As Boolean
        Return lt.empty() AndAlso l Is Nothing
    End Function

    Public Shared Function register(ByVal allocator As Func(Of var, pointer(Of T), event_comb)) As Boolean
        If allocator Is Nothing Then
            Return False
        Else
            Return atomic.set_if_nothing(l, allocator)
        End If
    End Function

    Public Shared Function [erase]() As Boolean
        If l Is Nothing Then
            Return False
        Else
            l = Nothing
            Return True
        End If
    End Function

    Public Shared Function register(ByVal type As String,
                                    ByVal allocator As Func(Of var, pointer(Of T), event_comb)) As Boolean
        If String.IsNullOrEmpty(type) OrElse allocator Is Nothing Then
            Return False
        Else
            Return lt.set(type, allocator)
        End If
    End Function

    Public Shared Function [erase](ByVal type As String) As Boolean
        Return lt.erase(type)
    End Function

    Public Shared Sub clear()
        [erase]()
        lt.clear()
    End Sub

    Public Shared Function resolve(ByVal v As var, ByVal o As pointer(Of T)) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If v Is Nothing Then
                                      Return False
                                  End If
                                  Dim allocator As Func(Of var, pointer(Of T), event_comb) = Nothing
                                  If lt.get(v("type"), allocator) OrElse
                                     (Not l Is Nothing AndAlso eva(allocator, l)) Then
                                      assert(Not allocator Is Nothing)
                                      o.renew()
                                      ec = allocator(v, o)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  Dim r As T = Nothing
                                  Return ec.end_result() AndAlso
                                         wrapper.wrap(v, +o, r) AndAlso
                                         eva(o, r) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Shared Function resolve(Of DT As T)(ByVal v As var, ByVal o As pointer(Of T)) As event_comb
        Dim ec As event_comb = Nothing
        Dim r As pointer(Of DT) = Nothing
        Return New event_comb(Function() As Boolean
                                  r = New pointer(Of DT)()
                                  ec = constructor(Of DT).resolve(v, r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         eva(o, +r) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Private Sub New()
    End Sub
End Class
