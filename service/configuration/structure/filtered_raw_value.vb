
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Friend Class filtered_raw_value(Of T)
    Private ReadOnly m As map(Of String, vector(Of pair(Of T, vector(Of pair(Of String, String)))))

    Public Sub New()
        m = New map(Of String, vector(Of pair(Of T, vector(Of pair(Of String, String)))))()
    End Sub

    Public Function empty() As Boolean
        Return m.empty()
    End Function

    Public Sub insert(ByVal name As String,
                      ByVal value As T,
                      ByVal filters As vector(Of pair(Of String, String)))
        m(name).emplace_back(pair.emplace_of(value, filters))
    End Sub

    Public Sub foreach(ByVal d As Action(Of String, T, vector(Of pair(Of String, String))))
        assert(Not d Is Nothing)
        m.stream().foreach(m.on_pair(Sub(ByVal k As String,
                                         ByVal v As vector(Of pair(Of T, vector(Of pair(Of String, String)))))
                                         If Not v.null_or_empty() Then
                                             v.stream().foreach(
                                                 Sub(ByVal p As pair(Of T, vector(Of pair(Of String, String))))
                                                     assert(Not p Is Nothing)
                                                     d(k, p.first, p.second)
                                                 End Sub)
                                         End If
                                     End Sub))
    End Sub
End Class
