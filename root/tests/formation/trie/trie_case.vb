
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utt

Friend Class trie_case
    Inherits random_run_case

    Private ReadOnly m As map(Of String, Int32)
    Private ReadOnly t As stringtrie(Of Int32)

    Public Sub New(ByVal validate As Boolean)
        t = New stringtrie(Of Int32)()
        If validate Then
            m = New map(Of String, Int32)()
        End If
        insert_call(0.39995, AddressOf find)
        insert_call(0.5, AddressOf insert)
        insert_call(0.1, AddressOf [erase])
        insert_call(0.00005, AddressOf clear)
    End Sub

    Public Overrides Function finish() As Boolean
        t.clear()
        If validate() Then
            m.clear()
        End If
        Return MyBase.finish()
    End Function

    Private Shared Function random_key() As String
        Return strleft(guid_str(), 4)
    End Function

    Private Shared Function random_value() As Int32
        Return rnd_int(min_int32, max_int32)
    End Function

    Private Function validate() As Boolean
        Return Not m Is Nothing
    End Function

    Private Sub find()
        Dim s As String = Nothing
        s = random_key()
        Dim it As stringtrie(Of Int32).iterator = Nothing
        it = t.find(s)
        If validate() Then
            Dim mr As Boolean = False
            mr = (m.find(s) <> m.end())
            If mr Then
                Dim b As Boolean = False
                b = assertion.is_true(it <> t.end(),
                               "finding results of {",
                               s,
                               "} are inconsistant") AndAlso
                    assertion.is_true((+it).has_value,
                                "finding results of {",
                                s,
                                "} are inconsistant") AndAlso
                    assertion.equal((+it).value,
                                 m(s),
                                 New lazier(Of String)(Function() strcat("values of {",
                                                                         s,
                                                                         "} are inconsistant, it's ",
                                                                         m(s),
                                                                         " in map, but ",
                                                                         (+it).value,
                                                                         " in trie")))
            Else
                assertion.is_true(it = t.end() OrElse Not (+it).has_value)
            End If
        End If
    End Sub

    Private Sub insert()
        Dim s As String = Nothing
        Dim i As Int32 = 0
        s = random_key()
        i = random_value()
        Dim r As pair(Of stringtrie(Of Int32).iterator, Boolean) = Nothing
        r = t.emplace(s, i)
        assertion.is_not_null(r)
        assertion.is_not_null(r.first)
        assertion.not_equal(r.first, t.end())
        If validate() Then
            If r.second Then
                assertion.equal(m.find(s), m.end())
                m(s) = i
            End If
            assertion.not_equal(t.find(s), t.end())
        End If
    End Sub

    Private Sub [erase]()
        Dim s As String = Nothing
        s = random_key()
        Dim r As Boolean = False
        r = t.erase(s)
        If validate() Then
            assertion.equal(r, m.erase(s), "erase results of {", s, "} are inconsistant")
        End If
    End Sub

    Private Sub clear()
        t.clear()
        If validate() Then
            m.clear()
        End If
    End Sub
End Class
