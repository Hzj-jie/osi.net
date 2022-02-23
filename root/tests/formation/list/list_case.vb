
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.utt
Imports osi.root.formation

Friend Class list_case
    Inherits random_run_case

    Private ReadOnly l As list(Of String) = Nothing
    Private ReadOnly v As vector(Of String) = Nothing

    Private Function validate() As Boolean
        Return v IsNot Nothing
    End Function

    Private Sub first()
        'confirm at least one element in list
        l.push_back(guid_str())
        If validate() Then
            v.push_back(l.back())
            assertion.is_true(v.erase(l.front()), "cannot find first element of list in vector.")
        End If
        l.front() = guid_str()
        If validate() Then
            v.push_back(l.front())
        End If
    End Sub

    Private Sub last()
        'confirm at least one element in list
        l.push_front(guid_str())
        If validate() Then
            v.push_back(l.front())
            assertion.is_true(v.erase(l.back()), "cannot find last element of list in vector.")
        End If
        l.back() = guid_str()
        If validate() Then
            v.push_back(l.back())
        End If
    End Sub

    Private Sub insert()
        'confirm at least one element in list
        l.push_back(guid_str())
        If validate() Then
            v.push_back(l.back())
        End If

        Dim it As list(Of String).iterator = Nothing
        If validate() Then
            Dim s As String = Nothing
            s = v(rnd_int(0, v.size()))
            If rnd_int(0, 2) = 0 Then
                it = l.find(s)
                assertion.is_true(it <> l.end(), "cannot find " + s + " in list.")
            Else
                it = l.rfind(s)
                assertion.is_true(it <> l.rend(), "cannot find " + s + " in list.")
            End If
        Else
            it = l.find(l(rnd_int(0, l.size())))
        End If
        it = l.insert(it, guid_str())
        If validate() Then
            v.push_back(+it)
        End If
    End Sub

    Private Sub push_back()
        Dim s As String = Nothing
        s = guid_str()
        l.push_back(s)
        assertion.equal(l.back(), s, "l.push_back(" + s + ") <> l.back()")
        If validate() Then
            v.push_back(s)
        End If
    End Sub

    Private Sub push_front()
        Dim s As String = Nothing
        s = guid_str()
        l.push_front(s)
        assertion.equal(l.front(), s, "l.push_front(" + s + ") <> l.front()")
        If validate() Then
            v.push_back(s)
        End If
    End Sub

    Private Sub pop_front()
        'confirm at least one element in list
        l.push_back(guid_str())
        If validate() Then
            v.push_back(l.back())
            v.erase(l.front())
        End If
        l.pop_front()
    End Sub

    Private Sub pop_back()
        'confirm at least one element in list
        l.push_front(guid_str())
        If validate() Then
            v.push_back(l.front())
            v.erase(l.back())
        End If
        l.pop_back()
    End Sub

    Private Sub size()
        If validate() Then
            assertion.equal(l.size(), v.size(), "l.size() <> v.size()")
            Dim it As list(Of String).iterator = Nothing
            it = l.begin()
            Dim count As Int64
            count = 0
            While it <> l.end()
                count += 1
                it += 1
            End While
            assertion.equal(l.size(), count, "l.size() <> count")
        Else
            l.size()
        End If
    End Sub

    Private Sub empty()
        If validate() Then
            assertion.equal(l.empty(), v.empty(), "l.empty() <> v.empty()")
        Else
            l.empty()
        End If
    End Sub

    Private Sub find()
        'confirm at least one element.
        l.push_back(guid_str())
        If validate() Then
            v.push_back(l.back())
            Dim s As String = Nothing
            s = v(rnd_int(0, v.size()))
            assertion.not_equal(l.find(s), l.end(), "l.find(" + s + ") = l.end()")
        Else
            l.find(l(rnd_int(0, l.size())))
        End If
    End Sub

    Private Sub rfind()
        'confirm at least one element.
        l.push_front(guid_str())
        If validate() Then
            v.push_back(l.front())
            Dim s As String = Nothing
            s = v(rnd_int(0, v.size()))
            assertion.not_equal(l.rfind(s), l.rend(), "l.rfind(" + s + ") = l.rend()")
        Else
            l.rfind(l(rnd_int(0, l.size())))
        End If
    End Sub

    Private Sub at()
        'confirm at least one element.
        l.push_back(guid_str())
        Dim i As Int64 = Nothing
        i = rnd_int(0, l.size())
        If validate() Then
            v.push_back(l.back())
            assertion.is_true(v.erase(l(i)), "v.erase(" + l(i) + ") returns false.")
        End If
        l(i) = guid_str()
        If validate() Then
            v.push_back(l(i))
        End If
    End Sub

    Private Sub [erase]()
        'confirm at least one element.
        l.push_back(guid_str())
        If validate() Then
            v.push_back(l.back())
        End If
        If rnd_bool() AndAlso validate() Then
            Dim i As Int64 = Nothing
            i = rnd_int(0, v.size())
            l.erase(v(i))
            assert(v.erase(v(i)))
        Else
            Dim i As UInt32 = Nothing
            i = rnd_uint(0, l.size())
            Dim s As String = Nothing
            s = l(i)
            l.erase(i)
            If validate() Then
                assertion.is_true(v.erase(s), "cannot find " + s + " in vector.")
            End If
        End If
    End Sub

    Private Sub clone()
        Dim d2 As list(Of String) = Nothing
        copy(d2, l)
        assertion.equal(object_compare(l, d2), object_compare_undetermined, "d2 is the same object as l.")
        assertion.equal(compare(l, d2), 0, "l<>d2 after clone.")
    End Sub

    Private Sub clear()
        l.clear()
        If validate() Then
            v.clear()
        End If
        assertion.is_true(l.empty(), "l.empty()=false after clear.")
    End Sub

    Public Sub New(ByVal validation As Boolean)
        MyBase.New()
        l = New list(Of String)()
        If validation Then
            v = New vector(Of String)()
        End If
        insert_call(0.05, AddressOf first)
        insert_call(0.05, AddressOf last)
        insert_call(0.12, AddressOf insert)
        insert_call(0.13, AddressOf push_back)
        insert_call(0.13, AddressOf push_front)
        insert_call(0.05, AddressOf pop_back)
        insert_call(0.05, AddressOf pop_front)
        insert_call(0.05, AddressOf size)
        insert_call(0.05, AddressOf empty)
        insert_call(0.1, AddressOf find)
        insert_call(0.1, AddressOf rfind)
        insert_call(0.05, AddressOf at)
        insert_call(0.05, AddressOf [erase])
        insert_call(0.01, AddressOf clone)
        insert_call(0.01, AddressOf clear)
    End Sub

    Public Overrides Function finish() As Boolean
        l.clear()
        If validate() Then
            v.clear()
        End If
        Return MyBase.finish()
    End Function
End Class
