
Imports System.Threading
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.template
Imports osi.root.lock.slimlock

'a thread-safe queue
Public Class qless(Of T, lock_t As islimlock)
    Private f As pointernode(Of T) = Nothing
    Private e As pointernode(Of T) = Nothing
    Private _size As UInt32 = uint32_0
    Private l As lock_t

    Public Function size() As UInt32
        Return _size
    End Function

    Public Function empty() As Boolean
        Return size() = uint32_0
    End Function

    Public Sub clear()
        l.wait()
        f = Nothing
        e = Nothing
        _size = uint32_0
        l.release()
    End Sub

    Public Sub push(ByVal d As T)
        emplace(copy_no_error(d))
    End Sub

    Public Sub emplace(ByVal d As T)
        Dim n As pointernode(Of T) = Nothing
        n = New pointernode(Of T)(1)
        n.emplace(d)
        l.wait()
        If e Is Nothing Then
#If DEBUG Then
            assert(f Is Nothing)
            assert(_size = 0)
#End If
            f = n
        Else
            e.pointer(0) = n
        End If
        e = n
        _size += uint32_1
        l.release()
    End Sub

    Public Function pop() As T
        Dim o As T = Nothing
        If pop(o) Then
            Return o
        Else
            Return Nothing
        End If
    End Function

    Public Function pop(ByRef o As T) As Boolean
        Dim rtn As Boolean = False
        l.wait()
        If f Is Nothing Then
#If DEBUG Then
            assert(e Is Nothing)
            assert(_size = 0)
#End If
            rtn = False
        Else
            o = +f
            f = f.pointer(0)
            If f Is Nothing Then
                e = Nothing
            End If
#If DEBUG Then
            assert(_size > 0)
#End If
            _size -= uint32_1
            rtn = True
        End If
        l.release()
        Return rtn
    End Function
End Class

Public Class qless(Of T)
    Inherits qless(Of T, slimlock.monitorlock)
End Class
