﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utils

Partial Public NotInheritable Class tar
    Public NotInheritable Class reader
        Private ReadOnly fs As fs
        Private ReadOnly v As vector(Of String)
        Private ReadOnly m As MemoryStream
        Private i As UInt32

        Public Sub New(ByVal v As vector(Of String))
            Me.New(default_fs.instance, v)
        End Sub

        Private Sub New(ByVal fs As fs, ByVal v As vector(Of String))
            assert(Not fs Is Nothing)
            assert(Not v Is Nothing)
            Me.fs = fs
            Me.v = v
            Me.m = New MemoryStream()
        End Sub

        Public Shared Function of_testing(ByVal fs As testing_fs) As reader
            assert(Not fs Is Nothing)
            Return New reader(fs, fs.list_files())
        End Function

        Public Shared Function unzip(ByVal v As vector(Of String)) As reader
            Return New reader(zip_fs.instance, v)
        End Function

        Public Function [next](ByRef name As String, ByRef o As MemoryStream) As Boolean
            While m.eos() AndAlso i < v.size()
                m.clear()
                Using m.keep_position()
                    If Not fs.read(v(i), m) Then
                        Return False
                    End If
                End Using
                i += uint32_1
            End While
            If m.eos() Then
                Return False
            End If
            Return bytes_serializer.consume_from(m, name) AndAlso
                   bytes_serializer.consume_from(m, o)
        End Function

        Public Sub foreach(ByVal f As Action(Of String, MemoryStream))
            assert(Not f Is Nothing)
            Dim n As String = Nothing
            Dim m As MemoryStream = Nothing
            While [next](n, m)
                Try
                    f(n, m)
                Catch ex As break_lambda
                    Return
                End Try
            End While
        End Sub

        Public Function dump() As vector(Of tuple(Of String, MemoryStream))
            Dim v As vector(Of tuple(Of String, MemoryStream)) = Nothing
            v = New vector(Of tuple(Of String, MemoryStream))()
            Dim s As String = Nothing
            Dim m As MemoryStream = Nothing
            While [next](s, m)
                v.emplace_back(tuple.emplace_of(s, m))
            End While
            Return v
        End Function

        Public Function index() As vector(Of tuple(Of String, UInt32))
            Dim v As vector(Of tuple(Of String, UInt32)) = Nothing
            v = New vector(Of tuple(Of String, UInt32))()
            Dim s As String = Nothing
            Dim m As MemoryStream = Nothing
            While [next](s, m)
                v.emplace_back(tuple.of(s, m.unread_length()))
            End While
            Return v
        End Function
    End Class
End Class
