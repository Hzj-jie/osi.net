
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.utils
Imports osi.root.procedure
Imports osi.root.formation

Public Module _double_keys
    Public Function merge_key(ByVal key() As Byte, ByVal prefix As Byte) As Byte()
        assert(Not isemptyarray(key))
        Dim r() As Byte = Nothing
        ReDim r(array_size(key))
        r(0) = prefix
        memcpy(r, 1, key)
        Return r
    End Function

    Public Function is_merged_key(ByVal key() As Byte, ByVal prefix As Byte, ByRef original() As Byte) As Boolean
        Dim l As Int32 = 0
        l = array_size(key)
        If l <= 1 Then
            Return False
        ElseIf key(0) = prefix Then
            l -= 1
            ReDim original(l - 1)
            memcpy(original, 0, key, 1, l)
            Return True
        Else
            Return False
        End If
    End Function

    Public Function half_keycount(ByVal d As Func(Of pointer(Of Int64), event_comb),
                                  ByVal r As pointer(Of Int64)) As event_comb
        assert(Not d Is Nothing)
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If r Is Nothing Then
                                      r = New pointer(Of Int64)()
                                  End If
                                  ec = d(r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      If (+r) >= 0 Then
                                          Return eva(r, (+r) >> 1) AndAlso
                                                 goto_end()
                                      Else
                                          Return False
                                      End If
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function

    Public Function select_list(ByVal d As Func(Of pointer(Of vector(Of Byte())), event_comb),
                                ByVal [select] As _do_val_ref(Of Byte(), Byte(), Boolean),
                                ByVal result As pointer(Of vector(Of Byte()))) As event_comb
        assert(Not d Is Nothing)
        assert(Not [select] Is Nothing)
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If result Is Nothing Then
                                      result = New pointer(Of vector(Of Byte()))()
                                  End If
                                  ec = d(result)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      If +result Is Nothing Then
                                          Return False
                                      Else
                                          Dim l As vector(Of Byte()) = Nothing
                                          l = New vector(Of Byte())
                                          For i As Int32 = 0 To (+result).size() - 1
                                              Dim b() As Byte = Nothing
                                              'the timestamp key is modified at the last
                                              If [select]((+result)(i), b) Then
                                                  l.emplace_back(b)
                                              End If
                                          Next
                                          Return eva(result, l) AndAlso
                                                 goto_end()
                                      End If
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function
End Module
