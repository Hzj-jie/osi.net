
Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure

Public Interface ifs
    Function create(ByVal path As String,
                    ByVal r As pointer(Of inode),
                    Optional ByVal wait_ms As Int64 = npos) As event_comb
    Function open(ByVal path As String, ByVal o As pointer(Of inode)) As event_comb
    Function exist(ByVal path As String, ByVal r As pointer(Of Boolean)) As event_comb
End Interface

Public Module _ifs
    <Extension()> Public Function list(ByVal fs As ifs,
                                       ByVal path As String,
                                       ByVal r As pointer(Of vector(Of String))) As event_comb
        Dim ec As event_comb = Nothing
        Dim n As pointer(Of inode) = Nothing
        Return New event_comb(Function() As Boolean
                                  If fs Is Nothing Then
                                      Return False
                                  Else
                                      n = New pointer(Of inode)()
                                      ec = fs.open(path, n)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() AndAlso
                                     Not (+n) Is Nothing Then
                                      ec = (+n).subnodes(r)
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
End Module