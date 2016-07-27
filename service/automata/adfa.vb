
Imports osi.root.template
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils

'acyclic deterministic finite automaton
Public Class adfa(Of KEY_T,
                     CHILD_COUNT As _int64,
                     KEY_TO_INDEX As _to_uint32(Of KEY_T))
    Inherits adfa(Of KEY_T, CHILD_COUNT, KEY_TO_INDEX, Int32)
End Class

Public Class adfa(Of KEY_T,
                     CHILD_COUNT As _int64,
                     KEY_TO_INDEX As _to_uint32(Of KEY_T),
                     RESULT_T)
    Inherits adfa(Of KEY_T, 
                     CHILD_COUNT, 
                     KEY_TO_INDEX, 
                     RESULT_T, 
                     trie(Of KEY_T, Func(Of KEY_T(), UInt32, RESULT_T, Boolean), CHILD_COUNT, KEY_TO_INDEX))
End Class

Public Class adfa(Of KEY_T,
                     CHILD_COUNT As _int64,
                     KEY_TO_INDEX As _to_uint32(Of KEY_T),
                     RESULT_T,
                     TRIE_T As trie(Of KEY_T, Func(Of KEY_T(), UInt32, RESULT_T, Boolean), CHILD_COUNT, KEY_TO_INDEX))
    Private ReadOnly trie As TRIE_T

    Public Sub New()
        trie = alloc(Of TRIE_T)()
    End Sub

    Public Function insert(ByVal key() As KEY_T,
                           ByVal value As Func(Of KEY_T(), UInt32, RESULT_T, Boolean)) As Boolean
        If value Is Nothing Then
            'inserting an empty array is supported by trie
            Return False
        Else
            Return trie.insert(key, value) <> trie.end()
        End If
    End Function

    Public Function [erase](ByVal key() As KEY_T) As Boolean
        Return trie.erase(key)
    End Function

    Public Sub clear()
        trie.clear()
    End Sub

    Public Function insert(ByVal kvs() As pair(Of KEY_T(), 
                                                  Func(Of KEY_T(), UInt32, RESULT_T, Boolean))) As Boolean
        If isemptyarray(kvs) Then
            Return False
        Else
            For i As Int32 = 0 To array_size(kvs) - 1
                If kvs(i) Is Nothing OrElse Not insert(kvs(i).first, kvs(i).second) Then
                    Return False
                End If
            Next
            Return True
        End If
    End Function

    Public Function parse(ByVal s() As KEY_T,
                          ByVal r As RESULT_T,
                          ByVal match As Func(Of KEY_T(), UInt32, Boolean),
                          ByVal mismatch As Func(Of KEY_T(), UInt32, Boolean)) As Boolean
        If isemptyarray(s) Then
            Return False
        Else
            Dim i As Int32 = 0
            Dim l As Int32 = 0
            l = array_size(s)
            While i < l
                Dim it As trie(Of KEY_T, 
                                  Func(Of KEY_T(), UInt32, RESULT_T, Boolean), 
                                  CHILD_COUNT, 
                                  KEY_TO_INDEX).iterator = Nothing
                it = trie.findfront(s, i)
                assert(Not it Is Nothing AndAlso (it = trie.end() OrElse Not (+it) Is Nothing))
                If it = trie.end() Then
                    If mismatch Is Nothing OrElse mismatch(s, i) Then
                        i += 1
                    Else
                        Return False
                    End If
                Else
                    assert(Not (+it) Is Nothing AndAlso (+it).has_value)
                    If (match Is Nothing OrElse match(s, i)) AndAlso
                       (+it).value(s, i, r) Then
                        If (+it).is_root() Then
                            i += 1
                        Else
                            i += (+it).length()
                            assert(i <= l)
                        End If
                    Else
                        Return False
                    End If
                End If
            End While
            Return True
        End If
    End Function

    Public Function parse(ByVal s() As KEY_T, ByVal r As RESULT_T) As Boolean
        Return parse(s, r, Nothing, Nothing)
    End Function

    Public Function parse(ByVal s() As KEY_T,
                          ByVal match As Func(Of KEY_T(), UInt32, Boolean),
                          ByVal mismatch As Func(Of KEY_T(), UInt32, Boolean)) As Boolean
        Return parse(s, Nothing, match, mismatch)
    End Function

    Public Function parse(ByVal s() As KEY_T) As Boolean
        Return parse(s, Nothing, Nothing, Nothing)
    End Function
End Class
