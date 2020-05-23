
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.template

Public NotInheritable Class object_unique_set(Of T As Class)
    Inherits object_unique_set(Of T, _true)
End Class

Public Class object_unique_set(Of T As Class, THREADSAFE As _boolean)
    Private Shared ReadOnly thread_safe As Boolean
    Private ReadOnly v As vector(Of T)
    Private l As duallock

    Shared Sub New()
        thread_safe = +(alloc(Of THREADSAFE)())
    End Sub

    Public Sub New()
        v = New vector(Of T)()
    End Sub

    Protected Overridable Function object_same(ByVal i As T, ByVal j As T) As Boolean
        Return object_compare(i, j) = 0
    End Function

    Private Function unlocked_insert(ByVal o As T) As Boolean
        Dim i As UInt32 = 0
        While i < v.size()
            If object_same(v(i), o) Then
                Return False
            End If
            i += uint32_1
        End While
        v.emplace_back(o)
        Return True
    End Function

    Public Function insert(ByVal i As T) As Boolean
        If i Is Nothing Then
            Return False
        End If
        If thread_safe Then
            Return l.writer_locked(Function() unlocked_insert(i))
        End If
        Return unlocked_insert(i)
    End Function

    Public Sub clear()
        If thread_safe Then
            l.writer_locked(Sub() v.clear())
        Else
            v.clear()
        End If
    End Sub

    Private Function unlock_erase(ByVal w As T) As Boolean
        Dim i As UInt32 = 0
        While i < v.size()
            If object_same(v(i), w) Then
                v.erase(i)
                Return True
            End If
            i += uint32_1
        End While
        Return False
    End Function

    Public Function [erase](ByVal w As T) As Boolean
        If w Is Nothing Then
            Return False
        End If
        If thread_safe Then
            Return l.writer_locked(Function() unlock_erase(w))
        End If
        Return unlock_erase(w)
    End Function

    Default Public Property at(ByVal i As UInt32) As T
        Get
            If thread_safe Then
                Return l.reader_locked(Function() v(i))
            End If
            Return v(i)
        End Get
        Set(ByVal value As T)
            If thread_safe Then
                l.writer_locked(Sub() v(i) = value)
            Else
                v(i) = value
            End If
        End Set
    End Property

    Private Function unlocked_get(ByVal i As UInt32, ByRef o As T) As Boolean
        If v.available_index(i) Then
            o = v(i)
            Return True
        End If
        Return False
    End Function

    Public Function [get](ByVal i As UInt32, ByRef o As T) As Boolean
        If Not thread_safe Then
            Return unlocked_get(i, o)
        End If

        Dim x As T = Nothing
        If l.reader_locked(Function() As Boolean
                               Return unlocked_get(i, x)
                           End Function) Then
            o = x
            Return True
        End If
        Return False
    End Function

    Public Function size() As UInt32
        Return If(thread_safe, l.reader_locked(Function() v.size()), v.size())
    End Function

    Public Function empty() As Boolean
        Return If(thread_safe, l.reader_locked(Function() v.empty()), v.empty())
    End Function

    Public Sub foreach(ByVal f As Action(Of T))
        If thread_safe Then
            l.reader_locked(Sub()
                                v.stream().foreach(f)
                            End Sub)
        Else
            v.stream().foreach(f)
        End If
    End Sub
End Class
