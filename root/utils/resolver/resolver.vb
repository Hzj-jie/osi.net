
Imports osi.root.lock
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.formation

Partial Public Class resolver
    Public Shared Event registered(ByVal T As Type, ByVal resolver As Func(Of Object))
    Public Shared Event erased(ByVal T As Type)
    Private Shared ReadOnly creator As map(Of comparable_type, Func(Of Object)) = Nothing
    Private Shared l As duallock
    Private Shared _fixed As Boolean = False

    Shared Sub New()
        creator = New map(Of comparable_type, Func(Of Object))()
    End Sub

    'test purpose only
    Public Shared Function registered_event_count() As Int64
        Return attached_delegate_count(registeredEvent)
        'Return attached_event_count(GetType(_resolver), "registered")
    End Function

    Public Shared Function erased_event_count() As Int64
        Return attached_delegate_count(erasedEvent)
        'Return attached_event_count(GetType(_resolver), "erased")
    End Function

    Private Shared Function default_resolver(Of T)(ByVal i As T) As Func(Of T)
        Return Function() As T
                   Return i
               End Function
    End Function

    Private Shared Sub lock_reader(ByVal d As Action)
        assert(Not d Is Nothing)
        If fixed() Then
            d()
        Else
            l.reader_locked(d)
        End If
    End Sub

    Public Shared Sub fix()
        l.writer_locked(Sub() _fixed = True)
    End Sub

    Public Shared Function fixed() As Boolean
        Return _fixed
    End Function

    Public Shared Function size() As Int32
        Return creator.size()
    End Function

    Public Shared Sub register(ByVal T As Type, ByVal i As Object)
        assert(Not T Is Nothing)
        assert(Not i Is Nothing)
        register(T, default_resolver(i))
    End Sub

    Public Shared Sub register(Of T)(ByVal i As T)
        assert(Not i Is Nothing)
        register(Of T)(default_resolver(i))
    End Sub

    Public Shared Sub register(ByVal T As Type, ByVal d As Func(Of Object))
        assert(Not T Is Nothing)
        assert(Not d Is Nothing)
        If fixed() Then
            assert(False, "should not register after fix() has been called")
        Else
            l.writer_locked(Sub() creator(New comparable_type(T)) = d)
            RaiseEvent registered(T, d)
        End If
    End Sub

    Private Shared Function assert_create(Of T2)(ByVal T As Type, ByVal d As Func(Of T2)) As Object
        assert(Not T Is Nothing)
        assert(Not d Is Nothing)
        Dim rtn As Object = Nothing
        rtn = d()
        If rtn Is Nothing AndAlso Not T.IsValueType() Then
            assert(False, "failed to resolve ", T.full_name(), " via ", d.method_identity())
        End If
        Return rtn
    End Function

    Public Shared Sub register(Of T)(ByVal d As Func(Of T))
        assert(Not d Is Nothing)
        Dim type As Type = Nothing
        type = GetType(T)
        register(type, Function() As Object
                           Return assert_create(type, d)
                       End Function)
    End Sub

    Public Shared Function resolve(ByVal T As Type, ByRef o As Object) As Boolean
        assert(Not T Is Nothing)
        Dim create As Func(Of Object) = Nothing
        create = resolver(T)
        If create Is Nothing Then
            Return False
        Else
            o = assert_create(T, create)
            Return True
        End If
    End Function

    Public Shared Function resolve(ByVal T As Type) As Object
        assert(Not T Is Nothing)
        Dim o As Object = Nothing
        assert(resolve(T, o), "type ", T.full_name(), " has not been registered.")
        Return o
    End Function

    Public Shared Function resolve(Of T)(ByRef o As T) As Boolean
        Dim i As Object = Nothing
        Return resolve(GetType(T), i) AndAlso cast(Of T)(i, o)
    End Function

    Public Shared Function resolve(Of T)() As T
        Return cast(Of T)(resolve(GetType(T)))
    End Function

    Private Shared Function unlocked_has_resolver(ByVal T As comparable_type) As Boolean
        Return creator.find(T) <> creator.end()
    End Function

    Public Shared Function has_resolver(ByVal T As Type) As Boolean
        assert(Not T Is Nothing)
        Dim rtn As Boolean
        lock_reader(Sub()
                        rtn = unlocked_has_resolver(New comparable_type(T))
                    End Sub)
        Return rtn
    End Function

    Public Shared Function has_resolver(Of T)() As Boolean
        Return has_resolver(GetType(T))
    End Function

    Public Shared Function resolver(ByVal T As Type) As Func(Of Object)
        assert(Not T Is Nothing)
        Dim rtn As Func(Of Object) = Nothing
        Dim ct As comparable_type = Nothing
        ct = New comparable_type(T)
        lock_reader(Sub()
                        If unlocked_has_resolver(ct) Then
                            rtn = creator(ct)
                        Else
                            rtn = Nothing
                        End If
                    End Sub)
        Return rtn
    End Function

    Public Shared Function resolver(Of T)() As Func(Of Object)
        Return resolver(GetType(T))
    End Function

    Public Shared Function [erase](ByVal T As Type) As Boolean
        If Not T Is Nothing AndAlso l.writer_locked(Function() creator.erase(New comparable_type(T))) Then
            RaiseEvent erased(T)
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function [erase](Of T)() As Boolean
        Return [erase](GetType(T))
    End Function

    Public Shared Function resolve_or_alloc(Of T)() As T
        If has_resolver(Of T)() Then
            Return resolve(Of T)()
        Else
            Return alloc(Of T)()
        End If
    End Function

    Public Shared Function check_resolve_or_alloc(Of T As Class)(ByVal i As T) As T
        If i Is Nothing Then
            Return resolve_or_alloc(Of T)()
        Else
            Return i
        End If
    End Function
End Class
