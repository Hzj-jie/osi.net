
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt
Imports osi.service.cache

Public Class cache_case
    Inherits random_run_case

    Private Shared ReadOnly enc As Text.Encoding
    Private ReadOnly s1 As icache(Of String, Byte())
    Private ReadOnly s2 As icache(Of String, Byte())

    Shared Sub New()
        enc = Text.Encoding.Unicode()
    End Sub

    Public Sub New(ByVal s1 As icache(Of String, Byte()),
                   ByVal validate As Boolean)
        assert(Not s1 Is Nothing)
        Me.s1 = s1
        If validate Then
            s2 = map_cache(Of String, Byte())()
        End If
        insert_call(0.3, AddressOf [set])
        insert_call(0.3, AddressOf [get])
        insert_call(0.125, AddressOf size)
        insert_call(0.0001, AddressOf clear)
        insert_call(0.125, AddressOf [erase])
        insert_call(0.1, AddressOf have)
        insert_call(0.0499, AddressOf empty)
    End Sub

    Private Sub [set]()
        Dim k As String = Nothing
        Dim v() As Byte = Nothing
        rnd_key_value(k, v)
        s1.set(k, v)
        If validate() Then
            s2.set(k, v)
        End If
    End Sub

    Private Sub [get]()
        Dim k As String = Nothing
        k = rnd_key()
        If validate() Then
            Dim v() As Byte = Nothing
            If s1.get(k, v) Then
                assertion.is_true(key_value(k, v))
                assertion.is_true(s2.get(k, v))
                assertion.is_true(key_value(k, v))
            Else
                assertion.is_false(s2.get(k, v))
            End If
        Else
            s1.get(k, Nothing)
        End If
    End Sub

    Private Sub size()
        Dim v As Int64 = 0
        v = s1.size()
        If validate() Then
            assertion.equal(v, s2.size())
        End If
    End Sub

    Private Sub clear()
        s1.clear()
        If validate() Then
            s2.clear()
        End If
    End Sub

    Private Sub [erase]()
        Dim k As String = Nothing
        k = rnd_key()
        If validate() Then
            assertion.equal(s1.erase(k), s2.erase(k))
        Else
            s1.erase(k)
        End If
    End Sub

    Private Sub have()
        Dim k As String = Nothing
        k = rnd_key()
        If validate() Then
            assertion.equal(s1.have(k), s2.have(k))
        Else
            s1.have(k)
        End If
    End Sub

    Private Sub empty()
        If validate() Then
            assertion.equal(s1.empty(), s2.empty())
        Else
            s1.empty()
        End If
    End Sub

    Private Function validate() As Boolean
        Return Not s2 Is Nothing
    End Function

    'return true if the content of value is already the same as expected
    Private Shared Function key_value(ByVal key As String, ByRef value() As Byte) As Boolean
        Const mult As Int32 = 4
        Dim l As Int32 = 0
        l = enc.GetByteCount(key)
        Dim r As Boolean = False
        If value Is Nothing OrElse array_size(value) <> mult * l Then
            r = False
            ReDim value(mult * l - 1)
        Else
            r = True
        End If
        Dim b() As Byte = Nothing
        b = enc.GetBytes(key)
        assert(array_size(b) = l)
        For i As Int32 = 0 To mult - 1
            For j As Int32 = 0 To l - 1
                Dim ind As Int32 = 0
                ind = i * l + j
                If value(ind) <> b(j) Then
                    r = False
                    value(ind) = b(j)
                End If
            Next
        Next
        Return r
    End Function

    Private Shared Function rnd_key() As String
        Return strleft(guid_str(), 4)
    End Function

    Private Shared Sub rnd_key_value(ByRef key As String, ByRef value() As Byte)
        key = rnd_key()
        assert(Not key_value(key, value))
    End Sub
End Class
