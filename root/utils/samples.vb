
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Public NotInheritable Class samples
    Public Shared Function [of](Of T)(ByVal f As Func(Of T), ByVal count As UInt32) As samples(Of T)
        Return New samples(Of T)(f, count)
    End Function

    Private Sub New()
    End Sub
End Class

Public NotInheritable Class samples(Of T)
    Private ReadOnly v As vector(Of T)
    Private ReadOnly tc As debug_thread_checker
    Private index As Int64

    Public Sub New(ByVal f As Func(Of T), ByVal count As UInt32)
        Me.New(create_vector(f, count))
    End Sub

    Public Sub New(ByVal count As UInt32)
        Me.New(rnd_register(Of T).ref(), count)
    End Sub

    Public Sub New(ByVal v As vector(Of T))
        assert(Not v.null_or_empty())
        Me.v = v
        Me.tc = New debug_thread_checker()
        Me.index = 0
    End Sub

    Private Shared Function create_vector(ByVal f As Func(Of T), ByVal count As UInt32) As vector(Of T)
        assert(Not f Is Nothing)
        assert(count > 0)
        Dim r As vector(Of T) = Nothing
        r = New vector(Of T)()
        For i As UInt32 = 0 To count - uint32_1
            r.emplace_back(f())
        Next
        Return r
    End Function

    Public Function [next]() As T
        tc.assert()
        index = unchecked_inc(index)
        Return v.modget(index)
    End Function

    Public Sub reset()
        tc.assert()
        index = 0
    End Sub

    Public Function map(Of R)(ByVal f As Func(Of T, R)) As samples(Of R)
        Return New samples(Of R)(v.map(f))
    End Function
End Class
