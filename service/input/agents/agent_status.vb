
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.formation

Friend Class agent_status
    Public Event down_action(ByVal c As [case])
    Public Event up_action(ByVal c As [case])
    Public Event press_action(ByVal c As [case])
    Public Event issue(ByVal c As [case])
    Private ReadOnly is_down As [set](Of Int32)
    Private ReadOnly mode As mode

    Public Sub New(ByVal mode As mode)
        Me.mode = mode
        Me.is_down = New [set](Of Int32)()
    End Sub

    Public Sub new_status(ByVal ParamArray inputs() As Int32)
        up_to(inputs)
        down_to(inputs)
    End Sub

    Public Sub down_to(ByVal ParamArray inputs() As Int32)
        For i As Int32 = 0 To array_size(inputs) - 1
            If is_down.find(inputs(i)) = is_down.end() Then
                assert(is_down.insert(inputs(i)) <> is_down.end())
                Dim c2 As [case] = Nothing
                c2 = New [case](mode, action.down, int32_bytes(inputs(i)))
                RaiseEvent issue(c2)
                RaiseEvent down_action(c2)
            End If

            Dim c As [case] = Nothing
            c = New [case](mode, action.press, int32_bytes(inputs(i)))
            RaiseEvent issue(c)
            RaiseEvent press_action(c)
        Next
    End Sub

    Public Sub up_to(ByVal ParamArray inputs() As Int32)
        Dim it As [set](Of Int32).iterator = Nothing
        Dim es As vector(Of Int32) = Nothing
        es = New vector(Of Int32)()
        it = is_down.begin()
        While it <> is_down.end()
            If Not inputs.has(+it) Then
                Dim c As [case] = Nothing
                c = New [case](mode, action.up, int32_bytes(+it))
                RaiseEvent issue(c)
                RaiseEvent up_action(c)
                es.emplace_back(+it)
            End If
            it += 1
        End While
        For i As Int32 = 0 To es.size() - 1
            assert(is_down.erase(es(i)))
        Next
    End Sub
End Class
