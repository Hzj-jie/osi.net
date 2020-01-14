
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.lock

Public Class pool(Of T As Class)
    Protected Overridable Function create() As T
        Return alloc(Of T)()
    End Function

    Private ReadOnly p As vector(Of pair(Of T, Boolean))
    Private l As slimlock.monitorlock

    Protected Sub New(ByVal max_size As UInt32, ByVal fill As Boolean)
        p = New vector(Of pair(Of T, Boolean))(max_size)
        If fill Then
            For i As Int32 = 0 To p.size() - 1
                set_i(i)
            Next
        End If
    End Sub

    Protected Sub New(ByVal fill As Boolean)
        Me.New(default_pool_max_size, fill)
    End Sub

    Public Sub New(ByVal max_size As UInt32)
        Me.New(max_size, False)
    End Sub

    Public Sub New()
        Me.New(default_pool_max_size)
    End Sub

    Protected Sub set_i(ByVal i As Int32, Optional ByRef n As T = Nothing)
        assert(i >= 0 AndAlso i < p.size())
        assert(p(i) Is Nothing)
        n = create()
        assert(Not n Is Nothing)
        p.emplace(i, pair.emplace_of(n, True))
    End Sub

    Public Function unlocked_get(ByRef o As T, ByRef index As Int32) As Boolean
        For i As Int32 = 0 To p.size() - 1
            If Not p(i) Is Nothing AndAlso
               p(i).second Then
                assert(Not p(i).first Is Nothing)
                p(i).second = False
                o = p(i).first
                index = i
                Return True
            End If
        Next

        For i As Int32 = 0 To p.size() - 1
            If p(i) Is Nothing Then
                set_i(i, o)
                index = i
                Return True
            End If
        Next

        Return False
    End Function

    Public Function [get](ByRef o As T, ByRef index As Int32) As Boolean
        l.wait()
        Dim b As Boolean = False
        b = unlocked_get(o, index)
        l.release()
        Return b
    End Function

    Public Function release(ByVal index As Int32) As Boolean
        If index >= 0 AndAlso
           index < p.size() AndAlso
           Not p(index).second Then
            p(index).second = True
            Return True
        Else
            Return False
        End If
    End Function
End Class
