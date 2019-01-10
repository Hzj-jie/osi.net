
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.template

Public Module _hasharray_ext
    Private Function insert(Of T, UNIQUE As _boolean) _
                           (ByVal this As hasharray(Of T, UNIQUE),
                            ByVal that As hasharray(Of T, UNIQUE),
                            ByVal f As Func(Of T, fast_pair(Of hasharray(Of T, UNIQUE).iterator, Boolean))) As Boolean
        assert(Not f Is Nothing)
        If this Is Nothing OrElse that Is Nothing Then
            Return False
        End If
        Dim it As hasharray(Of T, UNIQUE).iterator = Nothing
        it = that.begin()
        While it <> that.end()
            assert(f(+it).first <> this.end())
            it += 1
        End While
        Return True
    End Function

    <Extension()> Public Function insert(Of T, UNIQUE As _boolean)(ByVal this As hasharray(Of T, UNIQUE),
                                                                   ByVal that As hasharray(Of T, UNIQUE)) As Boolean
        Return insert(this, that, AddressOf this.insert)
    End Function

    <Extension()> Public Function emplace(Of T, UNIQUE As _boolean)(ByVal this As hasharray(Of T, UNIQUE),
                                                                    ByVal that As hasharray(Of T, UNIQUE)) As Boolean
        Return insert(this, that, AddressOf this.emplace)
    End Function
End Module
