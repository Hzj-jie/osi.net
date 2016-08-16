
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

    Private ReadOnly a() As slot

    Public Sub New(ByVal size As UInt32)
        ReDim Me.a(size - uint32_1)
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

    Public Function [New](ByVal id As UInt32, ByVal c As Func(Of T), ByRef o As T) As Boolean
        assert(Not c Is Nothing)
        If id < size() Then
            If Not a(id).d Then
                a(id).l.wait()
                If Not a(id).d Then
                    a(id).v = c()
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
        Return [New](id, AddressOf alloc(Of T), o)
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
