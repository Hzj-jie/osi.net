﻿
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.lock
Imports lock_t = osi.root.lock.slimlock.monitorlock

Public Class arrayless(Of T)
    Private Structure slot
        Public d As Boolean
        Public l As lock_t
        Public v As T
    End Structure

    Private ReadOnly c As Func(Of T)
    Private ReadOnly a() As slot

    Public Sub New(ByVal [New] As Func(Of T), ByVal size As UInt32)
        If [New] Is Nothing Then
            Me.c = AddressOf alloc(Of T)
        Else
            Me.c = [New]
        End If
        ReDim Me.a(size - uint32_1)
    End Sub

    Public Sub New(ByVal size As UInt32)
        Me.New(Nothing, size)
    End Sub

    Public Sub clear()
        For i As UInt32 = 0 To size() - uint32_1
            a(i).d = False
        Next
    End Sub

    Public Function size() As UInt32
        Return array_size(a)
    End Function

    Public Function created_count() As UInt32
        Dim r As UInt32 = 0
        For i As UInt32 = 0 To size() - uint32_1
            If a(i).d Then
                r += uint32_1
            End If
        Next
        Return r
    End Function

    ' Create a new instance in an empty slot by using @c, and set the new @id and @o. If there is no empty slot, this
    ' function returns false.
    Public Function [next](ByVal c As Func(Of T), ByRef id As UInt32, ByRef o As T) As Boolean
        assert(Not c Is Nothing)
        For i As UInt32 = 0 To size() - uint32_1
            If Not a(i).d Then
                Dim found As Boolean = False
                a(i).l.wait()
                If Not a(i).d Then
                    a(i).v = nothrow(c)
                    a(i).d = True
                    id = i
                    o = a(i).v
                    found = True
                End If
                a(i).l.release()
                If found Then
                    Return True
                End If
            End If
        Next
        Return False
    End Function

    Public Function [next](ByVal c As Func(Of T), ByRef id As UInt32) As T
        Dim o As T = Nothing
        assert([next](c, id, o))
        Return o
    End Function

    Public Function [next](ByRef id As UInt32, ByRef o As T) As Boolean
        Return [next](c, id, o)
    End Function

    Public Function [next](ByRef id As UInt32) As T
        Dim o As T = Nothing
        assert([next](id, o))
        Return o
    End Function

    ' Create or return the data at @id to @o. If the slot is empty, use @c to create a new instance of @T. If @id is not
    ' in the range [0, #size()), this function returns false.
    Public Function [New](ByVal id As UInt32, ByVal c As Func(Of T), ByRef o As T) As Boolean
        assert(Not c Is Nothing)
        If id < size() Then
            If Not a(id).d Then
                a(id).l.wait()
                If Not a(id).d Then
                    a(id).v = nothrow(c)
                    a(id).d = True
                End If
                a(id).l.release()
            End If
            assert(a(id).d)
            o = a(id).v
            Return True
        Else
            Return False
        End If
    End Function

    Public Function [New](ByVal id As UInt32, ByVal c As Func(Of T)) As T
        Dim o As T = Nothing
        assert([New](id, c, o))
        Return o
    End Function

    Public Function [New](ByVal id As UInt32, ByRef o As T) As Boolean
        Return [New](id, c, o)
    End Function

    Public Function [New](ByVal id As UInt32) As T
        Dim o As T = Nothing
        assert([New](id, o))
        Return o
    End Function

    Public Function has(ByVal id As UInt32, ByRef o As Boolean) As Boolean
        If id < size() Then
            o = a(id).d
            Return True
        Else
            Return False
        End If
    End Function

    Public Function has(ByVal id As UInt32) As Boolean
        Dim o As Boolean = False
        assert(has(id, o))
        Return o
    End Function

    ' Returns true if id < size() and a(id) has been created.
    Public Function [get](ByVal id As UInt32, ByRef o As T) As Boolean
        If id < size() Then
            If Not a(id).d Then
                Return False
            Else
                Dim r As Boolean = False
                a(id).l.wait()
                If a(id).d Then
                    o = a(id).v
                    r = True
                Else
                    r = False
                End If
                a(id).l.release()
                Return r
            End If
        Else
            Return False
        End If
    End Function

    Default Public ReadOnly Property data(ByVal id As UInt32) As T
        Get
            Dim o As T = Nothing
            assert([get](id, o))
            Return o
        End Get
    End Property

    Public Function [erase](ByVal id As UInt32) As Boolean
        If id < size() Then
            If Not a(id).d Then
                Return False
            Else
                Dim r As Boolean = False
                a(id).l.wait()
                If a(id).d Then
                    a(id).d = False
                    a(id).v = Nothing
                    r = True
                Else
                    r = False
                End If
                a(id).l.release()
                Return r
            End If
        Else
            Return False
        End If
    End Function
End Class
