
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.argument

' For client to directly construct an instance of T from var. i.e. constructor(Of T).resolve(v, r)
Public NotInheritable Class constructor(Of T)
    Private Shared ReadOnly lt As New unique_strong_map(Of String, Func(Of var, ref(Of T), event_comb))()
    Private Shared l As Func(Of var, ref(Of T), event_comb)

    Public Shared Function empty() As Boolean
        Return lt.empty() AndAlso l Is Nothing
    End Function

    Public Shared Function register(ByVal allocator As Func(Of var, ref(Of T), event_comb)) As Boolean
        If allocator Is Nothing Then
            Return False
        End If
        Return atomic.set_if_nothing(l, allocator)
    End Function

    Public Shared Function [erase]() As Boolean
        If l Is Nothing Then
            Return False
        End If
        l = Nothing
        Return True
    End Function

    Public Shared Function register(ByVal type As String,
                                    ByVal allocator As Func(Of var, ref(Of T), event_comb)) As Boolean
        If type.null_or_empty() OrElse allocator Is Nothing Then
            Return False
        End If
        Return lt.set(type, allocator)
    End Function

    Public Shared Function [erase](ByVal type As String) As Boolean
        Return lt.erase(type)
    End Function

    Public Shared Sub clear()
        [erase]()
        lt.clear()
    End Sub

    Public Shared Function resolve(ByVal v As var, ByVal o As ref(Of T)) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If v Is Nothing Then
                                      Return False
                                  End If
                                  Dim allocator As Func(Of var, ref(Of T), event_comb) = Nothing
                                  If lt.get(v("type"), allocator) OrElse
                                     (Not l Is Nothing AndAlso eva(allocator, l)) Then
                                      assert(Not allocator Is Nothing)
                                      o.renew()
                                      ec = allocator(v, o)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                                  Return False
                              End Function,
                              Function() As Boolean
                                  Dim r As T = Nothing
                                  Return ec.end_result() AndAlso
                                         wrapper.wrap(v, +o, r) AndAlso
                                         eva(o, r) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Shared Function resolve(Of DT As T)(ByVal v As var, ByVal o As ref(Of T)) As event_comb
        Dim ec As event_comb = Nothing
        Dim r As ref(Of DT) = Nothing
        Return New event_comb(Function() As Boolean
                                  r = New ref(Of DT)()
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

    Public Shared Function resolve(Of RT As T)(ByVal v As var, ByVal o As ref(Of RT)) As event_comb
        Dim ec As event_comb = Nothing
        Dim r As ref(Of T) = Nothing
        Return New event_comb(Function() As Boolean
                                  r = New ref(Of T)()
                                  ec = resolve(v, r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Dim t As RT = Nothing
                                  Return ec.end_result() AndAlso
                                         cast(+r, t) AndAlso
                                         eva(o, t) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Private Sub New()
    End Sub
End Class
