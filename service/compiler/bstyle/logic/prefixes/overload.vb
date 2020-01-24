
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class overload
        Private ReadOnly m As map(Of String, String)

        Public Sub New()
            m = New map(Of String, String)()
        End Sub

        Public Function define(ByVal name As String, ByVal return_type As String) As Boolean
            Dim it As map(Of String, String).iterator = Nothing
            it = m.find(name)
            If it <> m.end() AndAlso Not strsame((+it).second, return_type) Then
                raise_error(error_type.user,
                            "Return type of ",
                            name,
                            " has been redefined to ",
                            return_type,
                            ", original type is ",
                            (+it).second)
                Return False
            End If
            If it = m.end() Then
                m.emplace(name, return_type)
            End If
            Return True
        End Function

        Public Function dump() As String
            Dim w As writer = Nothing
            w = New writer()
            Dim it As map(Of String, String).iterator = Nothing
            it = m.begin()
            While it <> m.end()
                ' Define a set of "dummy" functions for return-type-of- check.
                assert(builders.of_callee((+it).first,
                                          (+it).second,
                                          New vector(Of pair(Of String, String))(),
                                          Function() As Boolean
                                              Return True
                                          End Function).to(w))
                it += 1
            End While
            Return w.dump()
        End Function

        Private NotInheritable Class statement_ref
            Implements statement

            Private ReadOnly ref As overload_ref

            Public Sub New(ByVal m As overload)
                Me.ref = New overload_ref(m)
            End Sub

            Private NotInheritable Class overload_ref
                Inherits writer.delayed

                Private ReadOnly m As overload

                Public Sub New(ByVal m As overload)
                    assert(Not m Is Nothing)
                    Me.m = m
                End Sub

                Public Overrides Function ToString() As String
                    Return m.dump()
                End Function
            End Class

            Public Sub export(ByVal o As writer) Implements statement.export
                o.append(ref)
            End Sub
        End Class

        Public Shared Sub register(ByVal p As statements, ByVal l As logic_rule_wrapper)
            assert(Not p Is Nothing)
            assert(Not l Is Nothing)
            p.register(New statement_ref(l.overload))
        End Sub
    End Class
End Class
