
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils
Imports osi.service.interpreter.fullstack.executor

Namespace fullstack.syntax
    Public Class domain_manager
        Private d As domain

        Public Sub New()
            d = New domain()
        End Sub

        Public Function domain() As domain
            Return d
        End Function

        Public Sub start_new_domain()
            d = New domain(d)
        End Sub

        Public Sub finish_domain()
            d = d.parent()
        End Sub
    End Class

    Public Class domain
        Private ReadOnly p As domain
        Private ReadOnly level As UInt32
        Private ReadOnly m As map(Of String, variable)
        Private inc As Int32

        Public Sub New(ByVal p As domain)
            Me.New()
            assert(Not p Is Nothing)
            Me.p = p
            Me.level = parent().level + 1
        End Sub

        Public Sub New()
            Me.m = New map(Of String, variable)()
            Me.p = Nothing
            Me.level = 0
        End Sub

        Public Function parent() As domain
            assert(Not p Is Nothing)
            Return p
        End Function

        Private Function define(ByVal name As String, ByVal v As variable) As Boolean
            assert(Not v Is Nothing)
            If String.IsNullOrEmpty(name) OrElse
               m.find(name) = m.end() Then
                If Not String.IsNullOrEmpty(name) Then
                    m.insert(name, v)
                End If
                inc += 1
                Return True
            Else
                Return False
            End If
        End Function

        Public Function define(ByVal name As String,
                               ByVal t As type,
                               ByVal value As Object,
                               ByRef var As variable) As Boolean
            var = New variable(t, value, level, inc)
            Return define(name, var)
        End Function

        'anonymous variable
        Public Function define(ByVal t As type, ByVal value As Object) As variable
            Dim v As variable = Nothing
            assert(define(Nothing, t, value, v))
            Return v
        End Function

        Public Function resolve(ByVal name As String, ByRef v As variable) As Boolean
            If String.IsNullOrEmpty(name) Then
                Return False
            Else
                Dim it As map(Of String, variable).iterator = Nothing
                it = m.find(name)
                If it = m.end() Then
                    Return False
                Else
                    v = (+it).second
                    Return True
                End If
            End If
        End Function
    End Class
End Namespace
