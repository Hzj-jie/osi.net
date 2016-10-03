
Imports System.Net
Imports osi.root.constants

<global_init(global_init_level.foundamental)>
Friend Module _comparer_registry
    Sub New()
        comparer.register(Function(i As IPAddress, j As IPAddress) As Int32
                              assert(Not i Is Nothing)
                              assert(Not j Is Nothing)
                              If i.AddressFamily() = j.AddressFamily() Then
                                  Return memcmp(i.GetAddressBytes(), j.GetAddressBytes())
                              Else
                                  Return i.AddressFamily() - j.AddressFamily()
                              End If
                          End Function)
        comparer.register(Function(i As String, j As String) As Int32
                              Return strcmp(i, j)
                          End Function)
    End Sub

    Private Sub init()
    End Sub
End Module
