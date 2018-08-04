
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

' A ref-counted based map to return or create an instance from the key and predefined allocator function. It always
' returns a ref_ptr, calling ref_ptr.unref and reaching 0 will remove the reference in the ref_map itself.
Public NotInheritable Class ref_map(Of KEY_T, VALUE_T)
    Private ReadOnly allocators As unordered_map(Of KEY_T, Func(Of VALUE_T))
    Private ReadOnly instances As unordered_map(Of KEY_T, ref_ptr(Of VALUE_T))

    Public Sub New()
        allocators = New unordered_map(Of KEY_T, Func(Of VALUE_T))()
        instances = New unordered_map(Of KEY_T, ref_ptr(Of VALUE_T))()
    End Sub

    Public Sub register(ByVal key As KEY_T, ByVal allocator As Func(Of VALUE_T))
        SyncLock (allocators)
            assert(allocators.emplace(key, allocator).second)
        End SyncLock
    End Sub

    Public Function [get](ByVal key As KEY_T) As VALUE_T

    End Function

    Public Function register_get(ByVal key As KEY_T, ByVal allocator As Func(Of VALUE_T)) As VALUE_T
        register(key, allocator)
        Return [get](key)
    End Function
End Class
