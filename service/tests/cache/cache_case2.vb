
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.cache

' Ensure icache implementation can store the key-value pairs correctly.
Public Class cache_case2
    Inherits random_run_case

    Private ReadOnly m As unordered_map(Of Int32, Int32)
    Private ReadOnly c As islimcache2(Of Int32, Int32)

    Public Sub New(ByVal c As islimcache2(Of Int32, Int32))
        assert(c IsNot Nothing)
        m = New unordered_map(Of Int32, Int32)()
        Me.c = c

        insert_call(0.3, AddressOf [set])
        insert_call(0.3, AddressOf [get])
        insert_call(0.125, AddressOf size)
        insert_call(0.0001, AddressOf clear)
        insert_call(0.125, AddressOf [erase])
        insert_call(0.1, AddressOf have)
        insert_call(0.0499, AddressOf empty)
    End Sub

    Private Shared Function random_key() As Int32
        Return rnd_int(min_int16, max_int16)
    End Function

    Private Shared Function random_value() As Int32
        Return rnd_int()
    End Function

    Private Sub [set]()
        Dim key As Int32 = 0
        Dim value As Int32 = 0
        key = random_key()
        value = random_value()

        c.set(key, value)
        m(key) = value
    End Sub

    Private Sub assert_key_value(ByVal key As Int32, ByVal value As Int32)
        assertion.not_equal(m.find(key), m.end())
        assertion.equal((+m.find(key)).second, value)
    End Sub

    Private Sub [get]()
        Dim key As Int32 = 0
        key = random_key()
        Dim value As Int32 = 0
        If c.get(key, value) Then
            assert_key_value(key, value)
        Else
            assertion.equal(m.find(key), m.end())
        End If
    End Sub

    Private Sub size()
        assertion.equal(c.size(), m.size())
    End Sub

    Private Sub clear()
        c.clear()
        m.clear()
    End Sub

    Private Sub [erase]()
        Dim key As Int32 = 0
        key = random_key()
        assertion.equal(c.erase(key), m.erase(key))
    End Sub

    Private Sub have()
        Dim key As Int32 = 0
        key = random_key()
        If c.have(key) Then
            assertion.not_equal(m.find(key), m.end())
        End If
    End Sub

    Private Sub empty()
        assertion.equal(c.empty(), m.empty())
    End Sub
End Class
