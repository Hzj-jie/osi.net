﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public Class scope(Of T As scope(Of T))
    Public MustInherit Class call_hierarchy
        Private ReadOnly main_name As String
        ' To -> From
        Private ReadOnly m As New unordered_map(Of String, vector(Of String))()
        Private ReadOnly tm As New unordered_map(Of String, Boolean)()

        Protected Sub New(ByVal main_name As String)
            assert(Not main_name.null_or_whitespace())
            Me.main_name = main_name
        End Sub

        Protected Sub New()
            Me.New("main")
        End Sub

        Protected MustOverride Function current_function_name() As [optional](Of String)

        Public Sub [to](ByVal name As String)
            assert(Not name.null_or_whitespace())
            Dim from As String = current_function_name().or_else(main_name)
            assert(Not from.null_or_whitespace())
            If Not name.Equals(from) Then
                m(name).emplace_back(from)
            End If
        End Sub

        Default Public ReadOnly Property can_reach_root(ByVal f As String) As Boolean
            Get
                assert(Not f.null_or_whitespace())
                If f.Equals(main_name) Then
                    Return True
                End If
                Dim it As unordered_map(Of String, Boolean).iterator = tm.find(f)
                If it <> tm.end() Then
                    Return (+it).second
                End If
                Dim r As Boolean = False
                Dim it2 As unordered_map(Of String, vector(Of String)).iterator = m.find(f)
                If it2 <> m.end() Then
                    Dim v As vector(Of String) = (+it2).second
                    assert(Not v.null_or_empty())
                    For i As UInt32 = 0 To v.size() - uint32_1
                        If can_reach_root(v(i)) Then
                            r = True
                            Exit For
                        End If
                    Next
                End If
                tm(f) = r
                Return r
            End Get
        End Property

        Public Function filter(ByVal f As String, ByVal o As Func(Of String)) As Func(Of String)
            Return AddressOf New filtered_writer(Me, f, o).str
        End Function

        Private NotInheritable Class filtered_writer
            Private ReadOnly ch As call_hierarchy
            Private ReadOnly f As String
            Private ReadOnly o As Func(Of String)

            Public Sub New(ByVal ch As call_hierarchy, ByVal f As String, ByVal o As Func(Of String))
                assert(Not ch Is Nothing)
                assert(Not f.null_or_whitespace())
                assert(Not o Is Nothing)
                Me.ch = ch
                Me.f = f
                Me.o = o
            End Sub

            Public Function str() As String
                If (scope_arguments.remove_unused_functions Or True) AndAlso Not ch(f) Then
                    Return ""
                End If
                Return o()
            End Function
        End Class
    End Class
End Class
