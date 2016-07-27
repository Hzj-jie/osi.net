
Imports osi.root.connector
Imports osi.root.delegates

Public Module _foreach
    Public Function foreach(Of T1, T2)(ByVal f As Func(Of _do(Of T1, T2, Boolean, Boolean), Boolean),
                                       ByVal f2 As _do(Of T1, T2, Boolean)) As Boolean
        assert(Not f Is Nothing)
        If f2 Is Nothing Then
            Return False
        Else
            Return f(Function(ByRef x As T1, ByRef y As T2, ByRef c As Boolean) As Boolean
                         c = True
                         Return f2(x, y)
                     End Function)
        End If
    End Function

    Public Function foreach(Of T1, T2)(ByVal f As Func(Of _do(Of T1, T2, Boolean, Boolean), Boolean),
                                       ByVal f2 As _do(Of T1, Boolean, Boolean)) As Boolean
        assert(Not f Is Nothing)
        If f2 Is Nothing Then
            Return False
        Else
            Return f(Function(ByRef x, ByRef y, ByRef c) f2(x, c))
        End If
    End Function

    Public Function foreach(Of T1, T2)(ByVal f As Func(Of _do(Of T1, T2, Boolean, Boolean), Boolean),
                                       ByVal f2 As _do(Of T2, Boolean, Boolean)) As Boolean
        assert(Not f Is Nothing)
        If f2 Is Nothing Then
            Return False
        Else
            Return f(Function(ByRef x, ByRef y, ByRef c) f2(y, c))
        End If
    End Function

    Public Function foreach(Of T1, T2)(ByVal f As Func(Of _do(Of T1, T2, Boolean, Boolean), Boolean),
                                       ByVal f2 As void(Of T1, T2, Boolean)) As Boolean
        assert(Not f Is Nothing)
        If f2 Is Nothing Then
            Return False
        Else
            Return f(Function(ByRef i As T1, ByRef j As T2, ByRef c As Boolean) As Boolean
                         f2(i, j, c)
                         Return True
                     End Function)
        End If
    End Function

    Public Function foreach(Of T1, T2)(ByVal f As Func(Of _do(Of T1, T2, Boolean, Boolean), Boolean),
                                       ByVal f2 As void(Of T1, T2)) As Boolean
        assert(Not f Is Nothing)
        If f2 Is Nothing Then
            Return False
        Else
            Return f(Function(ByRef i As T1, ByRef j As T2, ByRef c As Boolean) As Boolean
                         c = True
                         f2(i, j)
                         Return True
                     End Function)
        End If
    End Function

    Public Function foreach(Of T)(ByVal f As Func(Of _do(Of T, Boolean, Boolean), Boolean),
                                  ByVal f2 As _do(Of T, Boolean)) As Boolean
        assert(Not f Is Nothing)
        If f2 Is Nothing Then
            Return False
        Else
            Return f(Function(ByRef x As T, ByRef c As Boolean) As Boolean
                         c = True
                         Return f2(x)
                     End Function)
        End If
    End Function

    Public Function foreach(Of T)(ByVal f As Func(Of _do(Of T, Boolean, Boolean), Boolean),
                                  ByVal f2 As Func(Of T, Boolean)) As Boolean
        assert(Not f Is Nothing)
        If f2 Is Nothing Then
            Return False
        Else
            Return f(Function(ByRef x As T, ByRef c As Boolean) As Boolean
                         c = True
                         Return f2(x)
                     End Function)
        End If
    End Function

    Public Function foreach(Of T)(ByVal f As Func(Of _do(Of T, Boolean, Boolean), Boolean),
                                  ByVal f2 As void(Of T)) As Boolean
        assert(Not f Is Nothing)
        If f2 Is Nothing Then
            Return False
        Else
            Return f(Function(ByRef i As T, ByRef c As Boolean) As Boolean
                         c = True
                         f2(i)
                         Return True
                     End Function)
        End If
    End Function

    Public Function foreach(Of T)(ByVal f As Func(Of _do(Of T, Boolean, Boolean), Boolean),
                                  ByVal f2 As Action(Of T)) As Boolean
        assert(Not f Is Nothing)
        If f2 Is Nothing Then
            Return False
        Else
            Return f(Function(ByRef x As T, ByRef c As Boolean) As Boolean
                         c = True
                         f2(x)
                         Return True
                     End Function)
        End If
    End Function
End Module
