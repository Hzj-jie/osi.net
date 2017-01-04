
Imports osi.root.connector
Imports osi.root.formation

Namespace logic
    Public Class scope
        ' The offset of current scope, i.e. the sum of all its ancient scopes.
        Private ReadOnly p As scope
        Private ReadOnly m As map(Of String, UInt32)
        Private c As scope
        Private s As UInt32

        Public Sub New()
            Me.New(Nothing)
        End Sub

        Public Sub New(ByVal parent As scope)
            Me.p = parent
            Me.m = New map(Of String, UInt32)()
            Me.c = Nothing
            Me.s = 0
        End Sub

        Public Function start_scope() As scope
            assert(c Is Nothing)
            c = New scope(Me)
            Return c
        End Function

        Public Function end_scope() As scope
            If Not p Is Nothing Then
                p.c = Nothing
            End If
            Return p
        End Function

        Public Function define(ByVal name As String) As Boolean
            If m.find(name) = m.end() Then
                s += 1
                m(name) = s
                Return True
            Else
                Return False
            End If
        End Function

        ' Find @name in @m, and return the offset to the "end" / "top" of current scope.
        Private Function find(ByVal name As String, ByRef offset As UInt32) As Boolean
            Dim it As map(Of String, UInt32).iterator = Nothing
            it = m.find(name)
            If it <> m.end() Then
                offset = s - (+it).second
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function export(ByVal offset As UInt32) As String
            Return strcat("rel", offset)
        End Function

        Public Function export(ByVal name As String, ByRef o As UInt32) As Boolean
            Dim offset As UInt32 = 0
            Dim size As UInt32 = 0
            Dim s As scope = Nothing
            s = Me
            While Not s Is Nothing
                If s.find(name, offset) Then
                    o = offset + size
                    Return True
                End If
                size += s.s
                s = s.p
            End While
            Return False
        End Function

        Public Function export(ByVal name As String, ByRef o As String) As Boolean
            Dim offset As UInt32 = 0
            If export(name, offset) Then
                o = export(offset)
                Return True
            Else
                Return False
            End If
        End Function
    End Class
End Namespace
