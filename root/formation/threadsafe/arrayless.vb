
Imports osi.root.constants
Imports osi.root.connector
Imports lock_t = osi.root.lock.slimlock.monitorlock

Public NotInheritable Class arrayless
    Public Shared Function [New](Of T)(ByVal n As Func(Of UInt32, T), ByVal size As UInt32) As arrayless(Of T)
        Return New arrayless(Of T)(n, size)
    End Function

    Public Shared Function [New](Of T)(ByVal n As Func(Of T), ByVal size As UInt32) As arrayless(Of T)
        Return New arrayless(Of T)(n, size)
    End Function

    Private Sub New()
    End Sub
End Class

Public Class arrayless(Of T)
    Private Structure slot
        Public d As Boolean
        Public l As lock_t
        Public v As T
    End Structure

    Private ReadOnly c As Func(Of UInt32, T)
    Private ReadOnly a() As slot

    Private Shared Function remove_parameter(ByVal i As Func(Of T)) As Func(Of UInt32, T)
        If i Is Nothing Then
            Return Nothing
        Else
            Return Function(ByVal id As UInt32) As T
                       Return i()
                   End Function
        End If
    End Function

    Public Sub New(ByVal [New] As Func(Of UInt32, T), ByVal size As UInt32)
        If [New] Is Nothing Then
            Me.c = remove_parameter(AddressOf alloc(Of T))
        Else
            Me.c = [New]
        End If
        ReDim Me.a(size - uint32_1)
    End Sub

    Public Sub New(ByVal [New] As Func(Of T), ByVal size As UInt32)
        Me.New(remove_parameter([New]), size)
    End Sub

    Public Sub New(ByVal size As UInt32)
        Me.New([default](Of Func(Of UInt32, T)).null, size)
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
    Public Function [next](ByVal c As Func(Of UInt32, T), ByRef id As UInt32, ByRef o As T) As Boolean
        For i As UInt32 = 0 To size() - uint32_1
            If Not a(i).d Then
                Dim found As Boolean = False
                a(i).l.wait()
                If Not a(i).d Then
                    assert(Not c Is Nothing)
                    a(i).v = nothrow(c, i)
                    a(i).d = True
                    ' We expect the [New] function returns a valid instance, otherwise it means the allocation failed.
                    If type_info(Of T).is_valuetype OrElse Not a(i).v Is Nothing Then
                        id = i
                        o = a(i).v
                        found = True
                    End If
                End If
                a(i).l.release()
                If found Then
                    Return True
                End If
            End If
        Next
        Return False
    End Function

    Public Function [next](ByVal c As Func(Of T), ByRef id As UInt32, ByRef o As T) As Boolean
        Return [next](remove_parameter(c), id, o)
    End Function

    Public Function [next](ByVal c As Func(Of UInt32, T), ByRef id As UInt32) As T
        Dim o As T = Nothing
        assert([next](c, id, o))
        Return o
    End Function

    Public Function [next](ByVal c As Func(Of T), ByRef id As UInt32) As T
        Return [next](remove_parameter(c), id)
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
    Public Function [New](ByVal id As UInt32, ByVal c As Func(Of UInt32, T), ByRef o As T) As Boolean
        If id < size() Then
            If Not a(id).d Then
                a(id).l.wait()
                If Not a(id).d Then
                    assert(Not c Is Nothing)
                    a(id).v = nothrow(c, id)
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

    Public Function [New](ByVal id As UInt32, ByVal c As Func(Of T), ByRef o As T) As Boolean
        Return [New](id, remove_parameter(c), o)
    End Function

    Public Function [New](ByVal id As UInt32, ByVal c As Func(Of UInt32, T)) As T
        Dim o As T = Nothing
        assert([New](id, c, o))
        Return o
    End Function

    Public Function [New](ByVal id As UInt32, ByVal c As Func(Of T)) As T
        Return [New](id, remove_parameter(c))
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
