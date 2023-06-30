
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt

Friend Class heap_case
    Inherits random_run_case

    Private Const size As Int64 = 1048576
    Private ReadOnly h As heap(Of Int64) = Nothing
    Private ReadOnly s() As Boolean = Nothing

    Private Function validate() As Boolean
        Return s.Length() > 0
    End Function

    Private Sub push()
        Dim i As Int64 = 0
        i = rnd_int(0, size)
        If validate() Then
            If Not s(i) Then
                s(i) = True
                Dim j As Int64 = 0
                j = h.push(i)
                assertion.less(j, h.size())
                assertion.more_or_equal(j, 0)
            End If
        Else
            h.push(i)
        End If
    End Sub

    Private Function pop(ByRef i As Int64) As Boolean
        If h.pop(i) Then
            assertion.less(i, size)
            assertion.more_or_equal(i, 0)
            If validate() Then
                assertion.is_true(s(i))
                s(i) = False
            End If
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub pop()
        Dim i As Int64 = 0
        Dim j As Int64 = 0
        If pop(i) AndAlso pop(j) AndAlso validate() Then
            assertion.more(i, j)
        End If
    End Sub

    Private Sub [erase]()
        If h.empty() Then
            assertion.is_false(h.erase(rnd_int(0, size)))
        Else
            assertion.is_true(h.erase(rnd_int(0, h.size())))
        End If
    End Sub

    Public Sub New(Optional ByVal validate As Boolean = True)
        h = New heap(Of Int64)()
        If validate Then
            ReDim s(size - 1)
        Else
            ReDim s(-1)
        End If
        insert_call(0.475, AddressOf push)
        insert_call(0.175, AddressOf pop)
        insert_call(0.35, AddressOf [erase])
    End Sub

    Public Overrides Function finish() As Boolean
        arrays.clear(s)
        h.clear()
        Return MyBase.finish()
    End Function
End Class
