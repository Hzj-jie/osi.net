
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.connector
Imports lock_t = osi.root.lock.slimlock.monitorlock

Public NotInheritable Class collectionless(Of T)
    Private ReadOnly v As vector(Of T)
    Private ReadOnly f As [set](Of UInt32)
    Private l As lock_t

    Public Sub New()
        v = New vector(Of T)()
        f = New [set](Of UInt32)()
    End Sub

    Public Sub clear()
        l.wait()
        v.clear()
        v.shrink_to_fit()
        f.clear()
        l.release()
    End Sub

    Public Function free_pool_size() As UInt32
        Return f.size()
    End Function

    Public Function pool_size() As UInt32
        Return v.size()
    End Function

    Public Function emplace(ByVal d As T) As UInt32
        Dim r As UInt32 = uint32_0
        l.wait()
        If f.empty() Then
            v.emplace_back(d)
            r = v.size() - uint32_1
        Else
            r = +(f.begin())
            f.erase(f.begin())
            v(r) = d
        End If
        l.release()
        Return r
    End Function

    Public Function push(ByVal v As T) As UInt32
        Return emplace(copy_no_error(v))
    End Function

    Public Sub [erase](ByVal p As UInt32)
        l.wait()
        v(p) = [default](Of T).null
        f.emplace(p)
        l.release()
    End Sub

    Public Function size() As UInt32
        l.wait()
        Dim r As UInt32 = uint32_0
#If DEBUG Then
        Dim x As Int64 = 0
        x = CLng(v.size()) - f.size()
        assert(x >= 0)
        assert(x <= max_uint32)
        r = CUInt(x)
#Else
        r = v.size() - f.size()
#End If
        l.release()
        Return r
    End Function

    Public Function empty() As Boolean
        Return size() = uint32_0
    End Function

    Default Public ReadOnly Property at(ByVal p As UInt32) As T
        Get
            Return v(p)
        End Get
    End Property
End Class
