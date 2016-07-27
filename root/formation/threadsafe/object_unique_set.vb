
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.template
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.delegates

Public Class object_unique_set(Of T As Class)
    Inherits object_unique_set(Of T, _true)
End Class

Public Class object_unique_set(Of T As Class, THREADSAFE As _boolean)
    Private Shared ReadOnly need_lock As Boolean = False
    Private ReadOnly v As vector(Of T) = Nothing
    Private l As duallock

    Shared Sub New()
        need_lock = +(alloc(Of THREADSAFE)())
    End Sub

    Public Sub New()
        v = New vector(Of T)()
    End Sub

    Protected Overridable Function object_same(ByVal i As T, ByVal j As T) As Boolean
        Return object_compare(i, j) = 0
    End Function

    Private Function unlocked_insert(ByVal o As T) As Boolean
        For i As Int64 = 0 To v.size() - 1
            If object_same(v(i), o) Then
                Return False
            End If
        Next
        v.push_back(o)
        Return True
    End Function

    Public Function insert(ByVal i As T) As Boolean
        If i Is Nothing Then
            Return False
        ElseIf need_lock Then
            Return l.writer_locked(Function() unlocked_insert(i))
        Else
            Return unlocked_insert(i)
        End If
    End Function

    Public Sub clear()
        If need_lock Then
            l.writer_locked(Sub() v.clear())
        Else
            v.clear()
        End If
    End Sub

    Private Function unlock_erase(ByVal w As T) As Boolean
        For i As UInt32 = 0 To v.size() - 1
            If object_same(v(i), w) Then
                v.erase(i)
                Return True
            End If
        Next
        Return False
    End Function

    Public Function [erase](ByVal w As T) As Boolean
        If w Is Nothing Then
            Return False
        ElseIf need_lock Then
            Return l.writer_locked(Function() unlock_erase(w))
        Else
            Return unlock_erase(w)
        End If
    End Function

    Default Public Property at(ByVal i As UInt32) As T
        Get
            Return l.reader_locked(Function() v(i))
        End Get
        Set(ByVal value As T)
            l.writer_locked(Sub() v(i) = value)
        End Set
    End Property

    Public Function [get](ByVal i As UInt32, ByRef o As T) As Boolean
        Dim x As T = Nothing
        If l.reader_locked(Function() As Boolean
                               If v.available_index(i) Then
                                   x = v(i)
                                   Return True
                               Else
                                   Return False
                               End If
                           End Function) Then
            o = x
            Return True
        Else
            Return False
        End If
    End Function

    Public Function size() As UInt32
        Return If(need_lock, l.reader_locked(Function() v.size()), v.size())
    End Function

    Public Function empty() As Boolean
        Return If(need_lock, l.reader_locked(Function() v.empty()), v.size())
    End Function

    Public Function foreach(ByVal d As _do(Of T, Boolean, Boolean)) As Boolean
        Return If(need_lock, l.reader_locked(Function() v.foreach(d)), v.foreach(d))
    End Function
End Class
