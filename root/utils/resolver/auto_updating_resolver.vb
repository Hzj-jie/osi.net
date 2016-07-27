
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.lock
Imports osi.root.formation
Imports osi.root.connector

Partial Public Class resolver
    Private Shared Function auto_updating_resolve(Of VT) _
                                                 (ByVal T As Type,
                                                  ByRef p As disposer(Of weak_pointer(Of VT))) _
                                                 As Boolean
        If T Is Nothing Then
            Return False
        Else
            Dim r As weak_pointer(Of disposer(Of weak_pointer(Of VT))) = Nothing
            Dim handle_registered As registeredEventHandler =
                Sub(x As Type, y As Func(Of Object))
                    If comparable_type.compare_type(x, T) = 0 AndAlso Not y Is Nothing Then
                        Dim o As VT = Nothing
                        o = cast(Of VT)(y())
                        If isdebugmode() Then
                            raise_error("auto_updating_resolver updates ",
                                        T.FullName(),
                                        " as ",
                                        GetType(VT).FullName())
                        End If
                        Dim ad As disposer(Of weak_pointer(Of VT)) = Nothing
                        ad = (+r)
                        If Not ad Is Nothing Then
                            ad.get().set(o)
                        End If
                    End If
                End Sub
            Dim handle_erased As erasedEventHandler =
                Sub(x As Type)
                    If comparable_type.compare_type(x, T) = 0 Then
                        If isdebugmode() Then
                            raise_error("auto_updating_resolver updates ",
                                        T.FullName(),
                                        " as Nothing")
                        End If
                        r.get().clear()
                    End If
                End Sub
            p = New disposer(Of weak_pointer(Of VT)) _
                            (New weak_pointer(Of VT)(),
                             disposer:=Sub(x As weak_pointer(Of VT))
                                           RemoveHandler registered, handle_registered
                                           RemoveHandler erased, handle_erased
                                       End Sub)
            r = New weak_pointer(Of disposer(Of weak_pointer(Of VT)))(p)
            AddHandler registered, handle_registered
            AddHandler erased, handle_erased
            handle_registered(T, resolver(T))
            Return True
        End If
    End Function

    Public Shared Function auto_updating_resolve(ByVal T As Type,
                                                 ByRef p As disposer(Of weak_pointer(Of Object))) _
                                                As Boolean
        Return auto_updating_resolve(Of Object)(T, p)
    End Function

    Public Shared Function auto_updating_resolve(ByVal T As Type) As disposer(Of weak_pointer(Of Object))
        Dim p As disposer(Of weak_pointer(Of Object)) = Nothing
        assert(auto_updating_resolve(T, p))
        Return p
    End Function

    Public Shared Function auto_updating_resolve(Of T) _
                                                (ByRef p As disposer(Of weak_pointer(Of T))) _
                                                As Boolean
        Return auto_updating_resolve(GetType(T), p)
    End Function

    Public Shared Function auto_updating_resolve(Of T)() As disposer(Of weak_pointer(Of T))
        Dim p As disposer(Of weak_pointer(Of T)) = Nothing
        assert(auto_updating_resolve(Of T)(p))
        Return p
    End Function
End Class
