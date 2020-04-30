
Imports osi.service.cache
Imports osi.root.formation
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utils

Partial Friend Class cached_ikeyvalue
    Private Class caches
        Private ReadOnly v As islimcache2(Of array_pointer(Of Byte), Byte())
        Private ReadOnly h As islimcache2(Of array_pointer(Of Byte), Boolean)
        Private ReadOnly s As islimcache2(Of array_pointer(Of Byte), Int64)
        Private ReadOnly max_value_size As UInt64

        Public Sub New(ByVal cached_count As UInt64,
                       ByVal max_value_size As UInt64)
            mapheap_slimecache2(Me.v, cached_count)
            mapheap_slimecache2(Me.h, cached_count)
            mapheap_slimecache2(Me.s, cached_count)
            Me.max_value_size = max_value_size
        End Sub

        Public Function read_get(ByVal key() As Byte, ByRef value() As Byte) As Boolean
            If v.get(array_pointer.of(key), value) Then
                Return True
            ElseIf havenot(key) Then
                value = Nothing
                Return True
            Else
                Return False
            End If
        End Function

        Public Function seek_get(ByVal key() As Byte, ByRef result As Boolean) As Boolean
            Dim h As Boolean = False
            Dim hn As Boolean = False
            h = have(key)
            hn = havenot(key)
            assert(Not (h AndAlso hn))
            If h Then
                result = True
                Return True
            ElseIf hn Then
                result = False
                Return True
            Else
                Return False
            End If
        End Function

        Public Function sizeof_get(ByVal key() As Byte, ByRef result As Int64) As Boolean
            Dim b() As Byte = Nothing
            If s.get(array_pointer.of(key), result) Then
                Return True
            ElseIf v.get(array_pointer.of(key), b) Then
                result = If(b Is Nothing, npos, array_size(b))
                Return True
            ElseIf havenot(key) Then
                result = npos
                Return True
            Else
                Return False
            End If
        End Function

        Public Function havenot(ByVal key() As Byte) As Boolean
            Dim v1() As Byte = Nothing
            Dim v2 As Boolean = False
            Dim v3 As Int64 = 0
            Return (v.get(array_pointer.of(key), v1) AndAlso
                    v1 Is Nothing) OrElse
                   (h.get(array_pointer.of(key), v2) AndAlso
                    Not v2) OrElse
                   (s.get(array_pointer.of(key), v3) AndAlso
                    v3 = npos)
        End Function

        Public Function have(ByVal key() As Byte) As Boolean
            Dim v1() As Byte = Nothing
            Dim v2 As Boolean = False
            Dim v3 As Int64 = 0
            Return (v.get(array_pointer.of(key), v1) AndAlso
                    Not v1 Is Nothing) OrElse
                   (h.get(array_pointer.of(key), v2) AndAlso
                    v2) OrElse
                   (s.get(array_pointer.of(key), v3) AndAlso
                    v3 >= 0)
        End Function

        Public Sub read_set(ByVal key() As Byte, ByVal result() As Byte)
            If result Is Nothing OrElse array_size(result) < max_value_size Then
                v.set(array_pointer.of(key), result)
            End If
            h.set(array_pointer.of(key), Not result Is Nothing)
            s.set(array_pointer.of(key), If(result Is Nothing, npos, array_size(result)))
        End Sub

        Public Sub append_set(ByVal key() As Byte, ByVal value() As Byte, ByVal result As Boolean)
            If result Then
                h.set(array_pointer.of(key), True)
                s.erase(array_pointer.of(key))
                v.erase(array_pointer.of(key))
            End If
        End Sub

        Public Sub delete_set(ByVal key() As Byte, ByVal result As Boolean)
            h.set(array_pointer.of(key), False)
            s.set(array_pointer.of(key), npos)
            v.set(array_pointer.of(key), Nothing)
        End Sub

        Public Sub seek_set(ByVal key() As Byte, ByVal result As Boolean)
            h.set(array_pointer.of(key), result)
            If Not result Then
                v.set(array_pointer.of(key), Nothing)
                s.set(array_pointer.of(key), npos)
            End If
        End Sub

        Public Sub modify_set(ByVal key() As Byte, ByVal value() As Byte, ByVal result As Boolean)
            If result Then
                read_set(key, value)
            End If
        End Sub

        Public Sub sizeof_set(ByVal key() As Byte, ByVal result As Int64)
            h.set(array_pointer.of(key), result >= 0)
            s.set(array_pointer.of(key), result)
            If result = npos Then
                v.set(array_pointer.of(key), Nothing)
            End If
        End Sub

        Public Sub retire_set()
            h.clear()
            v.clear()
            s.clear()
        End Sub

        Public Sub heartbeat_set()
            retire_set()
        End Sub
    End Class
End Class
