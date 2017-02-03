
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.template
Imports osi.root.connector

Public Module _fixed_stack_wrapper
    <Extension()> Public Function back(Of T, _MAX_SIZE As _int64) _
                                      (ByVal this As ref(Of fixed_stack(Of T, _MAX_SIZE))) As T
        assert(Not this Is Nothing)
        Return this.p.back()
    End Function

    <Extension()> Public Sub pop(Of T, _MAX_SIZE As _int64) _
                                (ByVal this As ref(Of fixed_stack(Of T, _MAX_SIZE)))
        assert(Not this Is Nothing)
        this.p.pop()
    End Sub

    <Extension()> Public Sub push(Of T, _MAX_SIZE As _int64) _
                                 (ByVal this As ref(Of fixed_stack(Of T, _MAX_SIZE)),
                                  ByVal i As T)
        assert(Not this Is Nothing)
        this.p.push(i)
    End Sub

    <Extension()> Public Sub emplace(Of T, _MAX_SIZE As _int64) _
                                    (ByVal this As ref(Of fixed_stack(Of T, _MAX_SIZE)),
                                     ByVal i As T)
        assert(Not this Is Nothing)
        this.p.emplace(i)
    End Sub

    <Extension()> Public Function empty(Of T, _MAX_SIZE As _int64) _
                                       (ByVal this As ref(Of fixed_stack(Of T, _MAX_SIZE))) As Boolean
        assert(Not this Is Nothing)
        Return this.p.empty()
    End Function

    <Extension()> Public Function full(Of T, _MAX_SIZE As _int64) _
                                      (ByVal this As ref(Of fixed_stack(Of T, _MAX_SIZE))) As Boolean
        assert(Not this Is Nothing)
        Return this.p.full()
    End Function

    <Extension()> Public Sub clear(Of T, _MAX_SIZE As _int64) _
                                  (ByVal this As ref(Of fixed_stack(Of T, _MAX_SIZE)))
        assert(Not this Is Nothing)
        this.p.clear()
    End Sub
End Module

Public Structure fixed_stack(Of T, _MAX_SIZE As _int64)
    Private Shared ReadOnly MAX_SIZE As Int64 = 0
    Private index As Int64
    Private q() As T

    Shared Sub New()
        MAX_SIZE = +(alloc(Of _MAX_SIZE)())
        assert(DirectCast(Nothing, T()) Is Nothing)
        assert(DirectCast(Nothing, Int64) = 0)
    End Sub

    Private Sub create()
        If q Is Nothing Then
            ReDim q(CInt(MAX_SIZE) - 1)
            assert(Not q Is Nothing)
        End If
    End Sub

    Public Function back() As T
#If DEBUG Then
        assert(Not empty())
#End If
        Dim r As T = Nothing
        r = q(CInt(index) - 1)
        Return r
    End Function

    Public Sub pop()
#If DEBUG Then
        assert(Not empty())
#End If
        index -= 1
        q(CInt(index)) = Nothing
    End Sub

    Public Sub push(ByVal i As T)
        emplace(copy_no_error(i))
    End Sub

    Public Sub emplace(ByVal i As T)
        create()
#If DEBUG Then
        assert(Not full())
#End If
        q(CInt(index)) = i
        index += 1
    End Sub

    Private Function index_is(ByVal expected As Int64) As Boolean
        assert(index >= 0 AndAlso index <= MAX_SIZE)
        Return index = expected
    End Function

    Public Function empty() As Boolean
        Return index_is(0)
    End Function

    Public Function full() As Boolean
        Return index_is(MAX_SIZE)
    End Function

    Public Sub clear()
        index = 0
        memclr(q)
    End Sub
End Structure
