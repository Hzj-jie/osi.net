
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    Public Class scope
        Private ReadOnly parent As scope
        Private ReadOnly offsets As map(Of String, UInt64)
        Private ReadOnly types As map(Of String, String)
        Private child As scope

        Public Sub New()
            Me.New(Nothing)
        End Sub

        Public Sub New(ByVal parent As scope)
            Me.parent = parent
            Me.offsets = New map(Of String, UInt64)()
            Me.types = New map(Of String, String)()
            Me.child = Nothing
        End Sub

        Public Function start_scope() As scope
            assert(child Is Nothing)
            child = New scope(Me)
            Return child
        End Function

        Public Function end_scope() As scope
            If Not parent Is Nothing Then
                parent.child = Nothing
            End If
            Return parent
        End Function

        Public Function define(ByVal name As String, ByVal type As String) As Boolean
            assert(Not String.IsNullOrEmpty(name))
            assert(Not String.IsNullOrEmpty(type))
            If offsets.find(name) = offsets.end() Then
                offsets(name) = size() + uint64_1
                types(name) = type
                Return True
            Else
                Return False
            End If
        End Function

        Public Function is_root() As Boolean
            Return parent Is Nothing
        End Function

        Public Function empty() As Boolean
            Return size() = uint32_0
        End Function

        Public Function size() As UInt32
            assert(offsets.size() = types.size())
            Return offsets.size()
        End Function

        ' Find @name in @m, and return the offset to the "end" / "top" of current scope.
        Private Function find(ByVal name As String, ByRef offset As UInt64) As Boolean
            Dim o As UInt64 = 0
            If offsets.find(name, o) Then
                offset = size() - o
                Return True
            Else
                Return False
            End If
        End Function

        Public Function type(ByVal name As String, ByRef o As String) As Boolean
            Dim s As scope = Nothing
            s = Me
            While Not s Is Nothing
                If s.types.find(name, o) Then
                    Return True
                End If
                s = s.parent
            End While
            Return False
        End Function

        Public Function type(ByVal name As String) As String
            Dim o As String = Nothing
            assert(type(name, o))
            Return o
        End Function

        Public Function export(ByVal name As String, ByRef o As data_ref) As Boolean
            Dim offset As UInt64 = 0
            Dim size As UInt64 = 0
            Dim s As scope = Nothing
            s = Me
            While Not s Is Nothing
                If s.find(name, offset) Then
                    If s.is_root() Then
                        ' To allow a callee to access global variables.
                        Return data_ref.abs(CLng(s.size() - offset - 1), o)
                    Else
                        Return data_ref.rel(CLng(offset + size), o)
                    End If
                End If
                size += s.size()
                s = s.parent
            End While
            Return False
        End Function

        Public Function export(ByVal name As String, ByRef o As String) As Boolean
            Dim ref As data_ref = Nothing
            Return export(name, ref) AndAlso
                   assert(Not ref Is Nothing) AndAlso
                   ref.export(o)
        End Function
    End Class
End Namespace
