
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.connector
Imports osi.root.utils

Public Class istrkeyvt_node
    Implements inode

    Public ReadOnly accessor As istrkeyvt
    Public ReadOnly properties_property As istrkeyvt_property
    Public ReadOnly subnodes_property As istrkeyvt_property
    Private ReadOnly p As String

    Public Sub New(ByVal path As String, ByVal accessor As istrkeyvt)
        assert(Not accessor Is Nothing)
        assert(Not path.null_or_empty())
        Me.accessor = accessor
        Me.p = path
        Me.properties_property = istrkeyvt_property.create_properties_property(path, accessor)
        Me.subnodes_property = istrkeyvt_property.create_subnodes_property(path, accessor)
    End Sub

    Public Function create(ByVal name As String,
                           ByVal o As ref(Of iproperty),
                           Optional ByVal wait_ms As Int64 = npos) As event_comb Implements inode.create
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If valid_name(name) Then
                                      ec = properties_property.locked_push_back(name, wait_ms)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      ec = open(name, o)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function open(ByVal name As String,
                         ByVal o As ref(Of iproperty)) As event_comb Implements inode.open
        Return sync_async(Function() As Boolean
                              Return valid_name(name) AndAlso
                                     eva(o, New istrkeyvt_property(path(), name, accessor))
                          End Function)
    End Function

    Public Function path() As String Implements inode.path
        Return p
    End Function

    Public Function properties(ByVal r As ref(Of vector(Of String))) As event_comb Implements inode.properties
        Return properties_property.read(r)
    End Function

    Public Function subnodes(ByVal r As ref(Of vector(Of String))) As event_comb Implements inode.subnodes
        Return subnodes_property.read(r)
    End Function
End Class
