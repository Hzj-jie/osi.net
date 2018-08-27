
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.lock

' A ref-counted based map to return or create an instance from the key and predefined allocator function. It always
' returns a ref_ptr, calling ref_ptr.unref and reaching 0 will remove the reference in the ref_map itself.
Public NotInheritable Class ref_map(Of KEY_T, VALUE_T)
    Private ReadOnly allocators As unordered_map(Of KEY_T, Func(Of VALUE_T))
    Private ReadOnly instances As unordered_map(Of KEY_T, ref_ptr(Of VALUE_T))
    Private ReadOnly allocators_lock As rwlock
    Private ReadOnly instances_lock As rwlock

    Public Sub New()
        allocators = New unordered_map(Of KEY_T, Func(Of VALUE_T))()
        instances = New unordered_map(Of KEY_T, ref_ptr(Of VALUE_T))()
        allocators_lock = New rwlock()
        instances_lock = New rwlock()
    End Sub

    Public Sub register(ByVal key As KEY_T, ByVal allocator As Func(Of VALUE_T))
        ' TODO: Check whether the allocator has the same definition as the one in allocators.
        Using allocators_lock.scoped_write_lock()
            ' Two register function calls may run in parallel.
            allocators.emplace(key, allocator)
        End Using
    End Sub

    Public Function registered(ByVal key As KEY_T) As Boolean
        Using allocators_lock.scoped_read_lock()
            Return allocators.find(key) <> allocators.end()
        End Using
    End Function

    Public Function [get](ByVal key As KEY_T) As ref_ptr(Of VALUE_T)
        Using instances_lock.scoped_read_lock()
            Dim r As ref_ptr(Of VALUE_T) = Nothing
            If unlocked_get(key, r) Then
                Return r
            End If
        End Using
        Return create(key)
    End Function

    Public Function [get](ByVal key As KEY_T, ByVal allocator As Func(Of VALUE_T)) As ref_ptr(Of VALUE_T)
        register(key, allocator)
        Return [get](key)
    End Function

    Public Function created(ByVal key As KEY_T) As Boolean
        Using instances_lock.scoped_read_lock()
            Return instances.find(key) <> instances.end()
        End Using
    End Function

    Private Function unlocked_get(ByVal key As KEY_T, ByRef o As ref_ptr(Of VALUE_T)) As Boolean
        Dim it As unordered_map(Of KEY_T, ref_ptr(Of VALUE_T)).iterator = Nothing
        it = instances.find(key)
        If it <> instances.end() Then
            With (+it)
                .second.ref()
                o = .second
                Return True
            End With
        End If
        Return False
    End Function

    Private Function create(ByVal key As KEY_T) As ref_ptr(Of VALUE_T)
        Dim allocator As Func(Of VALUE_T) = Nothing
        Using allocators_lock.scoped_read_lock()
            Dim it As unordered_map(Of KEY_T, Func(Of VALUE_T)).iterator = Nothing
            it = allocators.find(key)
            assert(it <> allocators.end())
            allocator = (+it).second
        End Using
        Using instances_lock.scoped_write_lock()
            Dim r As ref_ptr(Of VALUE_T) = Nothing
            If unlocked_get(key, r) Then
                Return r
            End If
            r = ref_ptr.[New](allocator(),
                              Sub(ByVal i As VALUE_T)
                                  disposable(Of VALUE_T).dispose(i)
                                  instances.erase(key)
                              End Sub)
            assert(instances.emplace(key, r).second)
            Return r
        End Using
    End Function
End Class

Public NotInheritable Class ref_map
    Private NotInheritable Class global_instance(Of KEY_T, VALUE_T)
        Public Shared ReadOnly m As ref_map(Of KEY_T, VALUE_T)

        Shared Sub New()
            m = New ref_map(Of KEY_T, VALUE_T)()
        End Sub

        Private Sub New()
        End Sub
    End Class

    Public Shared Function [of](Of KEY_T, VALUE_T)() As ref_map(Of KEY_T, VALUE_T)
        Return global_instance(Of KEY_T, VALUE_T).m
    End Function

    Private Sub New()
    End Sub
End Class