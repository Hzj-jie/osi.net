
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    Public NotInheritable Class scope
        Private ReadOnly parent As scope
        Private ReadOnly m As map(Of String, variable_ref)
        Private child As scope

        Private NotInheritable Class variable_ref
            Public ReadOnly offset As UInt64
            Public ReadOnly type As String

            Public Sub New(ByVal offset As UInt64, ByVal type As String)
                assert(Not type.null_or_whitespace())
                Me.offset = offset
                Me.type = type
            End Sub
        End Class

        Public Sub New()
            Me.New(Nothing)
        End Sub

        Public Sub New(ByVal parent As scope)
            Me.parent = parent
            Me.m = New map(Of String, variable_ref)()
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
            Return define(name, size() + uint32_1, type)
        End Function

        Public Function define(ByVal name As String, ByVal offset As UInt64, ByVal type As String) As Boolean
            assert(Not name.null_or_whitespace())
            assert(Not type.null_or_whitespace())
            If m.find(name) <> m.end() Then
                errors.redefine(name, type, Me.type(name))
                Return False
            End If
            m.emplace(name, New variable_ref(offset, type))
            Return True
        End Function

        Public Function is_root() As Boolean
            Return parent Is Nothing
        End Function

        Public Function empty() As Boolean
            Return size() = uint32_0
        End Function

        Public Function size() As UInt32
            Return m.size()
        End Function

        ' Find @name in @m, and return the offset to the "end" / "top" of current scope.
        Private Function find(ByVal name As String, ByRef offset As UInt64) As Boolean
            Dim o As variable_ref = Nothing
            If Not m.find(name, o) Then
                Return False
            End If
            offset = CULng(size() - o.offset)
            Return True
        End Function

        Public Function type(ByVal name As String, ByRef o As String) As Boolean
            Dim s As scope = Nothing
            s = Me
            While Not s Is Nothing
                Dim r As variable_ref = Nothing
                If s.m.find(name, r) Then
                    o = r.type
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
                    End If
                    Return data_ref.rel(CLng(offset + size), o)
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
