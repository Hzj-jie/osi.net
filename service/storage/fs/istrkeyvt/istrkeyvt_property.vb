
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.connector

Public Class istrkeyvt_property
    Implements iproperty

    Public ReadOnly accessor As istrkeyvt
    Public ReadOnly key As String
    Private ReadOnly n As String
    Private ReadOnly p As String

    Public Sub New(ByVal path As String, ByVal name As String, ByVal accessor As istrkeyvt)
        assert(Not accessor Is Nothing)
        assert(Not path.null_or_empty())
        assert(Not name.null_or_empty())
        Me.accessor = accessor
        Me.p = path
        Me.n = name
        Me.key = property_key(path, name)
    End Sub

    Public Shared Function create_properties_property(ByVal path As String,
                                                      ByVal accessor As istrkeyvt) As istrkeyvt_property
        Return New istrkeyvt_property(path, properties_property_name, accessor)
    End Function

    Public Shared Function create_subnodes_property(ByVal path As String,
                                                    ByVal accessor As istrkeyvt) As istrkeyvt_property
        Return New istrkeyvt_property(path, subnode_property_name, accessor)
    End Function

    Public Function path() As String Implements iproperty.path
        Return p
    End Function

    Public Function name() As String Implements iproperty.name
        Return n
    End Function

    Public Function append(ByVal v() As Byte) As event_comb Implements iproperty.append
        Dim result As ref(Of Boolean) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  ec = accessor.append(key, v, result)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         (+result) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function [get](ByVal i As ref(Of Byte())) As event_comb Implements iproperty.get
        Return accessor.read(key, i, Nothing)
    End Function

    Public Function lock(Optional ByVal wait_ms As Int64 = npos) As event_comb Implements iproperty.lock
        Dim r As ref(Of Boolean) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  r = New ref(Of Boolean)()
                                  ec = accessor.lock(lock_property_key(key), r, wait_ms)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         (+r) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function locked(ByVal d As Func(Of event_comb),
                           Optional ByVal wait_ms As Int64 = npos) As event_comb
        Dim r As ref(Of Boolean) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  r = New ref(Of Boolean)()
                                  ec = accessor.locked(lock_property_key(key), d, r, wait_ms)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         (+r) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function release() As event_comb Implements iproperty.release
        Dim ec As event_comb = Nothing
        Dim r As ref(Of Boolean) = Nothing
        Return New event_comb(Function() As Boolean
                                  r = New ref(Of Boolean)()
                                  ec = accessor.release(lock_property_key(key), r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         (+r) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function [set](ByVal v() As Byte) As event_comb Implements iproperty.set
        Dim ec As event_comb = Nothing
        Dim r As ref(Of Boolean) = Nothing
        Return New event_comb(Function() As Boolean
                                  r = New ref(Of Boolean)()
                                  ec = accessor.modify(key, v, r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function locked_push_back(ByVal value() As Byte,
                                     Optional ByVal wait_ms As Int64 = npos) As event_comb
        Return locked(Function() As event_comb
                          Return unique_push_back(value)
                      End Function,
                      wait_ms)
    End Function

    Public Function locked_push_back(ByVal name As String,
                                     Optional ByVal wait_ms As Int64 = npos) As event_comb
        Return locked(Function() As event_comb
                          Return unique_push_back(name)
                      End Function,
                      wait_ms)
    End Function
End Class
