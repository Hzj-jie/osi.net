
Imports System.Runtime.CompilerServices
Imports osi.root.template
Imports osi.root.delegates
Imports osi.root.connector

Public Module foreach
    Private Function foreach(Of IT)(ByVal d As _do(Of IT, Boolean, Boolean),
                                    ByVal begin As Func(Of IT),
                                    ByVal inc As void(Of IT),
                                    ByVal finish As _do(Of IT, Boolean)) As Boolean
        assert(Not d Is Nothing)
        assert(Not begin Is Nothing)
        assert(Not inc Is Nothing)
        Dim i As IT = Nothing
        i = begin()
        While Not finish(i)
            Dim c As Boolean = False
            If Not d(i, c) Then
                Return False
            ElseIf Not c Then
                Return True
            End If
            inc(i)
        End While
        Return True
    End Function

    <Extension()> Public Function foreach(Of KEY_T As IComparable(Of KEY_T), VALUE_T, HASH_SIZE As _int64) _
                                         (ByVal m As hashmap(Of KEY_T, VALUE_T, HASH_SIZE),
                                          ByVal d As _do(Of KEY_T, VALUE_T, Boolean, Boolean)) As Boolean
        If m Is Nothing OrElse d Is Nothing Then
            Return False
        Else
            Return foreach(Function(ByRef i, ByRef c) d((+i).first, (+i).second, c),
                           Function() m.begin(),
                           Sub(ByRef x) x += 1,
                           Function(ByRef x) x = m.end())
        End If
    End Function

    <Extension()> Public Function foreach(Of KEY_T As IComparable(Of KEY_T), VALUE_T) _
                                         (ByVal m As map(Of KEY_T, VALUE_T),
                                          ByVal d As _do(Of KEY_T, VALUE_T, Boolean, Boolean)) As Boolean
        If m Is Nothing OrElse d Is Nothing Then
            Return False
        Else
            Return foreach(Function(ByRef i, ByRef c) d((+i).first, (+i).second, c),
                           Function() m.begin(),
                           Sub(ByRef x) x += 1,
                           Function(ByRef x) x = m.end())
        End If
    End Function

    <Extension()> Public Function foreach(Of T)(ByVal v As vector(Of T),
                                                ByVal d As _do(Of T, Boolean, Boolean)) As Boolean
        If v Is Nothing OrElse d Is Nothing Then
            Return False
        Else
            Return foreach(Function(ByRef i, ByRef c) d(v(i), c),
                           Function() 0,
                           Sub(ByRef x) x += 1,
                           Function(ByRef x) x = v.size())
        End If
    End Function
End Module
