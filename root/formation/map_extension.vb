
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector

Public Module _map_extension
    <Extension()> Public Function insert(Of KEY_T, VALUE_T)(ByVal this As map(Of KEY_T, VALUE_T),
                                                            ByVal that As map(Of KEY_T, VALUE_T)) As Boolean
        If this Is Nothing OrElse that Is Nothing Then
            Return False
        Else
            Dim it As map(Of KEY_T, VALUE_T).iterator = Nothing
            it = that.begin()
            While it <> that.end()
                assert(this.insert((+it).first, (+it).second) <> this.end())
                it += 1
            End While
            Return True
        End If
    End Function

    <Extension()> Public Sub renew(Of KEY_T, VALUE_T)(ByRef this As map(Of KEY_T, VALUE_T))
        If this Is Nothing Then
            this = New map(Of KEY_T, VALUE_T)()
        Else
            this.clear()
        End If
    End Sub
End Module
