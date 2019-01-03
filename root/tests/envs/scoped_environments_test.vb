
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.envs
Imports osi.root.utt

Public Class scoped_environments_test
    Inherits [case]

    Private Shared Function case1() As Boolean
        Dim key As String = Nothing
        Dim value As String = Nothing
        key = guid_str()
        value = guid_str()
        assertion.is_false(env_bool(key))
        Using New scoped_environments(key, value)
            Dim o As String = Nothing
            assertion.is_true(env_value(key, o))
            assertion.equal(o, value)
        End Using
        assertion.is_false(env_bool(key))
        Return True
    End Function

    Private Shared Function case2() As Boolean
        Dim kv(,) As String = Nothing
        ReDim kv(16 - 1, 2 - 1)
        For i As Int32 = 0 To array_size_i(kv) - 1
            kv(i, 0) = guid_str()
            kv(i, 1) = guid_str()
        Next
        For i As Int32 = 0 To array_size_i(kv) - 1
            assertion.is_false(env_bool(kv(i, 0)))
        Next
        Using New scoped_environments(kv)
            For i As Int32 = 0 To array_size_i(kv) - 1
                Dim o As String = Nothing
                assertion.is_true(env_value(kv(i, 0), o))
                assertion.equal(kv(i, 1), o)
            Next
        End Using
        For i As Int32 = 0 To array_size_i(kv) - 1
            assertion.is_false(env_bool(kv(i, 0)))
        Next
        Return True
    End Function

    Private Shared Function case3() As Boolean
        Dim kv()() As String = Nothing
        ReDim kv(16 - 1)
        For i As Int32 = 0 To array_size_i(kv) - 1
            ReDim kv(i)(2 - 1)
            kv(i)(0) = guid_str()
            kv(i)(1) = guid_str()
        Next
        For i As Int32 = 0 To array_size_i(kv) - 1
            assertion.is_false(env_bool(kv(i)(0)))
        Next
        Using New scoped_environments(kv)
            For i As Int32 = 0 To array_size_i(kv) - 1
                Dim o As String = Nothing
                assertion.is_true(env_value(kv(i)(0), o))
                assertion.equal(kv(i)(1), o)
            Next
        End Using
        For i As Int32 = 0 To array_size_i(kv) - 1
            assertion.is_false(env_bool(kv(i)(0)))
        Next
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return case1() AndAlso
               case2() AndAlso
               case3()
    End Function
End Class
