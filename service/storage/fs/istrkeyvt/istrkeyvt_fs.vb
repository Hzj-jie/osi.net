
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.procedure
Imports osi.root.utils

Public Class istrkeyvt_fs
    Implements ifs

    Public ReadOnly accessor As istrkeyvt

    Public Sub New(ByVal accessor As istrkeyvt)
        assert(Not accessor Is Nothing)
        Me.accessor = accessor
    End Sub

    Public Function create(ByVal path As String,
                           ByVal o As ref(Of inode),
                           Optional ByVal wait_ms As Int64 = npos) As event_comb Implements ifs.create
        Dim ec As event_comb = Nothing
        Dim pp As istrkeyvt_node = Nothing
        Dim r As ref(Of Boolean) = Nothing
        Return New event_comb(Function() As Boolean
                                  normalize_path(path)
                                  If valid_path(path) Then
                                      ec = (New istrkeyvt_node(parent_path(path), accessor)) _
                                                .subnodes_property.locked_push_back(last_level_name(path))
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      ec = open(path, o)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function

    Public Function exist(ByVal path As String, ByVal r As ref(Of Boolean)) As event_comb Implements ifs.exist
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  normalize_path(path)
                                  If valid_path(path) Then
                                      ec = accessor.seek(properties_property_key(path), r)
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

    Public Function open(ByVal path As String, ByVal r As ref(Of inode)) As event_comb Implements ifs.open
        Return sync_async(Function() As Boolean
                              normalize_path(path)
                              Return valid_path(path) AndAlso
                                     eva(r, New istrkeyvt_node(path, accessor))
                          End Function)
    End Function
End Class
