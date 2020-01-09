﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.formation

Partial Public NotInheritable Class syntaxer
    Public Shared ReadOnly detailed_debug_log As Boolean
    Public Shared ReadOnly debug_log As Boolean

    Private ReadOnly collection As syntax_collection
    Private ReadOnly root_types As vector(Of UInt32)

    Shared Sub New()
        detailed_debug_log = env_bool(env_keys("syntaxer", "detailed", "debug")) OrElse
                             env_bool(env_keys("syntaxer", "detailed", "debugging")) OrElse
                             env_bool(env_keys("syntaxer", "detailed", "debug", "log"))
        debug_log = env_bool(env_keys("syntaxer", "debug")) OrElse
                    env_bool(env_keys("syntaxer", "debugging")) OrElse
                    env_bool(env_keys("syntaxer", "debug", "log")) OrElse
                    detailed_debug_log
    End Sub

    Public Sub New(ByVal collection As syntax_collection, ByVal root_types As vector(Of UInt32))
        assert(Not collection Is Nothing)
        assert(collection.complete())
        assert(Not root_types.null_or_empty())
        Me.collection = collection
        Me.root_types = root_types
    End Sub

    Public Function match(ByVal v As vector(Of typed_word), ByRef root As typed_node) As Boolean
        assert(Not root_types.null_or_empty())
        If v.null_or_empty() Then
            Return True
        End If
        Dim ms As vector(Of UInt32) = Nothing
        ms = New vector(Of UInt32)()
        Dim p As UInt32 = 0
        While p < v.size()
            Dim op As UInt32 = 0
            op = p
            For i As UInt32 = 0 To root_types.size() - uint32_1
                Dim s As syntax = Nothing
                assert(collection.get(root_types(i), s))
                If s.match(v, p) Then
                    ms.emplace_back(root_types(i))
                    Exit For
                End If
                p = op
            Next
            If p = op Then
                Return False
            End If
        End While
        If assert(p = v.size() AndAlso Not ms.empty()) Then
            root = typed_node.of_root(v)
            p = 0
            For i As UInt32 = 0 To ms.size() - uint32_1
                Dim s As syntax = Nothing
                assert(collection.get(ms(i), s))
                assert(s.match(v, p, root))
            Next
            Return assert(p = v.size())
        End If
        Return False
    End Function

    Public Function type_id(ByVal name As String, ByRef o As UInt32) As Boolean
        Return collection.type_id(name, o)
    End Function

    Public Function type_id(ByVal name As String) As UInt32
        Return collection.type_id(name)
    End Function

    Public Function type_name(ByVal id As UInt32, ByRef o As String) As Boolean
        Return collection.type_name(id, o)
    End Function

    Public Function type_name(ByVal id As UInt32) As String
        Return collection.type_name(id)
    End Function
End Class
