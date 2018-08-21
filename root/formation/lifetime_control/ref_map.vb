
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
    End Sub

    Public Sub register(ByVal key As KEY_T, ByVal allocator As Func(Of VALUE_T))
        Using allocators_lock.scoped_write_lock
            assert(allocators.emplace(key, allocator).second)
        End Using
    End Sub

    Public Function [get](ByVal key As KEY_T) As ref_ptr(Of VALUE_T)
        SyncLock instances
            Dim it As unordered_map(Of KEY_T, ref_ptr(Of VALUE_T)).iterator = Nothing
            it = instances.find(key)
            If it <> instances.end() Then
                With (+it)
                    .second.ref()
                    Return .second
                End With
            End If
        End SyncLock
        Return create(key)
    End Function

    Public Function [get](ByVal key As KEY_T, ByVal allocator As Func(Of VALUE_T)) As ref_ptr(Of VALUE_T)
        register(key, allocator)
        Return [get](key)
    End Function

    Private Function create(ByVal key As KEY_T) As ref_ptr(Of VALUE_T)
        Dim allocator As Func(Of VALUE_T) = Nothing
        Dim it As unordered_map(Of KEY_T, Func(Of VALUE_T)).iterator = Nothing
        SyncLock allocators
            it = allocators.find(key)
            assert(it <> allocators.end())
        End SyncLock
        Dim r As ref_ptr(Of VALUE_T) = Nothing
        r = ref_ptr.[New]((+it).second(),
                          Sub(ByVal i As VALUE_T)
                              disposable(Of VALUE_T).dispose(i)
                              instances.erase(key)
                          End Sub)
        assert(instances.emplace(key, r).second)
        Return r
    End Function
End Class
